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
    var Office365Service;
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
            Office365Service = (function () {
                function Office365Service(http) {
                    this.http = http;
                    var tenantId = 'hoozinespresso.onmicrosoft.com';
                    var clientId = '606c5216-95a4-4258-ae95-68099ab020e1';
                    var graphResource = 'https://graph.microsoft.com';
                    var locationUri = encodeURIComponent('http://localhost:50390/dash'); //window.location.href);
                    console.log(window.location.hash);
                    if (!localStorage.getItem('auth_token'))
                        window.location.href = "https://login.microsoftonline.com/" + tenantId + "/oauth2/authorize?response_type=id_token&client_id=" + clientId + "&redirect_uri=" + locationUri + "&state=SomeState&nonce=SomeNonce";
                }
                Office365Service.prototype.get = function (path) {
                    return { status: 'not yet implemented' };
                };
                Office365Service.prototype.handleError = function (error) {
                    console.error(error);
                    return Observable_1.Observable.throw(error.json().error || 'Server error');
                };
                Office365Service = __decorate([
                    core_1.Injectable(), 
                    __metadata('design:paramtypes', [http_1.Http])
                ], Office365Service);
                return Office365Service;
            }());
            exports_1("Office365Service", Office365Service);
        }
    }
});
//# sourceMappingURL=o365.service.js.map