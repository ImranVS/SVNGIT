import {Component, OnInit, ComponentFactoryResolver, ComponentFactory, ElementRef, ComponentRef, ViewChild, ViewContainerRef} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';

import {RESTService} from '../../../core/services';

import * as ServiceTabs from './windows-tabs.collection';
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

    tabsData: any;
    activeTabComponent: ComponentRef<{}>;
    constructor(private resolver: ComponentFactoryResolver, private elementRef: ElementRef, private route: ActivatedRoute) { }
    selectTab(tab: any) {
        // Activate selected tab
        this.tabsData.forEach(tab => tab.active = false);
        tab.active = true;
        // Dispose current tab if one already active
        if (this.activeTabComponent)
            this.activeTabComponent.destroy();
        let factory = this.resolver.resolveComponentFactory(ServiceTabs[tab.component]);
        this.activeTabComponent = this.target.createComponent(factory);
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
                "title": " PowerScripts Audit Log",
                "component": "PowerScriptsAudit",
                "path": "/app/configurator/components/microsoftsettings/powerscripts-audit.component",
                "active": false
            },
            {
                "title": "Exchnage Mail Probe",
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