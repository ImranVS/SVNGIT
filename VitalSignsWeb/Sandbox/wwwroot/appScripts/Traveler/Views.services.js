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
var Observable_1 = require('rxjs/Observable');
var TravelerViewsServices = (function () {
    function TravelerViewsServices(http) {
        this.http = http;
        this._serviceUrl = 'http://private-405397-vitalsignstravelerapi.apiary-mock.com/traveler/views/';
    }
    TravelerViewsServices.prototype.getViewChartByName = function (viewName) {
        return this.http.get(this._serviceUrl + viewName)
            .map(function (res) { return res.json(); })
            .catch(this.handleError);
    };
    TravelerViewsServices.prototype.handleError = function (error) {
        console.error(error);
        return Observable_1.Observable.throw(error.json().error || 'TravelerViewsServices Service error');
    };
    TravelerViewsServices = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [http_1.Http])
    ], TravelerViewsServices);
    return TravelerViewsServices;
})();
exports.TravelerViewsServices = TravelerViewsServices;
//# sourceMappingURL=Views.services.js.map