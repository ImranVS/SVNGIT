import { Component, Input, OnInit } from '@angular/core';
import { HttpModule } from '@angular/http';

import { WidgetComponent, WidgetController } from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import { RESTService } from '../../../core/services';

import * as helpers from '../../../core/services/helpers/helpers';

declare var Highcharts: any;

@Component({
    template: '<loading-indicator [isLoading]="isLoading"></loading-indicator>',
    providers: [
        HttpModule,
        RESTService,
        helpers.DateTimeHelper
    ]
})
export class BubbleChartComponent implements WidgetComponent, OnInit {
    @Input() settings: any;
    chartData: any;
    activitiesList: string[];
    categoriesList: string[];
    errorMessage: string;
    chart: any;

    private isLoading: boolean = true;

    constructor(private service: RESTService, private widgetService: WidgetService, protected datetimeHelpers: helpers.DateTimeHelper) { }

    refresh(serviceUrl?: string) {

        this.loadData(serviceUrl);

    }

    ngOnInit() {

        this.loadData();

    }
    private loadData(serviceUrl?: string) {
        this.isLoading = true;
        this.service.get(serviceUrl || this.settings.url)
            .subscribe(
            (data) => {
                this.activitiesList = data.data[1];
                this.categoriesList = data.data[2];
                switch (this.settings.dateformat) {
                    case "date":
                        this.chartData = this.datetimeHelpers.toLocalDate(data.data[0]);
                        break;
                    case "datetime":
                        this.chartData = this.datetimeHelpers.toLocalDateTime(data.data[0]);
                        break;
                    case "hour":
                        this.chartData = this.datetimeHelpers.toLocalHour(data.data[0]);
                        break;
                    case "time":
                        this.chartData = this.datetimeHelpers.toLocalTime(data.data[0]);
                        break;
                    default:
                        this.chartData = data.data[0];
                        break;                
                }
                      
                
                if (this.chart) {
                    this.settings.chart.series = [];
                    this.chart.destroy();
                }

                let chart = this.chartData;
                let categories: string[] = [];
                let categories2: string[] = [];
                chart.series.map(serie => {

                    let length = this.settings.chart.series.push({
                        name: null,
                        data: []
                    });

                    this.settings.chart.series[length - 1].name = serie.title;
                    //NS added the line below this.settings.chart.xAxis.categories = []; - the categories array needs 
                    //to be cleared every time, otherwise, the chart does not redraw x axis values if the data is updated
                    this.settings.chart.xAxis.categories = [];
                    this.settings.chart.yAxis.categories = [];

                    // First loop to gather all data points labels
                    serie.segments.map(segment => {

                        if (this.settings.chart.xAxis) {
                            if (categories.indexOf(segment.label) == -1)
                                categories.push(segment.label);
                        }

                    });

                    this.settings.chart.xAxis.categories = this.categoriesList;
                    this.settings.chart.yAxis.categories = this.activitiesList;
                    
                    // Second loop to build data points with actual value or null if missing 
                    categories.map(category => {
                        var found = false;
                        for (var i = 0; i < serie.segments.length; i++) {
                            if (serie.segments[i].label == category) {
                                let segment = serie.segments[i];
                                this.settings.chart.series[length - 1].data.push({
                                    name:  { x: category, y: segment.label2 },
                                    y: segment.value,
                                    x: segment.value1,
                                    z: segment.value2
                                });   
                                found = true;
                            }
                        }
                        if (!found) {
                            this.settings.chart.series[length - 1].data.push({
                                name: { x: category, y: null },
                                y: null,
                                x: null,
                                z: 0
                            });
                        }
                    });

                    var outterThis = this;
                    this.settings.chart.xAxis.tickPositioner = function () {
                        var arr = [];
                        for (var i = 0; i < outterThis.categoriesList.length; i++) {
                            arr.push(i);
                        }
                        return arr;
                    }

                    this.settings.chart.yAxis.tickPositioner = function () {
                        var arr = [];
                        for (var i = 0; i < outterThis.activitiesList.length; i++) {
                            arr.push(i);
                        }
                        return arr;
                    }
                });
                this.isLoading = false;
                this.chart = new Highcharts.Chart(this.settings.chart);
                if (this.settings.callback)
                    this.settings.callback(this.settings.chart);
                
                
            },
            error => { this.errorMessage = <any>error; this.isLoading = false; });

    }

    public getSeries() {
        return this.settings.chart.series;
    }
}