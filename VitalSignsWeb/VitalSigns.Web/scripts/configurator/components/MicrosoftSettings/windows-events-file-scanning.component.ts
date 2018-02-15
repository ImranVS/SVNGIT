import {Component, OnInit,ViewChild} from '@angular/core';
import {RESTService} from '../../../core/services';
import {GridBase} from '../../../core/gridBase';
import {Router} from '@angular/router';
import {ActivatedRoute} from '@angular/router';
import { AppComponentService } from '../../../core/services';
import { AuthenticationService } from '../../../profiles/services/authentication.service';
import * as gridHelpers from '../../../core/services/helpers/gridutils';
declare var injectSVG: any;



@Component({
    templateUrl: '/app/configurator/components/MicrosoftSettings/windows-events-file-scanning.component.html',
    providers: [
        RESTService,
        gridHelpers.CommonUtils

    ]
})
export class WindowsEventFiles extends GridBase implements OnInit {
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    EventsName: any;
    errorMessage: string;
    id: string;
    isVisible: boolean = false;
    firstrowid: string;
    currentPageSize: any = 20;


    constructor(service: RESTService, private route: ActivatedRoute, private router: Router, appComponentService: AppComponentService,protected gridHelpers: gridHelpers.CommonUtils, private authService: AuthenticationService) {
        super(service, appComponentService);
        this.formName = "Windows Events File Scanning";

        this.service.get('/configurator/get_windows_events')
            .subscribe(
            response => {

                this.EventsName = response.data;
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
                name: this.gridHelpers.getGridPageName("WindowsEventFile", this.authService.CurrentUser.email),
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
        this.service.get(`/services/get_name_value?name=${this.gridHelpers.getGridPageName("WindowsEventFile", this.authService.CurrentUser.email)}`)
            .subscribe(
            (data) => {
                this.currentPageSize = Number(data.data.value);
                this.data.pageSize = this.currentPageSize;
                this.data.refresh();
            },
            (error) => this.errorMessage = <any>error
            );
        this.initialGridBind('/configurator/get_windows_events');
        injectSVG();
    }

    refreshGrid(event: wijmo.grid.CellRangeEventArgs) {
     
        this.id = event.panel.grid.selectedItems[0].id;

    }

    editGrid() {
        if (!this.id)
            this.id = this.firstrowid
        else
            this.id = this.id    
        this.router.navigateByUrl('windows/add/'+ this.id);

    }
    deleteWindowsEvent() {
        this.deleteGridRow('/configurator/delete_windows_events/');
        this.router.navigateByUrl('/configurator/windowslog');
     
    }
}


