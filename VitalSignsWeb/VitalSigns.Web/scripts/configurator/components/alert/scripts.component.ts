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
    templateUrl: '/app/configurator/components/alert/scripts.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class Scripts extends GridBase implements OnInit {
    errorMessage: string;
    formObject: any = {
        id: null,
        script_name: null,
        script_command: null,
        script_location: null
    };
    public formData = new FormData();

    get pageSize(): number {
        return this.data.pageSize;
    }

    constructor(service: RESTService, appComponentService: AppComponentService) {
        super(service, appComponentService);
        this.formName = "Script";
    }

    saveScript(dlg: wijmo.input.Popup) {
        this.saveGridRow('/configurator/save_script', dlg);
    }


    ngOnInit() {
        this.service.get('/configurator/get_scripts')
            .subscribe(
            (response) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data));
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
        this.showDialog(dlg);
        this.formObject.script_location = "";
        var location = <HTMLSpanElement>document.getElementById("scriptLocation");
        if (location != null) {
            location.innerHTML = "";
        }
    }

    showEditForm(dlg: wijmo.input.Popup) {
        this.errorMessage = "";
        this.formTitle = "Edit " + this.formName;
        this.formObject.id = this.flex.collectionView.currentItem.id;
        this.formObject.script_name = this.flex.collectionView.currentItem.script_name;
        this.formObject.script_command = this.flex.collectionView.currentItem.script_command;
        (<wijmo.collections.CollectionView>this.flex.collectionView).editItem(this.flex.collectionView.currentItem);
        this.showDialog(dlg);
        this.clearFileInput(document.getElementById("script_location"));
        var location = <HTMLSpanElement>document.getElementById("scriptLocation");
        if (location != null) {
            location.innerHTML = this.flex.collectionView.currentItem.script_location;
        }
    }

    clearFileInput(ctrl) {
        try {
            ctrl.value = null;
        } catch (ex) { }
        if (ctrl.value) {
            ctrl.parentNode.replaceChild(ctrl.cloneNode(true), ctrl);
        }
    }

    saveScripts(dlg: wijmo.input.Popup) {
        this.errorMessage = "";
        var saveUrl = '/configurator/save_script';
        var location = <HTMLSpanElement>document.getElementById("scriptLocation");
        if (this.flex.collectionView) {
            if (this.flex.collectionView.items.length > 0) {
                this.flex.collectionView.currentItem.script_name = this.formObject.script_name;
                this.flex.collectionView.currentItem.script_command = this.formObject.script_command;
                if (this.formObject.script_location == null || this.formObject.script_location == "") {
                    this.flex.collectionView.currentItem.script_location = location.innerHTML;
                    this.formObject.script_location = location.innerHTML;
                }
                else {
                    this.flex.collectionView.currentItem.script_location = this.formObject.script_location;
                }              
            }

        }
        if (this.formObject.script_location == null || this.formObject.script_location == "") {
            this.errorMessage = "Please upload a script.";
            //this.message.toggleVisibility(true, this.errorMessage);
            this.appComponentService.showErrorMessage(this.errorMessage);
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
            location.innerHTML = "";
            this.flex.refresh();
            dlg.hide();
        }
    }

    uploadScript(fileInput: any): void {
        //this.formObject.script_location = "~/uploads/" + fileInput.target.files[0].name;
        //var location = <HTMLSpanElement>document.getElementById("scriptLocation");
        //location.innerHTML = this.formObject.script_location + "<br/>";
        //this.service.put('/configurator/upload_script', this.formData)
        //    .subscribe(
        //    response => {
        //        this.formObject.script_location = response.data;
        //        var location = <HTMLSpanElement>document.getElementById("scriptLocation");
        //        location.innerHTML = response.data;

        //    },
        //    (error) => this.errorMessage = <any>error

        //    );
        //this.formData = null;
    }

    changeListener(fileInput: any) {
        this.postFile(fileInput);
        this.formObject.script_location = "~/uploads/" + fileInput.target.files[0].name;
        var location = <HTMLSpanElement>document.getElementById("scriptLocation");
        location.innerHTML = this.formObject.script_location + "<br/>";
        this.service.put('/configurator/upload_script', this.formData)
            .subscribe(
            response => {
                if (response.status == "Success") {
                    this.formObject.script_location = response.data;
                    var location = <HTMLSpanElement>document.getElementById("scriptLocation");
                    location.innerHTML = response.data;
                    this.appComponentService.showSuccessMessage(response.message);
                }
                else {
                    this.appComponentService.showErrorMessage(response.message);
                }
            },
            (error) => this.errorMessage = <any>error

            );
        //this.formData = null;
        this.formData = new FormData();

    }
    //send post file to server 
    postFile(inputValue: any): void {
        for (let i = 0; i < inputValue.target.files.length; i++) {
            this.formData.append("file-" + i.toString(), inputValue.target.files[i]);
        }

    }
}