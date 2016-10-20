System.register(['@angular/core', '../../core/widgets'], function(exports_1, context_1) {
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
    var ServiceOverallTab;
    return {
        setters:[
            function (core_1_1) {
                core_1 = core_1_1;
            },
            function (widgets_1_1) {
                widgets_1 = widgets_1_1;
            }],
        execute: function() {
            ServiceOverallTab = (function (_super) {
                __extends(ServiceOverallTab, _super);
                function ServiceOverallTab(resolver, widgetService) {
                    _super.call(this, resolver, widgetService);
                    this.resolver = resolver;
                    this.widgetService = widgetService;
                }
                ServiceOverallTab.prototype.ngOnInit = function () {
                    this.widgets = [
                        {
                            id: 'usersConnectionsDuringTheDay',
                            title: 'Users connections during the day',
                            name: 'ChartComponent',
                            css: 'col-xs-12 col-sm-12 col-md-6 col-lg-6',
                            settings: {
                                url: "/services/statistics?statname=Server.Users&deviceId=" + this.serviceId + "&operation=hourly",
                                chart: {
                                    chart: {
                                        renderTo: 'usersConnectionsDuringTheDay',
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
                            id: 'diskSpace',
                            title: 'Disk space',
                            name: 'ChartComponent',
                            css: 'col-xs-12 col-sm-12 col-md-6 col-lg-6',
                            settings: {
                                url: "/services/disk_space?deviceid=" + this.serviceId,
                                chart: {
                                    chart: {
                                        renderTo: 'diskSpace',
                                        type: 'bar',
                                        height: 300
                                    },
                                    title: { text: '' },
                                    subtitle: { text: '' },
                                    credits: {
                                        enabled: false
                                    },
                                    xAxis: {
                                        categories: []
                                    },
                                    yAxis: {
                                        min: 0,
                                        title: {
                                            text: 'Disk Space (GB)'
                                        }
                                    },
                                    exporting: {
                                        enabled: false
                                    },
                                    plotOptions: {
                                        pie: {
                                            allowPointSelect: true,
                                            cursor: 'pointer',
                                            dataLabels: {
                                                enabled: true,
                                                format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                                                style: {
                                                    color: 'black'
                                                }
                                            }
                                        },
                                        series: {
                                            stacking: 'normal'
                                        }
                                    },
                                    tooltip: {
                                        formatter: function () {
                                            return '<div style="font-size: 11px; font-weight: normal;">' + this.series.name + '<br /><strong>' + this.y.toFixed(2) + '</strong> (' + this.percentage.toFixed(1) + '%)</div>';
                                        },
                                        useHTML: true
                                    },
                                    series: [],
                                    colors: ['#5FBE7F', '#EF3A24']
                                }
                            }
                        },
                        {
                            id: 'cpuUsage',
                            title: 'CPU Usage',
                            name: 'ChartComponent',
                            css: 'col-xs-12 col-sm-12 col-md-6 col-lg-6',
                            settings: {
                                url: "/services/statistics?statname=Platform.System.PctCombinedCpuUtil&deviceId=" + this.serviceId + "&operation=hourly",
                                chart: {
                                    chart: {
                                        renderTo: 'cpuUsage',
                                        type: 'areaspline',
                                        height: 300
                                    },
                                    colors: ['#ef3a24'],
                                    title: { text: '' },
                                    subtitle: { text: '' },
                                    xAxis: {
                                        labels: {
                                            step: 6
                                        },
                                        categories: []
                                    },
                                    yAxis: {
                                        title: {
                                            text: 'Percent'
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
                                    series: [{
                                            name: '% Usage',
                                            data: []
                                        }]
                                }
                            }
                        },
                        {
                            id: 'memoryUsage',
                            title: 'Memory Usage',
                            name: 'ChartComponent',
                            css: 'col-xs-12 col-sm-12 col-md-6 col-lg-6',
                            settings: {
                                url: "/services/statistics?statname=Mem.PercentUsed&deviceId=" + this.serviceId + "&operation=hourly",
                                chart: {
                                    chart: {
                                        renderTo: 'memoryUsage',
                                        type: 'areaspline',
                                        height: 300
                                    },
                                    colors: ['#848484'],
                                    title: { text: '' },
                                    subtitle: { text: '' },
                                    xAxis: {
                                        labels: {
                                            step: 6
                                        },
                                        categories: []
                                    },
                                    yAxis: {
                                        title: {
                                            text: 'Percent'
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
                                    series: [{
                                            name: 'GB',
                                            data: []
                                        }]
                                }
                            }
                        }
                    ];
                    injectSVG();
                };
                ServiceOverallTab = __decorate([
                    core_1.Component({
                        selector: 'tab-overall',
                        templateUrl: '/app/services/components/service-overall-tab.component.html',
                        providers: [widgets_1.WidgetService]
                    }), 
                    __metadata('design:paramtypes', [core_1.ComponentFactoryResolver, widgets_1.WidgetService])
                ], ServiceOverallTab);
                return ServiceOverallTab;
            }(widgets_1.WidgetController));
            exports_1("ServiceOverallTab", ServiceOverallTab);
        }
    }
});
//# sourceMappingURL=service-overall-tab.component.js.map