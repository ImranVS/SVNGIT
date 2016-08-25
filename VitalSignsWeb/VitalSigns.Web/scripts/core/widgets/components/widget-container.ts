import {
Component,
ElementRef,
Input
} from '@angular/core';

import {WidgetContract} from '../models/widget-contract';

@Component({
    selector: 'my-widget',
    template: ''
})
export class WidgetContainer implements WidgetContract {
    @Input() id: string;
    @Input() title: string;
    @Input() path: string;
    @Input() name: string;
    @Input() css: string;
    @Input() settings: any;
}