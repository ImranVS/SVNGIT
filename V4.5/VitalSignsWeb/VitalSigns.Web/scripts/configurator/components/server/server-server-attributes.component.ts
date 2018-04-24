import {Component, AfterViewChecked, OnInit, Input} from '@angular/core'
import {ActivatedRoute} from '@angular/router';
import {Router} from '@angular/router';
import {HttpModule}    from '@angular/http';
import {RESTService} from '../../../core/services';
declare var injectSVG: any;

@Component({
 
    templateUrl: '/app/configurator/components/server/server-server-attibutes.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class ServerAttribute implements OnInit, AfterViewChecked  {
    
    deviceId: any;
    errorMessage: any;
    category: string;
    
    FieldName: string;
    Attributes: any[];
  

    constructor(private Attribute: RESTService, private router: Router, private route: ActivatedRoute) { }
       

    ngOnInit() {

        this.route.params.subscribe(params => {
            this.deviceId = params['service'];
            this.loadData();
        });
    }

    loadData() {
        
        this.Attribute.get('/configurator/' + this.deviceId + '/servers_attributes')
            .subscribe(
            response => {
                this.Attributes = response.data;
            },
            error => this.errorMessage = <any>error                      
        );        
    }   
    ngAfterViewChecked() {
        injectSVG();
    }
}