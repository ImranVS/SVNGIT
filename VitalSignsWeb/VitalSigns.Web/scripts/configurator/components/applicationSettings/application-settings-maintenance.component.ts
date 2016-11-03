import {Component, OnInit} from '@angular/core';
import {HttpModule}    from '@angular/http';
import {RESTService} from '../../../core/services';
import {GridBase} from '../../../core/gridBase';


@Component({
    templateUrl: '/app/configurator/components/applicationsettings/application-settings-maintenance.component.html', 
    providers: [
        HttpModule,
        RESTService
    ]
})
export class Maintenance extends GridBase  {  
    devices: string;
    errorMessage: string;
    dataMobileUsers: wijmo.collections.CollectionView;
    selectedSetting: any;
    selectedSettingValue: any;

    constructor(service: RESTService) {
        super(service);
        this.formName = "Maintenance";
      
    } 

    ngOnInit() {
        this.initialGridBind('/Configurator/get_maintenance');

        this.service.get('/Dashboard/mobile_user_devices')
            .subscribe(
            (response) => {
                this.dataMobileUsers = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data));
                this.dataMobileUsers.pageSize = 10;
            },
            (error) => this.errorMessage = <any>error
            );

      

    }
    saveMaintenance(dlg: wijmo.input.Popup) {
        this.saveGridRow('/Configurator/save_maintenancedata',dlg);  
    }
    deleteMaintenance() {      
        this.delteGridRow('/Configurator/delete_maintenancedata/');  
    }

    changeInDevices(devices: string) {
        this.devices = devices;
    }
}



