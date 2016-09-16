import {Component, AfterViewChecked, OnInit} from '@angular/core';
import {ROUTER_DIRECTIVES, Router} from '@angular/router';
import {HTTP_PROVIDERS}    from '@angular/http';

import {RESTService} from '../core/services';

declare var injectSVG: any;
declare var bootstrapNavigator: any;
@Component({
    selector: 'app-header',
    templateUrl: '/app/navigation/app.header.component.html',
    directives: [ROUTER_DIRECTIVES],
    providers: [
        HTTP_PROVIDERS,
        RESTService
    ]
})
export class AppHeader implements OnInit {
    errorMessage: string;

    deviceSummary: any;

    constructor(private service: RESTService, private router: Router) { }
    
    loadData() {      
        this.service.get('/Services/dashboard_summary')
            .subscribe(
            response => {
                this.deviceSummary = response.data;
                console.log(response);
            },
            error => {
                this.errorMessage = <any>error;
                console.log("in service Error");
            }
            );
    }

    ngOnInit() {
        this.loadData();
        //alert(this.deviceSummary);
        injectSVG();
        bootstrapNavigator();
        
     
    } 

}