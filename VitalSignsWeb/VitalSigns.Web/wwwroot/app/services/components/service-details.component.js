System.register(['@angular/core', '@angular/router', '@angular/http', '../../core/services', '../service-tab.collection'], function(exports_1, context_1) {
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
    var core_1, router_1, http_1, router_2, services_1, ServiceTabs;
    var ServiceDetails;
    return {
        setters:[
            function (core_1_1) {
                core_1 = core_1_1;
            },
            function (router_1_1) {
                router_1 = router_1_1;
                router_2 = router_1_1;
            },
            function (http_1_1) {
                http_1 = http_1_1;
            },
            function (services_1_1) {
                services_1 = services_1_1;
            },
            function (ServiceTabs_1) {
                ServiceTabs = ServiceTabs_1;
            }],
        execute: function() {
            ServiceDetails = (function () {
                function ServiceDetails(dataProvider, resolver, elementRef, router, route) {
                    this.dataProvider = dataProvider;
                    this.resolver = resolver;
                    this.elementRef = elementRef;
                    this.router = router;
                    this.route = route;
                    //.map(routeParams => routeParams.id);
                }
                ServiceDetails.prototype.selectTab = function (tab) {
                    // Activate selected tab
                    this.service.tabs.forEach(function (tab) { return tab.active = false; });
                    tab.active = true;
                    // Dispose current tab if one already active
                    if (this.activeTabComponent)
                        this.activeTabComponent.destroy();
                    // Lazy-load selected tab component
                    var factory = this.resolver.resolveComponentFactory(ServiceTabs[tab.component]);
                    this.activeTabComponent = this.target.createComponent(factory);
                    (this.activeTabComponent.instance).serviceId = this.deviceId;
                };
                ServiceDetails.prototype.ngOnInit = function () {
                    var _this = this;
                    console.log(this.router.routerState);
                    var parentActivatedRoute = this.router.routerState.root.children[0].params;
                    parentActivatedRoute.subscribe(function (params) {
                        _this.module = params['module'];
                    });
                    this.route.params.subscribe(function (params) {
                        _this.deviceId = params['service'];
                        // Get tabs associated with selected service
                        _this.dataProvider.get("/services/device_details?device_id=" + _this.deviceId + "&destination=" + _this.module)
                            .subscribe(function (response) {
                            _this.service = response.data;
                            _this.selectTab(_this.service.tabs[0]);
                            console.log(_this.service.tabs);
                        }, function (error) { return _this.errorMessage = error; });
                    });
                };
                ServiceDetails.prototype.getStatusDescription = function (status) {
                    switch (status) {
                        case 'ok':
                            return 'OK';
                        case 'notresponding':
                            return 'No <br /> resp.';
                        case 'issue':
                            return 'Issue';
                        case 'maintenance':
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
                            http_1.HttpModule,
                            services_1.RESTService
                        ]
                    }), 
                    __metadata('design:paramtypes', [services_1.RESTService, core_1.ComponentFactoryResolver, core_1.ElementRef, router_2.Router, router_1.ActivatedRoute])
                ], ServiceDetails);
                return ServiceDetails;
            }());
            exports_1("ServiceDetails", ServiceDetails);
        }
    }
});
//# sourceMappingURL=service-details.component.js.map