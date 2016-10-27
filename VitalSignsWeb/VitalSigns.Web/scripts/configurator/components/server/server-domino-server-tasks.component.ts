import {Component, OnInit, ViewChild, AfterViewInit, Input} from '@angular/core';
import { CommonModule } from '@angular/common';
import {HttpModule}    from '@angular/http';
import {RESTService} from '../../../core/services';
import {GridBase} from '../../../core/gridBase';

import {AppNavigator} from '../../../navigation/app.navigator.component';
import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';
import * as wjCoreModule from 'wijmo/wijmo.angular2.core';
import {ActivatedRoute} from '@angular/router';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';


@Component({
    templateUrl: '/app/configurator/components/server/server-domino-server-tasks.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class ServerTasks extends GridBase {
    selectedServers: string;
    deviceId: any;
    data: wijmo.collections.CollectionView;
    TaskNames: any;
    errorMessage: string;
    currentForm: FormGroup;
    selectedSettingValue: any;
    selectedName: string;

    constructor(service: RESTService, private formBuilder: FormBuilder, private route: ActivatedRoute) {
        super(service, '/Configurator/get_server_tasks_info/57ace45abf46711cd4681e15');

        this.currentForm = this.formBuilder.group({
            'setting': [''],
            'value': ['']
           


        });

        this.service.get('/Configurator/get_tasks_names')
            .subscribe(
            (response) => {
                this.TaskNames = response.data.TaskNames;

            },
            (error) => this.errorMessage = <any>error
            );
       
        
    }

    ngOnInit() {
       
    }
    addServerTask(frmDialog: wijmo.input.Popup) {
        this.addGridRow1(frmDialog);
        this.currentEditItem.device_id = "57ace45abf46711cd4681e15";

    }
    saveServerTasks(dlg: wijmo.input.Popup) {
        //this.buildPostData("add", dlg);
        this.selectedSettingValue = this.selectedName;
        alert(this.selectedSettingValue);

        this.saveGridRow1('/configurator/save_server_tasks', dlg);
    }

    buildPostData(setting: string, dlg) {
        var postData = {
            "setting": setting,
          
        };

        console.log(postData);
        this.currentForm.setValue(postData);
        //this.saveGridRow1('/configurator/save_server_tasks', postData, dlg)
        this.service.put('/Configurator/save_domino_server_tasks', postData)
            .subscribe(
            response => {

            });

    }



   
    delteServerTasks() {
        this.delteGridRow('/Configurator/delete_server_tasks/57ace45abf46711cd4681e15/');
    }
}



