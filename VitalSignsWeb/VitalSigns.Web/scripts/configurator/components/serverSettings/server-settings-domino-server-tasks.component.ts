import {Component, OnInit, ViewChild, AfterViewInit} from '@angular/core';
import { CommonModule } from '@angular/common';
import {HTTP_PROVIDERS}    from '@angular/http';
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
    directives: [
        wjFlexGrid.WjFlexGrid,
        wjFlexGrid.WjFlexGridColumn,
        wjFlexGrid.WjFlexGridCellTemplate,
        wjFlexGridFilter.WjFlexGridFilter,
        wjFlexGridGroup.WjGroupPanel,
        wjFlexInput.WjMenu,
        wjFlexInput.WjMenuItem,
        AppNavigator
    ],  
    providers: [
        HTTP_PROVIDERS,
        RESTService
    ]
})
export class DominoServerTasks extends GridBase  {  
    devices: string;
   
    currentForm: FormGroup;
    constructor(service: RESTService, private formBuilder: FormBuilder) {
        super(service, '/Configurator/get_domino_server_tasks');
        this.currentForm = this.formBuilder.group({
            'setting': [''],
            'value': [''],
            'devices': ['']


        });
        this.formName = "DominoServerTasks";
    }   
    changeInDevices(devices: string) {
        this.devices = devices;
    }

    applySetting() {
      var   slectedDominoServerValues: DominoServerTasksValue[] = [];
        for (var _i = 0; _i < this.flex.collectionView.sourceCollection.length; _i++) {
            var item = (<wijmo.collections.CollectionView>this.flex.collectionView.sourceCollection)[_i];
            if (item.is_selected) {
                var dominoserverObject = new DominoServerTasksValue();
                dominoserverObject.is_load = item.is_load;
                dominoserverObject.is_restart_asap = item.is_restart_asap;
                dominoserverObject.is_resart_later = item.is_resart_later;
                dominoserverObject.is_disallow = item.is_disallow;
                dominoserverObject.task_name = item.task_name;
                slectedDominoServerValues.push(dominoserverObject);
            }

        }
        var postData = {
            "setting": "",
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



