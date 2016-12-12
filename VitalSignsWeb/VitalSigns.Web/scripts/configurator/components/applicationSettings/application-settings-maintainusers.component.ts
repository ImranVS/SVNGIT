import {Component, OnInit} from '@angular/core';
import { CommonModule } from '@angular/common';
import {HttpModule}    from '@angular/http';
import {RESTService} from '../../../core/services';
import {GridBase} from '../../../core/gridBase';
import {AppNavigator} from '../../../navigation/app.navigator.component';
import {AppComponentService} from '../../../core/services';

@Component({
    templateUrl: '/app/configurator/components/applicationSettings/application-settings-maintainusers.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class MaintainUser extends GridBase implements OnInit {
    errorMessage: any;
    status: any;
    maintainRoles: any;
    usersRoles: any = [];
    checkedItems: any[];

    constructor(service: RESTService, appComponentService:AppComponentService) {
        super(service, appComponentService);
        this.formName = "User";     
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
        this.maintainRoles = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(["Configurator","UserManager","RemoteConsole"]));
    }
    rolesChecked(userrole: wijmo.input.MultiSelect) {
        this.usersRoles = [];
        for (var item of userrole.checkedItems) {
            this.usersRoles.push(item);
        }
    }
    resetPassWord() {
    

        this.service.get(`/Token/reset_password/?emailId=${this.flex.collectionView.currentItem.email}`)
            .subscribe(
            response => {
                
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data));

            },
            error => this.errorMessage = <any>error
            );
    }
    saveMaintainUser(dlg: wijmo.input.Popup) {
        this.currentEditItem.roles = this.usersRoles;  
        this.saveGridRow('/configurator/save_maintain_users', dlg);
    }
    delteMaintainUsers() {
        this.delteGridRow('/configurator/delete_maintain_users/');
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



