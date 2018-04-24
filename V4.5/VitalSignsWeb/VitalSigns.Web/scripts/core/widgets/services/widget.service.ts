import { Injectable } from '@angular/core';

import {WidgetComponent} from '../models/widget-component'
import {WidgetContract} from '../models/widget-contract';
import {WidgetController} from '../components/widget-controller';

export interface WidgetServiceItem {

    controller: WidgetController;
    contract: WidgetContract;
    component: WidgetComponent;

}

@Injectable()
export class WidgetService {

    private _widgets: WidgetServiceItem[] = [];
    private _controllers: WidgetController[] = [];
    private _properties: { [key: string]: any; } = {};

    getProperty(key: string) {

        return this._properties[key];

    }

    setProperty(key: string, value: any) {
    
        this._properties[key] = value;
        
        this._controllers.forEach(controller => {
        
            if (controller.onPropertyChanged)
                controller.onPropertyChanged(key, value);

        });

        this._widgets.forEach(widget => {

            if (widget.component.onPropertyChanged)
                widget.component.onPropertyChanged(key, value);

        });
        
    }
    
    exists(widgetId: string): boolean {

        return this._widgets.some(widget => widget.contract.id == widgetId);

    }

    refreshWidget(widgetId: string, settings?: any) {

        let widgetRef = this.findWidget(widgetId);

        return new Promise(function (resolve, reject) {
        
            if (!widgetRef)
                reject(`Widget ${widgetId} does not exist or is not registered.`);

            if (widgetRef.component.refresh) {
                widgetRef.component.refresh(settings);
                resolve();
            }

            reject(`Widget ${widgetId} does not support refresh.`);
            
        });
        
    }

    findWidget(widgetId: string): WidgetServiceItem {
    
        let widget = this._widgets
            .find(widget => widget.contract.id == widgetId);

        if (widget)
            return widget;

    }
    
    registerWidget(controller: WidgetController, contract: WidgetContract, component: WidgetComponent) {

        if (this.exists(contract.id))
            throw new Error(`Widget identifier (${contract.id}) already in use.`);
            
        this._widgets.push({
            controller: controller,
            contract: contract,
            component: component
        });

    }

    loadController(controller: WidgetController) {

        this._controllers.push(controller);

    }

    unloadController(controller: WidgetController) {
    
        this._widgets = this._widgets.filter(widget => widget.controller !== controller);

        this._controllers = this._controllers.filter(item => item !== controller);
            
    }
    
}