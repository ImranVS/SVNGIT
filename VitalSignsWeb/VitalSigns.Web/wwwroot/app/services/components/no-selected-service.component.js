System.register(['@angular/core'], function(exports_1, context_1) {
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
    var core_1;
    var NoSelectedService;
    return {
        setters:[
            function (core_1_1) {
                core_1 = core_1_1;
            }],
        execute: function() {
            NoSelectedService = (function () {
                function NoSelectedService() {
                }
                NoSelectedService = __decorate([
                    core_1.Component({
                        template: "\n<div id=\"noServerSelectedWrapper\">\n    <div id=\"noServerSelected\">\n        <img id=\"noServerImg\" class=\"svgInject\" src=\"img/menu/servers.svg\" title=\"Servers\" alt=\"Servers\" />\n        <h2>No server selected</h2>\n        <p>Choose a server by clicking a server on the list</p>\n        <button type=\"button\" class=\"btn btn-primary\">or Add server</button>\n    </div>\n</div>\n",
                    }), 
                    __metadata('design:paramtypes', [])
                ], NoSelectedService);
                return NoSelectedService;
            }());
            exports_1("NoSelectedService", NoSelectedService);
        }
    }
});
//# sourceMappingURL=no-selected-service.component.js.map