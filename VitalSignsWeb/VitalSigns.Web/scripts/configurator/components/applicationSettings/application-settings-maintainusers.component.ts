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
    status: any;
    superadmin: any;
    constructor(service: RESTService, appComponentService:AppComponentService) {
        super(service, appComponentService);
        this.formName = "Maintain Users";
        this.status = ["Active", "Inactive"];
        this.superadmin = ["Yes", "No"];

    }
    ngOnInit() {
        this.initialGridBind('/configurator/get_maintain_users');
    }
    saveMaintainUser(dlg: wijmo.input.Popup) {      
        this.saveGridRow('/configurator/save_maintain_users', dlg);
    }
    delteMaintainUsers() {
        this.delteGridRow('/configurator/delete_maintain_users/');
    }
    addMaintainUser(dlg: wijmo.input.Popup) {
        this.addGridRow(dlg);
        this.currentEditItem.login_name = "";
        this.currentEditItem.full_name = "";
        this.currentEditItem.email = "";
        this.currentEditItem.status = "Active";
        this.currentEditItem.super_admin = "No";
        this.currentEditItem.configurator_access = false;
        this.currentEditItem.console_command_access = false;
    }
}



