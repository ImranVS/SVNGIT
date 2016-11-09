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
    constructor(service: RESTService) {
        super(service);
        this.formName = "Critical Devices";
       
      
    }
    ngOnInit() {
        this.initialGridBind('/configurator/get_mobile_users');
    }
    saveMobileUsers(dlg: wijmo.input.Popup) {      
        this.saveGridRow('/configurator/save_mobileusers', dlg);
    }
    delteMobileUsers() {
        this.delteGridRow('/configurator/delete_mobile_users/');
    }
  
 
}



