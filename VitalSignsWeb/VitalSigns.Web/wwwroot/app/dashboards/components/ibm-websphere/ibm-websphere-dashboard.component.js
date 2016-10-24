System.register(['@angular/core', 'rxjs/Rx', '../../../core/widgets'], function(exports_1, context_1) {
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
    var core_1, widgets_1;
    var IBMWebsphereDashboard;
    return {
        setters:[
            function (core_1_1) {
                core_1 = core_1_1;
            },
            function (_1) {},
            function (widgets_1_1) {
                widgets_1 = widgets_1_1;
            }],
        execute: function() {
            IBMWebsphereDashboard = (function (_super) {
                __extends(IBMWebsphereDashboard, _super);
                function IBMWebsphereDashboard(resolver, widgetService) {
                    _super.call(this, resolver, widgetService);
                    this.resolver = resolver;
                    this.widgetService = widgetService;
                    this.widgets = [
                        {
                            id: 'websphereGrid',
                            title: 'Cells',
                            name: 'IBMWebsphereGrid',
                            css: 'col-xs-12 col-sm-12  col-md-12 col-lg-8',
                            settings: {}
                        },
                        {
                            id: 'serverStatus',
                            title: 'Status',
                            name: 'ChartComponent',
                            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-4',
                            settings: {
                                url: '/services/status_count?type=[WebSphereCell,WebSphereNode,WebSphere]&docfield=status_code',
                                chart: {
                                    chart: {
                                        renderTo: 'serverStatus',
                                        type: 'pie',
                                        height: 240
                                    },
                                    title: { text: '' },
                                    subtitle: { text: '' },
                                    xAxis: {
                                        categories: []
                                    },
                                    yAxis: {},
                                    plotOptions: {
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
                            id: 'websphereNodeGrid',
                            title: 'Nodes',
                            name: 'IBMWebsphereNodeGrid',
                            css: 'col-xs-12 col-sm-12  col-md-12 col-lg-8',
                            settings: {}
                        },
                        {
                            id: 'activeThreads',
                            title: 'Active Thread Count',
                            name: 'ChartComponent',
                            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-4',
                            settings: {
                                url: '/services/statistics?statName=ActiveThreadCount&operation=AVG&isChart=true',
                                chart: {
                                    chart: {
                                        renderTo: 'activeThreads',
                                        type: 'bar',
                                        height: 240
                                    },
                                    colors: ['#5fbe7f'],
                                    title: { text: '' },
                                    subtitle: { text: '' },
                                    xAxis: {
                                        labels: {
                                            step: 1
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
                            id: 'websphereServerGrid',
                            title: 'Servers',
                            name: 'IBMWebsphereServerGrid',
                            css: 'col-xs-12 col-sm-12  col-md-12 col-lg-8',
                            settings: {}
                        },
                        {
                            id: 'hungThreads',
                            title: 'Hung Thread Count',
                            name: 'ChartComponent',
                            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-4',
                            settings: {
                                url: '/services/statistics?statName=CurrentHungThreadCount&operation=AVG&isChart=true',
                                chart: {
                                    chart: {
                                        renderTo: 'hungThreads',
                                        type: 'bar',
                                        height: 240
                                    },
                                    colors: ['#5fbe7f'],
                                    title: { text: '' },
                                    subtitle: { text: '' },
                                    xAxis: {
                                        labels: {
                                            step: 1
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
                        }
                    ];
                }
                IBMWebsphereDashboard.prototype.ngOnInit = function () {
                    injectSVG();
                    bootstrapNavigator();
                };
                IBMWebsphereDashboard.prototype.onSelect = function (serviceId) {
                    this.serviceId = serviceId;
                };
                IBMWebsphereDashboard = __decorate([
                    core_1.Component({
                        selector: 'websphere-dashboard',
                        templateUrl: '/app/dashboards/components/ibm-websphere/ibm-websphere-dashboard.component.html',
                        providers: [widgets_1.WidgetService]
                    }), 
                    __metadata('design:paramtypes', [core_1.ComponentFactoryResolver, widgets_1.WidgetService])
                ], IBMWebsphereDashboard);
                return IBMWebsphereDashboard;
            }(widgets_1.WidgetController));
            exports_1("IBMWebsphereDashboard", IBMWebsphereDashboard);
        }
    }
});
//# sourceMappingURL=ibm-websphere-dashboard.component.js.map