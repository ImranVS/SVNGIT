'use strict';

//import {Component, EventEmitter, Inject, Input, ViewChild, AfterViewInit } from '@angular/core';
//import { CORE_DIRECTIVES } from '@angular/common';
//import { DataSvc } from '../../services/DataSvc';

import { Component, EventEmitter, Inject, ViewChild, Input, AfterViewInit, NgModule,OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {RESTService} from './services';
import {AppComponentService} from './services';

// Base class for all components demonstrating FlexGrid control.
@Component({
    selector: '',
    template: ''
})

export abstract class GridBase {
 
    protected service: RESTService;
    protected appComponentService: AppComponentService;
    data: wijmo.collections.CollectionView;
    currentEditItem: any;
    key: string;
    formName: string;
    formTitle: string;
    modal = true;
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    @ViewChild('flex1') flex1: wijmo.grid.FlexGrid;
    @ViewChild('mobileDeviceGrid') mobileDeviceGrid: wijmo.grid.FlexGrid;
    @ViewChild('attributeGrid') attributeGrid: wijmo.grid.FlexGrid;

    constructor(service: RESTService, appComponentService: AppComponentService) {  
        this.service = service; 
        this.appComponentService = appComponentService;
    }
    initialGridBind(dataURI: string) {
        this.service.get(dataURI)
            .subscribe(
            response => {
                if (response.status == "Success") {
                    console.log(response.data);
                    this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data));
                    this.data.pageSize = 10;
                } else {
                    this.appComponentService.showErrorMessage(response.message);
                }

            }, error => {
                var errorMessage = <any>error;
                this.appComponentService.showErrorMessage(errorMessage);
            });
               
    }
       
    get pageSize(): number {
        return this.data.pageSize;
    }
    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            if (this.flex) {
                (<wijmo.collections.IPagedCollectionView>this.flex.collectionView).pageSize = value;
            }
        }
    }
   
    saveGridRow(saveUrl: any, dlg?: wijmo.input.Popup, postdata?: any) {
        
        if (this.currentEditItem.id == "") {
           
            this.service.put(saveUrl, postdata ? postdata : this.currentEditItem)
           
                .subscribe(
                response => {
                    if (response.status == "Success") {
                        this.currentEditItem.id = response.data;
                        (<wijmo.collections.CollectionView>this.flex.collectionView).commitNew();
                        
                        dlg.hide();
                        this.appComponentService.showSuccessMessage(response.message);

                    } else {

                        this.appComponentService.showErrorMessage(response.message);
                    }

                }, error => {
                    var errorMessage = <any>error;
                    this.appComponentService.showErrorMessage(errorMessage);
                });
        }
        else {
            this.flex.collectionView.currentItem = this.currentEditItem;
            
            this.service.put(saveUrl, postdata ? postdata : this.currentEditItem)
                .subscribe(
                response => {
                    
                    if (response.status == "Success") {
                        (<wijmo.collections.CollectionView>this.flex.collectionView).commitEdit()
                        
                        dlg.hide();
                        this.appComponentService.showSuccessMessage(response.message);
                    } else {
                        this.appComponentService.showErrorMessage(response.message);
                    }

                }, error => {
                    var errorMessage = <any>error;
                    this.appComponentService.showErrorMessage(errorMessage);
                });

        }
       
    }
  
    addGridRow(dlg: wijmo.input.Popup) {
        this.formTitle = "Add " + this.formName;

        this.currentEditItem = (<wijmo.collections.CollectionView>this.flex.collectionView).addNew()
        this.currentEditItem.id = "";
        this.showDialog(dlg);
    }
    editGridRow(dlg: wijmo.input.Popup) {
        this.formTitle = "Edit " + this.formName;
       
        (<wijmo.collections.CollectionView>this.flex.collectionView).editItem(this.flex.collectionView.currentItem);
        this.currentEditItem = this.flex.collectionView.currentItem;       
        this.showDialog(dlg);
    }

    deleteGridRow(deleteUrl,confirmationMessage="Are you sure you want to delete this item?") {
        this.key = this.flex.collectionView.currentItem.id;
        if (confirm(confirmationMessage)) {
            this.service.delete(deleteUrl + this.key)
                .subscribe(
                response => {
                    if (response.status == "Success") {                       
                        this.appComponentService.showSuccessMessage(response.message);
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
    cancelEditAdd() {
        if ((<wijmo.collections.CollectionView>this.flex.collectionView).isAddingNew) {
            (<wijmo.collections.CollectionView>this.flex.collectionView).cancelNew();
        }
        else if ((<wijmo.collections.CollectionView>this.flex.collectionView).isEditingItem) {
            (<wijmo.collections.CollectionView>this.flex.collectionView).cancelEdit();
        }
    }
    showDialog(dlg: wijmo.input.Popup) {
        if (dlg) {
            dlg.modal = this.modal;
            dlg.hideTrigger = dlg.modal ? wijmo.input.PopupTrigger.None : wijmo.input.PopupTrigger.Blur;
            dlg.show();
        }
    };
    toggleColumnVisibility() {
        var flex = this.flex;
        var col = flex.columns[0];
        col.visible = !col.visible;
    };
    changeColumnSize() {
        var flex = this.flex;
        var col = flex.columns[0];
        col.visible = true;
        col.width = col.width < 0 ? 60 : -1;
        col = flex.rowHeaders.columns[0];
        col.width = col.width < 0 ? 40 : -1;
    };
    toggleRowVisibility() {
        var flex = this.flex;
        var row = flex.rows[0];
        row.visible = !row.visible;
    };
    changeRowSize() {
        var flex = this.flex;
        var row = flex.rows[0];
        row.visible = true;
        row.height = row.height < 0 ? 80 : -1;
        row = flex.columnHeaders.rows[0];
        row.height = row.height < 0 ? 80 : -1;
    };
    changeDefaultRowSize() {
        var flex = this.flex;
        flex.rows.defaultSize = flex.rows.defaultSize == 28 ? 65 : 28;
    };
    changeScrollPosition() {
        var flex = this.flex;
        if (flex.scrollPosition.y == 0) {
            var sz = flex.scrollSize;
            flex.scrollPosition = new wijmo.Point(-sz.width / 2, -sz.height / 2);
        } else {
            flex.scrollPosition = new wijmo.Point(0, 0);
        }
    };
  
}
