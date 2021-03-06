﻿import {Component, OnInit,ViewChild, ViewChildren} from '@angular/core';
import {CommonModule} from '@angular/common';
import {HttpModule}    from '@angular/http';
import {RESTService} from '../../../core/services';
import {GridBase} from '../../../core/gridBase';
import {AppNavigator} from '../../../navigation/app.navigator.component';
import { AppComponentService } from '../../../core/services';
import { AuthenticationService } from '../../../profiles/services/authentication.service';
import * as gridHelpers from '../../../core/services/helpers/gridutils';

@Component({
    templateUrl: '/app/configurator/components/ibmDomino/Ibm-travelerdatastore.component.html',
    providers: [
        HttpModule,
        RESTService,
        gridHelpers.CommonUtils
    ]
})
export class TravelerDataStore extends GridBase implements OnInit {
    errorMessage: any;
    travelerServers: any;
    currentPageSize: any = 20;
    testTravelerServers: any;
    usersByserver: any = [];
    checkedItems: any[];
    @ViewChildren('somespwd') somespwd;
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;

    constructor(service: RESTService, appComponentService: AppComponentService, protected gridHelpers: gridHelpers.CommonUtils, private authService: AuthenticationService) {
        super(service, appComponentService);
        this.formName = "Traveler Data Store";
    }  
    get pageSize(): number {
        return this.data.pageSize;
    }
    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            this.data.refresh();
            var obj = {
                name: this.gridHelpers.getGridPageName("TravelerDataStore", this.authService.CurrentUser.email),
                value: value
            };

            this.service.put(`/services/set_name_value`, obj)
                .subscribe(
                (data) => {

                },
                (error) => console.log(error)
                );

        }
    }            
    ngOnInit() {       
        this.service.get('/Configurator/get_travelerdatastore')
            .subscribe(
            (response) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data.travellerData));
                this.travelerServers = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray( response.data.travelerServers));               
                this.testTravelerServers = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data.travelerServers));
            },
            (error) => this.errorMessage = <any>error
            );
        this.service.get(`/services/get_name_value?name=${this.gridHelpers.getGridPageName("TravelerDataStore", this.authService.CurrentUser.email)}`)
            .subscribe(
            (data) => {
                this.currentPageSize = Number(data.data.value);
                this.data.pageSize = this.currentPageSize;
                this.data.refresh();
            },
            (error) => this.errorMessage = <any>error
            );
    }
    serversChecked(servers: wijmo.input.MultiSelect) {
        this.usersByserver = [];     
        for (var item of servers.checkedItems) {
            this.usersByserver.push(item.value);
        }   
    }
    saveTravelerDataStore(dlg: wijmo.input.Popup) {
        this.currentEditItem.used_by_servers = this.usersByserver;
        this.saveGridRow('/configurator/save_traveler_data_store', dlg);
    }
    delteTravelerDataStore() {
        this.deleteGridRow('/configurator/delete_traveler_data_store/');
    }
    editTravelerDataStore(dlg: wijmo.input.Popup) {
        this.editGridRow(dlg);
        this.checkedItems = [];
        var usedByServers = this.currentEditItem.used_by_servers;
        if (usedByServers) {
            for (var travelerItem of (<wijmo.collections.CollectionView>this.travelerServers).sourceCollection) {
                var server = usedByServers.filter((item) => item == travelerItem.value);
                if (server.length > 0)
                    this.checkedItems.push(travelerItem);
            }
        }     
    }
    addTravelerData(dlg: wijmo.input.Popup, servers: wijmo.input.MultiSelect) {
        this.addGridRow(dlg);
        this.currentEditItem.traveler_service_pool_name = "";
        this.currentEditItem.device_name = "";
        this.currentEditItem.data_store = "";
        this.currentEditItem.database_name = "";
        this.currentEditItem.port = "";
        this.currentEditItem.user_name = "";
        this.currentEditItem.password = "";
        this.currentEditItem.integrated_security = "";
        this.currentEditItem.test_scan_server = "";
        this.currentEditItem.used_by_servers = [];
        this.checkedItems = [];
    }

    savePwd(dialog: wijmo.input.Popup) {
        var pwd = this.somespwd.first.nativeElement.value;
        if (pwd == "") {
            this.appComponentService.showErrorMessage("You must enter a password");
        } else {
            this.currentEditItem.password = pwd;
            this.currentEditItem.is_modified = true;
            this.service.put('/configurator/save_traveler_data_store', this.currentEditItem)
                .subscribe(
                response => {
                    if (response.status == "Success") {
                        this.appComponentService.showSuccessMessage(response.message);
                    }
                    else {
                        this.appComponentService.showErrorMessage(response.message);
                    }
                    this.currentEditItem.is_modified = false;
                },
                (error) => {
                    this.errorMessage = <any>error
                    this.appComponentService.showErrorMessage(this.errorMessage);
                });
            dialog.hide();
        }
    }
}