﻿import {Component, OnInit} from '@angular/core';
import { CommonModule } from '@angular/common';
import {HttpModule}    from '@angular/http';
import {RESTService} from '../../../core/services';
import {GridBase} from '../../../core/gridBase';
import {AppNavigator} from '../../../navigation/app.navigator.component';
import { AppComponentService } from '../../../core/services';
import { AuthenticationService } from '../../../profiles/services/authentication.service';
import * as gridHelpers from '../../../core/services/helpers/gridutils';


@Component({
    templateUrl: '/app/configurator/components/applicationSettings/application-settings-maintainusers.component.html',
    providers: [
        HttpModule,
        RESTService,
        gridHelpers.CommonUtils
    ]
})
export class MaintainUser extends GridBase implements OnInit {
    errorMessage: any;
    status: any;
    maintainRoles: any;
    usersRoles: any = [];
    checkedItems: any[];
    loading = false;
    currentPageSize: any = 20;

    constructor(service: RESTService, appComponentService: AppComponentService, protected gridHelpers: gridHelpers.CommonUtils, private authService: AuthenticationService) {
        super(service, appComponentService);
        this.formName = "User";     
    }
    get pageSize(): number {
        return this.data.pageSize;
    }

    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            this.data.refresh();
            var obj = {
                name: this.gridHelpers.getGridPageName("MaintainUser", this.authService.CurrentUser.email),
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
        //this.service.get('/configurator/get_maintain_users')
        //    .subscribe(
        //    (response) => {
        //        this.maintainRoles = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data));
        //        console.log(response.data);
        //    },
        //    (error) => this.errorMessage = <any>error
        //    );
        this.initialGridBind('/configurator/get_maintain_users');
        this.maintainRoles = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(["Configurator", "UserManager", "RemoteConsole", "PowerScripts"]));
        this.service.get(`/services/get_name_value?name=${this.gridHelpers.getGridPageName("MaintainUser", this.authService.CurrentUser.email)}`)
            .subscribe(
            (data) => {
                this.currentPageSize = Number(data.data.value);
                this.data.pageSize = this.currentPageSize;
                this.data.refresh();
            },
            (error) => this.errorMessage = <any>error
            );
    }
    rolesChecked(userrole: wijmo.input.MultiSelect) {
        this.usersRoles = [];
        for (var item of userrole.checkedItems) {
            this.usersRoles.push(item);
        }
    }
    resetPassWord() {
        this.loading = true;
        this.service.get(`/Token/reset_password/?emailId=${this.flex.collectionView.currentItem.email}`)
            .subscribe(
            response => {
                if (response.status == "Success") {

                    this.appComponentService.showSuccessMessage(response.message);
                    this.loading = false;

                } else {
                    if (response.message.indexOf("SendPasswordEmail") == -1) {
                        this.appComponentService.showErrorMessage(response.message);
                        this.loading = false;
                    } else {
                        this.appComponentService.showErrorMessage("Error sending the password to the email address. Please ensure you have an SMTP server configured in the Alert Settings page.");
                        this.loading = false;
                        console.error(response.message);
                    }
                }
            },
            (error) => {
                this.errorMessage = <any>error;
                this.loading = false;
            }
            );
    }
    saveMaintainUser(dlg: wijmo.input.Popup) {
        this.loading = true;
        this.currentEditItem.roles = this.usersRoles;  
        var saveUrl = '/configurator/save_maintain_users';
        //this.saveGridRow('/configurator/save_maintain_users', dlg);
        if (this.currentEditItem.id == "") {
            this.service.put(saveUrl, this.currentEditItem)
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
                    this.loading = false;
                }, error => {
                    var errorMessage = <any>error;
                    this.appComponentService.showErrorMessage(errorMessage);
                    this.loading = false;
                });
        }
        else {
            this.flex.collectionView.currentItem = this.currentEditItem;
            this.service.put(saveUrl, this.currentEditItem)
                .subscribe(
                response => {
                    if (response.status == "Success") {
                        (<wijmo.collections.CollectionView>this.flex.collectionView).commitEdit()
                        dlg.hide();
                        this.appComponentService.showSuccessMessage(response.message);
                    } else {
                        this.appComponentService.showErrorMessage(response.message);
                    }
                    this.loading = false;
                }, error => {
                    var errorMessage = <any>error;
                    this.appComponentService.showErrorMessage(errorMessage);
                    this.loading = false;
                });

        }
    }
    delteMaintainUsers() {
        this.deleteGridRow('/configurator/delete_maintain_users/');
    }
    editUserGridRow(dlg: wijmo.input.Popup) {
        this.editGridRow(dlg);
        this.checkedItems = [];
        var roles = this.currentEditItem.roles;
        if (roles) {
            for (var userItem of (<wijmo.collections.CollectionView>this.maintainRoles).sourceCollection) {
                var urole = roles.filter((item) => item == userItem);
                if (urole.length > 0)
                    this.checkedItems.push(userItem);
            }
        }
    }
    addMaintainUser(dlg: wijmo.input.Popup, userrole: wijmo.input.MultiSelect) {
        this.addGridRow(dlg);
        this.currentEditItem.email = "";
        this.currentEditItem.full_name = "";
        this.currentEditItem.roles = [];
        this.currentEditItem.status = true;
        this.checkedItems = [];
    }
}



