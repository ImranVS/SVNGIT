import {Component, OnInit, ViewChild, AfterViewInit} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, REACTIVE_FORM_DIRECTIVES, REACTIVE_FORM_PROVIDERS, Validators} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import {HTTP_PROVIDERS}    from '@angular/http';
import {RESTService} from '../../core/services';
import {AppNavigator} from '../../navigation/app.navigator.component';
import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';
import * as wjCoreModule from 'wijmo/wijmo.angular2.core';;


@Component({
    templateUrl: '/app/configurator/components/configurator-servercredentials.component.html',
    directives: [REACTIVE_FORM_DIRECTIVES,
        wjFlexGrid.WjFlexGrid,
        wjFlexGrid.WjFlexGridColumn,
        wjFlexGrid.WjFlexGridCellTemplate,
        wjFlexGridFilter.WjFlexGridFilter,
        wjFlexGridGroup.WjGroupPanel,
        wjFlexInput.WjMenu,
        wjFlexInput.WjMenuItem,
        wjFlexInput.WjComboBox,
        AppNavigator
    ],
    viewProviders: [REACTIVE_FORM_PROVIDERS],
    providers: [
        HTTP_PROVIDERS,
        RESTService
    ]
})
export class ServerCredentials implements OnInit, AfterViewInit {
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    data: wijmo.collections.CollectionView;
    errorMessage: string;
    selectedDeviceType: string;
    //Columns in grid
    serverCredentialForm: FormGroup;
    ServerCredentialId: string;
    deviceTypes: any;
    get pageSize(): number {
        return this.data.pageSize;
    }

    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            this.data.refresh();
        }
    }
    constructor(private service: RESTService, private formBuilder: FormBuilder) {

        this.serverCredentialForm = this.formBuilder.group({
            'id': ['', Validators.required],

            'alias': ['', Validators.required],
            'user_id': ['', Validators.required],
            'password': [''],
          
            'device_type': ['']
        });
       
    }
    ngOnInit() {
        this.bindGrid();
        this.service.get('/services/server_list_selectlist_data')
            .subscribe(
            (response) => {
                this.deviceTypes = response.data.deviceTypeData;
                //delete this.deviceTypes[0];
               
                    this.deviceTypes.splice(0, 1);
               
              
            },
            (error) => this.errorMessage = <any>error
            );

    }
    bindGrid() {
        this.service.get('/Configurator/credentials')
            .subscribe(
            response => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data));
                this.data.pageSize = 10;

            },
            error => this.errorMessage = <any>error
            );

    }
    saveBusinessHours() {


    }

    ngAfterViewInit() {
        var self = this;
        this._updateView();

    }
    itemsSourceChangedHandler() {
        var flex = this.flex;
        if (!flex) {
            return;
        }

        // make columns 25% wider (for readability and to show how)
        for (var i = 0; i < flex.columns.length; i++) {
            flex.columns[i].width = flex.columns[i].renderSize * 1.25;
        }


        // set page size on the grid's internal collectionView
        if (flex.collectionView && this.pageSize) {
            (<wijmo.collections.IPagedCollectionView>flex.collectionView).pageSize = this.pageSize;
        }
    };



    private _updateView() {

        console.log(this.flex);
        if (!this.flex.collectionView) {
            return;
        }

        this.serverCredentialForm.setValue(this.flex.collectionView.currentItem);
        this.selectedDeviceType = this.flex.collectionView.currentItem.device_type;
        //alert(this.selectedDeviceType);
        console.log(this.flex.collectionView);

    }

    onSubmit(serverCredential: any): void {
        console.log(serverCredential);

        this.service.put(
            '/Configurator/save_server_credentials',
            serverCredential);
        this.bindGrid();
        this.flex.refresh();
    }
    addServerCredential() {
        this.serverCredentialForm.setValue(null);
    }
    editServerCredential() {
        this._updateView();
    }

    delteServerCredential() {
        this.ServerCredentialId = this.flex.collectionView.currentItem.id;
        
        if (confirm("Are you sure want to delete record")) {
            this.service.delete('/Configurator/delete_credential/' + this.ServerCredentialId);
        }
        this.bindGrid();
        this.flex.refresh();
    }
}



