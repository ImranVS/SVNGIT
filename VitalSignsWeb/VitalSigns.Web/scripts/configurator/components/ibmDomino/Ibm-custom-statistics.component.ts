import {Component, OnInit, ViewChild, AfterViewInit} from '@angular/core';
import { CommonModule } from '@angular/common';
import {HttpModule}    from '@angular/http';
import {RESTService} from '../../../core/services';
import {GridBase} from '../../../core/gridBase';

import {AppNavigator} from '../../../navigation/app.navigator.component';
import {AppComponentService} from '../../../core/services';
import {ServersLocationService} from '../serverSettings/serverattributes-view.service';


@Component({
    templateUrl: '/app/configurator/components/ibmDomino/Ibm-custom-statistics.component.html',
    providers: [
        HttpModule,
        RESTService,
        ServersLocationService
    ]
})
export class CustomStatistics extends GridBase implements OnInit {
    sererNames: any;
    errorMessage: any;
    devices: string="";
    deviceTypeData: any;
    checkedDevices: any;
    currentDeviceType: string = "Domino";

    constructor(service: RESTService, appComponentService: AppComponentService) {
        super(service, appComponentService);    
        this.formName = "Domino Custom Statistics";
        this.service.get('/Configurator/get_domino_servers')
            .subscribe(
            (response) => {
                this.sererNames = response.data.serversData;
            },
            (error) => this.errorMessage = <any>error
            );
    }
    changeInDevices(devices: string) {
        this.devices = devices;

    }

    ngOnInit() {
        this.initialGridBind('/configurator/get_custom_statistics');
    }
    saveCustomStatistics(dlg: wijmo.input.Popup) {
        this.currentEditItem.domino_servers = this.devices;
        this.saveGridRow('/configurator/save_custom_statistics', dlg);
    }
    delteCustomStatistics() {
        this.delteGridRow('/configurator/delete_custom_statistics/');
    }
    addCustomStat(dlg: wijmo.input.Popup) {
        this.addGridRow(dlg);
        this.currentEditItem.domino_servers = "";
        this.currentEditItem.stat_name = "";
        this.currentEditItem.yellow_threshold = "";
        this.currentEditItem.greater_than_or_less_than = "Less Than";
        this.currentEditItem.type_of_statistic = "Numeric";
        // this.currentEditItem.numeric_or_string = "Numeric"; 
        this.currentEditItem.eual_or_not_equal = "Equal";
        this.currentEditItem.times_in_a_row = 0;
        this.currentEditItem.console_command = "";
        this.checkedDevices = [];
        this.devices = "";

    }

    editCustomStat(dlg: wijmo.input.Popup) {
        this.editGridRow(dlg);        
        this.checkedDevices = this.currentEditItem.domino_servers;
        this.devices = this.currentEditItem.domino_servers;
    }
}



