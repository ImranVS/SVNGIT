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
    var IBMSametimeChatsTab;
    return {
        setters:[
            function (core_1_1) {
                core_1 = core_1_1;
            },
            function (widgets_1_1) {
                widgets_1 = widgets_1_1;
            }],
        execute: function() {
            IBMSametimeChatsTab = (function (_super) {
                __extends(IBMSametimeChatsTab, _super);
                function IBMSametimeChatsTab(resolver, widgetService) {
                    _super.call(this, resolver, widgetService);
                    this.resolver = resolver;
                    this.widgetService = widgetService;
                }
                IBMSametimeChatsTab.prototype.onPropertyChanged = function (key, value) {
                    if (key === 'serviceId') {
                        this.serviceId = value;
                        this.widgetService.refreshWidget('nWayChats', "/services/statistics?statName=Numberofnwaychats&deviceid=" + this.serviceId + "&operation=hourly")
                            .catch(function (error) { return console.log(error); });
                        this.widgetService.refreshWidget('activeNWayChats', "/services/statistics?statName=Numberofactivenwaychats&deviceid=" + this.serviceId + "&operation=hourly")
                            .catch(function (error) { return console.log(error); });
                        this.widgetService.refreshWidget('openChatSessions', "/services/statistics?statName=Numberofopenchatsessions&deviceid=" + this.serviceId + "&operation=hourly")
                            .catch(function (error) { return console.log(error); });
                        this.widgetService.refreshWidget('chatMessages', "/services/statistics?statName=Numberofchatmessages&deviceid=" + this.serviceId + "&operation=hourly")
                            .catch(function (error) { return console.log(error); });
                    }
                    _super.prototype.onPropertyChanged.call(this, key, value);
                };
                IBMSametimeChatsTab.prototype.ngOnInit = function () {
                    this.serviceId = this.widgetService.getProperty('serviceId');
                    this.widgets = [
                        {
                            id: 'nWayChats',
                            title: 'N-way Chats',
                            name: 'ChartComponent',
                            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-6',
                            settings: {
                                url: "/services/statistics?statName=Numberofnwaychats&deviceid=" + this.serviceId + "&operation=hourly",
                                chart: {
                                    chart: {
                                        renderTo: 'nWayChats',
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
                            id: 'activeNWayChats',
                            title: 'Active N-way Chats',
                            name: 'ChartComponent',
                            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-6',
                            settings: {
                                url: "/services/statistics?statName=Numberofactivenwaychats&deviceid=" + this.serviceId + "&operation=hourly",
                                chart: {
                                    chart: {
                                        renderTo: 'activeNWayChats',
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
                            id: 'openChatSessions',
                            title: 'Open Chat Sessions',
                            name: 'ChartComponent',
                            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-6',
                            settings: {
                                url: "/services/statistics?statName=Numberofopenchatsessions&deviceid=" + this.serviceId + "&operation=hourly",
                                chart: {
                                    chart: {
                                        renderTo: 'openChatSessions',
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
                            id: 'chatMessages',
                            title: 'Chat Messages',
                            name: 'ChartComponent',
                            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-6',
                            settings: {
                                url: "/services/statistics?statName=Numberofchatmessages&deviceid=" + this.serviceId + "&operation=hourly",
                                chart: {
                                    chart: {
                                        renderTo: 'chatMessages',
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
                IBMSametimeChatsTab = __decorate([
                    core_1.Component({
                        selector: 'tab-chats',
                        templateUrl: '/app/dashboards/components/ibm-sametime/ibm-sametime-chats-tab.component.html'
                    }), 
                    __metadata('design:paramtypes', [core_1.ComponentFactoryResolver, widgets_1.WidgetService])
                ], IBMSametimeChatsTab);
                return IBMSametimeChatsTab;
            }(widgets_1.WidgetController));
            exports_1("IBMSametimeChatsTab", IBMSametimeChatsTab);
        }
    }
});
//# sourceMappingURL=ibm-sametime-chats-tab.component.js.map