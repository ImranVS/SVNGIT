import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RESTService } from '../../../core/services';
import {GridBase} from '../../../core/gridBase';
import { AppComponentService } from '../../../core/services';
import { AuthenticationService } from '../../../profiles/services/authentication.service';
import * as gridHelpers from '../../../core/services/helpers/gridutils';

@Component({
    templateUrl: '/app/configurator/components/mobileusers/mobile-users.component.html',
    providers: [
        HttpModule,
        RESTService,
        gridHelpers.CommonUtils
    ]
})
export class MobileUser extends GridBase implements OnInit {
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    @ViewChild('mobileDeviceGrid') mobileDeviceGrid: wijmo.grid.FlexGrid; 
    sererNames: any;
    errorMessage: any;
    currentPageSize: any = 20;
    currentEditItem: any;
    mobileDeviceData: wijmo.collections.CollectionView;
    formObject: any = {
        id: null,
        threshold: null
    };

    constructor(service: RESTService, appComponentService: AppComponentService, protected gridHelpers: gridHelpers.CommonUtils, private authService: AuthenticationService) {
  
        super(service, appComponentService);
        this.formName = "Critical Device";
        
      
    }
    get pageSize(): number {
        return this.data.pageSize;
    }
    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            this.data.refresh();
            var obj = {
                name: this.gridHelpers.getGridPageName("MobileUser", this.authService.CurrentUser.email),
                value: value
            };

            this.service.put(`/services/set_name_value`, obj)
                .subscribe(
                (data) => {

                },
                (error) => console.log(error)
                );

        }
    }
    ngOnInit() {
        this.initialGridBind('/configurator/get_mobile_users');
        this.service.get('/configurator/get_all_mobile_devices')
            .subscribe(
            (response) => {
                this.mobileDeviceData = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data));
                this.mobileDeviceData.pageSize = this.currentPageSize;
            },
            (error) => this.errorMessage = <any>error
            );
        this.service.get(`/services/get_name_value?name=${this.gridHelpers.getGridPageName("MobileUser", this.authService.CurrentUser.email)}`)
            .subscribe(
            (data) => {
                this.currentPageSize = Number(data.data.value);
                this.data.pageSize = this.currentPageSize;
                this.data.refresh();
            },
            (error) => this.errorMessage = <any>error
            );
    }
    saveMobileUsers(dlg: wijmo.input.Popup) {      
        this.saveGridRow('/configurator/save_mobileusers', dlg);
    }
    saveMobileUsersMonitor(dlg: wijmo.input.Popup) {
        var saveUrl = '/configurator/save_mobileusers';
        if (this.formObject.id != "") {
            this.service.put(saveUrl, this.formObject)
                .subscribe(
                response => {
                    //assign new data sets to each grid based on the response data
                    if (response.status == "Success") {
                        this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data[0]));
                        this.mobileDeviceData = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data[1]));
                        this.data.pageSize = this.currentPageSize;
                        this.mobileDeviceData.pageSize = this.currentPageSize;
                        this.appComponentService.showSuccessMessage(response.message);
                    }
                    else {
                        this.appComponentService.showErrorMessage(response.message);
                    }
                },
                (error) => this.errorMessage = <any>error
                );
            (<wijmo.collections.CollectionView>this.mobileDeviceGrid.collectionView).commitEdit();
        }
        dlg.hide();
    }

    deleteMobileUsers() {
        var deleteUrl = '/configurator/delete_mobile_users/';
        this.key = this.flex.collectionView.currentItem.id;
        if (confirm("Are you sure want to delete this record?")) {
            this.service.delete(deleteUrl + this.key)
                .subscribe(
                response => {
                    if (response.status == "Success") {
                        this.appComponentService.showSuccessMessage(response.message);
                        this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data[0]));
                        this.mobileDeviceData = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data[1]));
                        this.data.pageSize = this.currentPageSize;
                        this.mobileDeviceData.pageSize = this.currentPageSize;
                    } else {
                        this.appComponentService.showErrorMessage(response.message);
                    }
                }, error => {
                    var errorMessage = <any>error;
                    this.appComponentService.showErrorMessage(errorMessage);
                });
            (<wijmo.collections.CollectionView>this.flex.collectionView).remove(this.flex.collectionView.currentItem);
        }
    }
    
    showEditForm(dlg: wijmo.input.Popup) {

        this.formTitle = "Edit " + this.formName;

        this.formObject.id = this.mobileDeviceGrid.collectionView.currentItem.id;
        this.formObject.treshold = 20;
        
        (<wijmo.collections.CollectionView>this.mobileDeviceGrid.collectionView).editItem(this.mobileDeviceGrid.collectionView.currentItem);
        this.showDialog(dlg);

    }
    get MobileDevicepageSize(): number {
        return this.mobileDeviceData.pageSize;
    }
    set MobileDevicepageSize(value: number) {

        if (this.mobileDeviceGrid)
        {
            if (this.mobileDeviceData.pageSize != value) {
                this.mobileDeviceData.pageSize = value;
                if (this.mobileDeviceGrid) {
                    (<wijmo.collections.IPagedCollectionView>this.mobileDeviceGrid.collectionView).pageSize = value;
                }
            }
        }
       
    }
 
}



