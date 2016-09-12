import {Component, Input, OnInit} from '@angular/core';
import {HTTP_PROVIDERS}    from '@angular/http';

import {WidgetComponent, WidgetController, WidgetService} from '../../../core/widgets';
import {RESTService} from '../../../core/services';

import {Chart} from '../models/chart';

declare var Highcharts: any;

@Component({
    template: '',
    providers: [
        HTTP_PROVIDERS,
        RESTService
    ]
})
export class ChartComponent implements WidgetComponent, OnInit {
    @Input() settings: any;

    errorMessage: string;
    chart: any;

    constructor(private service: RESTService, private widgetService: WidgetService) { }

    refresh(serviceUrl?: string) {

        this.loadData(serviceUrl);

    }
    
    ngOnInit() {

        this.loadData();

    }

    private loadData(serviceUrl? : string) {

        this.service.get(serviceUrl || this.settings.url)
            .subscribe(data => {

                if (this.chart) {
                    this.settings.chart.series = [];
                    this.chart.destroy();
                }

                // TODO: think about this one!
                let chart = <Chart>data.data;
                let first = true;
                
                chart.series.map(serie => {

                    let length = this.settings.chart.series.push({
                        name: null,
                        data: []
                    });

                    this.settings.chart.series[length - 1].name = serie.title;

                    serie.segments.map(segment => {

                        if (first && this.settings.chart.xAxis)
                            this.settings.chart.xAxis.categories.push(segment.label);

                        this.settings.chart.series[length - 1].data.push({
                            name: segment.label,
                            y: segment.value,
                            color: segment.color
                        });

                    });

                    first = false;
                });
                
                this.chart = new Highcharts.Chart(this.settings.chart);

            },
            error => this.errorMessage = <any>error);

    }
}