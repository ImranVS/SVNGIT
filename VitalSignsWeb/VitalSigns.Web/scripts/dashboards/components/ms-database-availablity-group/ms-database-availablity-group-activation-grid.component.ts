import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {HttpModule}    from '@angular/http';
import { ActivatedRoute } from '@angular/router';
import {WidgetComponent} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import {RESTService} from '../../../core/services';
import { AuthenticationService } from '../../../profiles/services/authentication.service';
import * as gridHelpers from '../../../core/services/helpers/gridutils';
import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';
import * as helpers from '../../../core/services/helpers/helpers';

@Component({
    selector: 'vs-ms-database-availablity-group-activation-grid',
    templateUrl: './app/dashboards/components/ms-database-availablity-group/ms-database-availablity-group-activation-grid.component.html',
    providers: [
        HttpModule,
        RESTService,
        helpers.GridTooltip,
        gridHelpers.CommonUtils
    ]
})
export class ActivationPreferencesGrid implements WidgetComponent, OnInit {
    @Input() settings: any;
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;  
    data: wijmo.collections.CollectionView;
    errorMessage: string;
    deviceId: any;
    serviceId: string;
    currentPageSize: any = 20;
    headers = ['Database Name'];

    constructor(private service: RESTService, private widgetService: WidgetService, private route: ActivatedRoute, protected toolTip: helpers.GridTooltip,
        protected gridHelpers: gridHelpers.CommonUtils, private authService: AuthenticationService) { }

    get pageSize(): number {
        return this.data.pageSize;
    }

    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            this.data.refresh();
            var obj = {
                name: this.gridHelpers.getGridPageName("Office365PasswordsGrid", this.authService.CurrentUser.email),
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
        this.route.params.subscribe(params => {
            if (params['service'])
                this.serviceId = params['service'];
            else {

                this.serviceId = this.widgetService.getProperty('serviceId');
            }
        });
        this.loaddata();
    }

    loaddata() {
        this.service.get(`/services/status_list?type=Database Availability Group&deviceId=${this.serviceId}`)
            .subscribe(
            (data) => {

                let dagServerDatabases = JSON.parse(data.data[0].dag_server_databases);
                let dataObj = {};
                let dataToDisplay = [];
                for (let i = 0; i < dagServerDatabases.length; i++) {
                    if (dataObj["Database Name"] !== undefined && dataObj["Database Name"] !== dagServerDatabases[i].database_name) {
                        dataToDisplay.push(dataObj);
                        dataObj = {};
                    }
                    dataObj["Database Name"] = dagServerDatabases[i].database_name;
                    dataObj["Activation Preference " + dagServerDatabases[i].action_preference] = {
                        server_name: dagServerDatabases[i].server_name,
                        is_active: dagServerDatabases[i].is_active
                    };
                    if (i === (dagServerDatabases.length - 1)) {
                        dataToDisplay.push(dataObj);
                    }

                    let ap = 'Activation Preference ' + dagServerDatabases[i].action_preference;
                    //console.log(ap);
                    if (this.headers.indexOf(ap) == -1) {
                        this.headers.push(ap);
                    }
                }

                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(dataToDisplay));
                this.data.pageSize = this.currentPageSize;
                this.data.moveCurrentToPosition(0);
                this.serviceId = this.data.currentItem.device_id;
            },
            (error) => this.errorMessage = <any>error
            );
    }

        

    
    
    onPropertyChanged(key: string, value: any) {
        if (key === 'serviceId') {
            this.serviceId = value;
            this.loaddata();

        }

    }

    }

