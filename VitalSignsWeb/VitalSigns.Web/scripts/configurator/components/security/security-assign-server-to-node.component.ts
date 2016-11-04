﻿
import {Component, OnInit, ViewChild, AfterViewInit} from '@angular/core';
import { CommonModule } from '@angular/common';
import {HttpModule}    from '@angular/http';
import {WidgetComponent, WidgetService} from '../../../core/widgets';
import {RESTService} from '../../../core/services';
import {GridBase} from '../../../core/gridBase';
import {AppNavigator} from '../../../navigation/app.navigator.component';
import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';
import * as wjCoreModule from 'wijmo/wijmo.angular2.core';


@Component({
    templateUrl: '/app/configurator/components/security/security-assign-server-to-node.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class Nodes extends GridBase {
    nodeNames: any;
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    data: wijmo.collections.CollectionView;
    errorMessage: string;
    services: any;
    nodes: any;

    constructor(service: RESTService) {
        super(service);
        this.formName = "Node Health";


        this.service.get('/configurator/get_nodes_health')
            .subscribe(
            (response) => {
                this.nodeNames = response.data.result;
                this.nodes = response.data.nodesData;

            },
            (error) => this.errorMessage = <any>error
            );

    }

    refreshGrid(event: wijmo.grid.CellRangeEventArgs) {
        console.log(`/configurator/get_nodes_services?id=${event.panel.grid.selectedItems[0].Id}`);
        this.service.get(`/configurator/get_nodes_services?id=${event.panel.grid.selectedItems[0].Id}`)
            .subscribe(
            (response) => {
                this.services = response.data;

            },
            (error) => this.errorMessage = <any>error
        );

    }
    saveNodesHealth(dlg: wijmo.input.Popup) {
        this.saveGridRow('/configurator/save_nodes_health', dlg);
    }
    deleteNodesHealth() {
        this.delteGridRow('/configurator/delete_nodes_health/');
    }
  

}


