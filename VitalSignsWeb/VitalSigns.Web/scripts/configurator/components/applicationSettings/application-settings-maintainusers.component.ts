import {Component, OnInit} from '@angular/core';
import { CommonModule } from '@angular/common';
import {HttpModule}    from '@angular/http';
import {RESTService} from '../../../core/services';
import {GridBase} from '../../../core/gridBase';

import {AppNavigator} from '../../../navigation/app.navigator.component';
import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';
import * as wjCoreModule from 'wijmo/wijmo.angular2.core';;


@Component({
    templateUrl: '/app/configurator/components/applicationSettings/application-settings-maintainusers.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class MaintainUser extends GridBase implements OnInit {
    constructor(service: RESTService) {
        super(service);
        this.formName = "Maintain Users";

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
        this.currentEditItem.status = "";
        this.currentEditItem.super_admin = "";
        this.currentEditItem.configurator_access = false;
        this.currentEditItem.console_command_access = false;
    }

}



