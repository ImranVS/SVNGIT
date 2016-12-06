
import {Component, OnInit, ComponentFactoryResolver, ComponentFactory, ElementRef, ComponentRef, ViewChild, ViewContainerRef} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';
import {Router} from '@angular/router';

import {RESTService} from '../../core/services';

import * as helpers from '../../core/services/helpers/helpers';

import {ServiceTab} from '../models/service-tab.interface';

import * as ServiceTabs from '../service-tab.collection';
import {WidgetService} from '../../core/widgets';
import {AppComponentService} from '../../core/services';
import {ServicesViewService} from '../services/services-view.service';
@Component({
    templateUrl: '/app/services/components/service-details.component.html',
    providers: [
        HttpModule,
        RESTService,
        WidgetService,
        helpers.DateTimeHelper
    ]
})
export class ServiceDetails implements OnInit {

    @ViewChild('tab', { read: ViewContainerRef }) target: ViewContainerRef;

    errorMessage: string;

    deviceId: any;
    service: any;
    module: string;
    activeTabComponent: ComponentRef<{}>;
    services: any[];
    data: string;
   // service: any
    protected appComponentService: AppComponentService;
    public servicesViewService: ServicesViewService;
    constructor(private dataProvider: RESTService, private resolver: ComponentFactoryResolver, private elementRef: ElementRef, private router: Router, private route: ActivatedRoute,
        private datetimeHelpers: helpers.DateTimeHelper, appComponentService: AppComponentService, servicesViewService: ServicesViewService) {
        //.map(routeParams => routeParams.id);
        this.appComponentService = appComponentService;
        this.servicesViewService = servicesViewService;
    }
    
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
        const parentActivatedRoute = this.router.routerState.root.children[0].params;
        parentActivatedRoute.subscribe(params => {
            this.module = params['module'];
        });
     
        this.route.params.subscribe(params => {
            this.deviceId = params['service'];
            // Get tabs associated with selected service
            this.dataProvider.get(`/services/device_details?device_id=${this.deviceId}&destination=${this.module}`)
                .subscribe(
                response => {
                    this.service = this.datetimeHelpers.toLocalDateTime(response.data);
                  this.data=response.data
                    this.selectTab(this.service.tabs[0]);
                    console.log(this.service.tabs);
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
    deleteServer() {
       
      //  this.dataProvider.delete('/configurator/delete_server/' + this.deviceId);
        this.dataProvider.delete('/configurator/delete_server/' + this.deviceId)
            .subscribe(
            response => {
                if (response.status == "Success") {
                    this.appComponentService.showSuccessMessage(response.message);
                   // this.router.navigate(['services', response.data]);
                    this.router.navigate(['services/' + this.module]);
                    this.servicesViewService.refreshServicesList();
                } else {
                    this.appComponentService.showErrorMessage(response.message);
                }

            }, error => {
                var errorMessage = <any>error;
                this.appComponentService.showErrorMessage(errorMessage);
            });
       
    }
   
    scanNow() {
     //   alert("Disks");

    }

}