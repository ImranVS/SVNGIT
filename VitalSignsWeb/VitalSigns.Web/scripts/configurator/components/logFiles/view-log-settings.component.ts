
import {RESTService} from '../../../core/services';
import {GridBase} from '../../../core/gridBase';
import {Router} from '@angular/router';
import {ActivatedRoute} from '@angular/router';
import {Component, OnInit, EventEmitter, Input, Output, ViewChild, AfterViewInit} from '@angular/core';
import {AppComponentService} from '../../../core/services';


@Component({
   
    templateUrl: '/app/configurator/components/logFiles/view-log-settings.component.html',
    providers: [
        RESTService
    ]
})
export class ViewLogs extends GridBase implements OnInit {
    @ViewChild('combo') combo: wijmo.input.ComboBox;
    @Output() type = new EventEmitter();
    @Input() currentDeviceType: string;
    devices: string;
    deviceTypeData: any;
    errorMessage: any;
    selectedDeviceType: any;
    logFile: any;

    id: string;
    constructor(service: RESTService, appComponentService: AppComponentService) {
        super(service,appComponentService);
        this.formName = "View Logs";

        this.service.get('/configurator/get_log_files')
            .subscribe(
            response => {

                this.deviceTypeData = response.data.combolist;

            },
            (error) => this.errorMessage = <any>error
            );

    }
    onDeviceTypeIndexChanged(event: wijmo.EventArgs) {
        
        this.currentDeviceType = this.combo.selectedItem.Text;
        this.type.emit(this.currentDeviceType);
       // alert(this.currentDeviceType)
        this.service.get('/configurator/get_read_files/' + this.selectedDeviceType)
            .subscribe(
            (response) => {
               this.logFile = response.data;
            },
            (error) => this.errorMessage = <any>error
            );
    }
    ngOnInit() {
       
        this.service.get('/configurator/get_log_files')
            .subscribe(
            (response) => {
                this.deviceTypeData = response.data.combolist;
            },
            (error) => this.errorMessage = <any>error
        );
       
    }
}



