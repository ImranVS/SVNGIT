import {Component, OnInit} from '@angular/core';
import {RESTService} from '../../../core/services';
import {GridBase} from '../../../core/gridBase';
import {Router} from '@angular/router';
import {ActivatedRoute} from '@angular/router';
import {AppComponentService} from '../../../core/services';


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
    isVisible: boolean = false;
    firstrowid: string;

    constructor(service: RESTService, private route: ActivatedRoute, private router: Router, appComponentService: AppComponentService) {
        super(service, appComponentService);
        this.formName = "Domino Log File Scanning";

        this.service.get('/configurator/get_log_scaning')
            .subscribe(
            response => {

                this.sererNames = response.data;
                this.firstrowid = response.data[0].id;
                console.log(this.firstrowid);

            },
            (error) => this.errorMessage = <any>error
            );

    }
   

    ngOnInit() {

        this.initialGridBind('/configurator/get_log_scaning');
    }

    refreshGrid(event: wijmo.grid.CellRangeEventArgs) {
        console.log(event.panel.grid.selectedItems[0].id);
        console.log(`/configurator/ibmDomino?id=${event.panel.grid.selectedItems[0].id}`);
      //  this.router.navigateByUrl('/configurator/ibmDomino?id=' + event.panel.grid.selectedItems[0].Id);
        this.id = event.panel.grid.selectedItems[0].id;

    }

    editGrid() {
        console.log(this.id);
        if (!this.id)
            this.id = this.firstrowid
        else
            this.id = this.id    
        this.router.navigateByUrl('ibmDomino/add/'+ this.id);

    }
    deleteLogFileScanning() {
        //if (!this.id)
        //    this.id = this.firstrowid
        //else
        //    this.id = this.id
        this.delteGridRow('/configurator/delete_log_file_scanning/');
        console.log(this.id);
        //this.service.delete(`/configurator/delete_log_file_scanning/`+this.id)
        //window.location.reload();
        //this.router.navigateByUrl('/configurator/ibmDomino?tab=1');
    
       
    }
}


