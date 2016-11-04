﻿import {Component, OnInit} from '@angular/core';
import {RESTService} from '../../../core/services';
import {GridBase} from '../../../core/gridBase';
import {Router, ActivatedRoute} from '@angular/router';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';

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

    constructor(service: RESTService, private route: ActivatedRoute, private router: Router, private formBuilder: FormBuilder) {
        super(service);
        this.formName = "Domino Event Log Scanning";
        this.LogFileScan = this.formBuilder.group({
            
            'log_file':['']
         

        });
      
        //this.route.params.subscribe(params => {

        //    this.id = params['id'];
        //});
        //this.service.get('/configurator/get_event_log_scaning?id=' + this.id)
        //    .subscribe(
        //    (response) => {
        //        this.sererNames = response.data;

        //    },
        //    (error) => this.errorMessage = <any>error
        //    );

    }
    ngOnInit() {
        this.route.params.subscribe(params => {
            this.id = params['id'];
            this.loadData();
        });
    }

    loadData() {

        this.service.get('/configurator/get_event_log_scaning?id=' + this.id)
            .subscribe(
            response => {
                this.sererNames = response.data.devicename;
                this.results = response.data.result;
                //this.attributes = response.data.device_attributes;

            },
            error => this.errorMessage = <any>error


            );


    }
    saveEventLog(dlg: wijmo.input.Popup) {      
        this.saveGridRow('/configurator/save_log_file_scanning', dlg);
    }
    deleteEventLog() {
        this.delteGridRow('/configurator/delete_event_log_file_scanning/');
    }
  

}


