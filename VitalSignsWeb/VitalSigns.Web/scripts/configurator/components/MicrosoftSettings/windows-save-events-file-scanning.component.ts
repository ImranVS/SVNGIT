import {Component, OnInit,ViewChild} from '@angular/core';
import {RESTService} from '../../../core/services';
import {GridBase} from '../../../core/gridBase';
import {Router, ActivatedRoute} from '@angular/router';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import {AppComponentService} from '../../../core/services';
import { ServersLocationService } from '../serverSettings/serverattributes-view.service';
import { AuthenticationService } from '../../../profiles/services/authentication.service';
import * as gridHelpers from '../../../core/services/helpers/gridutils';


@Component({
    templateUrl: '/app/configurator/components/MicrosoftSettings/windows-save-events-file-scanning.component.html',
    providers: [
        RESTService,
        ServersLocationService,
        gridHelpers.CommonUtils
    ]
})
export class AddEventFile extends GridBase implements OnInit {
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    EventsName: any;   
    errorMessage: string;
    id: string;
    results: any[];
    currentPageSize: any = 20;
    EventDefentions: FormGroup;
    serverLog: FormGroup;
    checkedDevices: any;
    devices: string = "";
    lastbuttonclick: string;
    currentDeviceTypes: any =  "Exchange,Skype For Business,Windows,Active Directory";
    constructor(service: RESTService, private route: ActivatedRoute, private router: Router, private formBuilder: FormBuilder, appComponentService: AppComponentService
        , protected gridHelpers: gridHelpers.CommonUtils, private authService: AuthenticationService) {
        super(service, appComponentService);
        this.formName = "Events";
       
        this.EventDefentions = this.formBuilder.group({
          'events': ['']
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
                name: this.gridHelpers.getGridPageName("AddEventFile", this.authService.CurrentUser.email),
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
        this.service.get(`/services/get_name_value?name=${this.gridHelpers.getGridPageName("AddEventFile", this.authService.CurrentUser.email)}`)
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
        this.service.get('/configurator/get_windows_event_scaning/' + this.id)
            .subscribe(
            response => {
                this.EventsName = response.data.devicename;
                this.results = response.data.result;
               this.checkedDevices = response.data.servers;
                this.devices = response.data.servers;
            },
            error => this.errorMessage = <any>error
            );
    }
    saveEventLog(dlg: wijmo.input.Popup) {
        
        if (this.lastbuttonclick === "Add") {
            (<wijmo.collections.CollectionView>this.flex.collectionView).commitNew(); 
        }
        else if (this.lastbuttonclick === "edit")
        {
            (<wijmo.collections.CollectionView>this.flex.collectionView).commitEdit(); 
        }
        dlg.hide();
    }
    editeventlog(dlg: wijmo.input.Popup) {
        this.lastbuttonclick = "edit";
        this.editGridRow(dlg);
    }
    deleteWindowsEvent() {
        var selectedrow = this.flex.collectionView.currentItem;
        var obj = this.results.find(x => x.alias_name == selectedrow.alias_name);
        var index = this.results.indexOf(obj); 
        (<wijmo.collections.CollectionView>this.flex.collectionView).remove(this.flex.collectionView.currentItem);
    }
    addWindowsEvent(dlg: wijmo.input.Popup) {
        this.lastbuttonclick = "Add";
        this.addGridRow(dlg);
        this.currentEditItem.alias_name = "";
        this.currentEditItem.message = "";
        this.currentEditItem.event_id = "";
        this.currentEditItem.source = "";
        this.currentEditItem.event_name = "";
        this.currentEditItem.event_level = "";
    }
    applySetting() {
        this.checkedDevices = this.devices;
        this.devices = this.devices;
        var postData = {
            "setting": this.results,
            "value": this.EventsName,
            "devices": this.devices,
        };
        
        this.serverLog.setValue(postData);
        this.service.put('/configurator/save_windows_event_servers/' + this.id, postData)
            .subscribe(
            response => {
                if (response.status == "Success") {

                    this.appComponentService.showSuccessMessage(response.message);
                    this.router.navigateByUrl('/configurator/windowslog');

                } else {

                    this.appComponentService.showErrorMessage(response.message);
                }
            });
        
    }
    CancelSettings() {

        this.router.navigateByUrl('/configurator/windowslog');
    }
    changeInDevices(devices: any) {
        this.devices = devices;
        this.checkedDevices = devices;
    }
  

}



