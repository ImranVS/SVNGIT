﻿import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import {HttpModule}    from '@angular/http';
import {RESTService} from '../../../core/services';
import {AppNavigator} from '../../../navigation/app.navigator.component';
import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';
import * as wjCoreModule from 'wijmo/wijmo.angular2.core';
import {GridBase} from '../../../core/gridBase';



@Component({
    templateUrl: '/app/configurator/components/alert/scripts.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class Scripts extends GridBase implements OnInit  {    
    errorMessage: string;
    formObject: any = {
        id: null,
        script_name: null,
        script_command: null,
        script_location: null
    };

    get pageSize(): number {
        return this.data.pageSize;
    }

    constructor(service: RESTService) {
        super(service);
        this.formName = "Script";
    }

    saveScript(dlg: wijmo.input.Popup) {
        this.saveGridRow('/configurator/save_script',dlg);
    }


    ngOnInit() {
        this.service.get('/configurator/get_scripts')
            .subscribe(
            (response) => {
                this.data = response.data;
            },
            (error) => this.errorMessage = <any>error
            );
    }
   
    deleteScript() {
        this.delteGridRow('/configurator/delete_script/');
    }

    showAddForm(dlg: wijmo.input.Popup) {
        this.errorMessage = "";
        this.formTitle = "Add " + this.formName;
        this.formObject.id = "";
        this.formObject.script_name = "";
        this.formObject.script_command = "";
        this.formObject.script_location = "";
        this.showDialog(dlg);
    }

    showEditForm(dlg: wijmo.input.Popup) {
        this.errorMessage = "";
        this.formTitle = "Edit " + this.formName;
        this.formObject.id = this.flex.collectionView.currentItem.id;
        this.formObject.script_name = this.flex.collectionView.currentItem.script_name;
        this.formObject.script_command = this.flex.collectionView.currentItem.script_command;
        this.formObject.script_location = this.flex.collectionView.currentItem.script_location;

        (<wijmo.collections.CollectionView>this.flex.collectionView).editItem(this.flex.collectionView.currentItem);
        this.showDialog(dlg);
    }

    saveScripts(dlg: wijmo.input.Popup) {
        var saveUrl = '/configurator/save_script';
        this.flex.collectionView.currentItem.script_name = this.formObject.script_name;
        this.flex.collectionView.currentItem.script_command = this.formObject.script_command;
        this.flex.collectionView.currentItem.script_location = this.formObject.script_location;
        if (this.formObject.id == "") {
            this.service.put(saveUrl, this.formObject)
                .subscribe(
                response => {
                    this.data = response.data;
                }
            );
            (<wijmo.collections.CollectionView>this.flex.collectionView).commitNew(); 
        }
        else {
            this.service.put(saveUrl, this.formObject)
                .subscribe(
                response => {
                    this.flex.collectionView.currentItem.id = response.data;
                }
                );
            (<wijmo.collections.CollectionView>this.flex.collectionView).commitEdit();
        }
        this.flex.refresh();
        dlg.hide();
    }

}