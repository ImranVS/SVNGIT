import {Component, Output, EventEmitter, ViewChildren, Input} from '@angular/core';
import {Router, ActivatedRoute} from '@angular/router';
import {RESTService} from '../../../core/services';
import {WidgetComponent, WidgetService} from '../../../core/widgets';

@Component({
    selector: 'traveler-filter',
    templateUrl: '/app/reports/filters/components/traveler-filter.component.html',
})
export class TravelerFilter {
    @ViewChildren('interval_selector') interval_selector: wijmo.input.ComboBox;
    @ViewChildren('server_selector') server_selector: wijmo.input.ComboBox;
    selectedServers: any;
    serverType: string;
    statName: string;
    @Input() widgetName: string;
    @Input() widgetURL: string;
    @Input() hideIntervalControl: boolean;
    @Input() hideMailServerControl: boolean;
    @Input() hideServerControl: boolean;
    servers: any;
    mail_servers: any;
    errorMessage: any;
    intervals: any;

    constructor(private service: RESTService, private router: Router, private route: ActivatedRoute, private widgetService: WidgetService) {
        this.intervals = ["000-001", "001-002", "002-005", "005-010", "010-030", "030-060", "060-120", "120-Inf"];
    }

    ngOnInit() {
        this.route.queryParams.subscribe(params => {
            this.statName = params['statname'];
        });
        this.service.get(`/services/status_list?type=Traveler`)
            .subscribe(
            (response) => {
                this.servers = response.data;
            },
            (error) => this.errorMessage = <any>error
        );
        this.service.get(`/services/status_list?type=Domino`)
            .subscribe(
            (response) => {
                this.mail_servers = response.data;
            },
            (error) => this.errorMessage = <any>error
            );
        if (this.hideServerControl == true) {
            var v1 = <HTMLDivElement>document.getElementById("dtServer");
            v1.style.display = "none";
        }
        if (this.hideMailServerControl == true) {
            var v1 = <HTMLDivElement>document.getElementById("dtAllServer");
            v1.style.display = "none";
        }
        if (this.hideIntervalControl == true) {
            var v1 = <HTMLDivElement>document.getElementById("selInterval");
            v1.style.display = "none";
        }
        if (this.hideServerControl == true && this.hideIntervalControl == true) {
            var v2 = <HTMLDivElement>document.getElementById("dtFilter");
            v2.style.display = "none";
        }
       
    }
    applyFilters(server_sel: wijmo.input.ComboBox, interval_sel: wijmo.input.ComboBox, mail_server_sel: wijmo.input.ComboBox) {
        if (this.hideIntervalControl == false) {
            var URL = ((this.widgetURL.includes("?")) ? (this.widgetURL + "&") : (this.widgetURL + "?")) + `deviceId=` + server_sel.selectedValue + `&paramvalue=` + interval_sel.selectedValue;
            //console.log(URL);
            this.widgetService.refreshWidget(this.widgetName, URL)
                .catch(error => console.log(error));
        }
        else {
            var URL = ((this.widgetURL.includes("?")) ? (this.widgetURL + "&") : (this.widgetURL + "?")) + `deviceId=` + server_sel.selectedValue + `&paramvalue=` + mail_server_sel.selectedValue;
            //console.log(URL);
            this.widgetService.refreshWidget(this.widgetName, URL)
                .catch(error => console.log(error));
        }
        

    }

}