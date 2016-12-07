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
import * as wjCoreModule from 'wijmo/wijmo.angular2.core';
import {AppComponentService} from '../../../core/services';


@Component({
    templateUrl: '/app/configurator/components/applicationSettings/application-settings-businesshours.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class BusinessHours extends GridBase implements OnInit {  
    selectedServers: string;

     constructor(service: RESTService, appComponentService: AppComponentService) {
         super(service, appComponentService);
         this.formName = "Business Hours";
     }  

    addBusinessHours(dlg: wijmo.input.Popup) {
        this.addGridRow(dlg);
        this.currentEditItem.name = "";
        this.currentEditItem.start_time = "";
        this.currentEditItem.duration = "";
        this.currentEditItem.sunday = false;
        this.currentEditItem.monday = false;
        this.currentEditItem.tuesday = false;
        this.currentEditItem.wednesday = false;
        this.currentEditItem.thursday = false;
        this.currentEditItem.friday = false;
        this.currentEditItem.saturday = false;
        this.currentEditItem.use_type = "2";

    }
    ngOnInit() {
        this.initialGridBind('/Configurator/get_business_hours');
    } 
    saveBusinessHour(dlg: wijmo.input.Popup) {    
        console.log(this.currentEditItem.name);
        console.log(this.currentEditItem.start_time);
        console.log(this.currentEditItem.duration);
        console.log(this.currentEditItem.use_type);
        console.log(this.currentEditItem.sunday);

        //if (this.currentEditItem.sunday == false && this.currentEditItem.monday == false && this.currentEditItem.tuesday == false &&
        //    this.currentEditItem.wednesday == false && this.currentEditItem.thursday == false && this.currentEditItem.friday == false &&
        //    this.currentEditItem.saturday == false) {
        //    alert("Please select at least one day");
        //}
        //else {

            this.saveGridRow('/Configurator/save_business_hours', dlg);
        //}

    }
    
    delteBusinessHour() {      
        this.delteGridRow('/Configurator/delete_business_hours/');  
    }

    editBusinessHours(dlg: wijmo.input.Popup) {
        this.editGridRow(dlg);
        //console.log(this.currentEditItem.use_type);
    }

    selectAllClick(index: any) {
        this.currentEditItem.sunday = true;
        this.currentEditItem.monday = true;
        this.currentEditItem.tuesday = true;
        this.currentEditItem.wednesday = true;
        this.currentEditItem.thursday = true;
        this.currentEditItem.friday = true;
        this.currentEditItem.saturday = true;
        
    }

    deselectAllClick(index: any) {

        this.currentEditItem.sunday = false;
        this.currentEditItem.monday = false;
        this.currentEditItem.tuesday = false;
        this.currentEditItem.wednesday = false;
        this.currentEditItem.thursday = false;
        this.currentEditItem.friday = false;
        this.currentEditItem.saturday = false;
        
    }
}



