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
    var IBMSametimeMeetingsTab;
    return {
        setters:[
            function (core_1_1) {
                core_1 = core_1_1;
            },
            function (widgets_1_1) {
                widgets_1 = widgets_1_1;
            }],
        execute: function() {
            IBMSametimeMeetingsTab = (function (_super) {
                __extends(IBMSametimeMeetingsTab, _super);
                function IBMSametimeMeetingsTab(resolver) {
                    _super.call(this, resolver);
                    this.resolver = resolver;
                }
                IBMSametimeMeetingsTab.prototype.ngOnInit = function () {
                    this.widgets = [
                        {
                            id: 'activeMeetingsUsers',
                            title: 'Active Meetings/Users in Meetings',
                            path: '/app/widgets/charts/components/chart.component',
                            name: 'ChartComponent',
                            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-6',
                            settings: {
                                url: '/sametime/active_meetings_users',
                                chart: {
                                    chart: {
                                        renderTo: 'activeMeetingsUsers',
                                        type: 'areaspline',
                                        height: 300
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
                IBMSametimeMeetingsTab = __decorate([
                    core_1.Component({
                        selector: 'tab-chats',
                        templateUrl: '/app/dashboards/components/ibm-sametime-meetings-tab.component.html',
                        directives: [widgets_1.WidgetContainer]
                    }), 
                    __metadata('design:paramtypes', [core_1.ComponentResolver])
                ], IBMSametimeMeetingsTab);
                return IBMSametimeMeetingsTab;
            }(widgets_1.WidgetController));
            exports_1("IBMSametimeMeetingsTab", IBMSametimeMeetingsTab);
        }
    }
});
//# sourceMappingURL=ibm-sametime-meetings-tab.component.js.map