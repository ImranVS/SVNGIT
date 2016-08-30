System.register(['@angular/core', '@angular/router', '../../../core/widgets', '../../../navigation/app.navigator.component'], function(exports_1, context_1) {
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
    var OfficeMailStatistics;
    return {
        setters:[
            function (core_1_1) {
                core_1 = core_1_1;
            },
            function (router_1_1) {
                router_1 = router_1_1;
            },
            function (widgets_1_1) {
                widgets_1 = widgets_1_1;
            },
            function (app_navigator_component_1_1) {
                app_navigator_component_1 = app_navigator_component_1_1;
            }],
        execute: function() {
            OfficeMailStatistics = (function (_super) {
                __extends(OfficeMailStatistics, _super);
                function OfficeMailStatistics(resolver) {
                    _super.call(this, resolver);
                    this.resolver = resolver;
                    this.widgets = [
                        {
                            id: 'top5ActiveMailboxes',
                            title: 'Top 5 mailboxes',
                            path: '/app/widgets/charts/components/chart.component',
                            name: 'ChartComponent',
                            css: 'col-xs-12 col-sm-8',
                            settings: {
                                url: '/mobile_user_devices/count_by_type',
                                chart: {
                                    chart: {
                                        renderTo: 'top5ActiveMailboxes',
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
                            id: 'activeUsers',
                            title: 'Active users',
                            path: '/app/widgets/charts/components/chart.component',
                            name: 'ChartComponent',
                            css: 'col-xs-12 col-sm-4',
                            settings: {
                                url: '/mobile_user_devices/count_by_type',
                                chart: {
                                    chart: {
                                        renderTo: 'activeUsers',
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
                                        bar: {
                                            dataLabels: {
                                                enabled: false
                                            },
                                            groupPadding: 0.1,
                                            borderWidth: 0
                                        },
                                        series: {
                                            pointPadding: 0
                                        },
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
                            id: 'top5InactiveMailboxes',
                            title: 'Top 5 inactive mailboxes',
                            path: '/app/widgets/charts/components/chart.component',
                            name: 'ChartComponent',
                            css: 'col-xs-12 col-sm-8',
                            settings: {
                                url: '/mobile_user_devices/count_by_type',
                                chart: {
                                    chart: {
                                        renderTo: 'top5InactiveMailboxes',
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
                            id: 'inactiveUsers',
                            title: 'Inactive users',
                            path: '/app/widgets/charts/components/chart.component',
                            name: 'ChartComponent',
                            css: 'col-xs-12 col-sm-4',
                            settings: {
                                url: '/mobile_user_devices/count_by_type',
                                chart: {
                                    chart: {
                                        renderTo: 'inactiveUsers',
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
                                        bar: {
                                            dataLabels: {
                                                enabled: false
                                            },
                                            groupPadding: 0.1,
                                            borderWidth: 0
                                        },
                                        series: {
                                            pointPadding: 0
                                        },
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
                        }
                    ];
                }
                OfficeMailStatistics.prototype.ngOnInit = function () {
                    injectSVG();
                };
                OfficeMailStatistics = __decorate([
                    core_1.Component({
                        templateUrl: '/app/dashboards/components/office365-mail-statistics.component.html',
                        directives: [router_1.ROUTER_DIRECTIVES, widgets_1.WidgetContainer, app_navigator_component_1.AppNavigator]
                    }), 
                    __metadata('design:paramtypes', [core_1.ComponentResolver])
                ], OfficeMailStatistics);
                return OfficeMailStatistics;
            }(widgets_1.WidgetController));
            exports_1("OfficeMailStatistics", OfficeMailStatistics);
        }
    }
});
//# sourceMappingURL=office365-mail-statistics.component.js.map