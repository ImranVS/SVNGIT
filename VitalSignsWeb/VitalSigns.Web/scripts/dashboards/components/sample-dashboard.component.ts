import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';
import {WidgetController, WidgetContainer, WidgetContract, WidgetService} from '../../core/widgets';

@Component({
    selector: 'sample-dashboard',
    templateUrl: '/app/dashboards/components/sample-dashboard.component.html',
    providers: [WidgetService]
})
export class SampleDashboard extends WidgetController {

    firstWidgets: WidgetContract[] = [
        {
            id: 'greetings1',
            title: 'Say hello',
            name: 'GreetingsWidget',
            css: 'col-xs-12 col-sm-12 col-md-6 ',
            settings: {
                name: 'John Doe'
            }
        },
        {
            id: 'greetings2',
            title: 'Say hello',
            name: 'GreetingsWidget',
            css: 'col-xs-12 col-sm-12 col-md-6 ',
            settings: {
                name: 'Pierre Smith'
            }
        },
        {
            id: 'greetings3',
            title: 'Say hello',
            name: 'MobileUsers',
            css: 'col-xs-12',
            settings: {
            }
        }
    ]
    
    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService) {
        super(resolver, widgetService);
    }

}