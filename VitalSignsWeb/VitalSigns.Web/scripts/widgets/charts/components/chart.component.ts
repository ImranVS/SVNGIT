import {Component, Input, OnInit} from '@angular/core';
import {HTTP_PROVIDERS}    from '@angular/http';

import {WidgetComponent} from '../../../core/widgets';
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

    constructor(private service: RESTService) { }

    ngOnInit() {
        
        this.service.get(this.settings.url)
            .subscribe(
            data => {
            
                var chart = <Chart>data;
                var first = true;
                
                chart.series.map(serie => {

                    var length = this.settings.chart.series.push({
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
                
                new Highcharts.Chart(this.settings.chart);
            },
            error => this.errorMessage = <any>error
            );
            
    }
}