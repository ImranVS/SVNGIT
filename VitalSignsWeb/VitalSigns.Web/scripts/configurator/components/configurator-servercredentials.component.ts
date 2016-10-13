import {Component, OnInit, ViewChild, AfterViewInit} from '@angular/core';
import { CommonModule } from '@angular/common';

import {HTTP_PROVIDERS}    from '@angular/http';
import {RESTService} from '../../core/services';
import {AppNavigator} from '../../navigation/app.navigator.component';
import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';
import * as wjCoreModule from 'wijmo/wijmo.angular2.core';;
import {GridBase} from '../../core/gridBase';

@Component({
    templateUrl: '/app/configurator/components/configurator-servercredentials.component.html',
    directives: [
        wjFlexGrid.WjFlexGrid,
        wjFlexGrid.WjFlexGridColumn,
        wjFlexGrid.WjFlexGridCellTemplate,
        wjFlexGridFilter.WjFlexGridFilter,
        wjFlexGridGroup.WjGroupPanel,
        wjFlexInput.WjMenu,
        wjFlexInput.WjMenuItem,
        wjFlexInput.WjComboBox,
        AppNavigator
    ],
    providers: [
        HTTP_PROVIDERS,
        RESTService
    ]
})
export class ServerCredentials extends GridBase implements OnInit {  
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    data: wijmo.collections.CollectionView;
    errorMessage: string;
    selectedDeviceType: string;
    //Columns in grid
  
    ServerCredentialId: string;
    deviceTypes: any;
 
  
    constructor(service: RESTService) {
        super(service, '/Configurator/credentials');
        this.formName = "Server Credentials";

        this.service.get('/services/server_list_selectlist_data')
            .subscribe(
            (response) => {
                this.deviceTypes = response.data.deviceTypeData;
              

                this.deviceTypes.splice(0, 1);


            },
            (error) => this.errorMessage = <any>error
            );
      
    } 
  

    saveServerCredential() {
        this.saveGridRow('/Configurator/save_server_credentials');
    }

    ngOnInit() {
        

    }

   

    delteServerCredential() {
       
        this.delteGridRow('/Configurator/delete_credential/');  

    }
}



