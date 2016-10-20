System.register(['@angular/core', './widget-container', '../../../app.widgets'], function(exports_1, context_1) {
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
    var core_1, widget_container_1, widgets;
    var WidgetController;
    return {
        setters:[
            function (core_1_1) {
                core_1 = core_1_1;
            },
            function (widget_container_1_1) {
                widget_container_1 = widget_container_1_1;
            },
            function (widgets_1) {
                widgets = widgets_1;
            }],
        execute: function() {
            WidgetController = (function () {
                function WidgetController(factoryResolver, widgetService) {
                    this.factoryResolver = factoryResolver;
                    this.widgetService = widgetService;
                }
                WidgetController.prototype.onPropertyChanged = function (key, value) { };
                WidgetController.prototype.ngAfterViewInit = function () {
                    var _this = this;
                    this.widgetService.loadController(this);
                    this.containers.forEach(function (containerRef) {
                        var container = containerRef._element.component;
                        var factory = _this.factoryResolver.resolveComponentFactory(widgets[container.name]);
                        var component = containerRef.createComponent(factory);
                        var widget = (component.instance);
                        widget.settings = container.settings;
                        _this.widgetService.registerWidget(_this, container, widget);
                    });
                };
                WidgetController.prototype.ngOnDestroy = function () {
                    this.widgetService.unloadController(this);
                };
                __decorate([
                    core_1.ViewChildren(widget_container_1.WidgetContainer, { read: core_1.ViewContainerRef }), 
                    __metadata('design:type', core_1.QueryList)
                ], WidgetController.prototype, "containers", void 0);
                return WidgetController;
            }());
            exports_1("WidgetController", WidgetController);
        }
    }
});
//# sourceMappingURL=widget-controller.js.map