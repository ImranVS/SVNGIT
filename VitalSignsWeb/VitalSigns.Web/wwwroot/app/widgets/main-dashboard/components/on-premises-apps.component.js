System.register(['@angular/core', '@angular/http', '../../../core/services'], function(exports_1, context_1) {
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
    var core_1, http_1, services_1;
    var OnPremisesApps;
    return {
        setters:[
            function (core_1_1) {
                core_1 = core_1_1;
            },
            function (http_1_1) {
                http_1 = http_1_1;
            },
            function (services_1_1) {
                services_1 = services_1_1;
            }],
        execute: function() {
            OnPremisesApps = (function () {
                function OnPremisesApps(service) {
                    this.service = service;
                }
                OnPremisesApps.prototype.loadData = function () {
                    var _this = this;
                    this.service.get('/services/status_summary_by_type')
                        .subscribe(function (data) { return _this.onPremApps = data; }, function (error) { return _this.errorMessage = error; });
                };
                OnPremisesApps.prototype.ngOnInit = function () {
                    this.loadData();
                };
                __decorate([
                    core_1.Input(), 
                    __metadata('design:type', Object)
                ], OnPremisesApps.prototype, "settings", void 0);
                OnPremisesApps = __decorate([
                    core_1.Component({
                        templateUrl: './app/widgets/main-dashboard/components/on-premises-apps.component.html',
                        providers: [
                            http_1.HttpModule,
                            services_1.RESTService
                        ]
                    }), 
                    __metadata('design:paramtypes', [services_1.RESTService])
                ], OnPremisesApps);
                return OnPremisesApps;
            }());
            exports_1("OnPremisesApps", OnPremisesApps);
        }
    }
});
//# sourceMappingURL=on-premises-apps.component.js.map