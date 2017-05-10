import { Component, OnInit, ViewChild, ViewChildren } from '@angular/core';
import {HttpModule}    from '@angular/http';
import {RESTService} from '../../../core/services';
import {GridBase} from '../../../core/gridBase';
import {AppComponentService} from '../../../core/services';
@Component({
    templateUrl: '/app/configurator/components/applicationSettings/application-settings-servercredentials.component.html',
    providers: [
        HttpModule,
        RESTService
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
 
  
    constructor(service: RESTService, appComponentService: AppComponentService) {
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
    ngOnInit() {
        this.initialGridBind('/Configurator/get_credentials');   
    }
    saveServerCredential(dlg: wijmo.input.Popup) { 
        this.saveGridRow('/Configurator/save_credentials',dlg);
    }
    
    delteServerCredential() {
       
        this.delteGridRow('/Configurator/delete_credential/');  

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



