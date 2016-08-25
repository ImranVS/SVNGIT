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
    var StatusSummary;
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
            StatusSummary = (function () {
                function StatusSummary(service) {
                    this.service = service;
                }
                StatusSummary.prototype.loadData = function () {
                    var _this = this;
                    this.service.get('/status_summary')
                        .subscribe(function (data) { return _this.statusSummary = data; }, function (error) { return _this.errorMessage = error; });
                };
                StatusSummary.prototype.ngOnInit = function () {
                    this.loadData();
                };
                __decorate([
                    core_1.Input(), 
                    __metadata('design:type', Object)
                ], StatusSummary.prototype, "settings", void 0);
                StatusSummary = __decorate([
                    core_1.Component({
                        templateUrl: './app/widgets/main-dashboard/components/status-summary.component.html',
                        providers: [
                            http_1.HTTP_PROVIDERS,
                            services_1.RESTService
                        ]
                    }), 
                    __metadata('design:paramtypes', [services_1.RESTService])
                ], StatusSummary);
                return StatusSummary;
            }());
            exports_1("StatusSummary", StatusSummary);
        }
    }
});
//# sourceMappingURL=status-summary.component.js.map