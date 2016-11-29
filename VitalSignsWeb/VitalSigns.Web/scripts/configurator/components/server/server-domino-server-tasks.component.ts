import {Component, OnInit, ViewChild,Input} from '@angular/core';
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
export class ServerTasks extends GridBase implements OnInit {
    selectedServers: string;
    deviceId: any;
    data: wijmo.collections.CollectionView;
    TaskNames: any;
    errorMessage: string;
    currentForm: FormGroup;
    selectedSettingValue: any;
    selectedName: string;

    constructor(service: RESTService, private formBuilder: FormBuilder, private route: ActivatedRoute) {      
        super(service);
        this.formName = "Server Task";
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
        this.route.params.subscribe(params => {
            this.deviceId = params['service'];
            //this.loadData();
        });

        this.initialGridBind('/Configurator/get_server_tasks_info/'+ this.deviceId );
    }
    addServerTask(frmDialog: wijmo.input.Popup) {
        this.addGridRow(frmDialog);
        this.currentEditItem.is_selected = "";
        this.currentEditItem.task_name = "";
        this.currentEditItem.is_load = false;
        this.currentEditItem.is_restart_asap = false;
        this.currentEditItem.is_resart_later = false;
        this.currentEditItem.is_disallow = false;
        this.currentEditItem.device_id = this.deviceId;

    }
    saveServerTasks(dlg: wijmo.input.Popup) {
        //this.buildPostData("add", dlg);
        this.selectedSettingValue = this.selectedName;
       
       // alert(this.selectedSettingValue);

        //this.currentEditItem.task_name = Text;
        this.saveGridRow('/configurator/save_server_tasks', dlg);
        
    }



    buildPostData(setting: string, dlg) {
        var postData = {
            "setting": setting,
          
        };

        console.log(postData);
        this.currentForm.setValue(postData);
       
        //this.saveGridRow('/configurator/save_server_tasks', postData, dlg)
        this.service.put('/Configurator/save_domino_server_tasks', postData)
            .subscribe(
            response => {

            });

    }



   
    delteServerTasks() {
        this.delteGridRow('/Configurator/delete_server_tasks/'+ this.deviceId +'/');
    }
}



