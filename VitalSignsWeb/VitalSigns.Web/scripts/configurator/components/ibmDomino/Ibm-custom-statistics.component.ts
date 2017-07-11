import {Component, OnInit, ViewChild, AfterViewInit} from '@angular/core';
import { CommonModule } from '@angular/common';
import {HttpModule}    from '@angular/http';
import {RESTService} from '../../../core/services';
import {GridBase} from '../../../core/gridBase';
import { AuthenticationService } from '../../../profiles/services/authentication.service';
import * as gridHelpers from '../../../core/services/helpers/gridutils';
import {AppNavigator} from '../../../navigation/app.navigator.component';
import {AppComponentService} from '../../../core/services';
import {ServersLocationService} from '../serverSettings/serverattributes-view.service';


@Component({
    templateUrl: '/app/configurator/components/ibmDomino/Ibm-custom-statistics.component.html',
    providers: [
        HttpModule,
        RESTService,
        ServersLocationService,
        gridHelpers.CommonUtils

    ]
})
export class CustomStatistics extends GridBase implements OnInit {
    sererNames: any;
    errorMessage: any;
    devices: string = "";
    deviceTypeData: any;
    currentPageSize: any = 20;
    checkedDevices: any;
    currentDeviceType: string = "Domino";

    constructor(service: RESTService, appComponentService: AppComponentService, protected gridHelpers: gridHelpers.CommonUtils, private authService: AuthenticationService) {
        super(service, appComponentService);
        this.formName = "Domino Custom Statistics";
        this.service.get('/Configurator/get_domino_servers')
            .subscribe(
            (response) => {
                this.sererNames = response.data.serversData;
            },
            (error) => this.errorMessage = <any>error
            );
    }
    changeInDevices(devices: any) {
        this.devices = devices;
        this.checkedDevices = devices;
    }
    get pageSize(): number {
        return this.data.pageSize;
    }
    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            this.data.refresh();
            var obj = {
                name: this.gridHelpers.getGridPageName("CustomStatistics", this.authService.CurrentUser.email),
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
        this.initialGridBind('/configurator/get_custom_statistics');
        this.service.get(`/services/get_name_value?name=${this.gridHelpers.getGridPageName("CustomStatistics", this.authService.CurrentUser.email)}`)
            .subscribe(
            (data) => {
                this.currentPageSize = Number(data.data.value);
                this.data.pageSize = this.currentPageSize;
                this.data.refresh();
            },
            (error) => this.errorMessage = <any>error
            );
    }


    saveCustomStatistics(dlg: wijmo.input.Popup) {
        this.currentEditItem.domino_servers = this.devices;
        this.saveGridRow('/configurator/save_custom_statistics', dlg);
    }
    delteCustomStatistics() {
        this.delteGridRow('/configurator/delete_custom_statistics/');
    }

    addCustomStat(dlg: wijmo.input.Popup) {
        this.addGridRow(dlg);
        this.currentEditItem.domino_servers = "";
        this.currentEditItem.stat_name = "";
        this.currentEditItem.yellow_threshold = "";
        this.currentEditItem.greater_than_or_less_than = "Less Than";
        this.currentEditItem.type_of_statistic = "Numeric";
        // this.currentEditItem.numeric_or_string = "Numeric"; 
        this.currentEditItem.eual_or_not_equal = "Equal";
        this.currentEditItem.times_in_a_row = 0;
        this.currentEditItem.console_command = "";
        this.checkedDevices = [];
        this.devices = "";

    }

    editCustomStat(dlg: wijmo.input.Popup) {
        this.editGridRow(dlg);
        this.checkedDevices = this.currentEditItem.domino_servers;
        this.devices = this.currentEditItem.domino_servers;
    }


}

