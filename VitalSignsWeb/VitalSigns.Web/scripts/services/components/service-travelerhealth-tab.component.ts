import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';

import {WidgetController, WidgetContainer, WidgetContract} from '../../core/widgets';
import {WidgetService} from '../../core/widgets/services/widget.service';
import {AppNavigator} from '../../navigation/app.navigator.component';


import {ServiceTab} from '../models/service-tab.interface';

declare var injectSVG: any;


@Component({
    templateUrl: '/app/services/components/service-travelerhealth-tab.component.html',
    providers: [WidgetService]
})
export class ServiceTravelerHealthTab extends WidgetController implements OnInit {
    deviceId: any;
    service: any;
    widgets: WidgetContract[]; 
       
    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private route: ActivatedRoute) {
        super(resolver, widgetService);
    }

    ngOnInit() {
        this.route.params.subscribe(params => {
            this.deviceId = params['service'];

        });
        this.widgets = [
            {
                id: 'ServiceTravelerHealthGrid',
                title: 'Traveler Health',
                name: 'ServiceTravelerHealthGrid',
                css: 'col-xs-12'
            },
            {
                id: 'successfuldevicesyncs',
                title: 'Successful Device Syncs',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-12 col-md-6 col-lg-6',
                settings: {
                    url: `/services/statistics?statName=Traveler.IncrementalDeviceSyncs&deviceid=${this.deviceId}&operation=hourly`,
                    chart: {
                        chart: {
                            renderTo: 'successfuldevicesyncs',
                            type: 'areaspline',
                            height: 300
                        },
                        colors: ['#5fbe7f'],
                        title: { text: '' },
                        subtitle: { text: '' },
                        xAxis: {
                            labels: {
                                step: 6
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
            },

            {
                id: 'httpsessions',
                title: 'HTTP Sessions',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-12 col-md-6 col-lg-6',
                settings: {
                    url: `/services/statistics?statName=Http.CurrentConnections&deviceid=${this.deviceId}&operation=hourly`,
                    chart: {
                        chart: {
                            renderTo: 'httpsessions',
                            type: 'areaspline',
                            height: 300
                        },
                        colors: ['#5fbe7f'],
                        title: { text: '' },
                        subtitle: { text: '' },
                        xAxis: {
                            labels: {
                                step: 6
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
            },
            {
                id: 'allocatedjavamemory',
                title: 'Allocated Java Memory',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-12 col-md-6 col-lg-6',
                settings: {
                    url: `/services/statistics?statName=Traveler.Memory.Java.Current&deviceid=${this.deviceId}&operation=hourly`,
                    chart: {
                        chart: {
                            renderTo: 'allocatedjavamemory',
                            type: 'areaspline',
                            height: 300
                        },
                        colors: ['#5fbe7f'],
                        title: { text: '' },
                        subtitle: { text: '' },
                        xAxis: {
                            labels: {
                                step: 6
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
            },

            {
                id: 'allocatedCmemory',
                title: 'Allocated C Memory',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-12 col-md-6 col-lg-6',
                settings: {
                    url: `/services/statistics?statName=Traveler.Memory.C.Current&deviceid=${this.deviceId}&operation=hourly`,
                    chart: {
                        chart: {
                            renderTo: 'allocatedCmemory',
                            type: 'areaspline',
                            height: 300
                        },
                        colors: ['#5fbe7f'],
                        title: { text: '' },
                        subtitle: { text: '' },
                        xAxis: {
                            labels: {
                                step: 6
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
            },

          {
              id: 'ServiceTravelerMailServersGrid',
                title: 'Traveler Health Mail Servers',
                name: 'ServiceTravelerMailServersGrid',
                css: 'col-xs-12'
            }
        ]


        injectSVG();
        
    }

}