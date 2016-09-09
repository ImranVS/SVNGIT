import {Component, ComponentResolver, OnInit, Injector} from '@angular/core';

import {WidgetController, WidgetContainer, WidgetContract, WidgetService} from '../../../core/widgets';

import {ServiceTab} from '../../../services/models/service-tab.interface';

declare var injectSVG: any;

@Component({
    selector: 'tab-chats',
    templateUrl: '/app/dashboards/components/ibm-sametime/ibm-sametime-meetings-tab.component.html',
    directives: [WidgetContainer],
    providers: [WidgetService]
})
export class IBMSametimeMeetingsTab extends WidgetController implements OnInit, ServiceTab {

    widgets: WidgetContract[];
    serviceId: string;

    constructor(protected resolver: ComponentResolver, protected widgetService: WidgetService) {
        super(resolver, widgetService);
    }
    
    ngOnInit() {
    
        this.widgets = [
            {
                id: 'activeMeetingsUsers',
                title: 'Active Meetings/Users in Meetings',
                path: '/app/widgets/charts/components/chart.component',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6 col-md-6 col-lg-6',
                settings: {
                    url: '/sametime/active_meetings_users',
                    chart: {
                        chart: {
                            renderTo: 'activeMeetingsUsers',
                            type: 'areaspline',
                            height: 300
                        },
                        colors: ['#5fbe7f'],
                        title: { text: '' },
                        subtitle: { text: '' },
                        xAxis: {
                            labels: {
                                step: 1
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
            }
        ];
    
        injectSVG();
    }

}