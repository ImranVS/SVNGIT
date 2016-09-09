import { Component, ViewChildren, ComponentResolver, ComponentFactory, QueryList, ViewContainerRef, ViewChild } from '@angular/core';

import {WidgetContainer, } from './widget-container';
import {WidgetContract} from '../models/widget-contract';
import {WidgetService} from '../services/widget.service';

declare var System: any;

export class WidgetController {

    @ViewChildren(WidgetContainer, { read: ViewContainerRef }) containers: QueryList<ViewContainerRef>;

    constructor(protected resolver: ComponentResolver, protected widgetService: WidgetService) {}
        
    ngAfterViewInit() {
    
        this.containers.map(containerRef => {

            var container: WidgetContainer = <WidgetContainer>(<any>containerRef)._element.component;
            
            System.import(container.path).then(component => {

                this.resolver
                    .resolveComponent(component[container.name])
                    .then((factory: ComponentFactory<any>) => {

                        var component = containerRef.createComponent(factory);
                        component.instance.settings = container.settings;

                        this.widgetService.registerWidget(container, component.instance);

                    });

            });
            
        });
        
    }

}