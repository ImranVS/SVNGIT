import {Component, OnInit, ComponentFactoryResolver, ComponentFactory, ElementRef, ComponentRef, ViewChild, ViewContainerRef} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';

import {RESTService} from '../../../core/services';

import {ServiceTab} from '../../../services/models/service-tab.interface';

import * as ServiceTabs from '../../../services/service-tab.collection';

@Component({
    selector: 'vs-sametime-details',
    templateUrl: '/app/dashboards/components/ibm-sametime/ibm-sametime-details.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class IBMSametimeDetails implements OnInit {

    @ViewChild('tab', { read: ViewContainerRef }) target: ViewContainerRef;

    errorMessage: string;

    serviceId: any;
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
        (<ServiceTab>(this.activeTabComponent.instance)).serviceId = this.serviceId;
        
    }
    
    ngOnInit() {

        
        
    }

    ngAfterViewInit() {
        this.route.params.subscribe(params => {

            this.serviceId = '1';

            // Get tabs associated with selected service
            this.dataProvider.get(`/services/device_details?device_id=57d30363bf467154b0bd9e94&destination=dashboard`)
                .subscribe(
                data => {
                    this.service = data.data;
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