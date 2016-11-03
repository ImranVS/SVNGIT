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
    templateUrl: '/app/configurator/components/ibmDomino/Ibm-servertask-definition.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class ServerTaskDefinition extends GridBase {
    constructor(service: RESTService) {
        super(service);
        this.formName = "Domino Server Task Definition";

    }
    ngOnInit() {
        this.initialGridBind('/configurator/get_server_task_definiton');
    }
    saveServerTaskDefinition(dlg: wijmo.input.Popup) {
        this.saveGridRow('/configurator/save_server_task_definition', dlg);
    }
    delteServerTaskDefinition() {
        this.delteGridRow('/configurator/delete_server_task_definition/');
    }
}



