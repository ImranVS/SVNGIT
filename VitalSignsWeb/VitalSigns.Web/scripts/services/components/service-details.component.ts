
import {Component, OnInit, ComponentFactoryResolver, ComponentFactory, ElementRef, ComponentRef, ViewChild, ViewContainerRef, Injector} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';
import {Router} from '@angular/router';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';

import {RESTService} from '../../core/services';

import * as helpers from '../../core/services/helpers/helpers';

import {ServiceTab} from '../models/service-tab.interface';

import * as ServiceTabs from '../service-tab.collection';
import { WidgetService } from '../../core/widgets/services/widget.service';
import {AppComponentService} from '../../core/services';
import { ServicesViewService } from '../services/services-view.service';

declare var injectSVG: any;

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
    modal = true;
    deviceName: any;
    suspendTemporarilyForm: FormGroup;
   // service: any
    protected appComponentService: AppComponentService;
    public servicesViewService: ServicesViewService;
    constructor(private formBuilder: FormBuilder,private dataProvider: RESTService, private resolver: ComponentFactoryResolver, private elementRef: ElementRef, private router: Router, private route: ActivatedRoute,
        private datetimeHelpers: helpers.DateTimeHelper, appComponentService: AppComponentService, servicesViewService: ServicesViewService) {
        //.map(routeParams => routeParams.id);

        this.suspendTemporarilyForm = this.formBuilder.group({
            'id': [''],
            'name': [''],
            'password': [''],
            'device_id': [''],
            'duration': [''],
       

        });
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

        this.route.parent.params.subscribe(params => {

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
                    this.deviceName = response.data.name;
                    this.deviceId=response.data.id
                 
                },
                error => this.errorMessage = <any>error
                );


        });

        injectSVG();
       
        
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
        if (confirm("Are you sure want to delete this record?")) {
            this.dataProvider.delete('/configurator/delete_server/' + this.deviceId)
                .subscribe(
                response => {
                    if (response.status == "Success") {
                        this.router.navigate(['services/' + this.module]);
                        this.servicesViewService.refreshServicesList();
                        this.appComponentService.showSuccessMessage(response.message);

                    } else {
                        this.appComponentService.showErrorMessage(response.message);
                    }

                }, error => {
                    var errorMessage = <any>error;
                    this.appComponentService.showErrorMessage(errorMessage);
                });
        }
    }
   
    scanNow() {
       
        this.dataProvider.put('/Configurator/save_scan_now/' + this.deviceId, this.deviceId)
            .subscribe(

            response => {

                if (response.status == "Success") {

                    this.appComponentService.showSuccessMessage(response.message);

                } else {

                    this.appComponentService.showErrorMessage(response.message);
                }
            });

    }
    suspendTemporarly(dlg: wijmo.input.Popup) {
        if (dlg) {
            dlg.modal = this.modal;
            dlg.hideTrigger = dlg.modal ? wijmo.input.PopupTrigger.None : wijmo.input.PopupTrigger.Blur;
            dlg.show();

        }
    }

    SaveSuspendTemporarly(suspendTemporarily: any, dialog: wijmo.input.Popup) {
        
        suspendTemporarily.name = this.deviceName;
        suspendTemporarily.device_id = this.deviceId;
       
        this.dataProvider.put('/Configurator/save_suspend_temporarly', suspendTemporarily)
            .subscribe(

            response => {

                if (response.status == "Success") {

                    this.appComponentService.showSuccessMessage(response.message);
                  
                } else {

                    this.appComponentService.showErrorMessage(response.message);
                }
            });
        dialog.hide();
        //
    }
}