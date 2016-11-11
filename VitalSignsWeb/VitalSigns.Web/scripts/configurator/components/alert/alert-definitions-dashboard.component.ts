import {Component, OnInit} from '@angular/core';
import { CommonModule } from '@angular/common';
import {HttpModule}    from '@angular/http';
import {RESTService} from '../../../core/services';
import {AppNavigator} from '../../../navigation/app.navigator.component';

declare var injectSVG: any;
declare var bootstrapNavigator: any;

@Component({
    templateUrl: '/app/configurator/components/alert/alert-definitions-dashboard.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class AlertDefinitionsDashboard implements OnInit  {    
    errorMessage: string;

    constructor(service: RESTService) {
        
    }

    ngOnInit() {
        injectSVG();
        bootstrapNavigator();
    }
}