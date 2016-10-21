import {Component, OnInit, ViewChild, AfterViewInit, Input} from '@angular/core';
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
    templateUrl: '/app/configurator/components/server/server-domino-server-tasks.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class ServerTasks extends GridBase {
    selectedServers: string;
    constructor(service: RESTService) {
        super(service, '/Configurator/get_business_hours');
        
    }

    ngOnInit() {
        alert("hiiiiiiiiiiiii");
    }
    //saveBusinessHour() {
    //    this.saveGridRow('/Configurator/save_business_hours');

    //}

    //delteBusinessHour() {
    //    this.delteGridRow('/Configurator/delete_business_hours/');
    //}
}



