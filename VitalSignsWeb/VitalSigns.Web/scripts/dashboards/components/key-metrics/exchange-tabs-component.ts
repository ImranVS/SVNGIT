import {Component, OnInit, ComponentFactoryResolver, ComponentFactory, ElementRef, ComponentRef, ViewChild, ViewContainerRef} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';

import {RESTService} from '../../../core/services';

import * as ServiceTabs from './exchange-tab.collection';
declare var injectSVG: any;

@Component({
    templateUrl: '/app/dashboards/components/key-metrics/exchange-tabs-component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class ExchnagesTab implements OnInit {
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
                "title": "Mailboxes",
                "component": "ExchangemailstatisticsviewGrid",
                "path": "/app/dashboards/components/key-metrics/exchange-mailbox-view.component",
                "active": false
            },
            {
                "title": "Users ",
                "component": "ExchangemailAccessviewGrid",
                "path": "/app/dashboards/components/key-metrics/exchange-mail-access-view.component",
                "active": false
            },   
        ];
        this.selectTab(this.tabsData[0]);
    };
    ngAfterViewChecked() {
        injectSVG();
    }
}