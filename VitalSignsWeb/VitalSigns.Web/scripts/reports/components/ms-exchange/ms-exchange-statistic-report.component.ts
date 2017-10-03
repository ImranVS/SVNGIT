import { Component, ComponentFactoryResolver, OnInit } from '@angular/core';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';
import { WidgetController, WidgetContract } from '../../../core/widgets';
import { WidgetService } from '../../../core/widgets/services/widget.service';
import { RESTService } from '../../../core/services/rest.service';
import * as helpers from '../../../core/services/helpers/helpers';
declare var injectSVG: any;

@Component({
    templateUrl: '/app/reports/components/ms-exchange/ms-exchange-statistic-report.component.html',
    providers: [
        WidgetService,
        RESTService,
        helpers.UrlHelperService
         
    ]
})
export class ExchnageStatisticReport extends WidgetController {
    contextMenuSiteMap: any;
    widgets: WidgetContract[];
    paramtype: string;
    currentDeviceType: string = "Exchnage";
    currentWidgetName: string = `statisticchart`;
    currentWidgetURL: string;
    
    statName: string;
    constructor(
        protected resolver: ComponentFactoryResolver,
        protected widgetService: WidgetService,
        private service: RESTService,
        private router: Router,
        private route: ActivatedRoute,
        protected urlHelpers: helpers.UrlHelperService) {

        super(resolver, widgetService, true, router, route);

    }

    ngOnInit() {
        this.route.queryParams.subscribe(params => {
            this.statName = params['statname'];
        });
        this.service.get('/navigation/sitemaps/exchange_reports')
            .subscribe
            (
            data => this.contextMenuSiteMap = data,
            error => console.log(error)
            );

        this.currentWidgetURL = `/reports/summarystats_chart`;
        this.widgets = [
            {
                id: 'statisticchart',
                title: '',
                name: 'ChartComponent',
                settings: {
                    url: this.currentWidgetURL + '?statName=[Mail_DeliverySuccessRate,Mail_DeliverCount,Mail_ReceivedCount,Mail_FailCount,Mail_ReceivedSizeMB,Mail_SentCount,Mail_SentSizeMB]',
                    dateformat: 'date',
                    chart: {
                        chart: {
                            renderTo: 'statisticchart',
                            type: 'spline',
                            height: 540
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
                                enabled: true,
                                text: 'Percent'
                            }
                        },
                        plotOptions: {
                            bar: {
                                dataLabels: {
                                    enabled: false
                                },
                                groupPadding: 0.1,
                                borderWidth: 0
                            },
                            series: {
                                pointPadding: 0
                            }
                        },
                        legend: {
                            enabled: true
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

    //onPropertyChanged(key: string, value: any) {
    //    if (key === 'widgetTitle') {
    //        this.widgets[0].title = value;
    //    }
    //}
    ///reports/summarystats_aggregation ? type = Exchange & aggregationType=sum & statName=[Mail.Transferred, Mail.TotalRouted, Mail.Delivered]`;
    
}