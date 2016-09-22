﻿import {Component, ComponentResolver, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';

import {WidgetController, WidgetContainer, WidgetContract, WidgetService} from '../../core/widgets';
import {AppNavigator} from '../../navigation/app.navigator.component';


import {ServiceTab} from '../models/service-tab.interface';
declare var injectSVG: any;
declare var bootstrapNavigator: any;

@Component({
    templateUrl: '/app/services/components/service-travelerhealth-tab.component.html',
    directives: [WidgetContainer, AppNavigator],
    providers: [WidgetService]
})
export class ServiceTravelerHealth extends WidgetController implements OnInit {
    deviceId: any;
    widgets: WidgetContract[]; 
    //widgetmail: WidgetContract;
       
       
    
    constructor(protected resolver: ComponentResolver, protected widgetService: WidgetService, private route: ActivatedRoute) {
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
                path: '/app/services/components/service-travelerhealth-grid.component',
                name: 'ServiceTravelerHealthGrid',
                css: 'col-xs-12'
            },

            {
                id: 'successfuldevicesyncs',
                title: 'Successful Device Syncs',
                path: '/app/widgets/charts/components/chart.component',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-12 col-md-6 col-lg-6',
                settings: {
                    url: '/services/statistics?statName=Traveler.IncrementalDeviceSyncs&deviceid=' + this.deviceId +'&operation=hourly',
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
                path: '/app/widgets/charts/components/chart.component',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-12 col-md-6 col-lg-6',
                settings: {
                    url: '/services/statistics?statName=Http.CurrentConnections&deviceid=' + this.deviceId +'&operation=hourly',
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
                path: '/app/widgets/charts/components/chart.component',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-12 col-md-6 col-lg-6',
                settings: {
                    url: '/services/statistics?statName=Traveler.Memory.Java.Current&deviceid=' + this.deviceId +'&operation=hourly',
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
                path: '/app/widgets/charts/components/chart.component',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-12 col-md-6 col-lg-6',
                settings: {
                    url: '/services/statistics?statName=Traveler.Memory.C.Current&deviceid=' + this.deviceId +'&operation=hourly',
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
                path: '/app/services/components/service-travelermailservers-grid.component',
                name: 'ServiceTravelerMailServersGrid',
                css: 'col-xs-12'
            }
        ]

        //this.widgetmail =
            
        //        {
           
        //        id: 'ServiceTravelerMailServersGrid',
        //        title: 'Traveler Health Mail Servers',
        //        path: '/app/services/components/service-travelerhealth-grid.component',
        //        name: 'ServiceTravelerHealthGrid',
        //        css: 'col-xs-12'
         

        //        }
         
        

        injectSVG();
        bootstrapNavigator();
    }

}