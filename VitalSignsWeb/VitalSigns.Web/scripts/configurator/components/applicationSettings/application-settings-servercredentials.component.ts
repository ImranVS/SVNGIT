import {Component, OnInit } from '@angular/core';
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
    errorMessage: string;
    selectedDeviceType: string;  
    ServerCredentialId: string;
    deviceTypes: any;
    isVisible: boolean = false;
 
  
    constructor(service: RESTService, appComponentService: AppComponentService) {
        super(service, appComponentService);
        this.formName = "Server Credentials";
        this.service.get('/Configurator/get_server_types')
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
       
    }
    editServerCredential(dlg: wijmo.input.Popup) {
     
        this.editGridRow(dlg);
        this.currentEditItem.is_modified = false;
        this.currentEditItem.password = "****";
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
}



