import {Component, OnInit, ViewChild, AfterViewInit} from '@angular/core';
import {RESTService} from '../../../core/services';
import {GridBase} from '../../../core/gridBase';


@Component({
    templateUrl: '/app/configurator/components/mobileusers/mobile-users.component.html',
    providers: [RESTService]
})
export class MobileUser extends GridBase implements OnInit {
    sererNames: any;
    errorMessage: any;
    currentEditItem: any;
    mobileDeviceData: wijmo.collections.CollectionView;
    
    constructor(service: RESTService) {
  
        super(service);
        this.formName = "Critical Devices";
        
      
    }
    ngOnInit() {
        this.initialGridBind('/configurator/get_mobile_users');
        this.service.get('/configurator/get_all_mobile_devices')
            .subscribe(
            (response) => {
                this.mobileDeviceData = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data));
                this.mobileDeviceData.pageSize = 10;
            },
            (error) => this.errorMessage = <any>error
            );
    }
    saveMobileUsers(dlg: wijmo.input.Popup) {      
        this.saveGridRow('/configurator/save_mobileusers', dlg);
    }
    delteMobileUsers() {
        this.delteGridRow('/configurator/delete_mobile_users/');
    }
    get MobileDevicepageSize(): number {
        return this.mobileDeviceData.pageSize;
    }
    set MobileDevicepageSize(value: number) {

        if (this.mobileDeviceGrid)
        {
            if (this.mobileDeviceData.pageSize != value) {
                this.mobileDeviceData.pageSize = value;
                if (this.mobileDeviceGrid) {
                    (<wijmo.collections.IPagedCollectionView>this.mobileDeviceGrid.collectionView).pageSize = value;
                }
            }
        }
       
    }
 
}



