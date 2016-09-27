import { Component, ViewChildren, ComponentFactoryResolver, ComponentFactory, QueryList, ViewContainerRef, ViewChild, OnDestroy, AfterViewInit } from '@angular/core';

import {WidgetContainer} from './widget-container';
import {WidgetComponent} from '../models/widget-component';
import {WidgetContract} from '../models/widget-contract';
import {WidgetService} from '../services/widget.service';

import * as widgets from '../../../app.widgets';

export class WidgetController implements AfterViewInit, OnDestroy {

    @ViewChildren(WidgetContainer, { read: ViewContainerRef }) containers: QueryList<ViewContainerRef>;

    constructor(
        protected factoryResolver: ComponentFactoryResolver,
        protected widgetService: WidgetService
    ) {}

    ngAfterViewInit() {
    
        this.containers.forEach(containerRef => {

            var container: WidgetContainer = <WidgetContainer>(<any>containerRef)._element.component;
            
            let factory = this.factoryResolver.resolveComponentFactory(widgets[container.name]);
            let component = containerRef.createComponent(factory);
            let widget = <WidgetComponent>(component.instance);

            widget.settings = container.settings;
            
            this.widgetService.registerWidget(this, container, widget);

        });
        
    }

    ngOnDestroy() {

        this.widgetService.unloadController(this);

    }

}