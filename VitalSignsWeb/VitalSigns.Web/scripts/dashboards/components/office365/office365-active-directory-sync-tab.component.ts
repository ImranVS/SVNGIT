import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';
import { Office365Grid } from './office365-grid.component';
import { ActivatedRoute } from '@angular/router'; 
import { ActiveDirectoryStatsGrid } from './office365-active-directory-stats-grid.component';
import {WidgetController, WidgetContainer, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import { AppNavigator } from '../../../navigation/app.navigator.component';
import { ServiceTab } from '../../../services/models/service-tab.interface';
declare var injectSVG: any;
@Component({
    templateUrl: '/app/dashboards/components/office365/office365-active-directory-sync-tab.component.html'
    //providers: [WidgetService]
})
export class Office365ActiveDirectorySyncTab extends WidgetController implements OnInit, ServiceTab {
    serviceId: string;
    widgets: WidgetContract[]
    nodeName: string;

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private route: ActivatedRoute) {
        super(resolver, widgetService);
    }

    ngOnInit() {
        this.route.params.subscribe(params => {
            if (params['service']) {
                var res: string[] = params['service'].split(';');
                if (res.length > 1) {
                    this.nodeName = res[1];
                }
                this.serviceId = res[0];
            }
            else {
                var res: string[] = this.serviceId.split(';');
                if (res.length > 1) {
                    this.nodeName = res[1];
                }
                this.serviceId = res[0];
            }
        });
        this.widgets = [
            {
                id: 'activedirectorysync',
                title: 'Active Directory Sync',
                name: 'ChartComponent',
                css: 'col-xs-9 col-sm-4',
                settings: {
                    url: `/dashboard/group_by_ad_sync_interval?deviceId=${this.serviceId}`,
                    chart: {
                        chart: {
                            renderTo: 'activedirectorysync',
                            type: 'pie',
                            height: 240
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
                        plotOptions: {
                            pie: {
                                allowPointSelect: true,
                                cursor: 'pointer',
                                dataLabels: {
                                    enabled: false
                                },
                                showInLegend: true,
                                innerSize: '70%'
                            }
                        },
                        legend: {
                            labelFormatter: function () {
                                return '<div style="font-size: 10px; font-weight: normal;">' + this.name + '</div>';
                            }
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
            },
            {
                id: 'ActivedirectoryStatsGrid',
                title: 'AD Status Grid',
                name: 'ActiveDirectoryStatsGrid',
                css: 'col-xs-15 col-sm-8 col-md-6 col-lg-6',
                settings: { ServiceId: this.serviceId }
            },
        ];
        injectSVG();
    }

    onPropertyChanged(key: string, value: any) {

        if (key === 'serviceId') {
            this.serviceId = value;

            this.widgetService.refreshWidget('activedirectorysync', `/dashboard/group_by_ad_sync_interval?deviceId=${this.serviceId}`)
                .catch(error => console.log(error));
            this.widgetService.refreshWidget('ActivedirectoryStatsGrid', { ServiceId: this.serviceId })
                .catch(error => console.log(error));

        }

    }
}