﻿import {Component, AfterViewChecked, OnChanges, SimpleChange, Input, ViewChildren} from '@angular/core';
import {Router} from '@angular/router';
import {HttpModule}    from '@angular/http';

import {RESTService} from '../core/services';

import {AuthenticationService} from '../profiles/services/authentication.service';

declare var injectSVG: any;
declare var bootstrapZeus: any;
declare var bootstrapNavigator: any;

@Component({
    selector: 'app-header',
    templateUrl: '/app/navigation/app.header.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class AppHeader implements OnChanges {
    @ViewChildren('password') password;
    @Input() anonymous: boolean;

    errorMessage: string;
    deviceName: string;
    deviceSummary: any;

    constructor(
        private service: RESTService,
        private router: Router,
        private authService: AuthenticationService) { }
    
    loadData() {   
        
        this.service.get('/services/dashboard_summary')
            .subscribe(
            response => {
                this.deviceSummary = response.data;
            },
            error => {
                this.errorMessage = <any>error;
                console.log("in service Error");
            }
            );

    }

    changeDeviceName() {

        if (this.deviceName != "") {
            this.router.navigateByUrl('services/dashboard?devicename='+this.deviceName);
        }

    }
       
    ngOnChanges(changes: { [propKey: string]: SimpleChange }) {

        if (!this.anonymous) {

            this.loadData();
            injectSVG();
            bootstrapZeus();
            bootstrapNavigator();

        }

    } 
    changePassword(dialog: wijmo.input.Popup) {
        var passwordVal = this.password.first.nativeElement.value;
        if (passwordVal == "") {
           
        } else {
            this.service.get('/configurator/reset_password?emailId=' + this.authService.CurrentUser.email + '&password=' + passwordVal)
                .subscribe(
                response => {
                    
                });
            dialog.hide();
        }


    }
}