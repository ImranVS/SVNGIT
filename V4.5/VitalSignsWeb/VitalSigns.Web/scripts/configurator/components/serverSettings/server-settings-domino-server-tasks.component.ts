import {Component, OnInit,ViewChild} from '@angular/core';
import { CommonModule } from '@angular/common';
import {HttpModule}    from '@angular/http';
import {RESTService} from '../../../core/services';
import {GridBase} from '../../../core/gridBase';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import { AppNavigator } from '../../../navigation/app.navigator.component';
import { AuthenticationService } from '../../../profiles/services/authentication.service';
import * as gridHelpers from '../../../core/services/helpers/gridutils';
import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';
import * as wjCoreModule from 'wijmo/wijmo.angular2.core';
import {DominoServerTasksValue} from '../../models/domino-server-tasks';
import {AppComponentService} from '../../../core/services';

import {ServersLocationService} from './serverattributes-view.service';



@Component({
    templateUrl: '/app/configurator/components/serverSettings/server-settings-domino-server-tasks.component.html',
    providers: [
        HttpModule,
        RESTService, ServersLocationService, gridHelpers.CommonUtils
    ]
})
export class DominoServerTasks implements OnInit  {  
    protected service: RESTService;
    protected appComponentService: AppComponentService;
    checkedDevices: any;
    errorMessage: string;
    currentPageSize: any = 20;
    devices: string = "";
    attributes: string[] = [];
    currentDeviceType: string = "Domino";
    selectedDeviceType: string = "Domino";
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    data: wijmo.collections.CollectionView;
    currentForm: FormGroup;
    constructor(service: RESTService, private formBuilder: FormBuilder, appComponentService: AppComponentService, protected gridHelpers: gridHelpers.CommonUtils, private authService: AuthenticationService
) {  
        this.service = service; 
        this.appComponentService = appComponentService;     
        this.currentForm = this.formBuilder.group({
            'setting': [''],
            'value': [''],
            'devices': ['']
        });
    } 
    get pageSize(): number {
        return this.data.pageSize;
    }
    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            this.data.refresh();
            var obj = {
                name: this.gridHelpers.getGridPageName("DominoServerTasks", this.authService.CurrentUser.email),
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
    selectionChangedHandler = () => {
        console.log(this.flex.collectionView.currentItem);
        (<wijmo.collections.CollectionView>this.flex.collectionView).commitEdit()
       
    }

    ngOnInit() {
        this.service.get('/Configurator/get_domino_server_tasks')
            .subscribe(
            response => {
                if (response.status == "Success") {
                    this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data));
                    this.data.pageSize = this.currentPageSize;
                } else {
                    this.appComponentService.showErrorMessage(response.message);
                }

            }, error => {
                var errorMessage = <any>error;
                this.appComponentService.showErrorMessage(errorMessage);
            });
        this.service.get(`/services/get_name_value?name=${this.gridHelpers.getGridPageName("DominoServerTasks", this.authService.CurrentUser.email)}`)
            .subscribe(
            (data) => {
                this.currentPageSize = Number(data.data.value);
                this.data.pageSize = this.currentPageSize;
                this.data.refresh();
            },
            (error) => this.errorMessage = <any>error
            );
    } 
    changeInDevices(devices: any) {
        this.devices = devices;
        this.checkedDevices = devices;
    }

    applySetting() {
        this.buildPostData("add");

    }

    removeSetting() {
      
        this.buildPostData("remove");
       
    }

    buildPostData(setting: string) {
        var slectedDominoServerValues: DominoServerTasksValue[] = [];
        for (var _i = 0; _i < this.flex.collectionView.sourceCollection.length; _i++) {
          // console.log(this.flex.collectionView.sourceCollection.length);
            var item = (<wijmo.collections.CollectionView>this.flex.collectionView.sourceCollection)[_i];
            console.log(item);
            if (item.is_selected) {
                var dominoserverObject = new DominoServerTasksValue();
                dominoserverObject.id = item.id;
                dominoserverObject.is_load = item.is_load;
                dominoserverObject.is_restart_asap = item.is_restart_asap;
                dominoserverObject.is_resart_later = item.is_resart_later;
                dominoserverObject.is_disallow = item.is_disallow;
                dominoserverObject.task_name = item.task_name;
                slectedDominoServerValues.push(dominoserverObject);
                console.log(dominoserverObject);
            }

        }
        var postData = {
            "setting": setting,
            "value": slectedDominoServerValues,
            "devices": this.devices
        };
        this.currentForm.setValue(postData);
        this.service.put('/Configurator/save_domino_server_tasks', postData)
            .subscribe(
            response => {

                if (response.status == "Success") {

                    this.appComponentService.showSuccessMessage(response.message);

                } else {

                    this.appComponentService.showErrorMessage(response.message);
                }
            });

    }

    serverCheck(value, event) {

        if (event.target.checked) {
            // this.attributes.push(value);
            this.flex.collectionView.currentItem.is_selected = true;
        }
        else {
            // this.attributes.splice(this.devices.indexOf(value), 1);
            this.flex.collectionView.currentItem.is_selected = false;
        }
    }
}



