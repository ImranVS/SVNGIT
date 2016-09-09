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

                //9/6/2016 NS modified the call below - the API returns series data at the child level, e.g.,
                //{"data":{"title":"Http.CurrentConnections","series":[{"title":"Http.CurrentConnections","segments":[{"label":"01:00 AM","value":9.3809523809523814},{"label":"02:00 AM","value":9.0714285714285712},{"label":"03:00 AM","value":9.3684210526315788},{"label":"04:00 AM","value":9.11111111111111},{"label":"05:00 AM","value":8.3},{"label":"06:00 AM","value":8.8461538461538467},{"label":"07:00 AM","value":8.31578947368421},{"label":"08:00 AM","value":9.0},{"label":"09:00 AM","value":9.2222222222222214},{"label":"10:00 AM","value":8.3333333333333339},{"label":"11:00 AM","value":9.0},{"label":"12:00 PM","value":9.2222222222222214},{"label":"01:00 PM","value":9.2941176470588243},{"label":"02:00 PM","value":9.75},{"label":"03:00 PM","value":9.8636363636363633},{"label":"04:00 PM","value":9.384615384615385},{"label":"05:00 PM","value":9.4642857142857135},{"label":"06:00 PM","value":9.0476190476190474},{"label":"07:00 PM","value":8.5769230769230766},{"label":"08:00 PM","value":8.64516129032258},{"label":"09:00 PM","value":8.2857142857142865},{"label":"10:00 PM","value":8.8},{"label":"11:00 PM","value":9.25925925925926},{"label":"12:00 AM","value":0.0}]}]},"status":"OK","message":"Success"}
                //So when assigning a chart variable, need to get to the sub-level by calling .data
                //var chart = <Chart>data;
                var chart = <Chart>data.data;
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