import {Component, ComponentResolver, OnInit} from '@angular/core';
import {ROUTER_DIRECTIVES} from '@angular/router';

import {WidgetController, WidgetContainer, WidgetContract} from '../../core/widgets';
import {AppNavigator} from '../../navigation/app.navigator.component';


import {ServiceTab} from '../models/service-tab.interface';
declare var injectSVG: any;
declare var bootstrapNavigator: any;

@Component({
    templateUrl: '/app/services/components/service-travelerhealth-tab.component.html',
    directives: [ROUTER_DIRECTIVES, WidgetContainer, AppNavigator]
})
export class ServiceTravelerHealth extends WidgetController implements OnInit {

    widgets: WidgetContract[]; 
       
       
    
    constructor(protected resolver: ComponentResolver) {
        super(resolver);
    }

    ngOnInit() {
       
        this.widgets = [
            {
                id: 'dynamicGrid',
                title: 'Traveler Health',
                path: '/app/widgets/grid/components/dynamic-grid.component',
                name: 'DynamicGrid',
                css: 'col-xs-12',
                settings: {
                    
                    url: '/DashBoard/578fc2e21d2f58b3d1a398ff/traveler-health',
                    columns: [{ header: "Resource Constraint", binding: "resource_constraint", name: "resource_constraint", width: "*" },
                        { header: "Traveler Details", binding: "traveler_details", name: "traveler_details", width: "*" },
                        { header: "Traveler HeartBeat", binding: "traveler_heartbeat", name: "traveler_heartbeat", width: "*" },
                        { header: "Traveler Servlet", binding: "traveler_servlet", name: "traveler_servlet", width: "*" },
                        { header: "Traveler Users", binding: "traveler_users", name: "traveler_users", width: "*" },
                        { header: "Traveler HA", binding: "traveler_ha", name: "traveler_ha", width: "*" },
                        { header: "Traveler IncrementalSyncs", binding: "traveler_incremental_syncs", name: "traveler_incremental_syncs", width: "*" },
                        { header: "Http Status", binding: "http_status", name: "http_status", width: "*" },
                        { header: "Traveler DevicesAPIStatus", binding: "traveler_devices_api_status", name: "traveler_devices_api_status", width: "*" }

                    ]

                }
            },
            {
                id: 'successfuldevicesyncs',
                title: 'Successful Device Syncs',
                path: '/app/widgets/charts/components/chart.component',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-12 col-md-6 col-lg-6',
                settings: {
                    url: 'http://localhost:1234/services/statistics?statName=Traveler.IncrementalDeviceSyncs&deviceid=4&operation=hourly',
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
                    url: 'http://localhost:1234/services/statistics?statName=Http.CurrentConnections&deviceid=4&operation=hourly',
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
                    url: 'http://localhost:1234/services/statistics?statName=Traveler.Memory.Java.Current&deviceid=4&operation=hourly',
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
                    url: 'http://localhost:1234/services/statistics?statName=Traveler.Memory.C.Current&deviceid=4&operation=hourly',
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
                id: 'sampleGrid',
                title: 'Traveler Health Mail Servers',
                path: '/app/widgets/grid/components/sample-grid.component',
                name: 'SampleGrid',
                css: 'col-xs-12',
                settings: {
                    url: 'http://localhost:1234/DashBoard/57ad00715c6c6c0efcdf6a73/traveler-mailservers'
                }
            }

        ]

        

        injectSVG();
        bootstrapNavigator();
    }

}