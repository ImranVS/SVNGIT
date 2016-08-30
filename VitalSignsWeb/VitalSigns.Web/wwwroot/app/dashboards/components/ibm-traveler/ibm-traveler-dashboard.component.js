System.register(['@angular/core', '@angular/router', 'rxjs/Rx', '../../../core/widgets', '../../../navigation/app.navigator.component'], function(exports_1, context_1) {
    "use strict";
    var __moduleName = context_1 && context_1.id;
    var __extends = (this && this.__extends) || function (d, b) {
        for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
    var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
        var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
        if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
        else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
        return c > 3 && r && Object.defineProperty(target, key, r), r;
    };
    var __metadata = (this && this.__metadata) || function (k, v) {
        if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
    };
    var core_1, router_1, widgets_1, app_navigator_component_1;
    var IBMTravelerDashboard;
    return {
        setters:[
            function (core_1_1) {
                core_1 = core_1_1;
            },
            function (router_1_1) {
                router_1 = router_1_1;
            },
            function (_1) {},
            function (widgets_1_1) {
                widgets_1 = widgets_1_1;
            },
            function (app_navigator_component_1_1) {
                app_navigator_component_1 = app_navigator_component_1_1;
            }],
        execute: function() {
            IBMTravelerDashboard = (function (_super) {
                __extends(IBMTravelerDashboard, _super);
                function IBMTravelerDashboard(resolver) {
                    _super.call(this, resolver);
                    this.resolver = resolver;
                    this.widgets = [
                        {
                            id: 'mobileUsersTable',
                            title: 'Mobile users',
                            path: '/app/widgets/mobile-users/components/mobile-users-list.component',
                            name: 'MobileUsers',
                            css: 'col-xs-12 col-sm-12 col-md-12 col-lg-8',
                            settings: {}
                        },
                        {
                            id: 'mobileDevicesChart',
                            title: 'Mobile devices',
                            path: '/app/widgets/charts/components/chart.component',
                            name: 'ChartComponent',
                            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-4',
                            settings: {
                                url: '/mobile_user_devices/count_by_type',
                                chart: {
                                    chart: {
                                        renderTo: 'mobileDevicesChart',
                                        type: 'bar',
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
                            id: 'mobileDevicesOSChart',
                            title: 'Mobile devices OS for all Servers',
                            path: '/app/widgets/charts/components/chart.component',
                            name: 'ChartComponent',
                            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-4',
                            settings: {
                                url: '/mobile_user_devices/count_by_os',
                                chart: {
                                    chart: {
                                        renderTo: 'mobileDevicesOSChart',
                                        type: 'bar',
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
                            id: 'syncTimeChart',
                            title: 'Sync times',
                            path: '/app/widgets/charts/components/chart.component',
                            name: 'ChartComponent',
                            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-2',
                            settings: {
                                url: '/mobile_user_devices/group_by_sync_interval',
                                chart: {
                                    chart: {
                                        renderTo: 'syncTimeChart',
                                        type: 'pie',
                                        height: 300
                                    },
                                    title: { text: '' },
                                    subtitle: { text: '' },
                                    credits: {
                                        enabled: false
                                    },
                                    exporting: {
                                        enabled: false
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
                                    tooltip: {
                                        formatter: function () {
                                            return '<div style="font-size: 11px; font-weight: normal;">' + this.key + '<br /><strong>' + this.y + '</strong> (' + this.percentage.toFixed(1) + '%)</div>';
                                        },
                                        useHTML: true
                                    },
                                    legend: {
                                        labelFormatter: function () {
                                            return '<div style="font-size: 10px; font-weight: normal;">' + this.name + '</div>';
                                        }
                                    },
                                    series: []
                                }
                            }
                        },
                        {
                            id: 'deviceCountUserChart',
                            title: 'Device count / user',
                            path: '/app/widgets/charts/components/chart.component',
                            name: 'ChartComponent',
                            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-2',
                            settings: {
                                url: '/mobile_user_devices/count_per_user',
                                chart: {
                                    chart: {
                                        renderTo: 'deviceCountUserChart',
                                        type: 'pie',
                                        height: 300
                                    },
                                    title: { text: '' },
                                    subtitle: { text: '' },
                                    credits: {
                                        enabled: false
                                    },
                                    exporting: {
                                        enabled: false
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
                                    tooltip: {
                                        formatter: function () {
                                            return '<div style="font-size: 11px; font-weight: normal;">' + this.key + '<br /><strong>' + this.y + '</strong> (' + this.percentage.toFixed(1) + '%)</div>';
                                        }
                                    },
                                    legend: {
                                        labelFormatter: function () {
                                            return '<div style="font-size: 10px; font-weight: normal;">' + this.name + '</div>';
                                        }
                                    },
                                    series: []
                                }
                            }
                        }
                    ];
                }
                IBMTravelerDashboard.prototype.ngOnInit = function () {
                    injectSVG();
                    bootstrapNavigator();
                };
                IBMTravelerDashboard = __decorate([
                    core_1.Component({
                        selector: 'traveler-dashboard',
                        templateUrl: '/app/dashboards/components/ibm-traveler-dashboard.component.html',
                        directives: [router_1.ROUTER_DIRECTIVES, widgets_1.WidgetContainer, app_navigator_component_1.AppNavigator]
                    }), 
                    __metadata('design:paramtypes', [core_1.ComponentResolver])
                ], IBMTravelerDashboard);
                return IBMTravelerDashboard;
            }(widgets_1.WidgetController));
            exports_1("IBMTravelerDashboard", IBMTravelerDashboard);
        }
    }
});
//# sourceMappingURL=ibm-traveler-dashboard.component.js.map