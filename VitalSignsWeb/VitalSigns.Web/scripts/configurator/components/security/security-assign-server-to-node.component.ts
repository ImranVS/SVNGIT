import { Component, OnInit, EventEmitter, Input, Output, ViewChild, AfterViewInit} from '@angular/core';
import { CommonModule } from '@angular/common';
import {HttpModule}    from '@angular/http';
import {WidgetComponent} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import {RESTService} from '../../../core/services';
import {GridBase} from '../../../core/gridBase';
import {AppComponentService} from '../../../core/services';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import { ServersLocationService } from '../serverSettings/serverattributes-view.service';
import { ServersLocation } from '../server-list-location.component';
import * as helpers from '../../../core/services/helpers/helpers';


@Component({
    templateUrl: '/app/configurator/components/security/security-assign-server-to-node.component.html',
    providers: [
        HttpModule,
        RESTService,
        ServersLocationService,
        ServersLocation,
        helpers.DateTimeHelper
    ]
})
export class Nodes extends GridBase {
    nodeNames: any;
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    data: wijmo.collections.CollectionView;
    errorMessage: string;
    services: any;
    nodes: any;
    servernodes: any;
    nodesHealth: FormGroup;
    devices: string;
    selectedNode: string = null;
    visibility: boolean = true;
    isVisible: boolean = false;
    firstrowid: string;
    id: string;
    locations: string;
    checkedDevices: any;

    constructor(service: RESTService, private formBuilder: FormBuilder, appComponentService: AppComponentService, protected datetimeHelpers: helpers.DateTimeHelper) {
        super(service, appComponentService);
        this.formName = "Nodes";

        this.nodesHealth = this.formBuilder.group({
            'setting': [''],
            'value': [''],
            'devices': ['']


        });

        this.service.get('/configurator/get_nodes_health')
            .subscribe(
            (response) => {
                this.nodeNames = this.datetimeHelpers.toLocalDateTime(response.data.result);
                this.nodes = response.data.nodesData; 
                this.locations = response.data.locations;             
                this.firstrowid = response.data.result[0].Id;
                this.service.get(`/configurator/get_nodes_services?id=`+this.firstrowid)
                    .subscribe(
                    (response) => {
                        this.services = response.data;
                    },
                    (error) => this.errorMessage = <any>error
                    );
                console.log(this.firstrowid);
            },
            (error) => this.errorMessage = <any>error
            );
      

    }


   
    refreshGrid(event: wijmo.grid.CellRangeEventArgs) {
        this.id = event.panel.grid.selectedItems[0].Id;
        this.service.get(`/configurator/get_nodes_services?id=`+ this.id)
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
  
    applySetting() {
        var devices = [''];
        var postData;
        if (this.devices != null) {
            postData = {
                "setting": this.selectedNode,
                "value": [''],
                "devices": this.devices
            };
        }
        else {
            postData = {
                "setting": this.selectedNode,
                "value": [''],
                "devices": devices
            };
        }
        this.nodesHealth.setValue(postData);
        this.service.put('/configurator/save_nodes_servers', postData)
            .subscribe(
            response => {
                if (response.status == "Success") {

                    this.appComponentService.showSuccessMessage(response.message);
                   

                } else {

                    this.appComponentService.showErrorMessage(response.message);
                }
            });
    }
    changeInDevices(devices: any) {
        this.devices = devices;
        this.checkedDevices = devices;
    }

    onItemsSourceChanged() {
        var row = this.flex.columnHeaders.rows[0];
        row.wordWrap = true;
        // autosize first header row
        this.flex.autoSizeRow(0, true);

    }
}



