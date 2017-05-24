import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import {HttpModule}    from '@angular/http';
import {RESTService} from '../../../core/services';

import {GridBase} from '../../../core/gridBase';
import {AppComponentService} from '../../../core/services';

@Component({
    templateUrl: '/app/configurator/components/serverImport/server-import-websphere.component.html',
    providers: [
        HttpModule,
        RESTService, AppComponentService
    ]
})
export class WebSphereServerImport extends GridBase implements OnInit {
    webSphereServerImportData: any;
    webSphereServerNodeData: any;
    dominoServer: string;
    errorMessage: any;
    selectedLocation: string;
    deviceCredentialData: any;
    postData: any;
    scanSettings: string = "Scan Settings";
    webSphereSettings: string = "WebSphere Settings";
    currentStep: string = "1";
    memory_threshold: any;
    cpu_threshold: any;
   
    websphereData: any;
    constructor(service: RESTService, appComponentService: AppComponentService) {
        super(service, appComponentService);
        this.formName = "Cell Information";

    }

    ngOnInit() {
        this.service.get('/configurator/get_websohere_import')
            .subscribe(
            (response) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data.cellData));
                if (response.data.cellData.length > 0) {
                    this.webSphereServerNodeData = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data.nodeData));
                    this.webSphereServerNodeData.groupDescriptions.push(new wijmo.collections.PropertyGroupDescription("cell_name"));
                    this.webSphereServerNodeData.groupDescriptions.push(new wijmo.collections.PropertyGroupDescription("node_name"));               
                    this.webSphereServerNodeData.pageSize = 10;                   
                }
                this.deviceCredentialData = response.data.credentialsData;
                this.websphereData = response.data.websphereData;   
            },
            (error) => this.errorMessage = <any>error
            );
    }


    saveCellInfo(dlg: wijmo.input.Popup) {
        this.saveGridRow('/configurator/save_websphere_cell', dlg);
    }
    delteCellInfo() {
        this.delteGridRow('/configurator/delete_cellInfo/');
    }
    addCellInfo(dlg: wijmo.input.Popup) {
        this.addGridRow(dlg);
        this.currentEditItem.cell_name = "";
        this.currentEditItem.name = "";
        this.currentEditItem.host_name = "";
        this.currentEditItem.connection_type = "";
        this.currentEditItem.port_no = null;
        this.currentEditItem.global_security = false;
        this.currentEditItem.credentials = "";
        this.currentEditItem.Realm = "";
    }

    editCellInfo(dlg: wijmo.input.Popup) {
        this.editGridRow(dlg);

    }

    RefreshCell() {

        this.service.put('/configurator/get_websohere_nodes', this.flex.collectionView.currentItem)
            .subscribe(
            response => {
                            
            },
            (error) => this.errorMessage = <any>error

            );
    }
    serverCheck(value, event) {

        if (event.target.checked) {
            this.flex1.collectionView.currentItem.is_selected = true;
        }
        else {          
            this.flex1.collectionView.currentItem.is_selected = false;
        }
       
    }
    step1Click(): void {
        this.websphereData.selected_servers = [];
        for (var _i = 0; _i < this.flex1.collectionView.sourceCollection.length; _i++) {
            var item = (<wijmo.collections.CollectionView>this.flex1.collectionView.sourceCollection)[_i];
            if (item.is_selected) {               

                this.websphereData.selected_servers.push({
                    "cell_id": item.cell_id,
                    "node_id": item.node_id,
                    "server_id": item.server_id,
                    "server_name": item.server_name,
                    "node_name" : item.node_name
                });
            }

        }   
        this.currentStep = "2";
    }
    step2Click(): void {
        this.service.put('/configurator/save_websphere_servers', this.websphereData)
            .subscribe(
            response => {
                this.currentStep = "3";
            },
            (error) => this.errorMessage = <any>error

            );

    }
    step3Click(): void {
        this.currentStep = "1";
        this.ngOnInit();

    }

    selectAll() {
        for (var _i = 0; _i < this.flex1.collectionView.sourceCollection.length; _i++) {
            var item = (<wijmo.collections.CollectionView>this.flex1.collectionView.sourceCollection)[_i];
            item.is_selected = true;
            this.websphereData.selected_servers.push({
                "cell_id": item.cell_id,
                "node_id": item.node_id,
                "server_id": item.server_id,
                "server_name": item.server_name
            });
        }
        this.flex1.refresh();
    }

    deselectAll() {
        for (var _i = 0; _i < this.flex1.collectionView.sourceCollection.length; _i++) {
            var item = (<wijmo.collections.CollectionView>this.flex1.collectionView.sourceCollection)[_i];
            item.is_selected = false;
            this.websphereData.selected_servers.splice(this.websphereData.selected_servers.indexOf(item.id), 1);
        }
        this.flex1.refresh();
    }

    get pageSize(): number {
        return this.data.pageSize;
    }

    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            this.data.refresh();
        }
    }

    get srvpageSize(): number {
        return this.webSphereServerNodeData.pageSize;
    }

    set srvpageSize(value: number) {
        if (this.webSphereServerNodeData.pageSize != value) {
            this.webSphereServerNodeData.pageSize = value;
            this.webSphereServerNodeData.refresh();
        }
    }
}