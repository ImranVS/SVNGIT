import {Component, Input, OnInit, AfterViewChecked} from '@angular/core';
import {HTTP_PROVIDERS}    from '@angular/http';

import {WidgetComponent} from '../../../core/widgets';
import {RESTService} from '../../../core/services';

declare var injectSVG: any;

@Component({
    templateUrl: './app/widgets/main-dashboard/components/user-sessions.component.html',
    providers: [
        HTTP_PROVIDERS,
        RESTService
    ]
})
export class UserSessions implements WidgetComponent, OnInit, AfterViewChecked {
    @Input() settings: any;

    errorMessage: string;
    responses: string;

    usersessionsStatus: any;

    constructor(private service: RESTService) { }

    loadData() {

        this.service.get('/services/dashboard_stats')
            .subscribe(
            response => this.usersessionsStatus = response.data,
          
            error => this.errorMessage = <any>error
        );
      
    }


    ngOnInit() {
        this.loadData();
        
      
    }

    ngAfterViewChecked() {
        injectSVG();
    }

    
}