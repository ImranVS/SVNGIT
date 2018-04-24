/// <reference path="../../node_modules/reflect-metadata/reflect-metadata.d.ts" />
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
var Traveler = (function () {
    function Traveler(loader, elementRef) {
        var _this = this;
        this.loader = loader;
        this.elementRef = elementRef;
        // Some async action (maybe ajax response with html in it)
        setTimeout(function () { return _this.renderTemplate(); }, 1000);
    }
    Traveler.prototype.renderTemplate = function () {
        //this.loader.loadIntoLocation(
        //    toComponent('/templates/Traveler/MobileDevices.html', 'userdevices'),
        //    this.elementRef,
        //    'container'
        //);
        this.loader.loadIntoLocation(toComponent('/templates/Traveler/views/CountUserDevicesByOS.html', 'CountUserDevicesByOS'), this.elementRef, 'container');
        this.loader.loadIntoLocation(toComponent('/templates/Traveler/views/CountUserDevicesByType.html', 'CountUserDevicesByType'), this.elementRef, 'container');
    };
    Traveler = __decorate([
        core_1.Component({
            selector: 'traveler',
        }),
        core_1.View({
            template: "\n    <div>\n    <h1>Traveler</h1>\n    <div #container></div>\n    </div>\n  "
        }), 
        __metadata('design:paramtypes', [core_1.DynamicComponentLoader, core_1.ElementRef])
    ], Traveler);
    return Traveler;
})();
exports.Traveler = Traveler;
function toComponent(templateUrl, serviceUrl) {
    window[serviceUrl];
    return Object.create(serviceUrl);
    //@Component({
    //    selector: 'generic-selector',
    //    templateUrl: templateUrl,
    //    providers: [HTTP_PROVIDERS]
    //})
    //class GenericComponent implements OnInit {
    //    errorMessage: string;
    //    data: any;
    //    constructor(private http: Http) { }
    //    private _serviceUrl = 'http://private-405397-vitalsignstravelerapi.apiary-mock.com/traveler/';
    //    getTravelerData() {
    //        return this.http.get(this._serviceUrl + serviceUrl)
    //            .map(res => res.json())
    //            .catch(this.handleError);
    //    }
    //    ngOnInit() {
    //        this.getTravelerData().subscribe(
    //            data => this.data = data,
    //            error => this.errorMessage = <any>error
    //        );
    //    }
    //    private handleError(error: Response) {
    //        console.error(error);
    //        return Observable.throw(error.json().error || 'Mobile Devices Service error');
    //    }
    //}
    //return GenericComponent;
}
//class MobileDevice 
//{
//    constructor(private http: Http) { }
//    private _serviceUrl = 'http://private-568d1-vitalsignstravelerapi.apiary-mock.com/traveler/userdevices';
//    getMobileDevices() {  
//    console.log(this.http.get(this._serviceUrl));
//    return this.http.get(this._serviceUrl)
//        .map(res => <MobileDevice[]>res.json())
//        .catch(this.handleError);
//}
//# sourceMappingURL=Traveler.js.map