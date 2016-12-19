import { Component, ComponentFactoryResolver, OnInit } from '@angular/core';

import { RESTService } from '../../../core/services/rest.service';

declare var Highcharts: any;

declare var injectSVG: any;

@Component({
    templateUrl: '/app/reports/components/disk/disk-space-consumption.component.html',
    providers: [
        RESTService
    ]
})
export class DiskSpaceConsumptionReport implements OnInit {
    contextMenuSiteMap: any;
    private data: any[];
    private drives: any[] = [];

    private chartTpl: any = {
        chart: {
            renderTo: null,
            type: 'pie',
            height: 200
        },
        title: { text: '' },
        subtitle: { text: '' },
        xAxis: {
            categories: []
        },
        yAxis: {
            min: 0,
            endOnTick: false,
            allowDecimals: false,
            title: {
                enabled: false
            }
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
    };

    currentHideServerControl: boolean = false;
    currentHideDatePanel: boolean = true;
    currentDeviceType: string = "Domino";
    currentWidgetName: string = `avgcpuutilchart`;
    currentWidgetURL: string = `/dashboard/overall/disk-space`;

    constructor(private service: RESTService) { }

    renderChart(ref: any) {

        let drive = this.drives[ref.id];

        let driveChart = Object.assign({}, this.chartTpl);

        driveChart.chart.renderTo = ref.clientId;

        driveChart.series = [{
            name: drive.name,
            data: [
                {
                    name: 'Percent Free',
                    y: drive.percent_free,
                    color: '#008000'
                },
                {
                    name: "Percent Used",
                    y: 1 - drive.percent_free,
                    color: '#f80000'
                }
            ]
        }];

        new Highcharts.Chart(driveChart);

    }

    ngOnInit() {
        this.service.get('/navigation/sitemaps/disk_reports')
            .subscribe
            (
            data => this.contextMenuSiteMap = data,
            error => console.log(error)
            );

        let i = 0;
        
        //http://private-f4c5b-vitalsignssandboxserver.apiary-mock.com/reports/disk-space-consumption
        this.service.get(this.currentWidgetURL)
            .subscribe((response) => {
                //data: any[];
                response.data.forEach(server => server.drives.forEach(drive => {

                    drive.id = i++;

                    this.drives.push(drive);

                }));

                this.data = response.data;

            });

        injectSVG();
        

    }
}