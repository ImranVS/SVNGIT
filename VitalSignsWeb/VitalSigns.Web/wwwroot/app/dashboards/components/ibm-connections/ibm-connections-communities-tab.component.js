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
    var IBMConnectionsCommunitiesTab;
    return {
        setters:[
            function (core_1_1) {
                core_1 = core_1_1;
            },
            function (widgets_1_1) {
                widgets_1 = widgets_1_1;
            }],
        execute: function() {
            IBMConnectionsCommunitiesTab = (function (_super) {
                __extends(IBMConnectionsCommunitiesTab, _super);
                function IBMConnectionsCommunitiesTab(resolver, widgetService) {
                    _super.call(this, resolver, widgetService);
                    this.resolver = resolver;
                    this.widgetService = widgetService;
                }
                IBMConnectionsCommunitiesTab.prototype.ngOnInit = function () {
                    this.serviceId = this.widgetService.getProperty('serviceId');
                    var displayDate = (new Date()).toISOString().slice(0, 10);
                    this.widgets = [
                        {
                            id: 'communitiesByType',
                            title: 'Communities by Type',
                            name: 'ChartComponent',
                            css: 'col-xs-12 col-sm-6 col-md-4 col-lg-4',
                            settings: {
                                url: "/services/summarystats?statName=[COMMUNITY_TYPE_PRIVATE,COMMUNITY_TYPE_PUBLIC,COMMUNITY_TYPE_PUBLICINVITEONLY]&deviceid=" + this.serviceId + "&startDate=" + displayDate + "&endDate=" + displayDate,
                                chart: {
                                    chart: {
                                        renderTo: 'communitiesByType',
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
                            id: 'top5Communities',
                            title: 'Top 5 Most Active Communities',
                            name: 'ChartComponent',
                            css: 'col-xs-12 col-sm-6 col-md-4 col-lg-4',
                            settings: {
                                url: "/dashboard/connections/top_communities",
                                chart: {
                                    chart: {
                                        renderTo: 'top5Communities',
                                        type: 'bar',
                                        height: 240
                                    },
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
                                    plotOptions: {
                                        series: {
                                            stacking: 'normal'
                                        }
                                    },
                                    series: []
                                }
                            }
                        },
                        {
                            id: 'mostActiveCommunity',
                            title: 'Most Active Community',
                            name: 'ChartComponent',
                            css: 'col-xs-12 col-sm-6 col-md-4 col-lg-4',
                            settings: {
                                url: "/dashboard/connections/top_communities?count=1",
                                chart: {
                                    chart: {
                                        renderTo: 'mostActiveCommunity',
                                        type: 'pie',
                                        height: 240
                                    },
                                    title: { text: '' },
                                    subtitle: { text: '' },
                                    xAxis: {
                                        labels: {
                                            step: 1
                                        },
                                        categories: []
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
                                    series: []
                                }
                            }
                        }
                    ];
                    injectSVG();
                };
                IBMConnectionsCommunitiesTab = __decorate([
                    core_1.Component({
                        selector: 'tab-communities',
                        templateUrl: '/app/dashboards/components/ibm-connections/ibm-connections-communities-tab.component.html'
                    }), 
                    __metadata('design:paramtypes', [core_1.ComponentFactoryResolver, widgets_1.WidgetService])
                ], IBMConnectionsCommunitiesTab);
                return IBMConnectionsCommunitiesTab;
            }(widgets_1.WidgetController));
            exports_1("IBMConnectionsCommunitiesTab", IBMConnectionsCommunitiesTab);
        }
    }
});
//# sourceMappingURL=ibm-connections-communities-tab.component.js.map