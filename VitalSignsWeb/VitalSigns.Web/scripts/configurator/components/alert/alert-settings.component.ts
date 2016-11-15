import {Component, Input, OnInit, AfterViewInit, ViewChild, ViewChildren} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import {Router, ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';
import {WidgetComponent, WidgetService} from '../../../core/widgets';
import {RESTService} from '../../../core/services';

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
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    insertMode: boolean = false;
    alertSettings: FormGroup;
    errorMessage: string;
    profileEmail: string;
    formTitle: string;
    recurrencesChecked: boolean;
    persistentChecked: boolean;
    limitsChecked: boolean;
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
                this.data.pageSize = 10;
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

    onSubmit(nameValue: any): void {
        //var selected_events: EventsMaster[] = [];
        //for (var _i = 0; _i < this.flex.collectionView.sourceCollection.length; _i++) {
        //    var item = (<wijmo.collections.CollectionView>this.flex.collectionView.sourceCollection)[_i];
        //    if (item.NotificationOnRepeat) {
        //        var eventObject = new EventsMaster();
        //        eventObject.device_type = item.DeviceType;
        //        eventObject.event_type = item.EventType;
        //        selected_events.push(eventObject);
        //    }
        //}
        var selected_events = null;
        var alert_settings = this.alertSettings.value;

        this.dataProvider.put('/Configurator/save_alert_settings', { alert_settings, selected_events })
            .subscribe(
            response => {

            });
    }
}
