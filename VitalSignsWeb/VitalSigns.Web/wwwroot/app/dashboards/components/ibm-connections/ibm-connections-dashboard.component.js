System.register(['@angular/core', '@angular/router', '../../../core/widgets', '../../../navigation/app.navigator.component'], function(exports_1, context_1) {
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
    var core_1, router_1, widgets_1, app_navigator_component_1;
    var IBMConnectionsDashboard;
    return {
        setters:[
            function (core_1_1) {
                core_1 = core_1_1;
            },
            function (router_1_1) {
                router_1 = router_1_1;
            },
            function (widgets_1_1) {
                widgets_1 = widgets_1_1;
            },
            function (app_navigator_component_1_1) {
                app_navigator_component_1 = app_navigator_component_1_1;
            }],
        execute: function() {
            IBMConnectionsDashboard = (function (_super) {
                __extends(IBMConnectionsDashboard, _super);
                function IBMConnectionsDashboard(resolver) {
                    _super.call(this, resolver);
                    this.resolver = resolver;
                    this.widgets = [
                        {
                            id: 'connectionsDetails',
                            title: 'Connections Info',
                            path: '/app/dashboards/components/ibm-connections/ibm-connections-details.component',
                            name: 'IBMConnectionsDetails',
                            css: 'col-xs-12 col-sm-12 col-md-12 col-lg-12',
                            settings: {}
                        }
                    ];
                }
                IBMConnectionsDashboard.prototype.ngOnInit = function () {
                    injectSVG();
                    bootstrapNavigator();
                };
                IBMConnectionsDashboard = __decorate([
                    core_1.Component({
                        templateUrl: '/app/dashboards/components/ibm-connections/ibm-connections-dashboard.component.html',
                        directives: [router_1.ROUTER_DIRECTIVES, widgets_1.WidgetContainer, app_navigator_component_1.AppNavigator]
                    }), 
                    __metadata('design:paramtypes', [core_1.ComponentResolver])
                ], IBMConnectionsDashboard);
                return IBMConnectionsDashboard;
            }(widgets_1.WidgetController));
            exports_1("IBMConnectionsDashboard", IBMConnectionsDashboard);
        }
    }
});
//# sourceMappingURL=ibm-connections-dashboard.component.js.map