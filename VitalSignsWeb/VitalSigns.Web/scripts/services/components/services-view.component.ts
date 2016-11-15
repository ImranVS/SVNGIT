import {Component, AfterViewChecked, OnInit, Input} from '@angular/core'

import {ActivatedRoute} from '@angular/router';
import {Router} from '@angular/router';
import {HttpModule}    from '@angular/http';

import {RESTService} from '../../core/services';

declare var injectSVG: any;

@Component({
    templateUrl: '/app/services/components/services-view.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class ServicesView implements OnInit, AfterViewChecked {


    filterCount = 0;
    _searchText: string;
    @Input('searchText')
    public set searchText(val: string) {
        this._searchText = val;
        this.filterCount = 0;
    }
    public get searchText():string {
        return this._searchText ;
    }
    
    _searchType: string;
    @Input('searchType')
    public set searchType(val: string) {
        this._searchType = val;
        this.filterCount = 0;
    }
    public get searchType(): string {
        return this._searchType;
    }


    _searchStatus: string;
    @Input('searchType')
    public set searchStatus(val: string) {
        this._searchStatus = val;
        this.filterCount = 0;
    }
    public get searchStatus(): string {
        return this._searchStatus;
    }
      
    _searchLocation: string;
    @Input('searchType')
    public set searchLocation(val: string) {
        this._searchLocation = val;
        this.filterCount = 0;
    }
    public get searchLocation(): string {
        return this._searchLocation;
    } 

    errorMessage: string;
    module: string;
    status: string;
    services: any[];
   

    constructor(private service: RESTService, private router: Router, private route: ActivatedRoute) { }

    selectService(service: any) {

        // Activate selected tab
        this.services.forEach(service => service.active = false);
        service.active = true;  
        
        this.router.navigate(['services/' + this.module,  service.id ]);
    }
    setFilterCount(index: any) {
        this.filterCount= index+1;
    }

    loadData() {
        this.service.get('/Services/device_list')
            .subscribe(
            response => {
                if (this.module == "dashboard") {                  
                  var value=  response.data.filter((item) => item.is_enabled == true);
                  this.services = value;
                  console.log(value);
                }
                else {
                    this.services = response.data;
                }

            },
            error => this.errorMessage = <any>error
            );
    }

    ngOnInit() {
       
        this.route.params.subscribe(params => {
            this.module = params['module'];
            this.loadData(); 
        });
        
    }

    ngAfterViewChecked() {
        injectSVG();
    }

}