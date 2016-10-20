import {Component, OnInit, ComponentFactoryResolver, ComponentFactory, ElementRef, ComponentRef, ViewChild, ViewContainerRef} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';

import {RESTService} from '../../../core/services';

import * as ServiceTabs from './application-settings-tab.collection';

@Component({
    templateUrl: '/app/configurator/components/applicationSettings/application-settings-tabs.component.html',
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
        this.tabsData.forEach(tab => tab.active = false);
        tab.active = true;
        // Dispose current tab if one already active
        if (this.activeTabComponent)
            this.activeTabComponent.destroy();       
        let factory = this.resolver.resolveComponentFactory(ServiceTabs[tab.component]);       
        this.activeTabComponent = this.target.createComponent(factory);
    }
    ngOnInit() {   
        this.tabsData= [
            {
                "title": "Preferences",
                "component": "PreferencesForm",
                "path": "/app/configurator/components/applicationSettings/configurator-preferences.componenet",
                "active": false
            },
            {
                "title": "Credentials",
                "component": "ServerCredentials",
                "path": "/app/configurator/components/applicationSettings/application-settings-servercredentials.component",
                "active": false


            },
            {
                "title": "Business Hours",
                "component": "BusinessHours",
                "path": "/app/configurator/components/applicationSettings/applications-settings-businesshours.component",
                "active": false
            },
            {
                "title": "Maintainance",
                "component": "Maintenance",
                "path": "/app/configurator/components/applicationSettings/applications-settings-maintenance.component",
                "active": false
            },
            {
                "title": "Locations",
                "component": "Location",
                "path": "/app/configurator/components/applicationSettings/application-settings-locations.component",
                "active": false
            },
            {
                "title": "Users",
                "component": "MaintainUser",
                "path": "/app/configurator/components/applicationSettings/configurator-maintainusers.component",
                "active": false
            },
            {
                "title": "Traveller Data Store",
                "component": "TravelerDataStore",
                "path": "/app/configurator/components/applicationSettings/configurator-travelerdatastore.component",
                "active": false
            },
            {
                "title": "IBM Domino Settings",
                "component": "IbmDominoSettingsForm",
                "path": "/app/configurator/components/applicationSettings/application-settings-ibm-domino-settings.component",
                "active": false
            }

        ];
              this.selectTab(this.tabsData[0]);
             
        };
}