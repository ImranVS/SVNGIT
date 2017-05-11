import {Component, NgModule, Input, OnInit, AfterViewInit, ViewChild, ViewChildren} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import {Router, ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';
import {WidgetComponent} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import {RESTService} from '../../../core/services';
import {SuccessErrorMessageComponent} from '../../../core/components/success-error-message-component';
import { BrowserModule } from '@angular/platform-browser';
import { GridBase } from '../../../core/gridBase';
import { AppComponentService } from '../../../core/services';

@Component({
    templateUrl: '/app/configurator/components/alert/alert-settings.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})

export class AlertSettings extends GridBase implements WidgetComponent, OnInit {
    @Input() settings: any;
    @ViewChildren('name') inputName;
    @ViewChild('message') message;
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    @ViewChildren('primary_pwd') primary_pwd;
    @ViewChildren('secondary_pwd') secondary_pwd;
    @ViewChildren('alertsOn') alerts_on;
    @ViewChildren('alertsOff') alerts_off;
    @ViewChildren('alertOnLabel') alerts_on_label;
    @ViewChildren('alertOffLabel') alerts_off_label;
    insertMode: boolean = false;
    alertSettings: FormGroup;
    errorMessage: string;
    successMessage: string;
    profileEmail: string;
    formTitle: string;
    recurrencesChecked: boolean;
    persistentChecked: boolean;
    limitsChecked: boolean;
    alertsOn: boolean;
    selected_events: string[] = [];
    visibility: boolean = false;
    data: wijmo.collections.CollectionView;

    constructor(
        private formBuilder: FormBuilder,
        private dataProvider: RESTService,
        private router: Router,
        private route: ActivatedRoute, appComponentService: AppComponentService) { 
        super(dataProvider, appComponentService);
        this.alertSettings = this.formBuilder.group({
            'primary_host_name': [''],
            'primary_from': [''],
            'primary_user_id': [''], 
            'primary_port': [''],
            'primary_auth': [''],
            'primary_ssl': [''], 
            'primary_pwd': [''], 
            'primary_modified': [''],
            'secondary_host_name': [''],
            'secondary_from': [''],
            'secondary_user_id': [''], 
            'secondary_port': [''],
            'secondary_auth': [''], 
            'secondary_ssl': [''],
            'secondary_pwd': [''], 
            'secondary_modified': [''],
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
            'number_of_recurrences': [''],
            'alerts_on': ['']
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
            //The code below sets the bootstrap switch to on or off based on the AlertsOn setting in the name_value collection.
            //Since the switch element is custom, re-setting the CSS is necessary to present the state (on/off) correctly.
            this.alertsOn = alertobject['alerts_on'];
            this.alerts_on.first.nativeElement.checked = this.alertsOn;
            this.alerts_off.first.nativeElement.checked = !this.alertsOn;
            if (this.alertsOn) {
                this.alerts_on_label.first.nativeElement.className = "btn btn-default btn-on btn-sm active";
                this.alerts_off_label.first.nativeElement.className = "btn btn-default btn-off btn-sm";
            }
            else {
                this.alerts_off_label.first.nativeElement.className = "btn btn-default btn-off btn-sm active";
                this.alerts_on_label.first.nativeElement.className = "btn btn-default btn-on btn-sm";
            }
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
        if (this.flex != null) {
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
    }

    onSubmit(nameValue: any): void {
        this.errorMessage = "";
        this.successMessage = "";
        this.refreshCheckedEvents();
        this.alertSettings.value.alerts_on = this.alertsOn;
        var alert_settings = this.alertSettings.value;
        var selected_events = this.selected_events;
        if (this.selected_events.length == 0 && this.recurrencesChecked) {
            this.errorMessage = "No selection made. Please select at least one Events entry.";
            //this.message.toggleVisibility(true, this.errorMessage);
            this.appComponentService.showErrorMessage(this.errorMessage);
        }
        if (!this.errorMessage) {
            this.dataProvider.put('/configurator/save_alert_settings', { alert_settings, selected_events })
                .subscribe(
                response => {
                    //this.successMessage = response.message;
                    //this.message.toggleVisibility(false, this.successMessage);
                    if (response.status == "Success") {
                        this.appComponentService.showSuccessMessage(response.message);
                    }
                    else {
                        this.appComponentService.showErrorMessage(response.message);
                    }
                },
                (error) => {
                    this.errorMessage = <any>error;
                    this.appComponentService.showErrorMessage(this.errorMessage);
                }
            );
        } 
        this.selected_events = [];
    }

    selectAll() {
        for (var _i = 0; _i < this.flex.collectionView.sourceCollection.length; _i++) {
            var item = (<wijmo.collections.CollectionView>this.flex.collectionView.sourceCollection)[_i];
            item.NotificationOnRepeat = true;
        }
        this.flex.refresh();
    }

    deselectAll() {
        for (var _i = 0; _i < this.flex.collectionView.sourceCollection.length; _i++) {
            var item = (<wijmo.collections.CollectionView>this.flex.collectionView.sourceCollection)[_i];
            item.NotificationOnRepeat = false;
        }
        this.flex.refresh();
    }

    collapse(flex) {
        flex.collapseGroupsToLevel(0);
    }

    expand(flex) {
        var rows = flex.rows;
        for (var rowIdx = 0; rowIdx < rows.length; rowIdx++) {
            var rootRow = rows[rowIdx];
            if (rootRow.hasChildren) { rootRow.isCollapsed = false; }
        }
    }

    savePwdPrimary(dialog: wijmo.input.Popup) {
        var pwd = this.primary_pwd.first.nativeElement.value;
        if (pwd == "") {
            this.errorMessage = "You must enter a password";
            this.message.toggleVisibility(true, this.errorMessage);
        } else {
            this.alertSettings.value.primary_pwd = pwd;
            this.alertSettings.value.primary_modified = true;
            var selected_events = this.selected_events;
            var alert_settings = this.alertSettings.value;
            this.dataProvider.put('/configurator/save_alert_settings', { alert_settings, selected_events })
                .subscribe(
                response => {
                    if (response.status == "Success") {
                        //this.successMessage = "Password updated successfully";
                        //this.message.toggleVisibility(false, this.successMessage);
                        this.appComponentService.showSuccessMessage(response.message);
                    }
                    else {
                        //this.errorMessage = "Error updating the password";
                        //this.message.toggleVisibility(true, this.errorMessage);
                        this.appComponentService.showErrorMessage(response.message);
                    }
                    this.alertSettings.value.primary_modified = false;
                    this.alertSettings.value.secondary_modified = false;
                },
                (error) => {
                    this.errorMessage = <any>error
                    //this.errorMessage = this.errorMessage;
                    //this.message.toggleVisibility(true, this.errorMessage);
                    this.appComponentService.showErrorMessage(this.errorMessage);
                });
            dialog.hide();
        }
    }

    savePwdSecondary(dialog: wijmo.input.Popup) {
        var pwd = this.secondary_pwd.first.nativeElement.value;
        if (pwd == "") {
            this.errorMessage = "You must enter a password";
            this.message.toggleVisibility(true, this.errorMessage);
        } else {
            this.alertSettings.value.secondary_pwd = pwd;
            this.alertSettings.value.secondary_modified = true;
            var selected_events = this.selected_events;
            var alert_settings = this.alertSettings.value;
            this.dataProvider.put('/configurator/save_alert_settings', { alert_settings, selected_events })
                .subscribe(
                response => {
                    if (response.status == "Success") {
                        //this.successMessage = "Password updated successfully";
                        //this.message.toggleVisibility(false, this.successMessage);
                        this.appComponentService.showSuccessMessage(response.message);
                    }
                    else {
                        //this.errorMessage = "Error updating the password";
                        //this.message.toggleVisibility(true, this.errorMessage);
                        this.appComponentService.showErrorMessage(response.message);
                    }
                    this.alertSettings.value.primary_modified = false;
                    this.alertSettings.value.secondary_modified = false;
                },
                (error) => {
                    this.errorMessage = <any>error
                    //this.errorMessage = this.errorMessage;
                    //this.message.toggleVisibility(true, this.errorMessage);
                    this.appComponentService.showErrorMessage(this.errorMessage);
                });
            dialog.hide();
        }
    }

    setOn(on: boolean) {
        this.alertsOn = on;
    }

    clearPwd1() {
        this.alertSettings.value.primary_pwd = "";
        this.alertSettings.value.primary_modified = true;
        var selected_events = this.selected_events;
        var alert_settings = this.alertSettings.value;
        this.dataProvider.put('/configurator/save_alert_settings', { alert_settings, selected_events })
            .subscribe(
            response => {
                if (response.status == "Success") {
                    this.appComponentService.showSuccessMessage(response.message);
                }
                else {
                    this.appComponentService.showErrorMessage(response.message);
                }
                this.alertSettings.value.primary_modified = false;
                this.alertSettings.value.secondary_modified = false;
            },
            (error) => {
                this.errorMessage = <any>error
                this.appComponentService.showErrorMessage(this.errorMessage);
            });
    }

    clearPwd2() {
        this.alertSettings.value.secondary_pwd = "";
        this.alertSettings.value.secondary_modified = true;
        var selected_events = this.selected_events;
        var alert_settings = this.alertSettings.value;
        this.dataProvider.put('/configurator/save_alert_settings', { alert_settings, selected_events })
            .subscribe(
            response => {
                if (response.status == "Success") {
                    this.appComponentService.showSuccessMessage(response.message);
                }
                else {
                    this.appComponentService.showErrorMessage(response.message);
                }
                this.alertSettings.value.primary_modified = false;
                this.alertSettings.value.secondary_modified = false;
            },
            (error) => {
                this.errorMessage = <any>error
                this.appComponentService.showErrorMessage(this.errorMessage);
            });
    }

    clearEvents() {
        var selected_events = this.selected_events;
        var alert_settings = this.alertSettings.value;
        this.dataProvider.put('/configurator/clear_alerts', {})
            .subscribe(
            response => {
                if (response.status == "Success") {
                    this.appComponentService.showSuccessMessage(response.message);
                }
                else {
                    this.appComponentService.showErrorMessage(response.message);
                }
                this.alertSettings.value.primary_modified = false;
                this.alertSettings.value.secondary_modified = false;
            },
            (error) => {
                this.errorMessage = <any>error
                this.appComponentService.showErrorMessage(this.errorMessage);
            });
    }
}
