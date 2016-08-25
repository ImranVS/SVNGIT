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
var CountUserDevicesByOS = (function () {
    function CountUserDevicesByOS(_mobileDevicesService) {
        this._mobileDevicesService = _mobileDevicesService;
    }
    CountUserDevicesByOS.prototype.getCountUserDevicesByType = function () {
        var _this = this;
        this._mobileDevicesService.getViewChartByName('CountUserDevicesByOS')
            .subscribe(function (chart) { return _this.data = chart; }, function (error) { return _this.errorMessage = error; });
    };
    CountUserDevicesByOS.prototype.ngOnInit = function () {
        this.getCountUserDevicesByType();
    };
    CountUserDevicesByOS = __decorate([
        core_1.Component({
            selector: 'count-user-device-by-os',
            templateUrl: '/templates/Traveler/views/CountUserDevicesByOS.html',
            providers: [
                http_1.HTTP_PROVIDERS,
                Views_services_1.TravelerViewsServices
            ]
        }), 
        __metadata('design:paramtypes', [Views_services_1.TravelerViewsServices])
    ], CountUserDevicesByOS);
    return CountUserDevicesByOS;
})();
exports.CountUserDevicesByOS = CountUserDevicesByOS;
//# sourceMappingURL=CountUserDevicesByOS.js.map