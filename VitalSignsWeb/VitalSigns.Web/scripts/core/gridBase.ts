'use strict';

//import {Component, EventEmitter, Inject, Input, ViewChild, AfterViewInit } from '@angular/core';
//import { CORE_DIRECTIVES } from '@angular/common';
//import { DataSvc } from '../../services/DataSvc';

import { Component, EventEmitter, Inject, ViewChild, Input, AfterViewInit, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {RESTService} from './services';

// Base class for all components demonstrating FlexGrid control.
@Component({
    selector: '',
    templateUrl: ''
})

export abstract class GridBase  {
 
    protected service: RESTService;
    data: wijmo.collections.CollectionView;
    currentEditItem: any;
    key: string;
    formName: string;
    formTitle: string;
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    constructor(service: RESTService, dataURI: string) {   
        this.service = service;    
        this.service.get(dataURI)
            .subscribe(
            response => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data));
                this.data.pageSize = 10;

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
    getResponse(response: any) {
       // alert("in Get Response")
       // console.log(response.data);
       // this.currentEditItem.id = response.data;
        (<wijmo.collections.CollectionView>this.flex.collectionView).commitNew()
    }
    saveGridRow(saveUrl:any) {
        if (this.currentEditItem.id == "") {
            this.service.put( saveUrl, this.currentEditItem);//, this.getResponse);
            (<wijmo.collections.CollectionView>this.flex.collectionView).commitNew()
        }
        else {
            this.flex.collectionView.currentItem = this.currentEditItem;
            this.service.put(
                saveUrl,//'/Configurator/save_business_hours',
                this.currentEditItem);
            (<wijmo.collections.CollectionView>this.flex.collectionView).commitEdit()
        }
        
    }
    addGridRow() {
        this.formTitle = "Add " + this.formName;
        this.currentEditItem = (<wijmo.collections.CollectionView>this.flex.collectionView).addNew()
        this.currentEditItem.id = "";
    }
    editGridRow() {
        this.formTitle = "Edit " + this.formName;
        (<wijmo.collections.CollectionView>this.flex.collectionView).editItem(this.flex.collectionView.currentItem);
        this.currentEditItem = this.flex.collectionView.currentItem;
        console.log(this.currentEditItem);
    }

    delteGridRow(deleteUrl) {
        this.key = this.flex.collectionView.currentItem.id;
        if (confirm("Are you sure want to delete record")) {
            this.service.delete(deleteUrl + this.key);//'/Configurator/' + this.businessHourId + '/delete_business_hours');
            (<wijmo.collections.CollectionView>this.flex.collectionView).remove(this.flex.collectionView.currentItem);
        }

    }
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
