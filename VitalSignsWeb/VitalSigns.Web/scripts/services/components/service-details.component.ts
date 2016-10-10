import {Component, OnInit, ComponentFactoryResolver, ComponentFactory, ElementRef, ComponentRef, ViewChild, ViewContainerRef} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';

import {RESTService} from '../../core/services';

import {ServiceTab} from '../models/service-tab.interface';

import * as ServiceTabs from '../service-tab.collection';

@Component({
    templateUrl: '/app/services/components/service-details.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class ServiceDetails implements OnInit {

    @ViewChild('tab', { read: ViewContainerRef }) target: ViewContainerRef;

    errorMessage: string;

    deviceId: any;
    service: any;

    activeTabComponent: ComponentRef<{}>;

    constructor(private dataProvider: RESTService, private resolver: ComponentFactoryResolver, private elementRef: ElementRef, private route: ActivatedRoute) { }
    
    selectTab(tab: any) {
        // Activate selected tab
        this.service.tabs.forEach(tab => tab.active = false);
        tab.active = true;    
        // Dispose current tab if one already active
        if (this.activeTabComponent)
            this.activeTabComponent.destroy();

        // Lazy-load selected tab component
        let factory = this.resolver.resolveComponentFactory(ServiceTabs[tab.component]);
        this.activeTabComponent = this.target.createComponent(factory);
        (<ServiceTab>(this.activeTabComponent.instance)).serviceId = this.deviceId;
        
    }
    
    ngOnInit() {
    
        this.route.params.subscribe(params => {

            this.deviceId = params['service'];
           
            // Get tabs associated with selected service
            this.dataProvider.get(`/services/device_details?device_id=${this.deviceId}&destination=dashboard`)
                .subscribe(
                response => {
                   
                    this.service = response.data;
                    this.selectTab(this.service.tabs[0]);
                },
                error => this.errorMessage = <any>error
                );


        });
       
        
    }

    getStatusDescription(status: string) {

        switch (status) {
            case 'ok':
                return 'OK';
            case 'notresponding':
                return 'No <br /> resp.';
            case 'issue':
                return 'Issue';
            case 'maintenance':
                return 'Mainten.';
        }

    }
    
}