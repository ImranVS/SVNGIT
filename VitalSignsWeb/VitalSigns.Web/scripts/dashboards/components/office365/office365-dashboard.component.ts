import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';
import {HttpModule}    from '@angular/http';

import {WidgetController, WidgetContainer, WidgetContract} from '../../../core/widgets';
import {AppNavigator} from '../../../navigation/app.navigator.component';

import {RESTService} from '../../../core/services/rest.service';

declare var injectSVG: any;
declare var bootstrapNavigator: any;

@Component({
    templateUrl: '/app/dashboards/components/office365/office365-dashboard.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class Office365Dashboard implements OnInit {

    contextMenuSiteMap: any;

    constructor(private service: RESTService) { }

    ngOnInit() {

        this.service.get('/navigation/sitemaps/office365')
            .subscribe
            (
            data => this.contextMenuSiteMap = data,
            error => console.log(error)
            )

        injectSVG();
        bootstrapNavigator();

    }

}