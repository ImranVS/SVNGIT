﻿import { Component, AfterViewChecked, OnChanges, SimpleChange, Input, ViewChildren,OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {HttpModule}    from '@angular/http';
import {RESTService} from '../core/services';

import {AuthenticationService} from '../profiles/services/authentication.service';
import {Observable} from 'rxjs/Observable';
declare var injectSVG: any;
declare var bootstrapZeus: any;

import {AppComponentService} from '../core/services';
import * as helpers from '../core/services/helpers/helpers';

@Component({
    selector: 'app-header',
    templateUrl: '/app/navigation/app.header.component.html',
    providers: [
        HttpModule,
        RESTService,
        helpers.DateTimeHelper,
        helpers.UrlHelperService
    ]
})
export class AppHeader implements OnChanges,OnInit {
    @ViewChildren('password') password;
    @Input() anonymous: boolean;

    errorMessage: string;
    deviceName: string;
    deviceSummary: any;
    systemMessages: any;
    appComponentService: AppComponentService;
    error = '';
    success = '';
    appStatus: any;

    constructor(
        private service: RESTService,
        private router: Router,
        private route: ActivatedRoute,
        private authService: AuthenticationService, appComponentService: AppComponentService,
        private datetimeHelpers: helpers.DateTimeHelper, protected urlHelpers: helpers.UrlHelperService) {
        this.appComponentService = appComponentService;
    }
    
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
        this.service.get(`/dashboard/get_last_update`)
            .subscribe(
            response => this.appStatus = this.datetimeHelpers.toLocalDateTime(response.data),
            error => this.errorMessage = <any>error
            );
    }
    ngOnInit() {
        window.setInterval(() => {
            this.loadData();
        }, 30000);
    }


    changeDeviceName() {

        if (this.deviceName != "") {
            this.router.navigateByUrl(`services/dashboard?devicename=${this.deviceName}`);
        }
    }
       
    ngOnChanges(changes: { [propKey: string]: SimpleChange }) {

        if (!this.anonymous) {

            this.loadData();
            injectSVG();
            bootstrapZeus();
            
        }
    } 

    changePassword(dialog: wijmo.input.Popup) {
        var passwordVal = this.password.first.nativeElement.value;
        if (passwordVal == "") {
           
        } else {
            this.service.get('/Token/reset_password?emailId=' + this.authService.CurrentUser.email + '&password=' + passwordVal)
                .subscribe(
                response => {
                    this.appComponentService.showSuccessMessage("Password changed successfully");
                    this.password = "";                  
                },
                error => {
                    this.errorMessage = <any>error;
                    this.appComponentService.showErrorMessage(this.errorMessage);
                }
            );
            dialog.hide();
        }


    }
    dismissSystemMessages(dialog: wijmo.input.Popup) {
        this.service.get('/services/dismiss_system_messages')
            .subscribe(
            response => {
                this.systemMessages = null;
            });
            dialog.hide();    

    }
    showSystemMessages() {
        this.service.get('/services/get_system_messages')
            .subscribe(
            response => {
                this.systemMessages = this.datetimeHelpers.toLocalDateTime(response.data);
            });
    }
    logout() {
        this.authService.logout();
    }
}