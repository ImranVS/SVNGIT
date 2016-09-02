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
                function ServiceOverallTab(resolver) {
                    _super.call(this, resolver);
                    this.resolver = resolver;
                }
                ServiceOverallTab.prototype.ngOnInit = function () {
                    this.widgets = [
                        {
                            id: 'usersConnectionsDuringTheDay',
                            title: 'Users connections during the day',
                            path: '/app/widgets/charts/components/chart.component',
                            name: 'ChartComponent',
                            css: 'col-xs-12 col-sm-12 col-md-6 col-lg-6',
                            settings: {
                                url: '/services/1/overall/hourly-connections',
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
                            path: '/app/widgets/charts/components/chart.component',
                            name: 'ChartComponent',
                            css: 'col-xs-12 col-sm-12 col-md-6 col-lg-6',
                            settings: {
                                url: '/services/1/overall/disk-space',
                                chart: {
                                    chart: {
                                        renderTo: 'diskSpace',
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
                                            showInLegend: true
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
                                    series: [
                                        {
                                            name: 'Go',
                                            data: [],
                                            innerSize: '70%'
                                        }
                                    ]
                                }
                            }
                        },
                        {
                            id: 'cpuUsage',
                            title: 'CPU Usage',
                            path: '/app/widgets/charts/components/chart.component',
                            name: 'ChartComponent',
                            css: 'col-xs-12 col-sm-12 col-md-6 col-lg-6',
                            settings: {
                                url: "http://localhost:50278/services/" + this.serviceId + "/overall/cpu-usage",
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
                            path: '/app/widgets/charts/components/chart.component',
                            name: 'ChartComponent',
                            css: 'col-xs-12 col-sm-12 col-md-6 col-lg-6',
                            settings: {
                                url: "http://USDOPVSDEV11SVC.jnittech.com/services/" + this.serviceId + "/overall/memory-usage",
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
                        directives: [widgets_1.WidgetContainer]
                    }), 
                    __metadata('design:paramtypes', [core_1.ComponentResolver])
                ], ServiceOverallTab);
                return ServiceOverallTab;
            }(widgets_1.WidgetController));
            exports_1("ServiceOverallTab", ServiceOverallTab);
        }
    }
});
//# sourceMappingURL=service-overall-tab.component.js.map