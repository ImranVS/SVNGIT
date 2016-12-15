import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import {HttpModule}    from '@angular/http';
import {RESTService} from '../../../core/services';
import {ActivatedRoute} from '@angular/router';
import {Router} from '@angular/router';
import {GridBase} from '../../../core/gridBase';
import {AppComponentService} from '../../../core/services';

@Component({
    templateUrl: '/app/configurator/components/server/server-websphere-settings.component.html',
    providers: [
        HttpModule,
        RESTService, AppComponentService
    ]
})
export class WebSphereServerSettings extends GridBase implements OnInit {
    deviceId: string;
    websphereSettingsForm: FormGroup;
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
    limitsChecked: boolean;

    websphereData: any;
    constructor(service: RESTService, appComponentService: AppComponentService, private formBuilder: FormBuilder, private route: ActivatedRoute) {
        super(service, appComponentService);
        this.formName = "Cell Information";
        this.websphereSettingsForm = this.formBuilder.group({
            'id': [''],
            'cell_id': [''],
            'cell_name': [''],
            'name': [''],
            'host_name': [''],
            'connection_type': [''],
            'port_no': [''],
            'global_security': [''],
            'credentials_id': [''],
            'credentials_name': [''],
            'realm': [''],
            'user_name': [''],
            'password': [''],
            'nodes_data': ['']
        });


    }
    isLimitsChecked(ischecked: boolean) {
      
            this.limitsChecked = ischecked;
      

    }

    ngOnInit() {


        this.route.params.subscribe(params => {
            this.deviceId = params['service'];
            this.loadData();
        });
    }

    loadData() {


        //this.service.get('/Configurator/get_ibm_domino_settings')
        //    .subscribe(
        //    (data) => this.websphereSettingsForm.setValue(data.data),
        //    (error) => 
        //        this.errorMessage = <any>error
        //        this.appComponentService.showErrorMessage(this.errorMessage);
        //    }

        //    );    
        this.service.get('/configurator/get_sametime_websphere/'+ this.deviceId)
            .subscribe(
            (response) => {
                this.websphereSettingsForm.setValue(response.data.cellData);
                this.websphereSettingsForm.valueChanges.subscribe(websphereobject => {
                   
                    this.limitsChecked = websphereobject['global_security'];
                   
                });
               // this.websphereSettingsForm.setValue(response.data.cellData);
                if (response.data.cellData.length > 0) {
                    this.webSphereServerNodeData = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data.cellData[0].nodes_data));
                    this.webSphereServerNodeData.groupDescriptions.push(new wijmo.collections.PropertyGroupDescription("node_name"));
                    this.webSphereServerNodeData.pageSize = 10;
                }
                this.deviceCredentialData = response.data.credentialsData;
                this.websphereData = response.data.websphereData;
            },
            (error) => this.errorMessage = <any>error
            );
    }

    onSubmit(websphereSettingsForm: any): void {
       

        this.service.put('/Configurator/get_sametime_websphere_nodes/'+this.deviceId, websphereSettingsForm)
            .subscribe(
            response => {

                if (response.status == "Success") {

                    this.appComponentService.showSuccessMessage(response.message);

                } else {

                    this.appComponentService.showErrorMessage(response.message);
                }
            });



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

    //RefreshCell() {
    //    this.service.put('/configurator/get_websohere_nodes', this.websphereSettingsForm)
    //        .subscribe(
    //        response => {

    //        },
    //        (error) => this.errorMessage = <any>error

    //        );
    //}
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
                    "node_id": item.node_id,
                    "server_id": item.server_id,
                    "server_name": item.server_name
                });
            }
            this.step2Click();

        }
        this.currentStep = "2";
    }
    step2Click(): void {
        this.service.put('/configurator/save_websphere_servers', this.websphereData)
            .subscribe(
            response => {
                this.currentStep = "3";
                this.ngOnInit();
            },
            (error) => this.errorMessage = <any>error

            );

    }
    step3Click(): void {
        this.currentStep = "1";
        this.ngOnInit();

    }

}