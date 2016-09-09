﻿import {
    Component,
    ElementRef,
    Input,
    OnInit
} from '@angular/core';

import {WidgetContract} from '../models/widget-contract';
import {WidgetService} from '../services/widget.service';

@Component({
    selector: 'my-widget',
    template: ''
})
export class WidgetContainer implements WidgetContract, OnInit {

    @Input() contract: WidgetContract;

    @Input() id: string;
    @Input() title: string;
    @Input() path: string;
    @Input() name: string;
    @Input() css: string;
    @Input() settings: any;
    
    constructor(protected widgetService: WidgetService) { }

    ngOnInit() {

        if (this.contract) {
        
            this.id = this.id || this.contract.id;
            this.title = this.title || this.contract.title;
            this.path = this.path || this.contract.path;
            this.name = this.name || this.contract.name;
            this.css = this.css || this.contract.css;
            this.settings = this.settings || this.contract.settings;

        }
        
        if (!this.id) {

            let i : number = 1;

            while (this.widgetService.exists(`${this.name}${i}`))
                i++;

            this.id = `${this.name}${i}`;
        }
        
    }
}