System.register(['@angular/core', '@angular/http', '../../../core/widgets', '../../../core/services'], function(exports_1, context_1) {
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
    var core_1, http_1, widgets_1, services_1;
    var ChartComponent;
    return {
        setters:[
            function (core_1_1) {
                core_1 = core_1_1;
            },
            function (http_1_1) {
                http_1 = http_1_1;
            },
            function (widgets_1_1) {
                widgets_1 = widgets_1_1;
            },
            function (services_1_1) {
                services_1 = services_1_1;
            }],
        execute: function() {
            ChartComponent = (function () {
                function ChartComponent(service, widgetService) {
                    this.service = service;
                    this.widgetService = widgetService;
                }
                ChartComponent.prototype.refresh = function (serviceUrl) {
                    this.loadData(serviceUrl);
                };
                ChartComponent.prototype.ngOnInit = function () {
                    this.loadData();
                };
                ChartComponent.prototype.loadData = function (serviceUrl) {
                    var _this = this;
                    this.service.get(serviceUrl || this.settings.url)
                        .subscribe(function (data) {
                        if (_this.chart) {
                            _this.settings.chart.series = [];
                            _this.chart.destroy();
                        }
                        var chart = data.data;
                        var first = true;
                        chart.series.map(function (serie) {
                            var length = _this.settings.chart.series.push({
                                name: null,
                                data: []
                            });
                            _this.settings.chart.series[length - 1].name = serie.title;
                            serie.segments.map(function (segment) {
                                if (first && _this.settings.chart.xAxis)
                                    _this.settings.chart.xAxis.categories.push(segment.label);
                                _this.settings.chart.series[length - 1].data.push({
                                    name: segment.label,
                                    y: segment.value,
                                    color: segment.color
                                });
                            });
                            // TODO: [OM] not obvious to hard code string value there and it introduces a strong dependency with business rules
                            if (_this.settings.chart.xAxis.categories.length > 1 && serie.title == "Available" || serie.title == "Used") {
                                _this.settings.chart.chart.type = 'bar';
                            }
                            else if (serie.title.startsWith("Disk")) {
                                _this.settings.chart.chart.type = 'pie';
                            }
                            first = false;
                        });
                        _this.chart = new Highcharts.Chart(_this.settings.chart);
                        if (_this.settings.callback)
                            _this.settings.callback(_this.settings.chart);
                    }, function (error) { return _this.errorMessage = error; });
                };
                __decorate([
                    core_1.Input(), 
                    __metadata('design:type', Object)
                ], ChartComponent.prototype, "settings", void 0);
                ChartComponent = __decorate([
                    core_1.Component({
                        template: '',
                        providers: [
                            http_1.HttpModule,
                            services_1.RESTService
                        ]
                    }), 
                    __metadata('design:paramtypes', [services_1.RESTService, widgets_1.WidgetService])
                ], ChartComponent);
                return ChartComponent;
            }());
            exports_1("ChartComponent", ChartComponent);
        }
    }
});
//# sourceMappingURL=chart.component.js.map