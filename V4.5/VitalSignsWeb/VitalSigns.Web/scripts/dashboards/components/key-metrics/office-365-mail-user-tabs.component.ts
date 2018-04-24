import {Component, OnInit, ComponentFactoryResolver, ComponentFactory, ElementRef, ComponentRef, ViewChild, ViewContainerRef} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';

import {RESTService} from '../../../core/services';

import * as ServiceTabs from '../../../services/service-tab.collection';
//import * as ServiceTabs from './office-365-mail-users-tabs.collection';
declare var injectSVG: any;

@Component({
    templateUrl: '/app/dashboards/components/key-metrics/office-365-mail-user-tabs.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class Office365MailUserTabs implements OnInit {
    @ViewChild('tab', { read: ViewContainerRef }) target: ViewContainerRef;

    tabsData: any;
    activeTabComponent: ComponentRef<{}>;
    constructor(private resolver: ComponentFactoryResolver, private elementRef: ElementRef) { }
    selectTab(tab: any) {
        try {
            console.log(1)
            // Activate selected tab
            this.tabsData.forEach(tab => tab.active = false);
            console.log(2)
            tab.active = true;
            console.log(3)
            // Dispose current tab if one already active
            if (this.activeTabComponent)
                this.activeTabComponent.destroy();
            console.log(4)
            let factory = this.resolver.resolveComponentFactory(ServiceTabs[tab.component]);
            console.log(5)
            this.activeTabComponent = this.target.createComponent(factory);
            console.log(6)
            console.log(tab.component)
            console.log(ServiceTabs[tab.component])
        }catch(ex)
        {
            console.log(ex)
        }
    }
    ngOnInit() {
        this.tabsData = [
            {
                "title": "Mailboxes",
                "component": "Office365MailboxViewTab",
                "path": "/app/dashboards/components/key-metrics/office-365-mailbox-grid.component",
                "active": false
            },
            {
                "title": "Users",
                "component": "Office365UsersGrid",
                "path": "/app/dashboards/components/key-metrics/office-365-users-grid.component",
                "active": false
            },   
            {
                "title": "Licenses",
                "component": "Office365LicensesTab",
                "path": "/app/dashboards/components/key-metrics/office-365-licenses-tab.component",
                "active": false
            },
            {
                "title": "Licenses Eligible for Reassignment",
                "component": "Office365ReassignableLicensesTab",
                "path": "/app/dashboards/components/key-metrics/office-365-reassignable-licenses-tab.component",
                "active": false
            }
        ];
        this.selectTab(this.tabsData[0]);
    };
    ngAfterViewChecked() {
        injectSVG();
    }
}