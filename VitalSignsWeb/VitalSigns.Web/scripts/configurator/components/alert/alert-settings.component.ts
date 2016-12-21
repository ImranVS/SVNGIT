import {Component, NgModule, Input, OnInit, AfterViewInit, ViewChild, ViewChildren} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import {Router, ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';
import {WidgetComponent} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import {RESTService} from '../../../core/services';
import {SuccessErrorMessageComponent} from '../../../core/components/success-error-message-component';
import { BrowserModule  } from '@angular/platform-browser';


@Component({
    templateUrl: '/app/configurator/components/alert/alert-settings.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})

export class AlertSettings implements WidgetComponent, OnInit {
    @Input() settings: any;
    @ViewChildren('name') inputName;
    @ViewChild('message') message;
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    insertMode: boolean = false;
    alertSettings: FormGroup;
    errorMessage: string;
    successMessage: string;
    profileEmail: string;
    formTitle: string;
    recurrencesChecked: boolean;
    persistentChecked: boolean;
    limitsChecked: boolean;
    selected_events: string[] = [];
    visibility: boolean = false;
    data: wijmo.collections.CollectionView;

    constructor(
        private formBuilder: FormBuilder,
        private dataProvider: RESTService,
        private router: Router,
        private route: ActivatedRoute) { 

        this.alertSettings = this.formBuilder.group({
            'primary_host_name': [''],
            'primary_from': [''],
            'primary_user_id': [''], 
            'primary_port': [''],
            'primary_auth': [''],
            'primary_ssl': [''], 
            'primary_pwd': [''], 
            'secondary_host_name': [''],
            'secondary_from': [''],
            'secondary_user_id': [''], 
            'secondary_pwd': [''],
            'secondary_port': [''],
            'secondary_auth': [''], 
            'secondary_ssl': [''],
            'sms_account_sid': [''],
            'sms_auth_token': [''], 
            'sms_from': [''],
            'enable_persistent_alerting': [''],
            'alert_interval': [''],
            'alert_duration': [''],
            //'e_mail': [''],
            'enable_alert_limits': [''],
            'total_maximum_alerts_per_definition': [''],
            'total_maximum_alerts_per_day': [''],
            'enable_SNMP_traps': [''],
            'host_name': [''],
            'alert_about_recurrences_only': [''],
            'number_of_recurrences': ['']
        });
    }
    ngOnInit() {
        this.errorMessage = "";
        this.successMessage = "";
        this.route.params.subscribe(params => {        
            this.dataProvider.get('/Configurator/get_alert_settings')
                .subscribe(
                (data) => this.alertSettings.setValue(data.data),
                (error) => this.errorMessage = <any>error

                );
        });
        this.alertSettings.valueChanges.subscribe(alertobject => {
            this.recurrencesChecked = alertobject['alert_about_recurrences_only'];
            this.persistentChecked = alertobject['enable_persistent_alerting'];
            this.limitsChecked = alertobject['enable_alert_limits'];
        });
        this.dataProvider.get('/configurator/events_master_list')
            .subscribe(
            (data) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data));
                //this.data.pageSize = 10;
                var groupDesc = new wijmo.collections.PropertyGroupDescription('DeviceType');
                this.data.groupDescriptions.push(groupDesc);
            },
            (error) => this.errorMessage = <any>error
            );
    }

    isChecked(ischecked: boolean) {
        this.recurrencesChecked = ischecked;
    }

    isPAChecked(ischecked: boolean) {
        this.persistentChecked = ischecked;
    }

    isLimitsChecked(ischecked: boolean) {
        this.limitsChecked = ischecked;
    }

    refreshCheckedEvents() {
        if (this.flex.collectionView) {
            //(<wijmo.collections.CollectionView>this.flex.collectionView.sourceCollection).moveToFirstPage();
            for (var _i = 0; _i < this.flex.collectionView.sourceCollection.length; _i++) {
                var item = (<wijmo.collections.CollectionView>this.flex.collectionView.sourceCollection)[_i];
                var val = this.selected_events.filter((record) => record == item.Id);
                if (item.NotificationOnRepeat && val.length == 0) {
                    this.selected_events.push(item.Id);
                }
            }
        }
    }

    onSubmit(nameValue: any): void {
        this.errorMessage = "";
        this.successMessage = "";
        this.refreshCheckedEvents();
        var alert_settings = this.alertSettings.value;
        var selected_events = this.selected_events;
        if (this.selected_events.length == 0 && this.recurrencesChecked) {
            this.errorMessage = "No selection made. Please select at least one Events entry.";
            this.message.toggleVisibility(true, this.errorMessage);
        }
        if (!this.errorMessage) {
            this.dataProvider.put('/configurator/save_alert_settings', { alert_settings, selected_events })
                .subscribe(
                response => {
                    this.successMessage = response.message;
                    this.message.toggleVisibility(false, this.successMessage);
                },
            (error) => this.errorMessage = <any>error
            );
        } 
        this.selected_events = [];
    }
}
