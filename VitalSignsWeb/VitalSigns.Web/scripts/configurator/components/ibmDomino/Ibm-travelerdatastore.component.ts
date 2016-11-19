import {Component, OnInit,ViewChild} from '@angular/core';
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
    templateUrl: '/app/configurator/components/ibmDomino/Ibm-travelerdatastore.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class TravelerDataStore extends GridBase implements OnInit {
    datastore: any;
    errorMessage: any;
    travelerServers: any;

    constructor(service: RESTService) {
        super(service);
        this.formName = "Traveler Data Store";
        this.datastore = ["SQL Server", "DB2"];
       
    }
               
    ngOnInit() {
        this.initialGridBind('/configurator/get_travelerdatastore');
        this.service.get('/Configurator/get_traveller_servers')
            .subscribe(
            (response) => {
                this.travelerServers = response.data;
                console.log(this.travelerServers);
            },
            (error) => this.errorMessage = <any>error
            );
    }
    saveTravelerDataStore(dlg: wijmo.input.Popup) {
        this.saveGridRow('/configurator/save_traveler_data_store', dlg);
    }
    delteTravelerDataStore() {
        this.delteGridRow('/configurator/delete_traveler_data_store/');
    }


    addTravelerData(dlg: wijmo.input.Popup) {
        this.addGridRow(dlg);
        this.currentEditItem.traveler_service_pool_name = "";
        this.currentEditItem.device_name = "";
        this.currentEditItem.data_store = "";
        this.currentEditItem.database_name = "";
        this.currentEditItem.port = "";
        this.currentEditItem.user_name = "";
        this.currentEditItem.password = "";
        this.currentEditItem.integrated_security = "";
        this.currentEditItem.test_scan_server = "";
        this.currentEditItem.used_by_servers = "";
    }

}