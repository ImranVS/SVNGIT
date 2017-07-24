import { Component, Input, OnInit } from '@angular/core';
import { HttpModule } from '@angular/http';

import { WidgetComponent, WidgetController } from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import { RESTService } from '../../../core/services';

import * as helpers from '../../../core/services/helpers/helpers';

import { Chart } from '../models/chart';

import { ChartSeries } from '../models/chart-series';

declare var Highcharts: any;

@Component({
    template: '',
    providers: [
        HttpModule,
        RESTService,
        helpers.DateTimeHelper
    ]
})
export class ChartComponent implements WidgetComponent, OnInit {
    @Input() settings: any;
    
    errorMessage: string;
    chart: any;
    subseries: any;

    constructor(private service: RESTService, private widgetService: WidgetService, protected datetimeHelpers: helpers.DateTimeHelper) { }

    refresh(serviceUrl?: string) {

        this.loadData(serviceUrl);

    }

    ngOnInit() {

        this.loadData();

    }

    private loadData(serviceUrl?: string) {

        this.service.get(serviceUrl || this.settings.url)
            .subscribe(data => {
                if (this.settings.overrideName) {
                    data.data.series.forEach(serie => serie.title = this.settings.overrideName);
                }
                switch (this.settings.dateformat) {
                    case "date":
                        data.data = this.datetimeHelpers.toLocalDate(data.data);
                        break;
                    case "datetime":
                        data.data = this.datetimeHelpers.toLocalDateTime(data.data);
                        break;
                    case "hour":
                        data.data = this.datetimeHelpers.toLocalHour(data.data);
                        break;
                    case "time":
                        data.data = this.datetimeHelpers.toLocalTime(data.data);
                        break;
                }
                if (this.chart) {
                    this.settings.chart.series = [];
                    this.chart.destroy();
                }

                let chart = <Chart>data.data;

                let categories: string[] = []

                chart.series.map(serie => {

                    let length = this.settings.chart.series.push({
                        name: null,
                        data: []
                    });

                    this.settings.chart.series[length - 1].name = serie.title;
                    //NS added the line below this.settings.chart.xAxis.categories = []; - the categories array needs 
                    //to be cleared every time, otherwise, the chart does not redraw x axis values if the data is updated
                    this.settings.chart.xAxis.categories = [];

                    // First loop to gather all data points labels
                    serie.segments.map(segment => {

                        if (this.settings.chart.xAxis) {
                            if (categories.indexOf(segment.label) == -1)
                                categories.push(segment.label);
                        }

                    });
                    
                    this.settings.chart.xAxis.categories = categories;

                    // Second loop to build data points with actual value or null if missing 
                    categories.map(category => {

                        let segment = serie.segments.find(s => s.label == category);

                        if (segment)
                            this.settings.chart.series[length - 1].data.push({
                                name: category,
                                y: segment.value && !isNaN(segment.value) ? segment.value : 0,
                                color: segment.color,
                                drilldown: segment.drilldownname
                            });
                        else
                            this.settings.chart.series[length - 1].data.push({
                                name: category,
                                y: null
                            });

                    });

                    // TODO: [OM] not obvious to hard code string value there and it introduces a strong dependency with business rules
                    if (this.settings.chart.xAxis.categories.length > 1 && serie.title == "Available" || serie.title == "Used") {

                        this.settings.chart.chart.type = 'bar';
                    }
                    else if (serie.title.startsWith("Disk")) {

                        this.settings.chart.chart.type = 'pie';
                    }

                });

                this.subseries = [];

                //START DRILLDOWN
                if (chart.series2 != null) {
                    let chart2 = <Chart>data.data;

                    let categories2: string[] = []
                    chart2.series2.map(serie => {

                        let length = this.subseries.push({
                            id: null,
                            data: []
                        });

                        // First loop to gather all data points labels
                        serie.segments.map(segment => {

                            if (this.settings.chart.xAxis) {
                                if (categories2.indexOf(segment.label) == -1)
                                    categories2.push(segment.label);
                            }

                        });

                        // Second loop to build data points with actual value or null if missing 
                        categories2.map(category => {

                            let segment = serie.segments.find(s => s.label == category);
                            var x = [];
                            if (segment) {
                                x.push(segment.label);
                                x.push(segment.value);
                                this.subseries[length - 1].name = segment.drilldownname;
                                this.subseries[length - 1].id = segment.drilldownname;
                                this.subseries[length - 1].data.push(x);
                            }
                        });
                    });
                    this.subseries.splice(0, 1);
                    let chartSeries = new ChartSeries();
                    chartSeries.series = this.subseries;
                    chart.drilldown = chartSeries;
                    this.settings.chart.drilldown = chart.drilldown;
                }
                //END DRILLDOWN

                this.chart = new Highcharts.Chart(this.settings.chart);
                var chartjson = JSON.stringify(this.settings.chart);

                if (this.settings.callback)
                    this.settings.callback(this.settings.chart);

            },
            error => this.errorMessage = <any>error);

    }
}