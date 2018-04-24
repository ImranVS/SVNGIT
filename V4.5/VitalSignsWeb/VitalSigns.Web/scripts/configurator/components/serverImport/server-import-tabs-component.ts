import {Component, OnInit, ComponentFactoryResolver, ComponentFactory, ElementRef, ComponentRef, ViewChild, ViewContainerRef} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';

import {RESTService} from '../../../core/services';
import * as ServiceTabs from './server-import-tabs.collection';

declare var injectSVG: any;

@Component({
    templateUrl: '/app/configurator/components/serverImport/server-import-tabs-component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class ServerImports implements OnInit {
    @ViewChild('tab', { read: ViewContainerRef }) target: ViewContainerRef;

    tabsData: any;
    activeTabComponent: ComponentRef<{}>;
    constructor(private resolver: ComponentFactoryResolver, private elementRef: ElementRef) { }
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
    ngAfterViewChecked() {
        injectSVG();
    }
    ngOnInit() {
        this.tabsData = [
            {
                "title": "Domino",
                "component": "DominoServerImport",
                "path": "/app/configurator/components/serverimport/server-import-domino.component", 
                "active": false
            },
            {
                "title": "WebSphere",
                "component": "WebSphereServerImport",
                "path": "/app/configurator/components/serverimport/server-import-websphere.component",
                "active": false
            },
            {
                "title": "Microsoft Servers",
                "component": "MicrosoftServerImport",
                "path": "/app/configurator/components/serverimport/server-import-microsoft.component",
                "active": false
            }
           
        ];
        this.selectTab(this.tabsData[0]);
    };
}