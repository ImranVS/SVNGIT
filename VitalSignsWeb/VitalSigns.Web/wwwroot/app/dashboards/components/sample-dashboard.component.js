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
    var SampleDashboard;
    return {
        setters:[
            function (core_1_1) {
                core_1 = core_1_1;
            },
            function (widgets_1_1) {
                widgets_1 = widgets_1_1;
            }],
        execute: function() {
            SampleDashboard = (function (_super) {
                __extends(SampleDashboard, _super);
                function SampleDashboard(resolver, widgetService) {
                    _super.call(this, resolver, widgetService);
                    this.resolver = resolver;
                    this.widgetService = widgetService;
                    this.firstWidgets = [
                        {
                            id: 'greetings1',
                            title: 'Say hello',
                            name: 'GreetingsWidget',
                            css: 'col-xs-12 col-sm-12 col-md-6 ',
                            settings: {
                                name: 'John Doe'
                            }
                        },
                        {
                            id: 'greetings2',
                            title: 'Say hello',
                            name: 'GreetingsWidget',
                            css: 'col-xs-12 col-sm-12 col-md-6 ',
                            settings: {
                                name: 'Pierre Smith'
                            }
                        },
                        {
                            id: 'greetings3',
                            title: 'Say hello',
                            name: 'MobileUsers',
                            css: 'col-xs-12',
                            settings: {}
                        }
                    ];
                }
                SampleDashboard = __decorate([
                    core_1.Component({
                        selector: 'sample-dashboard',
                        templateUrl: '/app/dashboards/components/sample-dashboard.component.html',
                        providers: [widgets_1.WidgetService]
                    }), 
                    __metadata('design:paramtypes', [core_1.ComponentFactoryResolver, widgets_1.WidgetService])
                ], SampleDashboard);
                return SampleDashboard;
            }(widgets_1.WidgetController));
            exports_1("SampleDashboard", SampleDashboard);
        }
    }
});
//# sourceMappingURL=sample-dashboard.component.js.map