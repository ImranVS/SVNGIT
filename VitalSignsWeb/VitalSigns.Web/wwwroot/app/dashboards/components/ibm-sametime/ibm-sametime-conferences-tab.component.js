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
    var IBMSametimeConferencesTab;
    return {
        setters:[
            function (core_1_1) {
                core_1 = core_1_1;
            },
            function (widgets_1_1) {
                widgets_1 = widgets_1_1;
            }],
        execute: function() {
            IBMSametimeConferencesTab = (function (_super) {
                __extends(IBMSametimeConferencesTab, _super);
                function IBMSametimeConferencesTab(resolver, widgetService) {
                    _super.call(this, resolver, widgetService);
                    this.resolver = resolver;
                    this.widgetService = widgetService;
                }
                IBMSametimeConferencesTab.prototype.ngOnInit = function () {
                    this.serviceId = this.widgetService.getProperty('serviceId');
                    this.widgets = [
                        {
                            id: 'oneOnOneCalls',
                            title: 'One-on-one Calls',
                            name: 'ChartComponent',
                            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-6',
                            settings: {
                                url: "/services/statistics?statName=Totalcountofall1x1calls&deviceid=" + this.serviceId + "&operation=hourly",
                                chart: {
                                    chart: {
                                        renderTo: 'oneOnOneCalls',
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
                            id: 'multiUserCalls',
                            title: 'Multi-user Calls',
                            name: 'ChartComponent',
                            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-6',
                            settings: {
                                url: "/services/statistics?statName=Totalcountofallmultiusercalls&deviceid=" + this.serviceId + "&operation=hourly",
                                chart: {
                                    chart: {
                                        renderTo: 'multiUserCalls',
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
                            id: 'allCalls',
                            title: 'All Calls',
                            name: 'ChartComponent',
                            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-6',
                            settings: {
                                url: "/services/statistics?statName=Totalcountofallcalls&deviceid=" + this.serviceId + "&operation=hourly",
                                chart: {
                                    chart: {
                                        renderTo: 'allCalls',
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
                            id: 'allCallsAllUsers',
                            title: 'All Calls/All Users',
                            name: 'ChartComponent',
                            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-6',
                            settings: {
                                url: "/services/statistics?statName=[Countofallcalls,Countofallusers]&deviceid=" + this.serviceId + "&operation=hourly",
                                chart: {
                                    chart: {
                                        renderTo: 'allCallsAllUsers',
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
                            id: 'allOneOnOneCallsUsers',
                            title: 'All One-on-one Calls/All One-on-one Call Users',
                            name: 'ChartComponent',
                            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-6',
                            settings: {
                                url: "/services/statistics?statName=[Countofall1x1calls,Countofall1x1users]&deviceid=" + this.serviceId + "&operation=hourly",
                                chart: {
                                    chart: {
                                        renderTo: 'allOneOnOneCallsUsers',
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
                            id: 'allMultiUserCallsUsers',
                            title: 'All Multi-user Calls/All Multi-user Call Users',
                            name: 'ChartComponent',
                            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-6',
                            settings: {
                                url: "/services/statistics?statName=[Countofallmultiusercalls,Countofallmultiuserusers]&deviceid=" + this.serviceId + "&operation=hourly",
                                chart: {
                                    chart: {
                                        renderTo: 'allMultiUserCallsUsers',
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
                IBMSametimeConferencesTab.prototype.onPropertyChanged = function (key, value) {
                    if (key === 'serviceId') {
                        this.serviceId = value;
                        this.widgetService.refreshWidget('multiUserCalls', "/services/statistics?statName=Totalcountofallmultiusercalls&deviceid=" + this.serviceId + "&operation=hourly")
                            .catch(function (error) { return console.log(error); });
                        this.widgetService.refreshWidget('allCalls', "/services/statistics?statName=Totalcountofallcalls&deviceid=" + this.serviceId + "&operation=hourly")
                            .catch(function (error) { return console.log(error); });
                        this.widgetService.refreshWidget('allCallsAllUsers', "/services/statistics?statName=[Countofallcalls,Countofallusers]&deviceid=" + this.serviceId + "&operation=hourly")
                            .catch(function (error) { return console.log(error); });
                        this.widgetService.refreshWidget('allOneOnOneCallsUsers', "/services/statistics?statName=[Countofall1x1calls,Countofall1x1users]&deviceid=" + this.serviceId + "&operation=hourly")
                            .catch(function (error) { return console.log(error); });
                        this.widgetService.refreshWidget('allMultiUserCallsUsers', "/services/statistics?statName=[Countofallmultiusercalls,Countofallmultiuserusers]&deviceid=" + this.serviceId + "&operation=hourly")
                            .catch(function (error) { return console.log(error); });
                    }
                    _super.prototype.onPropertyChanged.call(this, key, value);
                };
                IBMSametimeConferencesTab = __decorate([
                    core_1.Component({
                        selector: 'tab-conferences',
                        templateUrl: '/app/dashboards/components/ibm-sametime/ibm-sametime-conferences-tab.component.html'
                    }), 
                    __metadata('design:paramtypes', [core_1.ComponentFactoryResolver, widgets_1.WidgetService])
                ], IBMSametimeConferencesTab);
                return IBMSametimeConferencesTab;
            }(widgets_1.WidgetController));
            exports_1("IBMSametimeConferencesTab", IBMSametimeConferencesTab);
        }
    }
});
//# sourceMappingURL=ibm-sametime-conferences-tab.component.js.map