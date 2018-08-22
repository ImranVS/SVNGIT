import {Component, OnInit, ComponentFactoryResolver, ComponentFactory, ElementRef, ComponentRef, ViewChild, ViewContainerRef} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';

import {RESTService} from '../../../core/services';
import { Tab } from '../../../common/models/tab.interface';
import * as ServiceTabs from './windows-tabs.collection';
import { AuthenticationService } from '../../../profiles/services/authentication.service';
declare var injectSVG: any;

@Component({
    templateUrl: '/app/configurator/components/MicrosoftSettings/windows-tabs-component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class WindowsLogSettings implements OnInit {
    @ViewChild('tab', { read: ViewContainerRef }) target: ViewContainerRef;

    tabsData: Tab[];
    activeTabComponent: ComponentRef<{}>;
    constructor(private resolver: ComponentFactoryResolver, private elementRef: ElementRef, private route: ActivatedRoute, private authService: AuthenticationService) { }
    selectTab(tab: any) {
        // Activate selected tab
        try {
            this.tabsData.forEach(tab => tab.active = false);
            tab.active = true;
            // Dispose current tab if one already active
            if (this.activeTabComponent)
                this.activeTabComponent.destroy();
            let factory = this.resolver.resolveComponentFactory(ServiceTabs[tab.component]);
            console.log(ServiceTabs);
            console.log(ServiceTabs[tab.component])
            this.activeTabComponent = this.target.createComponent(factory);
        }
        catch (ex) { console.log(ex) }
    }
    ngOnInit() {
        this.tabsData = [
            {
                "title": " Event Log Scanning",
                "component": "WindowsEventFiles",
                "path": "/app/components/WindowsSettings/windows-events-file-scanning.component",
                "active": false
            },
            {
                "title": " PowerScripts Management",
                "component": "PowerScriptsManagementTab",
                "path": "/app/configurator/components/microsoftsettings/powerscripts-management-tab.component",
                "active": false,
                "visible": this.authService.isCurrentUserInRole("PowerScripts")
            },
            {
                "title": " PowerScripts Audit Log",
                "component": "PowerScriptsAudit",
                "path": "/app/configurator/components/microsoftsettings/powerscripts-audit.component",
                "active": false
            },
            {
                "title": "Exchange Mail Probe",
                "component": "ExchnageMailProbe",
                "path": "/app/configurator/components/microsoftsettings/Exchange-mail-probe-scanning.component",
                "active": false
            }       
        ];
        var tab:number = 0;
        this.route.queryParams.subscribe(params => {
            if (params['tab']) {
                tab = Number.parseInt(params['tab']);
            }
        });
        this.selectTab(this.tabsData[tab]);    
    };

    ngAfterViewChecked() {
        injectSVG();
    }
}