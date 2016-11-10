import {Component, Output, EventEmitter, ViewChildren, Input} from '@angular/core';
import {Router, ActivatedRoute} from '@angular/router';
import {RESTService} from '../../../core/services';
import {WidgetComponent, WidgetService} from '../../../core/widgets';

@Component({
    selector: 'server-filter',
    templateUrl: '/app/reports/filters/components/server-filter.component.html',
})
export class ServerFilter {
    @ViewChildren('multisel1') multisel1: wijmo.input.MultiSelect;
    selectedServers: any;
    serverType: string;
    //@Output() name = new EventEmitter();
    //@Output() type = new EventEmitter();
    //@Output() status = new EventEmitter();
    //@Output() location = new EventEmitter();
    @Input() deviceType: any; 
    startDate: Date = new Date();
    endDate: Date = new Date();
    //selecttype: string = "";
    //data: wijmo.collections.CollectionView;
    deviceNameData: any;
    //deviceStatusData: any;
    //deviceLocationData: any;
    errorMessage: any;

    constructor(private service: RESTService, private router: Router, private route: ActivatedRoute, private widgetService: WidgetService) { }
    //deviceName: string = "";
    //deviceType: string = "-All-";
    //deviceLocation: string = "-All-";
    //deviceStatus: string = "-All-";
    ngOnInit() {
        this.route.queryParams.subscribe(params => {
            this.serverType = params['server-type'];
        });

        //this.service.get('/services/server_list_dropdown?type=Domino')
        //    .subscribe(
        //    (data) => {
        //        this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data.deviceName));
        //        this.data.pageSize = 10;
        //    },
        //    (error) => this.errorMessage = <any>error
        //    );
        //let paramstatus = null;
        //Get a query parameter if the page is called from the main dashboard via a link in a status component
        //this.route.queryParams.subscribe(params => paramstatus = params['status'] || '-All-');
        //this.name.emit('');
        //this.type.emit('-All-');
        //this.status.emit(paramstatus);
        //this.location.emit('-All-');
        this.service.get(`/services/server_list_dropdown?type=${this.deviceType}`)
            .subscribe(
            (response) => {
                this.deviceNameData = response.data.deviceNameData;
            },
            (error) => this.errorMessage = <any>error
            );
        //Set a selected value of the Status drop down box to the passed query parameter or -All- if no parameter is available
        //this.deviceStatus = paramstatus;
    }
    applyFilters(multisel1: wijmo.input.MultiSelect) {
        console.log(multisel1.checkedItems);
        //var v = (<wijmo.input.MultiSelect>document.getElementById("text1")).value;
        //var v = multisel1.checkedItems;
        var selectedServers = "";
        for (var item of multisel1.checkedItems) {
            if (selectedServers == "") 
                selectedServers = item.id;
            else 
                selectedServers += "," + item.id;
        }
        //this.router.navigate([/[^\?]*/.exec(this.router.url)[0]], {
        //    queryParams: {
        //        start: this.startDate.toISOString(),
        //        end: this.endDate.toISOString(),
        //        deviceId: selectedServers
        //    }

        //});
        this.widgetService.refreshWidget('avgcpuutilchart', `/reports/summarystats_chart?statName=Platform.System.PctCombinedCpuUtil&deviceId=` + selectedServers + `&start=` + this.startDate.toISOString() + `&end=` + this.endDate.toISOString())
            .catch(error => console.log(error));


    }

}