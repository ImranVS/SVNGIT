import {Component, OnInit, ComponentFactoryResolver, ComponentFactory, ElementRef, ComponentRef, ViewChild, ViewContainerRef} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';

import {RESTService} from '../../../core/services';

import * as ServiceTabs from './application-settings-tab.collection';

@Component({
    templateUrl: '/app/configurator/components/applicationSettings/applications-settings-tabs.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class ApplicationSettings implements OnInit {
    @ViewChild('tab', { read: ViewContainerRef }) target: ViewContainerRef;

    tabsData: any;
    activeTabComponent: ComponentRef<{}>;
    constructor(private resolver: ComponentFactoryResolver, private elementRef: ElementRef) { }
    selectTab(tab: any) {
        // Activate selected tab
        this.tabsData.forEach(tab => tab.active = "false");
        tab.active = "true";
        // Dispose current tab if one already active
        if (this.activeTabComponent)
            this.activeTabComponent.destroy();

        // Lazy-load selected tab component
       // console.log(tab.component);
        console.log(ServiceTabs[tab.component]);
        let factory = this.resolver.resolveComponentFactory(ServiceTabs[tab.component]);
        console.log(factory);
        this.activeTabComponent = this.target.createComponent(factory);
    }
    ngOnInit() {   
        this.tabsData= [
            {
                "title": "Preferences",
                "component": "NotYetImplemented",
                "path": "/app/not-yet-implemented.component",
                "active": "true"
            },
            {
                "title": "Credentials",
                "component": "ServerCredentials",
                "path": "/app/configurator/components/applicationSettings/configurator-servercredentials.component",
                "active": "false"


            },
            {
                "title": "Business Hours",
                "component": "BusinessHours",
                "path": "/app/configurator/components/applicationSettings/configurator-businesshours.component",
                "active": "false"
            },
            {
                "title": "Maintainance",
                "component": "NotYetImplemented",
                "path": "/app/configurator/components/applicationSettings/applications-settings-tabs.component",
                "active": "false"
            },
            {
                "title": "Locations",
                "component": "NotYetImplemented",
                "path": "/app/not-yet-implemented.component",
                "active": "false"
            },
            {
                "title": "Users",
                "component": "NotYetImplemented",
                "path": "/app/not-yet-implemented.component",
                "active": "false"
            },
            {
                "title": "Traveller Data Store",
                "component": "NotYetImplemented",
                "path": "/app/not-yet-implemented.component",
                "active": "false"
            },
            {
                "title": "IBM Damino Settings",
                "component": "IbmDominoSettingsForm",
                "path": "/app/configurator/components/applicationSettings/configurator-ibm-domino-settings.component",
                "active": "false"
            }

        ];
              this.selectTab(this.tabsData[0]);
             
        };
}