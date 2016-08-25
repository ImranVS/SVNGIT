System.register(['@angular/core', '@angular/router', 'rxjs/Rx', '../../core/widgets', '../../navigation/app.navigator.component'], function(exports_1, context_1) {
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
    var MainDashboard;
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
            MainDashboard = (function (_super) {
                __extends(MainDashboard, _super);
                function MainDashboard(resolver) {
                    _super.call(this, resolver);
                    this.resolver = resolver;
                    this.widgetAppsStatus = [
                        {
                            id: 'widgetOnPremisesApps',
                            title: null,
                            path: '/app/widgets/main-dashboard/components/app-status.component',
                            name: 'AppStatus',
                            css: null,
                            settings: {
                                serviceId: 1
                            }
                        },
                        {
                            id: 'widgetOnPremisesApps',
                            title: null,
                            path: '/app/widgets/main-dashboard/components/app-status.component',
                            name: 'AppStatus',
                            css: null,
                            settings: {
                                serviceId: 2
                            }
                        },
                        {
                            id: 'widgetOnPremisesApps',
                            title: null,
                            path: '/app/widgets/main-dashboard/components/app-status.component',
                            name: 'AppStatus',
                            css: null,
                            settings: {
                                serviceId: 3
                            }
                        },
                        {
                            id: 'widgetOnPremisesApps',
                            title: null,
                            path: '/app/widgets/main-dashboard/components/app-status.component',
                            name: 'AppStatus',
                            css: null,
                            settings: {
                                serviceId: 4
                            }
                        }
                    ];
                    this.widgetOnPremisesApps = {
                        id: 'widgetOnPremisesApps',
                        title: 'On premises applications',
                        path: '/app/widgets/main-dashboard/components/on-premises-apps.component',
                        name: 'OnPremisesApps',
                        css: 'col-md-6 col-lg-12',
                        settings: {}
                    };
                    this.widgetStatusSummary = {
                        id: 'widgetStatusSummary',
                        title: null,
                        path: '/app/widgets/main-dashboard/components/status-summary.component',
                        name: 'StatusSummary',
                        css: null,
                        settings: {}
                    };
                    this.weeklyEvents = {
                        id: 'weeklyEventsRepartitionChartWrapper',
                        title: 'Weekly events repartition',
                        path: '/app/widgets/charts/components/chart.component',
                        name: 'ChartComponent',
                        css: 'col-lg-12 col-md-6 col-sm-6',
                        settings: {
                            url: '/server_health/weekly_events',
                            chart: {
                                chart: {
                                    renderTo: 'weeklyEventsRepartitionChartWrapper',
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
                    };
                    this.lastMonthEvents = {
                        id: 'eventsCountChartWrapper',
                        title: 'Last month events',
                        path: '/app/widgets/charts/components/chart.component',
                        name: 'ChartComponent',
                        css: 'col-md-6 col-lg-12',
                        settings: {
                            url: '/server_health/last_month_events',
                            chart: {
                                chart: {
                                    renderTo: 'eventsCountChartWrapper',
                                    type: 'column',
                                    height: 270
                                },
                                title: { text: '' },
                                subtitle: { text: '' },
                                credits: { enabled: false },
                                exporting: { enabled: false },
                                legend: { enabled: false },
                                tooltip: {
                                    formatter: function () {
                                        return '<div style="font-size: 11px; font-weight: normal;">' + this.x + '<br /><strong>' + this.y + '</strong></div>';
                                    },
                                    useHTML: true
                                },
                                xAxis: {
                                    categories: []
                                },
                                yAxis: {
                                    title: { text: '' }
                                },
                                series: []
                            }
                        }
                    };
                }
                MainDashboard.prototype.ngOnInit = function () {
                    injectSVG();
                    bootstrapNavigator();
                };
                MainDashboard = __decorate([
                    core_1.Component({
                        selector: 'traveler-dashboard',
                        templateUrl: '/app/dashboards/components/main-dashboard.component.html',
                        directives: [router_1.ROUTER_DIRECTIVES, widgets_1.WidgetContainer, app_navigator_component_1.AppNavigator]
                    }), 
                    __metadata('design:paramtypes', [core_1.ComponentResolver])
                ], MainDashboard);
                return MainDashboard;
            }(widgets_1.WidgetController));
            exports_1("MainDashboard", MainDashboard);
        }
    }
});
//# sourceMappingURL=main-dashboard.component.js.map