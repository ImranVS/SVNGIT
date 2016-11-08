/// <reference path="maildelivery-tabs.collection.ts" />
import {Component, OnInit, ComponentFactoryResolver, ComponentFactory, ElementRef, ComponentRef, ViewChild, ViewContainerRef} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';

import {RESTService} from '../../../core/services';

import * as ServiceTabs from './maildelivery-tabs.collection';

@Component({
    templateUrl: '/app/dashboards/components/mail-delivery-status/maildelivery-tabs-component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class MailDeliveryStatus implements OnInit {
    @ViewChild('tab', { read: ViewContainerRef }) target: ViewContainerRef;

    tabsData: any;
    activeTabComponent: ComponentRef<{}>;
    constructor(private resolver: ComponentFactoryResolver, private elementRef: ElementRef) { }
    selectTab(tab: any) {
  
        this.tabsData.forEach(tab => tab.active = false);
        tab.active = true;
       
        if (this.activeTabComponent)
            this.activeTabComponent.destroy();
        let factory = this.resolver.resolveComponentFactory(ServiceTabs[tab.component]);
        this.activeTabComponent = this.target.createComponent(factory);
    }
    ngOnInit() {
        this.tabsData = [
            {
                "title": "Domino",
                "component": "DominoMailDeliveryStatus",
                "path": "/app/dashboards/components/mail-delivery-status/domino-mail-delivery-status.component'",
                "active": false
            }
            ,
            {
                "title": "Exchange",
                "component": "ExchangeMailDeliveryStatus",
                "path": "/app/dashboards/components/mail-delivery-status/exchange-mail-delivery-status.component'",
                "active": false
            }
        ];
        this.selectTab(this.tabsData[0]);
    };
}