import {Component, OnInit, ComponentResolver, ComponentFactory, ElementRef, ComponentRef, ViewChild, ViewContainerRef} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {HTTP_PROVIDERS}    from '@angular/http';

import {RESTService} from '../../../core/services';

import {ServiceTab} from '../../../services/models/service-tab.interface';

declare var System: any;

@Component({
    templateUrl: '/app/dashboards/components/ibm-sametime/ibm-sametime-details.component.html',
    providers: [
        HTTP_PROVIDERS,
        RESTService
    ]
})
export class IBMSametimeDetails implements OnInit {

    @ViewChild('tab', { read: ViewContainerRef }) target: ViewContainerRef;

    errorMessage: string;
    deviceid: any;
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
            
            // Get tabs associated with selected service
            //http://private-ad10c-ibm.apiary-mock.com/services/sametime/1
            //this.dataProvider.get(`/services/device_details?device_id=57d30363bf467154b0bd9e94&destination=dashboard`)
            this.dataProvider.get(`http://private-ad10c-ibm.apiary-mock.com/services/sametime/1`)
                .subscribe(
                data => {
                    this.service = data;
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