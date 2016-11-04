import {Component, OnInit, ComponentFactoryResolver, ComponentFactory, ElementRef, ComponentRef, ViewChild, ViewContainerRef} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';

import {RESTService} from '../../../core/services';

import * as ServiceTabs from './ibmdomino-tabs.collection';

@Component({
    templateUrl: '/app/configurator/components/ibmDomino/ibmdomino-tabs-component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class IBMDomino implements OnInit {
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
        this.tabsData = [
            {
                "title": "Custom Statistics",
                "component": "NotYetImplemented",
                "path": "/app/not-yet-implemented.component",
                "active": false
            },
            {
                "title": "Log File Scanning",
                "component": "NotYetImplemented",
                "path": "/app/not-yet-implemented.component",
                "active": false
            },
            {
                "title": "Notes Database Replicas",
                "component": "NotesDatabaseReplica",
                "path": "/app/configurator/components/ibmDomino/Ibm-notes-database-replicas.component",
                "active": false
            },
            {
                "title": "Notes Databases",
                "component": "NotesDatabases",
                "path": "/app/configurator/components/ibmDomino/Ibm-notes-databases.component",
                "active": false
            },
            {
                "title": "Server Task Definitions",
                "component": "ServerTaskDefinition",
                "path": "/app/configurator/components/ibmDomino/Ibm-servertask-definition.component",
                "active": false
            },
            {
                "title": "IBM Domino Settings",
                "component": "IbmDominoSettingsForm",
                "path": "/app/configurator/components/ibmDomino/Ibm-domino-settings.component",
                "active": false
            },
            {
                "title": "Traveler Data Store",
                "component": "TravelerDataStore",
                "path": "/app/configurator/components/ibmDomino/Ibm-travelerdatastore.component",
                "active": false
            }
        ];
        this.selectTab(this.tabsData[0]);
    };
}