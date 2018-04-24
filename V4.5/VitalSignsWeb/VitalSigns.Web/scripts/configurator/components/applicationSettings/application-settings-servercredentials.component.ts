import { Component, OnInit, ViewChild, ViewChildren } from '@angular/core';
import {HttpModule}    from '@angular/http';
import {RESTService} from '../../../core/services';
import {GridBase} from '../../../core/gridBase';
import { AppComponentService } from '../../../core/services';
import { AuthenticationService } from '../../../profiles/services/authentication.service';
import * as gridHelpers from '../../../core/services/helpers/gridutils';
@Component({
    templateUrl: '/app/configurator/components/applicationSettings/application-settings-servercredentials.component.html',
    providers: [
        HttpModule,
        RESTService,
        gridHelpers.CommonUtils

    ]
})
export class ServerCredentials extends GridBase implements OnInit {
    @ViewChild('flex') flex;
    @ViewChildren('somespwd') somespwd;
    errorMessage: string;
    selectedDeviceType: string;  
    ServerCredentialId: string;
    deviceTypes: any;
    isVisible: boolean = false;
    currentPageSize: any = 20;
 
  
    constructor(service: RESTService, appComponentService: AppComponentService, protected gridHelpers: gridHelpers.CommonUtils, private authService: AuthenticationService) {
        super(service, appComponentService);
        this.formName = "Server Credentials";
        this.service.get('/Configurator/get_server_credentials_businesshours')
            .subscribe(
            (response) => {
                this.deviceTypes = response.data.serverTypeData;
            },
            (error) => this.errorMessage = <any>error
            );     
    }  
    get pageSize(): number {
        return this.data.pageSize;
    }

    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            this.data.refresh();
            var obj = {
                name: this.gridHelpers.getGridPageName("ServerCredentials", this.authService.CurrentUser.email),
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
        this.initialGridBind('/Configurator/get_credentials');  
        this.service.get(`/services/get_name_value?name=${this.gridHelpers.getGridPageName("ServerCredentials", this.authService.CurrentUser.email)}`)
            .subscribe(
            (data) => {
                this.currentPageSize = Number(data.data.value);
                this.data.pageSize = this.currentPageSize;
                this.data.refresh();
            },
            (error) => this.errorMessage = <any>error
            ); 
    }
    saveServerCredential(dlg: wijmo.input.Popup) { 
        this.saveGridRow('/Configurator/save_credentials',dlg);
    }
    
    delteServerCredential() {
       
        this.deleteGridRow('/Configurator/delete_credential/');  

    }
    addServerCredential(dlg: wijmo.input.Popup) {
        this.addGridRow(dlg);
        this.currentEditItem.alias = "";
        this.currentEditItem.user_id = "";
        this.currentEditItem.device_type = "";
        this.currentEditItem.password = "";
        this.currentEditItem.is_modified = false;
    }
    editServerCredential(dlg: wijmo.input.Popup) {
        this.editGridRow(dlg);
        this.currentEditItem.is_modified = false;
        //this.currentEditItem.password = "****";
      //  this.currentEditItem.confirm_password = "****";   
    }
    valuechange(newValue) {

        if (this.currentEditItem.password == "****") {
            this.currentEditItem.is_modified = false;
        }
        else {
            this.currentEditItem.is_modified = true;
        }
    }

    savePwd(dialog: wijmo.input.Popup) {
        var pwd = this.somespwd.first.nativeElement.value;
        if (pwd == "") {
            this.appComponentService.showErrorMessage("You must enter a password");
        } else {
            this.currentEditItem.password = pwd;
            this.currentEditItem.is_modified = true;
            this.service.put('/configurator/save_credentials', this.currentEditItem)
                .subscribe(
                response => {
                    if (response.status == "Success") {
                        this.appComponentService.showSuccessMessage("Password updated successfully");
                    }
                    else {
                        this.appComponentService.showErrorMessage("Error updating the password");
                    }
                    this.currentEditItem.is_modified = false;
                },
                (error) => {
                    this.errorMessage = <any>error
                    this.appComponentService.showErrorMessage(this.errorMessage);
                });
            dialog.hide();
        }
    }
}



