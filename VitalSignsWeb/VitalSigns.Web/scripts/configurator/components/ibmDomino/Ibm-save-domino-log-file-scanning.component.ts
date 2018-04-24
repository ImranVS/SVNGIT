import {Component, OnInit} from '@angular/core';
import {RESTService} from '../../../core/services';
import {GridBase} from '../../../core/gridBase';
import {Router, ActivatedRoute} from '@angular/router';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import {AppComponentService} from '../../../core/services';
import { ServersLocationService } from '../serverSettings/serverattributes-view.service';
import { AuthenticationService } from '../../../profiles/services/authentication.service';
import * as gridHelpers from '../../../core/services/helpers/gridutils';


@Component({
    templateUrl: '/app/configurator/components/ibmDomino/Ibm-save-domino-log-file-scanning.component.html',
    providers: [
        RESTService,
        ServersLocationService,
        gridHelpers.CommonUtils
    ]
})
export class AddLogFile extends GridBase implements OnInit {
    sererNames: any;   
    errorMessage: string;
    logfiles: any;
    id: string;
    results: any;
    currentPageSize: any = 20;
    LogFileScan: FormGroup;
    serverLog: FormGroup;
    checkedDevices: any;
    devices: string = "";

    currentDeviceType: string = "Domino";

    constructor(service: RESTService, private route: ActivatedRoute, private router: Router, private formBuilder: FormBuilder, appComponentService: AppComponentService
        , protected gridHelpers: gridHelpers.CommonUtils, private authService: AuthenticationService) {
        super(service, appComponentService);
        this.formName = "Keyword";
        this.LogFileScan = this.formBuilder.group({
            
            'log_file': ['']
           
         

        });

        this.serverLog = this.formBuilder.group({
            
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
                name: this.gridHelpers.getGridPageName("AddLogFile", this.authService.CurrentUser.email),
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
        this.route.params.subscribe(params => {
            this.id = params['id'];

            if (!this.id)
                this.id = "-1";
            this.loadData();
           

        });
        this.service.get(`/services/get_name_value?name=${this.gridHelpers.getGridPageName("AddLogFile", this.authService.CurrentUser.email)}`)
            .subscribe(
            (data) => {
                this.currentPageSize = Number(data.data.value);
                this.data.pageSize = this.currentPageSize;
                this.data.refresh();
            },
            (error) => this.errorMessage = <any>error
            );
    }

    loadData() {

        this.service.get('/configurator/get_event_log_scaning/' + this.id)
            .subscribe(
            response => {
                this.sererNames = response.data.devicename;
                this.results = response.data.result;
               this.checkedDevices = response.data.servers;
                this.devices = response.data.servers;

            },
            error => this.errorMessage = <any>error
            );
    }
    saveEventLog(dlg: wijmo.input.Popup) {
      
        if (this.flex.collectionView) {

            
            var valueArr = this.flex.collectionView.items.map(function (item) { return item.keyword });
            console.log(valueArr)
            var isDuplicate = valueArr.some(function (item, idx) {
                return valueArr.indexOf(item) != idx
            });
            if (isDuplicate) {
                this.appComponentService.showErrorMessage("Domino event keyword already exists.");
                return;   
            } else {
                this.saveGridRow('/configurator/save_log_file_servers/' + this.id, dlg);
            } 
        }

    }
    deleteEventLog() {
        
        this.deleteGridRow('/configurator/delete_event_log_file_scanning/' + this.id + '/');

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
        this.checkedDevices = this.devices;
        this.devices = this.devices;
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
    changeInDevices(devices: any) {
        this.devices = devices;
        this.checkedDevices = devices;
    }
  

}



