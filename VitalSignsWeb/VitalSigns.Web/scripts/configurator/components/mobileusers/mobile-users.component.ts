import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RESTService } from '../../../core/services';
import {GridBase} from '../../../core/gridBase';
import {AppComponentService} from '../../../core/services';

@Component({
    templateUrl: '/app/configurator/components/mobileusers/mobile-users.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class MobileUser extends GridBase implements OnInit {
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    @ViewChild('mobileDeviceGrid') mobileDeviceGrid: wijmo.grid.FlexGrid; 
    sererNames: any;
    errorMessage: any;
    currentEditItem: any;
    mobileDeviceData: wijmo.collections.CollectionView;
    formObject: any = {
        id: null,
        threshold: null
    };

    constructor(service: RESTService, appComponentService: AppComponentService) {
  
        super(service, appComponentService);
        this.formName = "Critical Device";
        
      
    }
    ngOnInit() {
        this.initialGridBind('/configurator/get_mobile_users');
        this.service.get('/configurator/get_all_mobile_devices')
            .subscribe(
            (response) => {
                this.mobileDeviceData = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data));
                this.mobileDeviceData.pageSize = 10;
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
                        this.data.pageSize = 10;
                        this.mobileDeviceData.pageSize = 10;
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
                        this.data.pageSize = 10;
                        this.mobileDeviceData.pageSize = 10;
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



