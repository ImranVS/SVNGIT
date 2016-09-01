import {Component, OnInit, ComponentResolver, ComponentFactory, ElementRef, ComponentRef, ViewChild, ViewContainerRef} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {HTTP_PROVIDERS}    from '@angular/http';

import {RESTService} from '../../core/services';

import {ServiceTab} from '../models/service-tab.interface';

declare var System: any;

@Component({
    templateUrl: '/app/services/components/service-details.component.html',
    providers: [
        HTTP_PROVIDERS,
        RESTService
    ]
})
export class ServiceDetails implements OnInit {

    @ViewChild('tab', { read: ViewContainerRef }) target: ViewContainerRef;

    errorMessage: string;

    serviceId: any;
    service: any;

    activeTabComponent: ComponentRef<ServiceTab>;

    constructor(private dataProvider: RESTService, private resolver: ComponentResolver, private elementRef: ElementRef, private route: ActivatedRoute) { }
    
    selectTab(tab: any) {
    
        // Activate selected tab
        this.service.tabs.forEach(tab => tab.active = false);
        tab.active = true;    

        // Dispose current tab if one already active
        if (this.activeTabComponent)
            this.activeTabComponent.destroy();
            
        // Lazy-load selected tab component
        System.import(tab.path).then(component => {
            this.resolver
                .resolveComponent(component[tab.component])
                .then((factory: ComponentFactory<any>) => {
                    this.activeTabComponent = this.target.createComponent(factory);
                    this.activeTabComponent.instance.serviceId = this.serviceId;
                });
        });
        
    }
    
    ngOnInit() {
    
        this.route.params.subscribe(params => {

            this.serviceId = params['service'];
           
            // Get tabs associated with selected service
            this.dataProvider.get(`/services/device_details?id=${this.serviceId}&destination=dashboard`)
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
            case 'noIssue':
                return 'No <br /> issue';
            case 'notResponding':
                return 'No <br /> resp.';
            case 'issues':
                return 'Issues';
            case 'inMaintenance':
                return 'Mainten.';
        }

    }
    
}