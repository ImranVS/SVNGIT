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
    var IBMSametimeOverallTab;
    return {
        setters:[
            function (core_1_1) {
                core_1 = core_1_1;
            },
            function (widgets_1_1) {
                widgets_1 = widgets_1_1;
            }],
        execute: function() {
            IBMSametimeOverallTab = (function (_super) {
                __extends(IBMSametimeOverallTab, _super);
                function IBMSametimeOverallTab(resolver, widgetService) {
                    _super.call(this, resolver, widgetService);
                    this.resolver = resolver;
                    this.widgetService = widgetService;
                }
                IBMSametimeOverallTab.prototype.ngOnInit = function () {
                    this.serviceId = this.widgetService.getProperty('serviceId');
                    this.widgets = [
                        {
                            id: 'responseTimes',
                            title: 'Response Time',
                            name: 'ChartComponent',
                            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-6',
                            settings: {
                                url: "/services/statistics?statName=ResponseTime&deviceid=" + this.serviceId + "&operation=hourly",
                                chart: {
                                    chart: {
                                        renderTo: 'responseTimes',
                                        type: 'areaspline',
                                        height: 300
                                    },
                                    colors: ['#5fbe7f'],
                                    title: { text: '' },
                                    subtitle: { text: '' },
                                    xAxis: {
                                        labels: {
                                            step: 4
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
                            id: 'dailyUserLogins',
                            title: 'Daily User Logins',
                            name: 'ChartComponent',
                            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-6',
                            settings: {
                                url: "/services/statistics?statName=Users&deviceid=" + this.serviceId + "&operation=hourly",
                                chart: {
                                    chart: {
                                        renderTo: 'dailyUserLogins',
                                        type: 'areaspline',
                                        height: 300
                                    },
                                    colors: ['#5fbe7f'],
                                    title: { text: '' },
                                    subtitle: { text: '' },
                                    xAxis: {
                                        labels: {
                                            step: 4
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
                IBMSametimeOverallTab.prototype.onPropertyChanged = function (key, value) {
                    if (key === 'serviceId') {
                        this.serviceId = value;
                        this.widgetService.refreshWidget('responseTimes', "/services/statistics?statName=ResponseTime&deviceid=" + this.serviceId + "&operation=hourly")
                            .catch(function (error) { return console.log(error); });
                        this.widgetService.refreshWidget('dailyUserLogins', "/services/statistics?statName=Users&deviceid=" + this.serviceId + "&operation=hourly")
                            .catch(function (error) { return console.log(error); });
                    }
                    _super.prototype.onPropertyChanged.call(this, key, value);
                };
                IBMSametimeOverallTab = __decorate([
                    core_1.Component({
                        selector: 'tab-overall',
                        templateUrl: '/app/dashboards/components/ibm-sametime/ibm-sametime-overall-tab.component.html',
                    }), 
                    __metadata('design:paramtypes', [core_1.ComponentFactoryResolver, widgets_1.WidgetService])
                ], IBMSametimeOverallTab);
                return IBMSametimeOverallTab;
            }(widgets_1.WidgetController));
            exports_1("IBMSametimeOverallTab", IBMSametimeOverallTab);
        }
    }
});
//# sourceMappingURL=ibm-sametime-overall-tab.component.js.map