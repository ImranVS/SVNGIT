import {Component, OnInit} from '@angular/core';
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
import {AppComponentService} from '../../../core/services';


@Component({
    templateUrl: '/app/configurator/components/alert/alert-url.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class AlertURLs extends GridBase implements OnInit {
    errorMessage: string;
    formObject: any = {
        id: null,
        name: null,
        url: null,
    };
    public formData = new FormData();

    get pageSize(): number {
        return this.data.pageSize;
    }

    constructor(service: RESTService, appComponentService: AppComponentService) {
        super(service, appComponentService);
        this.formName = "URL";
    }



    ngOnInit() {
        this.service.get('/configurator/get_alert_urls')
            .subscribe(
            (response) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data));
            },
            (error) => this.errorMessage = <any>error
            );
    }

    delete_alert_url() {
        this.deleteGridRow('/configurator/delete_alert_url/');
    }

    showAddForm(dlg: wijmo.input.Popup) {
        this.errorMessage = "";
        this.formTitle = "Add " + this.formName;
        this.formObject.id = "";
        this.formObject.name = "";
        this.formObject.url = "";
        this.showDialog(dlg);
       
    }

    showEditForm(dlg: wijmo.input.Popup) {
        this.errorMessage = "";
        this.formObject.id = this.flex.collectionView.currentItem.id;
        this.formObject.name = this.flex.collectionView.currentItem.name;
        this.formObject.url = this.flex.collectionView.currentItem.url;
        this.formTitle = "Edit " + this.formName + "ID: " + this.formObject.id;
        (<wijmo.collections.CollectionView>this.flex.collectionView).editItem(this.flex.collectionView.currentItem);
        this.showDialog(dlg);
       
    }


  

    saveAlertUrl(dlg: wijmo.input.Popup) {
        this.errorMessage = "";
        var saveUrl = '/configurator/save_alert_url';
         if (this.flex.collectionView) {
            if (this.flex.collectionView.items.length > 0) {
                this.flex.collectionView.currentItem.name = this.formObject.name;
                this.flex.collectionView.currentItem.url = this.formObject.url;
                      
            }

        }
       
        if (!this.errorMessage) {
            if (this.formObject.id == "") {
                this.service.put(saveUrl, this.formObject)
                    .subscribe(
                    response => {
                        if (response.status == "Success") {
                            this.data = response.data;
                            this.appComponentService.showSuccessMessage(response.message);
                        }
                        else {
                            this.appComponentService.showErrorMessage(response.message);
                        }
                    }
                    );
                (<wijmo.collections.CollectionView>this.flex.collectionView).commitNew();
            }
            else {
                this.service.put(saveUrl, this.formObject)
                    .subscribe(
                    response => {
                        if (response.status == "Success") {
                            this.flex.collectionView.currentItem.id = response.data;
                            this.appComponentService.showSuccessMessage(response.message);
                        }
                        else {
                            this.appComponentService.showErrorMessage(response.message);
                        }
                    }
                    );
                (<wijmo.collections.CollectionView>this.flex.collectionView).commitEdit();
            }
           this.flex.refresh();
            dlg.hide();
        }
    }


}