import {Component, AfterViewChecked, OnInit} from '@angular/core';
import {ROUTER_DIRECTIVES, Router} from '@angular/router';
import {HTTP_PROVIDERS}    from '@angular/http';

import {RESTService} from '../../core/services';

declare var injectSVG: any;

@Component({
    templateUrl: '/app/services/components/services-view.component.html',
    directives: [ROUTER_DIRECTIVES],
    providers: [
        HTTP_PROVIDERS,
        RESTService
    ]
})
export class ServicesView implements OnInit, AfterViewChecked {

    errorMessage: string;

    services: any[];

    constructor(private service: RESTService, private router: Router) { }

    selectService(service: any) {

        // Activate selected tab
        this.services.forEach(service => service.active = false);
        service.active = true;  

        this.router.navigate(['services', service.id]);
    }

    loadData() {
        this.service.get('/Services/device_list')
            .subscribe(
            data => this.services = data.data,
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