var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var core_1 = require('angular2/core');
var http_1 = require('angular2/http');
var Views_services_1 = require('../Views.services');
var CountUserDevicesByType = (function () {
    function CountUserDevicesByType(_mobileDevicesService) {
        this._mobileDevicesService = _mobileDevicesService;
    }
    CountUserDevicesByType.prototype.getCountUserDevicesByType = function () {
        var _this = this;
        this._mobileDevicesService.getViewChartByName('CountUserDevicesByType')
            .subscribe(function (chart) { return _this.data = chart; }, function (error) { return _this.errorMessage = error; });
    };
    CountUserDevicesByType.prototype.getRoutes = function (component) {
        return Reflect.getMetadata('annotations', component)
            .filter(function (a) {
            console.log(a.constructor.name);
            return a.constructor.name === 'ComponentMetadata';
        }).pop();
    };
    CountUserDevicesByType.prototype.ngOnInit = function () {
        console.log(this.getRoutes(this.constructor));
        this.getCountUserDevicesByType();
    };
    CountUserDevicesByType = __decorate([
        core_1.Component({
            selector: 'count-user-device-by-type',
            templateUrl: '/templates/Traveler/views/CountUserDevicesByType.html',
            providers: [
                http_1.HTTP_PROVIDERS,
                Views_services_1.TravelerViewsServices
            ],
            properties: ['myParentConfig: my-parent-config']
        }), 
        __metadata('design:paramtypes', [Views_services_1.TravelerViewsServices])
    ], CountUserDevicesByType);
    return CountUserDevicesByType;
})();
exports.CountUserDevicesByType = CountUserDevicesByType;
//# sourceMappingURL=CountUserDevicesByType.js.map