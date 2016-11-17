import {Component, OnInit, ComponentFactoryResolver, ComponentFactory, ElementRef, ComponentRef, ViewChild, ViewContainerRef} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';

import {RESTService} from '../../../core/services';

import * as ServiceTabs from './log-files-tab.collection';

@Component({
    templateUrl: '/app/configurator/components/logFiles/log-files-tabs-component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class LogsTabs implements OnInit {
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
                "title": "Log Settings",
                "component": "Logs",
                "path": "/app/configurator/components/logFiles/log-settings-list.componenet.html",
                "active": false
            },
            {
                "title": "View Logs",
                "component": "ViewLogs",
                "path": "/app/configurator/components/logFiles/view-log-settings.component.html",
                "active": false


            },
          
        ];
        this.selectTab(this.tabsData[0]);
    };
}