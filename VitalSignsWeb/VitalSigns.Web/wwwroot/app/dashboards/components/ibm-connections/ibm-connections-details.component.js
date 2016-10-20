System.register(['@angular/core', '@angular/router', '@angular/http', '../../../core/widgets', '../../../core/services', '../../../services/service-tab.collection'], function(exports_1, context_1) {
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
    var core_1, router_1, http_1, widgets_1, services_1, ServiceTabs;
    var IBMConnectionsDetails;
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
            function (widgets_1_1) {
                widgets_1 = widgets_1_1;
            },
            function (services_1_1) {
                services_1 = services_1_1;
            },
            function (ServiceTabs_1) {
                ServiceTabs = ServiceTabs_1;
            }],
        execute: function() {
            IBMConnectionsDetails = (function () {
                function IBMConnectionsDetails(dataProvider, widgetService, resolver, elementRef, route) {
                    this.dataProvider = dataProvider;
                    this.widgetService = widgetService;
                    this.resolver = resolver;
                    this.elementRef = elementRef;
                    this.route = route;
                }
                IBMConnectionsDetails.prototype.selectTab = function (tab) {
                    // Activate selected tab
                    this.service.tabs.forEach(function (tab) { return tab.active = false; });
                    tab.active = true;
                    // Dispose current tab if one already active
                    if (this.activeTabComponent)
                        this.activeTabComponent.destroy();
                    // Lazy-load selected tab component
                    var factory = this.resolver.resolveComponentFactory(ServiceTabs[tab.component]);
                    this.activeTabComponent = this.target.createComponent(factory);
                    (this.activeTabComponent.instance).serviceId = this.serviceId;
                };
                IBMConnectionsDetails.prototype.ngOnInit = function () {
                    var _this = this;
                    this.route.params.subscribe(function (params) {
                        // Get tabs associated with selected service
                        _this.dataProvider.get("/services/device_details?device_id=" + _this.widgetService.getProperty('serviceId') + "&destination=dashboard")
                            .subscribe(function (data) {
                            _this.service = data.data;
                            _this.selectTab(_this.service.tabs[0]);
                        }, function (error) { return _this.errorMessage = error; });
                    });
                };
                IBMConnectionsDetails.prototype.getStatusDescription = function (status) {
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
                ], IBMConnectionsDetails.prototype, "target", void 0);
                IBMConnectionsDetails = __decorate([
                    core_1.Component({
                        selector: 'vs-connections-details',
                        templateUrl: '/app/dashboards/components/ibm-connections/ibm-connections-details.component.html',
                        providers: [
                            http_1.HttpModule,
                            services_1.RESTService
                        ]
                    }), 
                    __metadata('design:paramtypes', [services_1.RESTService, widgets_1.WidgetService, core_1.ComponentFactoryResolver, core_1.ElementRef, router_1.ActivatedRoute])
                ], IBMConnectionsDetails);
                return IBMConnectionsDetails;
            }());
            exports_1("IBMConnectionsDetails", IBMConnectionsDetails);
        }
    }
});
//# sourceMappingURL=ibm-connections-details.component.js.map