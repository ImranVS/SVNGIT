import { Component, ComponentFactoryResolver, OnInit,Input } from '@angular/core';
import { WidgetService } from '../../../core/widgets/services/widget.service';
import { WidgetComponent } from '../../../core/widgets';
import { RESTService } from '../../../core/services/rest.service';
import * as helpers from '../../../core/services/helpers/helpers';
declare var Highcharts: any;

declare var injectSVG: any;

@Component({
    templateUrl: '/app/reports/components/disk/disk-space-widget.component.html',
    providers: [
        WidgetService,
        RESTService,
        helpers.UrlHelperService
    ]
})
export class DiskSpaceWidgetReport implements WidgetComponent, OnInit {
    @Input() settings: any;
    contextMenuSiteMap: any;
    serviceId: any;
    private data: any[];
    private drives: any[] = [];
    isLoading: boolean = true;

   


    constructor(protected resolver: ComponentFactoryResolver, private service: RESTService, protected widgetService: WidgetService,
        protected urlHelpers: helpers.UrlHelperService) {
      
    }
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
    renderChart(ref: any) {

        let drive = this.drives[ref.id];

        let driveChart = Object.assign({}, this.chartTpl);

        driveChart.chart.renderTo = ref.clientId;
        //console.log(drive)
        try {
            if (drive.percent_free > 1)
                drive.percent_free = drive.disk_free / drive.disk_size;
        } catch (ex) {
            console.log(ex)
            drive.perfect_free = 0
        }

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
    loaddata(url: string){ 
        let i = 0;
        this.isLoading = true;
        //http://private-f4c5b-vitalsignssandboxserver.apiary-mock.com/reports/disk-space-consumption
        this.service.get(url)
            .finally(() => this.isLoading = false)
            .subscribe((response) => {
                this.drives = []
                //data: any[];
                response.data.forEach(server => server.drives.forEach(drive => {

                    drive.id = i++;

                    this.drives.push(drive);

                }));

                this.data = response.data;

            });

        injectSVG();
    }

    ngOnInit() {
        this.loaddata('/dashboard/overall/disk-space');
 
        injectSVG();
       



    }

    refresh(url: string) {
        this.loaddata(url);
    }
}