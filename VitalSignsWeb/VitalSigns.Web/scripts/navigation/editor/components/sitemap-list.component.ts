import {Component, OnInit} from '@angular/core';
import {HttpModule}    from '@angular/http';

import {RESTService} from '../../../core/services/rest.service';

@Component({
    selector: 'sitemap-list',
    templateUrl: '/app/navigation/editor/components/sitemap-list.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class SiteMapList implements OnInit {

    siteMapsList: any;
    errorMessage: string;

    constructor(private service: RESTService) { }

    ngOnInit() {

        this.service.get('https://private-f4c5b-vitalsignssandboxserver.apiary-mock.com/navigation/editor/site-maps')
            .subscribe(
            data => this.siteMapsList = data,
            error => this.errorMessage = <any>error
            );

    }

}