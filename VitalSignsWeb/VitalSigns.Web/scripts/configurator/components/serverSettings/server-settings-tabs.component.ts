import {Component, OnInit, ComponentFactoryResolver, ComponentFactory, ElementRef, ComponentRef, ViewChild, ViewContainerRef} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';

import {RESTService} from '../../../core/services';

import * as ServiceTabs from './server-settings-tab.collection';

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
    ngOnInit() {   
        this.tabsData= [
            {
                "title": "Server Atributes",
                "component": "DeviceAttributes",
                "path": "/app/configurator/components/serverSettings/configurator-server-attributes.component",
                "active": "true"
            },
            {
                "title": "Domino Server Tasks",
                "component": "DominoServerTasks",
                "path": "/app/configurator/components/serverSettings/configurator-domino-server-tasks.component",
                "active": false
            },
            {
                "title": "Windows Services",

                "component": "WindowsServices",
                "path": "/app/configurator/components/serverSettings/server-settings-windows-services .component",
                "active": false

            },
            {
                "title": "Disk Settings",
                "component": "ServerDiskSettings",
                "path": "/app/configurator/components/serverSettings/server-disk-settings.component",
                "active": false
            },
            {
                "title": "Location/Credentials/Business Hours",
                "component": "ServerLocations",
                "path": "/app/configurator/components/serverSettings/server-locations-credentials-businesshours.component",
               
                "active": false
            }

        ];
              this.selectTab(this.tabsData[0]);
             
        };
}