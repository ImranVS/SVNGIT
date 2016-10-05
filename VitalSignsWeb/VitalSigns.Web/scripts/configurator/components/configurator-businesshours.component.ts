import {Component, OnInit, ViewChild, AfterViewInit} from '@angular/core';
import { CommonModule } from '@angular/common';
import {HTTP_PROVIDERS}    from '@angular/http';
import {RESTService} from '../../core/services';
import {GridBase} from '../../core/gridBase';

import {AppNavigator} from '../../navigation/app.navigator.component';
import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';
import * as wjCoreModule from 'wijmo/wijmo.angular2.core';;


@Component({
    templateUrl: '/app/configurator/components/configurator-businesshours.component.html',
    directives: [
        wjFlexGrid.WjFlexGrid,
        wjFlexGrid.WjFlexGridColumn,
        wjFlexGrid.WjFlexGridCellTemplate,
        wjFlexGridFilter.WjFlexGridFilter,
        wjFlexGridGroup.WjGroupPanel,
        wjFlexInput.WjMenu,
        wjFlexInput.WjMenuItem,
        AppNavigator
    ],  
    providers: [
        HTTP_PROVIDERS,
        RESTService
    ]
})
export class BusinessHours extends GridBase  {  
    
    constructor(service: RESTService) {
        super(service, '/Configurator/business_hours');
        this.formName = "Business Hours";
    }   
    saveBusinessHour() {
        this.saveGridRow('/Configurator/save_business_hours');  
    }
    delteBusinessHour() {      
        this.delteGridRow('/Configurator/delete_business_hours/');  
    }
}



