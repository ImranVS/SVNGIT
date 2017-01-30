import {Component, Output, EventEmitter, ViewChildren, Input} from '@angular/core';
import {Router, ActivatedRoute} from '@angular/router';
import {RESTService} from '../../../core/services';
import {WidgetComponent} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';

@Component({
    selector: 'user-filter',
    templateUrl: '/app/reports/filters/components/user-filter.component.html',
})
export class UserFilter {
    @ViewChildren('multisel1') multisel1: wijmo.input.MultiSelect;
    selectedUsers: any;
    @Input() widgetName: string;
    @Input() widgetURL: string;
    errorMessage: any;
    userData: any;

    constructor(private service: RESTService, private router: Router, private route: ActivatedRoute, private widgetService: WidgetService) { }
    ngOnInit() {
        this.service.get(`/dashboard/connections/users`)
            .subscribe(
            (response) => {
                this.userData = response.data;
            },
            (error) => this.errorMessage = <any>error
            ); 
    }
    applyFilters(multisel1: wijmo.input.MultiSelect) {
        var selectedUsers = "";
        for (var item of multisel1.checkedItems) {
            if (selectedUsers == "") 
                selectedUsers = item.name;
            else 
                selectedUsers += "," + item.name;
        }
        var URL = ((this.widgetURL.includes("?")) ? (this.widgetURL + "&") : (this.widgetURL + "?")) + `userNames=` + selectedUsers;
        this.widgetService.refreshWidget(this.widgetName, URL )
            .catch(error => console.log(error));

    }

}