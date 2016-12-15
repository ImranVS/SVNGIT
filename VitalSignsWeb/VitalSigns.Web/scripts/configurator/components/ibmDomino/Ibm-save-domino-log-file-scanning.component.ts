﻿import {Component, OnInit} from '@angular/core';
import {RESTService} from '../../../core/services';
import {GridBase} from '../../../core/gridBase';
import {Router, ActivatedRoute} from '@angular/router';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import {AppComponentService} from '../../../core/services';

@Component({
    templateUrl: '/app/configurator/components/ibmDomino/Ibm-save-domino-log-file-scanning.component.html',
    providers: [
        RESTService
    ]
})
export class AddLogFile extends GridBase implements OnInit {
    sererNames: any;   
    errorMessage: string;
    logfiles: any;
    id: string;
    results: any;
    LogFileScan: FormGroup;
    serverLog: FormGroup;
    checkedDevices: any;
    devices: string = "";

    currentDeviceType: string = "Domino";

    constructor(service: RESTService, private route: ActivatedRoute, private router: Router, private formBuilder: FormBuilder, appComponentService: AppComponentService) {
        super(service, appComponentService);
        this.formName = "Keyword";
        this.LogFileScan = this.formBuilder.group({
            
            'log_file':['']
         

        });

        this.serverLog = this.formBuilder.group({
            'setting': [''],
            'value': [''],
            'devices': ['']


        });
      

    }
    ngOnInit() {
        this.route.params.subscribe(params => {
            this.id = params['id'];

            if (!this.id)
                this.id = "-1";
            this.loadData();
           

        });
    }

    loadData() {

        this.service.get('/configurator/get_event_log_scaning/' + this.id)
            .subscribe(
            response => {
                this.sererNames = response.data.devicename;
                this.results = response.data.result;
                this.checkedDevices = response.data.servers;
                this.devices = response.data.servers;
                //this.attributes = response.data.device_attributes;

            },
            error => this.errorMessage = <any>error
            );
    }
    saveEventLog(dlg: wijmo.input.Popup) {
        //if (this.results != null || this.results.count == 0)
       // dlg.hide();
        this.saveGridRow('/configurator/save_log_file_servers/' + this.id, dlg);

    }
    deleteEventLog() {
        
        this.delteGridRow('/configurator/delete_event_log_file_scanning/' + this.id + '/');

    }
    addlogScan(dlg: wijmo.input.Popup) {
        
        this.addGridRow(dlg);
        
            this.currentEditItem.keyword = "";
            this.currentEditItem.exclude = "";
            this.currentEditItem.one_alert_per_day = false;
            this.currentEditItem.scan_log = false;
            this.currentEditItem.scan_agent_log = false;
        

    }
    applySetting() {
      // this.saveGridRow('/configurator/save_log_file_servers/' + this.id, dlg);
        var postData = {
            "setting": this.results,
            "value": this.sererNames,
            "devices": this.devices,
        };

        this.serverLog.setValue(postData);
        this.service.put('/configurator/save_log_file_servers/' + this.id, postData)
            .subscribe(
            response => {
                if (response.status == "Success") {

                    this.appComponentService.showSuccessMessage(response.message);
                    this.router.navigateByUrl('/configurator/ibmDomino?tab=1');

                } else {

                    this.appComponentService.showErrorMessage(response.message);
                }
            });
        
    }
    CancelSettings() {

        this.router.navigateByUrl('/configurator/ibmDomino?tab=1');
    }
    changeInDevices(server: string) {
       
        this.devices = server;
    }
  

}



