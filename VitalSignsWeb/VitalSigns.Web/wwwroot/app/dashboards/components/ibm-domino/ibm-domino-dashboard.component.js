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
    var IBMDominoDashboard;
    return {
        setters:[
            function (core_1_1) {
                core_1 = core_1_1;
            },
            function (widgets_1_1) {
                widgets_1 = widgets_1_1;
            }],
        execute: function() {
            IBMDominoDashboard = (function (_super) {
                __extends(IBMDominoDashboard, _super);
                function IBMDominoDashboard(resolver, widgetService) {
                    _super.call(this, resolver, widgetService);
                    this.resolver = resolver;
                    this.widgetService = widgetService;
                    this.widgets = [
                        {
                            id: 'dominoGrid',
                            title: 'Domino Info',
                            name: 'IBMDominoGrid',
                            css: 'col-xs-12 col-sm-12  col-md-12 col-lg-9',
                            settings: {}
                        },
                        {
                            id: 'serverRoles',
                            title: 'Roles',
                            name: 'ChartComponent',
                            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-3',
                            settings: {
                                url: '/services/status_count?type=Domino&docfield=secondary_role',
                                chart: {
                                    chart: {
                                        renderTo: 'serverRoles',
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
                            id: 'serverStatus',
                            title: 'Status',
                            name: 'ChartComponent',
                            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-3',
                            settings: {
                                url: '/services/status_count?type=Domino&docfield=status_code',
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
                            id: 'serverOs',
                            title: 'Operating Systems',
                            name: 'ChartComponent',
                            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-3',
                            settings: {
                                url: '/services/status_count?type=Domino&docfield=operating_system',
                                chart: {
                                    chart: {
                                        renderTo: 'serverOs',
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
                        }
                    ];
                }
                IBMDominoDashboard.prototype.ngOnInit = function () {
                    injectSVG();
                    bootstrapNavigator();
                };
                IBMDominoDashboard = __decorate([
                    core_1.Component({
                        selector: 'sample-dashboard',
                        templateUrl: '/app/dashboards/components/ibm-domino/ibm-domino-dashboard.component.html',
                        providers: [widgets_1.WidgetService]
                    }), 
                    __metadata('design:paramtypes', [core_1.ComponentFactoryResolver, widgets_1.WidgetService])
                ], IBMDominoDashboard);
                return IBMDominoDashboard;
            }(widgets_1.WidgetController));
            exports_1("IBMDominoDashboard", IBMDominoDashboard);
        }
    }
});
//# sourceMappingURL=ibm-domino-dashboard.component.js.map