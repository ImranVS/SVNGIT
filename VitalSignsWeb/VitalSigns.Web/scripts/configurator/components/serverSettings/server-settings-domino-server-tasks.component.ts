import {Component, OnInit} from '@angular/core';
import { CommonModule } from '@angular/common';
import {HttpModule}    from '@angular/http';
import {RESTService} from '../../../core/services';
import {GridBase} from '../../../core/gridBase';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import {AppNavigator} from '../../../navigation/app.navigator.component';
import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';
import * as wjCoreModule from 'wijmo/wijmo.angular2.core';
import {DominoServerTasksValue} from '../../models/domino-server-tasks';


@Component({
    templateUrl: '/app/configurator/components/serverSettings/server-settings-domino-server-tasks.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class DominoServerTasks extends GridBase implements OnInit  {  
    devices: string;
    currentDeviceType:string="Domino"
   
    currentForm: FormGroup;
    constructor(service: RESTService, private formBuilder: FormBuilder) {
        super(service);
        this.currentForm = this.formBuilder.group({
            'setting': [''],
            'value': [''],
            'devices': ['']


        });
        this.formName = "DominoServerTasks";
    }  
    ngOnInit() {
        this.initialGridBind('/Configurator/get_domino_server_tasks');
    } 
    changeInDevices(devices: string) {
        this.devices = devices;
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
            var item = (<wijmo.collections.CollectionView>this.flex.collectionView.sourceCollection)[_i];
            if (item.is_selected) {
                var dominoserverObject = new DominoServerTasksValue();
                dominoserverObject.id = item.id;
                dominoserverObject.is_load = item.is_load;
                dominoserverObject.is_restart_asap = item.is_restart_asap;
                dominoserverObject.is_resart_later = item.is_resart_later;
                dominoserverObject.is_disallow = item.is_disallow;
                dominoserverObject.task_name = item.task_name;
                slectedDominoServerValues.push(dominoserverObject);
            }

        }
        var postData = {
            "setting": setting,
            "value": slectedDominoServerValues,
            "devices": this.devices
        };

        console.log(postData);
        this.currentForm.setValue(postData);
        this.service.put('/Configurator/save_domino_server_tasks', postData)
            .subscribe(
            response => {

            });
    }
}



