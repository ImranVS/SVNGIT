System.register(['@angular/core', '../../../core/widgets'], function(exports_1, context_1) {
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
    var IBMConnectionsOverviewTab;
    return {
        setters:[
            function (core_1_1) {
                core_1 = core_1_1;
            },
            function (widgets_1_1) {
                widgets_1 = widgets_1_1;
            }],
        execute: function() {
            IBMConnectionsOverviewTab = (function (_super) {
                __extends(IBMConnectionsOverviewTab, _super);
                function IBMConnectionsOverviewTab(resolver, widgetService) {
                    _super.call(this, resolver, widgetService);
                    this.resolver = resolver;
                    this.widgetService = widgetService;
                }
                IBMConnectionsOverviewTab.prototype.ngOnInit = function () {
                    this.serviceId = this.widgetService.getProperty('serviceId');
                    this.widgets = [
                        {
                            id: 'dailyActivities',
                            title: 'Daily Activities',
                            name: 'ChartComponent',
                            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-6',
                            settings: {
                                url: "/services/summarystats?statName=*_CREATED_LAST_DAY&deviceid=" + this.serviceId,
                                chart: {
                                    chart: {
                                        renderTo: 'dailyActivities',
                                        type: 'spline',
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
                            id: 'top5Tags',
                            title: 'Top 5 Tags',
                            name: 'ChartComponent',
                            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-6',
                            settings: {
                                url: "/dashboard/connections/top_tags?deviceid=" + this.serviceId + "&type=Bookmark&count=5",
                                chart: {
                                    chart: {
                                        renderTo: 'top5Tags',
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
                    injectSVG();
                };
                IBMConnectionsOverviewTab = __decorate([
                    core_1.Component({
                        selector: 'tab-overall',
                        templateUrl: '/app/dashboards/components/ibm-connections/ibm-connections-overview-tab.component.html'
                    }), 
                    __metadata('design:paramtypes', [core_1.ComponentFactoryResolver, widgets_1.WidgetService])
                ], IBMConnectionsOverviewTab);
                return IBMConnectionsOverviewTab;
            }(widgets_1.WidgetController));
            exports_1("IBMConnectionsOverviewTab", IBMConnectionsOverviewTab);
        }
    }
});
//# sourceMappingURL=ibm-connections-overview-tab.component.js.map