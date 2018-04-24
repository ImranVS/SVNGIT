import { Component, OnInit, ViewChildren, ViewChild} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import {Router, ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';
import {AppComponentService} from '../../../core/services';
import {RESTService} from '../../../core/services';
import * as helpers from '../../../core/services/helpers/helpers';

@Component({
    selector: 'preferences-form',
    templateUrl: '/app/configurator/components/applicationSettings/application-settings-preferences.componenet.html',
    providers: [
        HttpModule,
        helpers.DateTimeHelper,
        RESTService
    ]
})
export class PreferencesForm implements OnInit {
  
    @ViewChild('flex1') flex1: wijmo.grid.FlexGrid;
    @ViewChild('licensedetailsPopup') dlg: wijmo.input.Popup
    @ViewChildren('licencekey') licencekey;
    detailsdata: wijmo.collections.CollectionView;
    insertMode: boolean = false;
    preferencesForm: FormGroup;
    errorMessage: string;
    profileEmail: string;
    licenceKey: string;   
    expirationDate: any;
    units: any;
    availableUnits: any;
    purge_intreval: any;
    companyName: any;
    installType: any;
    licenseType: any;
    public formData = new FormData();

    appComponentService: AppComponentService;
    constructor(
        protected service: RESTService,
        private formBuilder: FormBuilder,
        private dataProvider: RESTService,
        private router: Router,
        private datetimeHelpers: helpers.DateTimeHelper,
        private route: ActivatedRoute, appComponentService: AppComponentService) {

        this.preferencesForm = this.formBuilder.group({
            'company_name': ['', Validators.required],
            'currency_symbol': ['', Validators.required],
            'monitoring_delay': [0, Validators.required],
            'threshold_show': [0, Validators.required],
            'purge_intreval': ['',],
            'dashboardonly_exec_summary_buttons': [false],
            'bing_key': ['']
        });     
        this.appComponentService = appComponentService;
    }

    ngOnInit() {
        this.dataProvider.get('/configurator/get_preferences')
            .subscribe(
            response => {
                if (response.data.licenseitem) {
                    this.expirationDate = new Date(response.data.licenseitem.ExpirationDate).toDateString();
                    this.units = response.data.licenseitem.units;
                    this.companyName = response.data.licenseitem.CompanyName;
                    this.licenseType = response.data.licenseitem.LicenseType;
                    this.installType = response.data.licenseitem.InstallType;
                    this.availableUnits = this.units - response.data.licenseitem.LicensesUsed;
                    //this.purge_intreval = response.data.licenseitem.purge_intreval;
                }
                this.preferencesForm.setValue(response.data.userpreference);
                //this.licenseForm.setValue(response.data.licenseInfo);
            },
            (error) => {
                this.errorMessage = <any>error
                this.appComponentService.showErrorMessage(this.errorMessage);
            }
            );
        this.purge_intreval = [
            { name: "No Limit", value: -1 },
            { name: "1 Months", value: 1 },
            { name: "3 Months", value: 3 },
            { name: "6 Months", value: 6},
            { name: "12 Months", value: 12 }
           
         ];
    }
    onSubmit(nameValue: any): void {
        this.dataProvider.put('/configurator/save_preferences', nameValue)
            .subscribe(
            response => {
                if (response.status == "Success") {
                    this.appComponentService.showSuccessMessage(response.message);
                }
                else {
                    this.appComponentService.showErrorMessage(response.message);
                }
            });
    }
    saveLicence(dialog: wijmo.input.Popup) {
        var licencekey=this.licencekey.first.nativeElement.value;
        if (licencekey == "") {
            this.appComponentService.showErrorMessage("You must enter a license key");
        } else
        {
            this.formData = null;
            this.formData = new FormData();
            this.formData.append("licKey", licencekey);
            this.appComponentService.showProgressBar();
            this.dataProvider.put('/configurator/save_licence', this.formData)
                .subscribe(
                response => {
                    this.dataProvider.get('/configurator/get_preferences')
                        .subscribe(
                        response => {
                            this.appComponentService.hideProgressBar();
                            if (response.status == "Success") {
                                this.appComponentService.showSuccessMessage("License key updated successfully");
                            }
                            else {
                                this.appComponentService.showErrorMessage("Error updating the license key");
                            }
                            this.expirationDate = new Date(response.data.licenseitem.ExpirationDate).toDateString();
                            this.units = response.data.licenseitem.units;
                            this.companyName = response.data.licenseitem.CompanyName;
                            this.licenseType = response.data.licenseitem.LicenseType;
                            this.installType = response.data.licenseitem.InstallType;
                            //this.preferencesForm.setValue(response.data.userpreference);
                            
                        },
                        (error) => {
                            this.appComponentService.hideProgressBar();
                            this.errorMessage = <any>error
                            this.appComponentService.showErrorMessage(this.errorMessage);
                        });
                });          
            dialog.hide();
        }
    }

    licensedetails() {
        this.service.get('/configurator/get_license_information')
            .subscribe(
            response => {
                this.detailsdata = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data));
                this.dlg.show();
            },
            error => this.errorMessage = <any>error
            );
    }
    closePopup() {
        this.dlg.hide();
    }
}