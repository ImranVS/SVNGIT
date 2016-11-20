import {Component, OnInit, ComponentFactoryResolver, ComponentFactory, ElementRef, ComponentRef, ViewChild, ViewContainerRef} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';

import {RESTService} from '../../../core/services';

import * as ServiceTabs from './alert-tab.collection';

@Component({
    templateUrl: '/app/configurator/components/alert/alert-tabs-component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class Alerts implements OnInit {
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
                "title": "Definitions",
                "component": "AlertDefinitionsDashboard",
                "path": "/app/configurator/components/alert/alert-definitions-dashboard.component",
                "active": false
            },
            {
                "title": "Settings",
                "component": "AlertSettings",
                "path": "/app/configurator/components/alert/alert-settings.component",
                "active": false
            },
            {
                "title": "Scripts",
                "component": "Scripts",
                "path": "/app/configurator/components/alert/scripts.component",
                "active": false
            },
            {
                "title": "History",
                "component": "AlertHistory",
                "path": "/app/configurator/components/alert/alerts-allalert-history.component",
                "active": false
            },
           
        ];
        this.selectTab(this.tabsData[0]);
    };
}