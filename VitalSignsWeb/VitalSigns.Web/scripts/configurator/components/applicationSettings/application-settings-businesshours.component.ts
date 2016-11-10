import {Component, OnInit} from '@angular/core';
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
    templateUrl: '/app/configurator/components/applicationSettings/application-settings-businesshours.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class BusinessHours extends GridBase implements OnInit {  
     selectedServers:string;
    constructor(service: RESTService) {
        super(service);
        this.formName = "Business Hours";
     }  

    addBusinessHours(dlg: wijmo.input.Popup) {
        this.addGridRow(dlg);
        this.currentEditItem.name = "";
        this.currentEditItem.start_time = "";
        this.currentEditItem.duration = "";
        this.currentEditItem.sunday = "";
        this.currentEditItem.monday = "";
        this.currentEditItem.tuesday = "";
        this.currentEditItem.wednesday = "";
        this.currentEditItem.thursday = "";
        this.currentEditItem.friday = "";
        this.currentEditItem.saturday = "";

    }
    ngOnInit() {
        this.initialGridBind('/Configurator/get_business_hours');
    } 
    saveBusinessHour(dlg: wijmo.input.Popup) {        
        this.saveGridRow('/Configurator/save_business_hours',dlg);  

    }
    
    delteBusinessHour() {      
        this.delteGridRow('/Configurator/delete_business_hours/');  
    }
}



