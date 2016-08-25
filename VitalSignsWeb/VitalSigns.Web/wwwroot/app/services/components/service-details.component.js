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
    var ServiceDetails;
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
            ServiceDetails = (function () {
                function ServiceDetails(dataProvider, resolver, elementRef, route) {
                    this.dataProvider = dataProvider;
                    this.resolver = resolver;
                    this.elementRef = elementRef;
                    this.route = route;
                }
                ServiceDetails.prototype.selectTab = function (tab) {
                    var _this = this;
                    // Activate selected tab
                    this.service.tabs.forEach(function (tab) { return tab.active = false; });
                    tab.active = true;
                    // Dispose current tab if one already active
                    if (this.activeTabComponent)
                        this.activeTabComponent.destroy();
                    // Lazy-load selected tab component
                    System.import(tab.path).then(function (component) {
                        _this.resolver
                            .resolveComponent(component[tab.component])
                            .then(function (factory) {
                            _this.activeTabComponent = _this.target.createComponent(factory);
                            _this.activeTabComponent.instance.serviceId = _this.serviceId;
                        });
                    });
                };
                ServiceDetails.prototype.ngOnInit = function () {
                    var _this = this;
                    this.route.params.subscribe(function (params) {
                        _this.serviceId = params['service'];
                        // Get tabs associated with selected service
                        _this.dataProvider.get("/services/" + _this.serviceId)
                            .subscribe(function (data) {
                            _this.service = data;
                            _this.selectTab(_this.service.tabs[0]);
                        }, function (error) { return _this.errorMessage = error; });
                    });
                };
                ServiceDetails.prototype.getStatusDescription = function (status) {
                    switch (status) {
                        case 'noIssue':
                            return 'No <br /> issue';
                        case 'notResponding':
                            return 'No <br /> resp.';
                        case 'issues':
                            return 'Issues';
                        case 'inMaintenance':
                            return 'Mainten.';
                    }
                };
                __decorate([
                    core_1.ViewChild('tab', { read: core_1.ViewContainerRef }), 
                    __metadata('design:type', core_1.ViewContainerRef)
                ], ServiceDetails.prototype, "target", void 0);
                ServiceDetails = __decorate([
                    core_1.Component({
                        templateUrl: '/app/services/components/service-details.component.html',
                        providers: [
                            http_1.HTTP_PROVIDERS,
                            services_1.RESTService
                        ]
                    }), 
                    __metadata('design:paramtypes', [services_1.RESTService, core_1.ComponentResolver, core_1.ElementRef, router_1.ActivatedRoute])
                ], ServiceDetails);
                return ServiceDetails;
            }());
            exports_1("ServiceDetails", ServiceDetails);
        }
    }
});
//# sourceMappingURL=service-details.component.js.map