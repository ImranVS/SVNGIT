import { Component, OnInit, ViewChild} from '@angular/core';
import { FormControl, NgModel, NgForm } from '@angular/forms';
import { RESTService } from '../../../core/services';
import { HttpModule } from '@angular/http';
import {GridBase} from '../../../core/gridBase';
import {Router} from '@angular/router';
import {ActivatedRoute} from '@angular/router';
import { AppComponentService } from '../../../core/services';
import { AuthenticationService } from '../../../profiles/services/authentication.service';
import * as gridHelpers from '../../../core/services/helpers/gridutils';
declare var injectSVG: any;



@Component({
    templateUrl: '/app/configurator/components/MicrosoftSettings/powerscripts-role-management.component.html',

    providers: [
        HttpModule,
        RESTService,
        gridHelpers.CommonUtils
    ]
})
export class PowerScriptsRoleManagement extends GridBase implements OnInit{
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    @ViewChild('flexScripts') flexScripts: wijmo.grid.FlexGrid;
    errorMessage: string;
    currentPageSize: any = 20;
    scripts: any;

    
   
    constructor(service: RESTService, private route: ActivatedRoute,private router: Router, appComponentService: AppComponentService, protected gridHelpers: gridHelpers.CommonUtils, private authService: AuthenticationService) {
        super(service, appComponentService);
        this.formName = "PowerScript Role";
    }

    get pageSize(): number {
        return this.data.pageSize;
    }
    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            this.data.refresh();
            var obj = {
                name: this.gridHelpers.getGridPageName("ExchnageMailProbe", this.authService.CurrentUser.email),
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
        this.service.get(`/services/get_name_value?name=${this.gridHelpers.getGridPageName("PowerScriptsRoleManagement", this.authService.CurrentUser.email)}`)
            .subscribe(
            (data) => {
                this.currentPageSize = Number(data.data.value);
                this.data.pageSize = this.currentPageSize;
                this.data.refresh();
            },
            (error) => this.errorMessage = <any>error
            );
        this.initialGridBind('/configurator/get_powerscript_roles');
        injectSVG();
    }
    
  
    initialGridBind(dataURI: string) {
        this.service.get(dataURI)
            .subscribe(
            response => {
                if (response.status == "Success") {
                    let scripts: string[] = response.data.scripts;
                    let scriptsObj = scripts.map(function (x) {
                        let pathArray: string[] = x.split('\\')
                        
                        return {
                            name: pathArray[pathArray.length - 1],
                            path: x,
                            is_selected: false,
                            device_type: pathArray[pathArray.length - 2] == "UserScripts" ? pathArray[pathArray.length - 3] : pathArray[pathArray.length - 2]
                        }
                    });
                    this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data.roles));
                    this.scripts = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(scriptsObj));
                    this.scripts.groupDescriptions.push(new wijmo.collections.PropertyGroupDescription("device_type"));
                    this.data.pageSize = 10;
                } else {
                    this.appComponentService.showErrorMessage(response.message);
                }

            }, error => {
                var errorMessage = <any>error;
                this.appComponentService.showErrorMessage(errorMessage);
            });

    }
    showAddForm(dlg: wijmo.input.Popup) {

        this.addGridRow(dlg);
        this.currentEditItem.id = "";
        this.currentEditItem.name = "";
        this.currentEditItem.file_paths = [];
        this.scripts.items.forEach(function (x) {
            x.is_selected = false;
        });
        
        this.scripts.refresh();
        this.showDialog(dlg);
    }
    saveRole(dlg: wijmo.input.Popup) {
        try {
            var selectedScripts: any[] = this.flexScripts.collectionView.items.filter(function (x) {
                return x.is_selected;
            }).map(function (x) {
                return x.path;
            });

            this.currentEditItem.file_paths = selectedScripts;
            var postdata = this.currentEditItem;

            this.saveGridRow('/configurator/save_powerscript_roles', dlg, postdata);
        } catch (ex) { console.log(ex) }
    } 

    saveGridRow(saveUrl: any, dlg?: wijmo.input.Popup, postdata?: any) {
        if (this.currentEditItem.id == "") {
            this.service.put(saveUrl, this.currentEditItem)
                .subscribe(
                response => {
                    //this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data.mailprobes));
                    //this.exchangedata = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data.exchangeservers));
                    if (response.status == "Success") {
                        this.currentEditItem.id = response.data;
                        (<wijmo.collections.CollectionView>this.flex.collectionView).commitNew();

                        dlg.hide();
                        this.initialGridBind('/configurator/get_powerscript_roles');
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
                        this.initialGridBind('/configurator/get_powerscript_roles');
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

    deleteRole() {
        this.deleteGridRow('/configurator/delete_powerscript_roles/');
    }
    itemsSourceChangedHandler(grid: wijmo.grid.FlexGrid) {
        try {
            grid.autoSizeColumns();
        } catch (ex) { console.log(ex) }
    }
    editGridRow(dlg: wijmo.input.Popup) {
        try {
            this.formTitle = "Edit " + this.formName;
            var mainThis = this;
            this.scripts.items.forEach(function (x) {
                x.is_selected = mainThis.flex.collectionView.currentItem.file_paths.indexOf(x.path) > -1;
            });
            (<wijmo.collections.CollectionView>this.flex.collectionView).editItem(this.flex.collectionView.currentItem);
            this.currentEditItem = this.flex.collectionView.currentItem;
            this.showDialog(dlg);
        } catch (ex) { console.log(ex) }
    }
}

