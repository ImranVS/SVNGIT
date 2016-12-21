import {Component, ComponentFactoryResolver, OnInit, Injector} from '@angular/core';

import {WidgetController, WidgetContainer, WidgetContract} from '../../core/widgets';
import {WidgetService} from '../../core/widgets/services/widget.service';

import {ServiceTab} from '../models/service-tab.interface';

declare var injectSVG: any;

@Component({
    selector: 'tab-overall',
    templateUrl: '/app/services/components/domino-health-tab.component.html',
    providers: [WidgetService]
})
export class DominoHealthTab extends WidgetController implements OnInit, ServiceTab {

    widgets: WidgetContract[];
    serviceId: string;

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService) {
        super(resolver, widgetService);
    }
    
    ngOnInit() {
    
        this.widgets = [
            {
                id: 'usersConnectionsDuringTheDay',
                title: 'Daily Activities',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-12 col-md-6 col-lg-6',
                settings: {
                    url: '/services/1/overall/hourly-connections',
                    chart: {
                        chart: {
                            renderTo: 'usersConnectionsDuringTheDay',
                            type: 'areaspline',
                            height: 300
                        },
                        colors: ['#5fbe7f'],
                        title: { text: '' },
                        subtitle: { text: '' },
                        xAxis: {
                            labels: {
                                step: 6
                            },
                            categories: []
                        },
                        legend: {
                            enabled: false
                        },
                        credits: {
                            enabled: false
                        },
                        exporting: {
                            enabled: false
                        },
                        series: []
                    }
                }
            },
            {
                id: 'id2',
                title: 'Top 5 Tags',
                name: 'NotYetImplemented',
                css: 'col-xs-12 col-sm-12 col-md-6 col-lg-6',
                settings: {}
            },
            {
                id: 'id3',
                title: 'Statistics',
                name: 'NotYetImplemented',
                css: 'col-xs-12 col-sm-12 col-md-6 col-lg-6',
                settings: {}
            }
        ];
    
        injectSVG();
    }

}