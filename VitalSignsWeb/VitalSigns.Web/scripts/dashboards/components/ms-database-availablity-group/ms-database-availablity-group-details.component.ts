import {Component, OnInit, ComponentFactoryResolver, ComponentFactory, ElementRef, ComponentRef, ViewChild, ViewContainerRef} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';
import {WidgetComponent} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import {RESTService} from '../../../core/services';

import {ServiceTab} from '../../../services/models/service-tab.interface';

import * as ServiceTabs from '../../../services/service-tab.collection';

@Component({
    selector: 'vs-ms-database-availablity-group-details',
    templateUrl: '/app/dashboards/components/ms-database-availablity-group/ms-database-availablity-group-details.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class DAGDetails implements OnInit {

    @ViewChild('tab', { read: ViewContainerRef }) target: ViewContainerRef;

    errorMessage: string;

    serviceId: any;
    service: any;
    nodeName: any;

    activeTabComponent: ComponentRef<{}>;
    
    constructor(private dataProvider: RESTService, private widgetService: WidgetService, private resolver: ComponentFactoryResolver, private elementRef: ElementRef, private route: ActivatedRoute) { }
    
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
        this.route.params.subscribe(params => {
            if (params['service'])
                this.serviceId = params['service'];
            else {
                this.serviceId = this.widgetService.getProperty('serviceId');
            }
        });
        var res = this.serviceId.split(';');
        this.route.params.subscribe(params => {

            // Get tabs associated with selected service
            this.dataProvider.get(`/services/device_details?device_id=${res[0]}&destination=dashboard`)
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