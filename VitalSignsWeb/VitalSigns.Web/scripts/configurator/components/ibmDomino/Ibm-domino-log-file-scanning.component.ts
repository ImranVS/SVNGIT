import {Component, OnInit} from '@angular/core';
import {RESTService} from '../../../core/services';
import {GridBase} from '../../../core/gridBase';
import {Router} from '@angular/router';
import {ActivatedRoute} from '@angular/router';
import { AppComponentService } from '../../../core/services';
import { AuthenticationService } from '../../../profiles/services/authentication.service';
import * as gridHelpers from '../../../core/services/helpers/gridutils';



@Component({
    templateUrl: '/app/configurator/components/ibmDomino/Ibm-domino-log-file-scanning.component.html',
    providers: [
        RESTService,
        gridHelpers.CommonUtils

    ]
})
export class DominoLogFiles extends GridBase implements OnInit {
    sererNames: any;
    errorMessage: string;
    id: string;
    isVisible: boolean = false;
    firstrowid: string;
    currentPageSize: any = 20;


    constructor(service: RESTService, private route: ActivatedRoute, private router: Router, appComponentService: AppComponentService,protected gridHelpers: gridHelpers.CommonUtils, private authService: AuthenticationService) {
        super(service, appComponentService);
        this.formName = "Domino Log File Scanning";

        this.service.get('/configurator/get_log_scaning')
            .subscribe(
            response => {

                this.sererNames = response.data;
                this.firstrowid = response.data[0].id;
            },
            (error) => this.errorMessage = <any>error
            );

    }
   
    get pageSize(): number {
        return this.data.pageSize;
    }
    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            this.data.refresh();
            var obj = {
                name: this.gridHelpers.getGridPageName("DominoLogFiles", this.authService.CurrentUser.email),
                value: value
            };

            this.service.put(`/services/set_name_value`, obj)
                .subscribe(
                (data) => {

                },
                (error) => console.log(error)
                );
            
        }
    }
    ngOnInit() {
        this.service.get(`/services/get_name_value?name=${this.gridHelpers.getGridPageName("DominoLogFiles", this.authService.CurrentUser.email)}`)
            .subscribe(
            (data) => {
                this.currentPageSize = Number(data.data.value);
                this.data.pageSize = this.currentPageSize;
                this.data.refresh();
            },
            (error) => this.errorMessage = <any>error
            );
        this.initialGridBind('/configurator/get_log_scaning');
    }

    refreshGrid(event: wijmo.grid.CellRangeEventArgs) {
      //  this.router.navigateByUrl('/configurator/ibmDomino?id=' + event.panel.grid.selectedItems[0].Id);
        this.id = event.panel.grid.selectedItems[0].id;

    }

    editGrid() {
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
        this.deleteGridRow('/configurator/delete_log_file_scanning/');
        //this.service.delete(`/configurator/delete_log_file_scanning/`+this.id)
        //window.location.reload();
        //this.router.navigateByUrl('/configurator/ibmDomino?tab=1');      
    }
}


