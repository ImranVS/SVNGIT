import {Component, Output, EventEmitter, ViewChildren, Input} from '@angular/core';
import {Router, ActivatedRoute} from '@angular/router';
import {RESTService} from '../../../core/services';
import {WidgetComponent} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';

@Component({
    selector: 'powerscripts-report-filter',
    templateUrl: '/app/reports/filters/components/powerscripts-report-filter.component.html',
})
export class PowerScriptsReportFilter {
    @ViewChildren('multisel1') multisel1: wijmo.input.MultiSelect;
    errorMessage: any;
    @Input() widgetName: string;
    data: any[]
    constructor(private service: RESTService, private router: Router, private route: ActivatedRoute, private widgetService: WidgetService) { }
    ngOnInit() {
        this.service.get('/services/get_powershell_scripts')
            .subscribe(
                data => {
                    this.data = data.data.scripts.map(function (x) { return { device_type: x.device_type } }).filter((obj, pos, arr) => arr.findIndex(y => y.device_type === obj.device_type) === pos);
                    this.data.unshift({ device_type: "All" });
                },
                error => this.errorMessage = <any>error
            );
    }

    checkedDeviceTypesChanged(event, selector: wijmo.input.MultiSelect) {
        console.log(selector.checkedItems);
        this.widgetService.refreshWidget(this.widgetName, selector.checkedItems)
            .catch(error => console.log(error));
    }

}