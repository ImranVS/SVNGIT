import { Component, ViewChildren, ComponentFactoryResolver, ComponentFactory, QueryList, ViewContainerRef, ViewChild, OnInit, OnDestroy, AfterViewInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { WidgetContainer } from './widget-container';
import { WidgetComponent } from '../models/widget-component';
import { WidgetContract } from '../models/widget-contract';
import { WidgetService } from '../services/widget.service';

import * as widgets from '../../../app.widgets';

export class WidgetController implements AfterViewInit, OnInit, OnDestroy {

    @ViewChildren(WidgetContainer) containers: QueryList<WidgetContainer>;

    inited: boolean = false;

    constructor(
        protected factoryResolver: ComponentFactoryResolver,
        protected widgetService: WidgetService,
        private notReuseRoute: boolean = false,
        private innerRouter: Router = null,
        private innerRoute: ActivatedRoute = null
    ) { }

    onPropertyChanged(key: string, value: any) { }

    ngOnInit() {
    
        if (this.notReuseRoute)
            this.innerRoute.queryParams.subscribe(params => {
            
                if (this.inited) {

                    this.innerRouter.navigate(
                        ['/forward', {
                            ref: this.innerRouter.url
                        }],
                        { skipLocationChange: true });

                }

            });

        this.inited = true;

    }

    ngAfterViewInit() {

        this.widgetService.loadController(this);
        if (this.containers) {
            this.containers.forEach(container => {

                let factory = this.factoryResolver.resolveComponentFactory(widgets[container.name]);
                let component = container.viewContainerRef.createComponent(factory);
                let widget = <WidgetComponent>(component.instance);

                widget.settings = container.settings;

                this.widgetService.registerWidget(this, container, widget);

            });
        }

    }

    ngOnDestroy() {

        this.widgetService.unloadController(this);

    }

}