import {Component, OnInit} from '@angular/core';
import {RESTService} from '../../../core/services';
import {GridBase} from '../../../core/gridBase';


@Component({
    templateUrl: '/app/configurator/components/ibmDomino/Ibm-domino-log-file-scanning.component.html',
    providers: [
        RESTService
    ]
})
export class DominoLogFiles extends GridBase implements OnInit {
    sererNames: any;
    errorMessage: string;
    id: string;
    constructor(service: RESTService) {
        super(service);
        this.formName = "Domino Log File Scanning";
      
        this.service.get('/configurator/get_log_scaning')
            .subscribe(
            response=> {               
                
                this.sererNames = response.data;

            },
            (error) => this.errorMessage = <any>error
            );
       
    }
  
   
    ngOnInit() {
        
        this.initialGridBind('/configurator/get_log_scaning');
    }  
    deleteLogFileScanning() {
        this.delteGridRow('/configurator/delete_log_file_scanning/');
    }
  

}



