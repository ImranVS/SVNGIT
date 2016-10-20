System.register(['@angular/core', '../../../core/widgets'], function(exports_1, context_1) {
    "use strict";
    var __moduleName = context_1 && context_1.id;
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
    var IBMSametimeDashboard;
    return {
        setters:[
            function (core_1_1) {
                core_1 = core_1_1;
            },
            function (widgets_1_1) {
                widgets_1 = widgets_1_1;
            }],
        execute: function() {
            IBMSametimeDashboard = (function () {
                function IBMSametimeDashboard() {
                }
                IBMSametimeDashboard.prototype.ngOnInit = function () {
                    injectSVG();
                    bootstrapNavigator();
                };
                IBMSametimeDashboard.prototype.onSelect = function (serviceId) {
                    this.serviceId = serviceId;
                };
                IBMSametimeDashboard = __decorate([
                    core_1.Component({
                        templateUrl: '/app/dashboards/components/ibm-sametime/ibm-sametime-dashboard.component.html',
                        providers: [widgets_1.WidgetService]
                    }), 
                    __metadata('design:paramtypes', [])
                ], IBMSametimeDashboard);
                return IBMSametimeDashboard;
            }());
            exports_1("IBMSametimeDashboard", IBMSametimeDashboard);
        }
    }
});
//# sourceMappingURL=ibm-sametime-dashboard.component.js.map