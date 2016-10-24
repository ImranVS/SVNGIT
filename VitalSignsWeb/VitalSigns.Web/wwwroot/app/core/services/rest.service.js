System.register(['@angular/core', '@angular/http', 'rxjs/Observable'], function(exports_1, context_1) {
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
    var core_1, http_1, Observable_1;
    var RESTService;
    return {
        setters:[
            function (core_1_1) {
                core_1 = core_1_1;
            },
            function (http_1_1) {
                http_1 = http_1_1;
            },
            function (Observable_1_1) {
                Observable_1 = Observable_1_1;
            }],
        execute: function() {
            RESTService = (function () {
                //serverUrl = 'http://localhost:5000';
                function RESTService(http) {
                    this.http = http;
                    // serverUrl = 'http://private-f4c5b-vitalsignssandboxserver.apiary-mock.com';
                    // serverUrl = 'http://private-ad10c-ibm.apiary-mock.com';
                    //serverUrl ='http://dev2.vsplus.jnitinc.com:5000';
                    this.serverUrl = 'http://localhost:1234';
                }
                RESTService.prototype.get = function (path) {
                    var serviceUrl = path.indexOf('://') > -1 ? path : this.serverUrl + path;
                    return this.http.get(serviceUrl)
                        .map(function (res) { return res.json(); })
                        .catch(this.handleError);
                };
                RESTService.prototype.post = function (path, body, callback) {
                    var serviceUrl = path.indexOf('://') > -1 ? path : this.serverUrl + path;
                    this.http.post(serviceUrl, body)
                        .subscribe(function (res) { callback(); });
                };
                RESTService.prototype.putAndCallback = function (path, body, callback) {
                    var serviceUrl = path.indexOf('://') > -1 ? path : this.serverUrl + path;
                    this.http.put(serviceUrl, body)
                        .subscribe(function (res) { callback(); });
                };
                RESTService.prototype.put = function (path, body) {
                    var serviceUrl = path.indexOf('://') > -1 ? path : this.serverUrl + path;
                    return this.http.put(serviceUrl, body)
                        .map(function (res) { return res.json(); })
                        .catch(this.handleError);
                };
                RESTService.prototype.delete = function (path, callback) {
                    var serviceUrl = path.indexOf('://') > -1 ? path : this.serverUrl + path;
                    this.http.delete(serviceUrl)
                        .subscribe(function (res) { callback(); });
                };
                RESTService.prototype.handleError = function (error) {
                    return Observable_1.Observable.throw(error || 'Server error');
                };
                RESTService = __decorate([
                    core_1.Injectable(), 
                    __metadata('design:paramtypes', [http_1.Http])
                ], RESTService);
                return RESTService;
            }());
            exports_1("RESTService", RESTService);
        }
    }
});
//# sourceMappingURL=rest.service.js.map