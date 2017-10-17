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
    constructor(private resolver: ComponentFactoryResolver, private elementRef: ElementRef, private route: ActivatedRoute) { }
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
                "component": "CustomStatistics",
                "path": "/app/configurator/components/ibmDomino/Ibm-custom-statistics.component", 
                "active": false
            },
            {
                "title": "Log File Scanning",
                "component": "DominoLogFiles",
                "path": "/app/components/ibmDomino/Ibm-domino-log-file-scanning.component'",
                "active": false
            },
            {
                "title": "Notes Database Replicas",
                "component": "NotesDatabaseReplica",
                "path": "app/components/ibmDomino/notesmail-probes.component",
                "active": false
            },
            {
                "title": "Notes Databases",
                "component": "NotesDatabases",
                "path": "/app/configurator/components/ibmDomino/Ibm-notes-databases.component",
                "active": false
            },
            {
                "title": "Notes Database Maile Probe",
                "component": "NotesMailProbes",
                "path": "/app/configurator/components/mail/notesmail-probes.component",
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
        var tab:number = 0;
        this.route.queryParams.subscribe(params => {
            if (params['tab']) {
                tab = Number.parseInt(params['tab']);
            }
        });
        this.selectTab(this.tabsData[tab]);    
    };
}