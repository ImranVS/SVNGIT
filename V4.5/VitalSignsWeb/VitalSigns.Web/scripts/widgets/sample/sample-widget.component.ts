import {Component, Input} from '@angular/core';

import {WidgetComponent} from '../../core/widgets';

@Component({
    template: '<div>Hello {{settings.name}}</div>',
})

export class GreetingsWidget implements WidgetComponent {
    @Input() settings: any;
}