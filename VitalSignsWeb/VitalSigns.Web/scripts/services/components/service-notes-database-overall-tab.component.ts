import {Component, ComponentFactoryResolver, OnInit, Injector} from '@angular/core';

import {WidgetController, WidgetContainer, WidgetContract} from '../../core/widgets';
import { WidgetService } from '../../core/widgets/services/widget.service';
import { RESTService } from '../../core/services';

import {ServiceTab} from '../models/service-tab.interface';

declare var injectSVG: any;

@Component({
    selector: 'tab-notes-database-overall',
    templateUrl: '/app/services/components/service-notes-database-overall-tab.component.html',
    providers: [
        WidgetService,
        RESTService
    ]
})
export class ServiceNotesDatabaseOverallTab extends WidgetController implements OnInit, ServiceTab {

    widgets: WidgetContract[];
    serviceId: string;
    chartName: any;
    data: any;
    errorMessage: any;
    messageText: string;

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private service: RESTService ) {
        super(resolver, widgetService);
    }
    
    ngOnInit() {

        this.service.get(`/dashboard/notes_database_test_result?deviceId=${this.serviceId}`)
            .subscribe(
            response => {
                this.data = response.data;
           
                this.messageText = 'Notes Databases can be monitored for replication, response time to open the default view, elapsed time to refresh all views, or based on document count. This graph represents ' + this.data.name + '. The most recent recorded value is ' + this.data.value;
            },
            error => this.errorMessage = <any>error
            );

        this.widgets = [
            {
                id: 'usersConnectionsDuringTheDay',
                title: this.chartName,
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-12 col-md-12 col-lg-12',
                settings: {
                    url: `/services/statistics?statname=ResponseTime&deviceId=${this.serviceId}&operation=hourly`,
                    dateformat: 'time',
                    overrideName: "milliseconds",
                    chart: {
                        chart: {
                            renderTo: 'usersConnectionsDuringTheDay',
                            type: 'areaspline',
                            height: 300
                        },
                        //colors: ['#5fbe7f'],
                        title: { text: '' },
                        subtitle: { text: '' },
                        yAxis: {
                       
                            title: { text: 'Milliseconds' }

                        },
                        xAxis: {
                            labels: {
                                step: 3
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