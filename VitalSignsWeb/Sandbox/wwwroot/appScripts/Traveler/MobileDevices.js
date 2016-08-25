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
var MobileDevices_service_1 = require('./MobileDevices.service');
var MobileDevices = (function () {
    function MobileDevices(_mobileDevicesService) {
        this._mobileDevicesService = _mobileDevicesService;
    }
    MobileDevices.prototype.getMobileDevices = function () {
        var _this = this;
        this._mobileDevicesService.getMobileDevices()
            .subscribe(function (devices) { return _this.devices = devices; }, function (error) { return _this.errorMessage = error; });
    };
    MobileDevices.prototype.ngOnInit = function () {
        this.getMobileDevices();
    };
    MobileDevices = __decorate([
        core_1.Component({
            selector: 'mobile-devices',
            templateUrl: '/templates/Traveler/MobileDevices.html',
            providers: [
                http_1.HTTP_PROVIDERS,
                MobileDevices_service_1.MobileDevicesService
            ]
        }), 
        __metadata('design:paramtypes', [MobileDevices_service_1.MobileDevicesService])
    ], MobileDevices);
    return MobileDevices;
})();
exports.MobileDevices = MobileDevices;
//# sourceMappingURL=MobileDevices.js.map