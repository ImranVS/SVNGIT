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

        this.service.get('/navigation/sitemaps')
            .subscribe(
            data => this.siteMapsList = data.sort((a, b) => {
                return a.title.localeCompare(b.title);
            }),
            error => this.errorMessage = <any>error
            );

    }

}