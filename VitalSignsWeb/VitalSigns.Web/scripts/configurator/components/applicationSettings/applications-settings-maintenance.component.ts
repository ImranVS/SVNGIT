import {Component, OnInit, ViewChild, AfterViewInit} from '@angular/core';
import { CommonModule } from '@angular/common';
import {HTTP_PROVIDERS}    from '@angular/http';
import {RESTService} from '../../../core/services';
import {GridBase} from '../../../core/gridBase';
import {ActivatedRoute} from '@angular/router';
import {WidgetComponent, WidgetService} from '../../../core/widgets';
import {AppNavigator} from '../../../navigation/app.navigator.component';
import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';
import * as wjCoreModule from 'wijmo/wijmo.angular2.core';;


@Component({
    templateUrl: '/app/configurator/components/applicationsettings/applications-settings-maintenance.component.html',
    directives: [
        wjFlexGrid.WjFlexGrid,
        wjFlexGrid.WjFlexGridColumn,
        wjFlexGrid.WjFlexGridCellTemplate,
        wjFlexGridFilter.WjFlexGridFilter,
        wjFlexGridGroup.WjGroupPanel,
        wjFlexInput.WjMenu,
        wjFlexInput.WjMenuItem,
        wjFlexInput.WjInputTime,
        AppNavigator
    ],  
    providers: [
        HTTP_PROVIDERS,
        RESTService
    ]
})
export class Maintenance extends GridBase  {  
    devices: string;
    errorMessage: string;
    dataMobileUsers: wijmo.collections.CollectionView;
    selectedSetting: any;
    selectedSettingValue: any;

    constructor(service: RESTService) {
        super(service, '/Configurator/get_maintenance');
        this.formName = "Maintenance";
      
    } 

    ngOnInit() {

        this.service.get('/Dashboard/mobile_user_devices')
            .subscribe(
            (response) => {
                this.dataMobileUsers = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data));
                this.dataMobileUsers.pageSize = 10;
            },
            (error) => this.errorMessage = <any>error
            );

      

    }
    saveMaintenance() {
        this.saveGridRow('/Configurator/save_maintenancedata');  
    }
    deleteMaintenance() {      
        this.delteGridRow('/Configurator/delete_maintenancedata/');  
    }

    changeInDevices(devices: string) {
        this.devices = devices;
    }
}



