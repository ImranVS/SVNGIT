import {Component, OnInit } from '@angular/core';
import {HttpModule}    from '@angular/http';
import {RESTService} from '../../../core/services';
import {GridBase} from '../../../core/gridBase';

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
 
  
    constructor(service: RESTService) {
        super(service);
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
}



