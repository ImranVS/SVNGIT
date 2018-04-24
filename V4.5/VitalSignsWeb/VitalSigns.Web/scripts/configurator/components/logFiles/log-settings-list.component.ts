import {Component, AfterViewChecked, OnInit, Input} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import {ActivatedRoute} from '@angular/router';
import {Router} from '@angular/router';
import {HttpModule}    from '@angular/http';
import {RESTService} from '../../../core/services';
declare var injectSVG: any;
import { DeviceAttributeValue } from '../../models/device-attribute';
import { AppComponentService } from '../../../core/services';


@Component({
   
    templateUrl: '/app/configurator/components/logFiles/log-settings-list.component.html',
    providers: [
        RESTService
    ]
})
export class Logs implements OnInit, AfterViewChecked {

  
    logsettingform: FormGroup;
    log_level: string;
    emailid: string;
    errorMessage: any;
    selectedlogs: any[];
    logNames: any[];
    //selectedLogLevel: string[];
    isselected: boolean;
    devices: string[] = [];
    postData: any;

    constructor(
        private formBuilder: FormBuilder,
        private service: RESTService,
        private router: Router,
        private route: ActivatedRoute, private appComponentService: AppComponentService) {
        this.logsettingform = this.formBuilder.group({
            'log_level': [],
            'emailid': [],
            'log_name': [],
           

        });
        //this.logsettingform = this.formBuilder.group({

        //    'setting': [''],
        //    'value': [''],
        //    'devices': ['']

        //});
    }

    ngOnInit() {


        this.route.params.subscribe(params => {
          //  this.deviceId = params['service'];
            this.loadData();
        });
    }

    loadData() {

        this.service.get('/configurator/get_log_files')
            .subscribe(
            response => {
                this.log_level = response.data.loglevel;
                this.logNames = response.data.logfilenames;
                


            },
            (error) => this.errorMessage = <any>error
            );


    }
    serverCheck(logname, event) {

        if (event.target.checked) {
            this.devices.push(logname);
            
        }
        //else {
        //    this.devices.splice(this.devices.indexOf(logName), 1);
        //  //  this.isselcted = false;
        //}
        
    }
    applySetting(nameValue: any) {       
        if (this.emailid == null)
        {
            this.appComponentService.showErrorMessage("Please enter an e-mail address");
        }
        else {
            this.postData = {
                "log_level": this.log_level,
                "emailid": this.emailid,
                "log_name": this.devices,

            };

            this.logsettingform.setValue(this.postData);
            this.service.put('/configurator/save_log_settings', this.postData)
                .subscribe(
                response => {
                    if (response.status == "Success") {
                        this.appComponentService.showSuccessMessage(response.message);
                    } else {
                        this.appComponentService.showErrorMessage(response.message);
                    }

                }, error => {
                    var errorMessage = <any>error;
                    this.appComponentService.showErrorMessage(errorMessage);
                });
        }
      
            
    }


    saveloglevel(nameValue: any) {
        console.log(this.log_level);
        if (this.log_level == null || this.log_level=="") {
            this.appComponentService.showErrorMessage("Please select Log Level.");
        }
        else {
            this.postData = {
                "log_level": this.log_level,
                "emailid": null,
                "log_name": null

            };

            this.logsettingform.setValue(this.postData);
            this.service.put('/configurator/save_log_settings', this.postData)
                .subscribe(
                response => {
                   
                    if (response.status == "Success") {
                        this.appComponentService.showSuccessMessage(response.message);
                    } else {
                        this.appComponentService.showErrorMessage(response.message);
                    }
                }, error => {
                    var errorMessage = <any>error;
                    this.appComponentService.showErrorMessage(errorMessage);
                });


        }
           

    }


    ngAfterViewChecked() {
        // injectSVG();
    }







    
    
}



