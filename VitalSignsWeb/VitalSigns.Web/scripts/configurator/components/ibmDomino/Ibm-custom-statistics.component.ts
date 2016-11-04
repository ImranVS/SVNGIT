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
export class CustomStatistics extends GridBase {
    sererNames: any;
    errorMessage: any;
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
    ngOnInit() {
        this.initialGridBind('/configurator/get_custom_statistics');
    }
    saveCustomStatistics(dlg: wijmo.input.Popup) {
        this.saveGridRow('/configurator/save_custom_statistics', dlg);
    }
    delteCustomStatistics() {
        this.delteGridRow('/configurator/delete_custom_statistics/');
    }
}



