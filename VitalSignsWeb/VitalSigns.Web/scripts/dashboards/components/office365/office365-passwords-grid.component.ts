import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {HttpModule}    from '@angular/http';
import { ActivatedRoute } from '@angular/router';
import {WidgetComponent} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import {RESTService} from '../../../core/services';

import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';
import * as helpers from '../../../core/services/helpers/helpers';

@Component({
    selector: 'vs-office365-passwords-grid',
    templateUrl: './app/dashboards/components/office365/office365-passwords-grid.component.html',
    providers: [
        HttpModule,
        RESTService,
        helpers.GridTooltip
    ]
})
export class Office365PasswordsGrid implements WidgetComponent, OnInit {
    @Input() settings: any;
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;  
    data: wijmo.collections.CollectionView;
    errorMessage: string;
    deviceId: any;
    serviceId: string;

    constructor(private service: RESTService, private widgetService: WidgetService, private route: ActivatedRoute, protected toolTip: helpers.GridTooltip) { }

    get pageSize(): number {
        return this.data.pageSize;
    }

    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            this.data.refresh();
        }
    }

    ngOnInit() {
        var serviceId = this.widgetService.getProperty('serviceId');
        if (serviceId) {
            var res = serviceId.split(';');
            this.deviceId = res[0];
        }
        else {
            this.route.params.subscribe(params => {
                if (params['service'])
                    this.deviceId = params['service'];
                else {
                    if (this.serviceId) {
                        var res = this.serviceId.split(';');
                        this.deviceId = res[0];
                    }
                }
            });
        }
        this.service.get(`/services/strong_pwd?deviceId=${this.deviceId}&isChart=false`)
            .subscribe(
            (data) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data));
                this.data.pageSize = 10;
                this.data.moveCurrentToPosition(0);
            },
            (error) => this.errorMessage = <any>error
            );
        this.toolTip.getTooltip(this.flex, 0, 2);
    }

    onPropertyChanged(key: string, value: any) {
        if (key === 'serviceId') {
            var url = '';
            this.service.get(url)
                .subscribe(
                (data) => {
                    this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data));
                    this.data.pageSize = 10;
                },
                (error) => this.errorMessage = <any>error
                );

        }

    }


    
}