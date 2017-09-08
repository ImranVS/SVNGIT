import { Component, OnInit, ComponentFactoryResolver, ComponentFactory, ElementRef, ComponentRef, ViewChild, ViewContainerRef } from '@angular/core';
//import { Component, OnInit,  ViewChild, ViewContainerRef } from '@angular/core';

import {ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';
import {WidgetComponent} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import {RESTService} from '../../../core/services';
import { ServiceTab } from '../../../services/models/service-tab.interface';
import * as ServiceTabs from '../../../services/service-tab.collection';

@Component({
    selector: 'vs-ms-exchange-details',
    templateUrl: '/app/dashboards/components/ms-exchange/ms-exchange-details.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class MsExchangeDetail implements OnInit {

    @ViewChild('tab', { read: ViewContainerRef }) target: ViewContainerRef;

    errorMessage: string;
    service: any;
    activeTabComponent: ComponentRef<{}>;
    constructor(private dataProvider: RESTService, private widgetService: WidgetService, private resolver: ComponentFactoryResolver,
        private elementRef: ElementRef,
        private route: ActivatedRoute) { }
    selectTab(tab: any) {
    
        // Activate selected tab
        this.service.tabs.forEach(tab => tab.active = false);
        tab.active = true;    

        // Dispose current tab if one already active
        if (this.activeTabComponent)
            this.activeTabComponent.destroy();
            
        // Lazy-load selected tab component
        if (!tab) return;
        let factory = this.resolver.resolveComponentFactory(ServiceTabs[tab.component]);
        this.activeTabComponent = this.target.createComponent(factory);
       
        
    }
    
    ngOnInit() {
       
        this.route.params.subscribe(params => {

            // Get tabs associated with selected service
            this.dataProvider.get(`/services/device_details?deviceType=Exchange&destination=Health`)
                .subscribe(
                data => {
                    this.service = data.data;
                    this.selectTab(this.service.tabs[0]);
                },
                error => this.errorMessage = <any>error
                );

        });
        
    }
    
}