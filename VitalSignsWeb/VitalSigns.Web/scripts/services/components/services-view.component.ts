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
    @Input() searchText
    @Input() searchType;
    @Input() searchStatus;
    @Input() searchLocation;

    errorMessage: string;
    module: string;
    services: any[];

    constructor(private service: RESTService, private router: Router, private route: ActivatedRoute) { }

    selectService(service: any) {

        // Activate selected tab
        this.services.forEach(service => service.active = false);
        service.active = true;  
        
        this.router.navigate(['services/' + this.module,  service.device_id ]);
    }

    loadData() {
        this.service.get('/Services/device_list')
            .subscribe(
            data => this.services = data.data,
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