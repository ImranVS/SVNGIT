import {Component, Output, EventEmitter, ViewChildren, Input} from '@angular/core';
import {Router, ActivatedRoute} from '@angular/router';
import {RESTService} from '../../../core/services';
import {WidgetComponent} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';

@Component({
    selector: 'server-availability-filter',
    templateUrl: '/app/reports/filters/components/server-availability-filter.component.html',
})
export class ServerAvailabilityFilter {
    @ViewChildren('multisel1') multisel1: wijmo.input.MultiSelect;
    selectedServers: any;
    statName: string;
    @Input() widgetName: string;
    @Input() widgetURL: string;
    @Input() hideMinValuePanel: boolean;
    currentDate: Date = new Date();
    errorMessage: any;
    deviceNameData: any;
    minValue: string;
    statTypeDropdown: string;
    
    constructor(private service: RESTService, private router: Router, private route: ActivatedRoute, private widgetService: WidgetService) { }
    ngOnInit() {

        this.service.get(`/services/server_list_dropdown`)
            .subscribe(
            (response) => {
                this.deviceNameData = response.data.deviceNameData;
            },
            (error) => this.errorMessage = <any>error
            );

        this.route.queryParams.subscribe(params => {
            this.statName = params['statname'];
        });

        if (this.hideMinValuePanel == true) {
            var v = <HTMLDivElement>document.getElementById("dtMinValue");
            v.style.display = "none";
        }

    }
    applyFilters(multisel1: wijmo.input.MultiSelect) {
     
        var selectedServers = "";
        for (var item of multisel1.checkedItems) {
            if (selectedServers == "") 
                selectedServers = item.id;
            else 
                selectedServers += "," + item.id;
        }

        var currentStartDate = (this.currentDate.getDate()).toString();
        var currentStartMonth = (this.currentDate.getMonth() + 1).toString();
        if (currentStartDate.length == 1)
            currentStartDate = '0' + currentStartDate;
        if (currentStartMonth.length == 1)
            currentStartMonth = '0' + currentStartMonth;

        var newCurrentDate = new Date(this.currentDate.getFullYear(), this.currentDate.getMonth(), this.currentDate.getDate());
        
        var URL = ((this.widgetURL.includes("?")) ? (this.widgetURL + "&") : (this.widgetURL + "?")) + `deviceId=` + selectedServers;;
        URL += "&month=" + newCurrentDate.toISOString() + `&minValue=` + this.minValue + `&reportType=` + this.statTypeDropdown;
      
        this.widgetService.refreshWidget(this.widgetName, URL )
            .catch(error => console.log(error));

    }

}