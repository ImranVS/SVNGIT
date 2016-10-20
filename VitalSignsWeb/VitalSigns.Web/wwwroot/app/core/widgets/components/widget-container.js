System.register(['@angular/core', '../services/widget.service'], function(exports_1, context_1) {
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
    var core_1, widget_service_1;
    var WidgetContainer;
    return {
        setters:[
            function (core_1_1) {
                core_1 = core_1_1;
            },
            function (widget_service_1_1) {
                widget_service_1 = widget_service_1_1;
            }],
        execute: function() {
            WidgetContainer = (function () {
                function WidgetContainer(widgetService) {
                    this.widgetService = widgetService;
                }
                WidgetContainer.prototype.ngOnInit = function () {
                    if (this.contract) {
                        this.id = this.id || this.contract.id;
                        this.title = this.title || this.contract.title;
                        this.name = this.name || this.contract.name;
                        this.css = this.css || this.contract.css;
                        this.settings = this.settings || this.contract.settings;
                    }
                    if (!this.id) {
                        var i = 1;
                        while (this.widgetService.exists("" + this.name + i))
                            i++;
                        this.id = "" + this.name + i;
                    }
                };
                __decorate([
                    core_1.Input(), 
                    __metadata('design:type', Object)
                ], WidgetContainer.prototype, "contract", void 0);
                __decorate([
                    core_1.Input(), 
                    __metadata('design:type', String)
                ], WidgetContainer.prototype, "id", void 0);
                __decorate([
                    core_1.Input(), 
                    __metadata('design:type', String)
                ], WidgetContainer.prototype, "title", void 0);
                __decorate([
                    core_1.Input(), 
                    __metadata('design:type', String)
                ], WidgetContainer.prototype, "name", void 0);
                __decorate([
                    core_1.Input(), 
                    __metadata('design:type', String)
                ], WidgetContainer.prototype, "css", void 0);
                __decorate([
                    core_1.Input(), 
                    __metadata('design:type', Object)
                ], WidgetContainer.prototype, "settings", void 0);
                WidgetContainer = __decorate([
                    core_1.Component({
                        selector: 'my-widget',
                        template: ''
                    }), 
                    __metadata('design:paramtypes', [widget_service_1.WidgetService])
                ], WidgetContainer);
                return WidgetContainer;
            }());
            exports_1("WidgetContainer", WidgetContainer);
        }
    }
});
//# sourceMappingURL=widget-container.js.map