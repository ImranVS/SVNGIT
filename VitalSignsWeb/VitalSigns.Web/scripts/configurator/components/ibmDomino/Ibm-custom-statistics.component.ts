import {Component, OnInit, ViewChild, AfterViewInit} from '@angular/core';
import { CommonModule } from '@angular/common';
import {HttpModule}    from '@angular/http';
import {RESTService} from '../../../core/services';
import {GridBase} from '../../../core/gridBase';

import {AppNavigator} from '../../../navigation/app.navigator.component';
import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';
import * as wjCoreModule from 'wijmo/wijmo.angular2.core';;


@Component({
    templateUrl: '/app/configurator/components/ibmDomino/Ibm-custom-statistics.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class CustomStatistics extends GridBase implements OnInit {
    sererNames: any;
    errorMessage: any;
    devices: string;
    deviceTypeData: any;
    checkedDevices: any;
    currentDeviceType: string = "Domino";

    constructor(service: RESTService) {
        super(service);    
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
        this.currentEditItem.yellow_threshold = 0;
        this.currentEditItem.greater_than_or_less_than = "";
        this.currentEditItem.times_in_a_row = 0;
        this.currentEditItem.console_command = "";
        this.checkedDevices = [];

    }

    editCustomStat(dlg: wijmo.input.Popup) {
        this.editGridRow(dlg);        
        this.checkedDevices = this.currentEditItem.domino_servers;
    }
}



