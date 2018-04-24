import {Component, OnInit, ComponentFactoryResolver, ComponentFactory, ElementRef, ComponentRef, ViewChild, ViewContainerRef} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';

import {RESTService} from '../../../core/services';
import * as ServiceTabs from './server-settings-tab.collection';
declare var injectSVG: any;

@Component({
    templateUrl: '/app/configurator/components/serverSettings/server-settings-tabs.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class ServerSettings implements OnInit {
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
        // Lazy-load selected tab component     
        let factory = this.resolver.resolveComponentFactory(ServiceTabs[tab.component]);     
        this.activeTabComponent = this.target.createComponent(factory);
    }
    ngAfterViewChecked() {
        injectSVG();
    }
    ngOnInit() {   
        this.tabsData= [
            {
                "title": "Server Attributes",
                "component": "DeviceAttributes",
                "path": "/app/configurator/components/serverSettings/server-settings-server-attributes.component",
                "active": "true"
            },
            {
                "title": "Domino Server Tasks",
                "component": "DominoServerTasks",
                "path": "/app/configurator/components/serverSettings/server-settings-domino-server-tasks.component",
                "active": false
            },
            {
                "title": "Disk Settings",
                "component": "ServerDiskSettings",
                "path": "/app/configurator/components/serverSettings/server-settings-disk-settings",
                "active": false
            },
            {
                "title": "Location/Credentials/Business Hours",
                "component": "ServerLocations",
                "path": "/app/configurator/components/serverSettings/server-settings-locations-credentials-businesshours.component",              
                "active": false
            }

        ];
              this.selectTab(this.tabsData[0]);
             
        };
}