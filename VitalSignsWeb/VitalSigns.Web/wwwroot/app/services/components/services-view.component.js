System.register(['@angular/core', '@angular/router', '@angular/http', '../../core/services'], function(exports_1, context_1) {
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
    var core_1, router_1, http_1, services_1;
    var ServicesView;
    return {
        setters:[
            function (core_1_1) {
                core_1 = core_1_1;
            },
            function (router_1_1) {
                router_1 = router_1_1;
            },
            function (http_1_1) {
                http_1 = http_1_1;
            },
            function (services_1_1) {
                services_1 = services_1_1;
            }],
        execute: function() {
            ServicesView = (function () {
                function ServicesView(service, router) {
                    this.service = service;
                    this.router = router;
                }
                ServicesView.prototype.selectService = function (service) {
                    // Activate selected tab
                    this.services.forEach(function (service) { return service.active = false; });
                    service.active = true;
                    this.router.navigate(['services', service.id]);
                };
                ServicesView.prototype.loadData = function () {
                    var _this = this;
                    this.service.get('/services')
                        .subscribe(function (data) { return _this.services = data; }, function (error) { return _this.errorMessage = error; });
                };
                ServicesView.prototype.ngOnInit = function () {
                    this.loadData();
                };
                ServicesView.prototype.ngAfterViewChecked = function () {
                    injectSVG();
                };
                ServicesView = __decorate([
                    core_1.Component({
                        templateUrl: '/app/services/components/services-view.component.html',
                        directives: [router_1.ROUTER_DIRECTIVES],
                        providers: [
                            http_1.HTTP_PROVIDERS,
                            services_1.RESTService
                        ]
                    }), 
                    __metadata('design:paramtypes', [services_1.RESTService, router_1.Router])
                ], ServicesView);
                return ServicesView;
            }());
            exports_1("ServicesView", ServicesView);
        }
    }
});
//# sourceMappingURL=services-view.component.js.map