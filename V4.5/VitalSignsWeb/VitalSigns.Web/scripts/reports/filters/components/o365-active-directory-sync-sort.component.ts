import {Component, Output, EventEmitter, ViewChildren, Input} from '@angular/core';
import {Router, ActivatedRoute} from '@angular/router';
import {RESTService} from '../../../core/services';
import {WidgetComponent} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';

@Component({
    selector: 'o365-active-directory-sync-sort',
    templateUrl: '/app/reports/filters/components/o365-active-directory-sync-sort.component.html',
})
export class Office365ActiveDirectorySyncSort {
    @ViewChildren('multisel1') multisel1: wijmo.input.MultiSelect;
    errorMessage: any;
    @Input() widgetName: string;

    constructor(private service: RESTService, private router: Router, private route: ActivatedRoute, private widgetService: WidgetService) { }
    ngOnInit() {
        
    }

    onChange(value) {

        console.log(value)
        this.widgetService.refreshWidget(this.widgetName, value)
            .catch(error => console.log(error));

    }
}