System.register(['@angular/core', '@angular/http', '../../../core/services/rest.service'], function(exports_1, context_1) {
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
    var core_1, http_1, rest_service_1;
    var Office365Dashboard;
    return {
        setters:[
            function (core_1_1) {
                core_1 = core_1_1;
            },
            function (http_1_1) {
                http_1 = http_1_1;
            },
            function (rest_service_1_1) {
                rest_service_1 = rest_service_1_1;
            }],
        execute: function() {
            Office365Dashboard = (function () {
                function Office365Dashboard(service) {
                    this.service = service;
                }
                Office365Dashboard.prototype.ngOnInit = function () {
                    var _this = this;
                    this.service.get('/navigation/sitemaps/office365')
                        .subscribe(function (data) { return _this.contextMenuSiteMap = data; }, function (error) { return console.log(error); });
                    injectSVG();
                    bootstrapNavigator();
                };
                Office365Dashboard = __decorate([
                    core_1.Component({
                        templateUrl: '/app/dashboards/components/office365/office365-dashboard.component.html',
                        providers: [
                            http_1.HttpModule,
                            rest_service_1.RESTService
                        ]
                    }), 
                    __metadata('design:paramtypes', [rest_service_1.RESTService])
                ], Office365Dashboard);
                return Office365Dashboard;
            }());
            exports_1("Office365Dashboard", Office365Dashboard);
        }
    }
});
//# sourceMappingURL=office365-dashboard.component.js.map