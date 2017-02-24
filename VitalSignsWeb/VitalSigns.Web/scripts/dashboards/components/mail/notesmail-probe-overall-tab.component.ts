import {Component, ComponentFactoryResolver, OnInit, Injector} from '@angular/core';

import {WidgetController, WidgetContainer, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';

import {ServiceTab} from '../../../services/models/service-tab.interface';

declare var injectSVG: any;

@Component({
    selector: 'notesmail-probe-tab-overall',
    templateUrl: '/app/dashboards/components/mail/notesmail-probe-overall-tab.component.html',
})
export class NotesMailProbeOverallTab extends WidgetController implements OnInit, ServiceTab {

    widgets: WidgetContract[];
    serviceId: string;

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService) {

        super(resolver, widgetService);

    }
    
    ngOnInit() {

        this.serviceId = this.widgetService.getProperty('serviceId');
        
        this.widgets = [
            {
                id: 'responseTimes',
                title: 'Response Time',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6 col-md-6 col-lg-6',
                settings: {
                    url: `/services/statistics?statName=ResponseTime&deviceid=${this.serviceId}&operation=hourly`,
                    dateformat: 'hour',
                    chart: {
                        chart: {
                            renderTo: 'responseTimes',
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
            }
        ];
        injectSVG();
    }
    onPropertyChanged(key: string, value: any) {

        if (key === 'serviceId') {

            this.serviceId = value;

            this.widgetService.refreshWidget('responseTimes', `/services/statistics?statName=ResponseTime&deviceid=${this.serviceId}&operation=hourly`)
                .catch(error => console.log(error));
        }

        super.onPropertyChanged(key, value);

    }

}