import {Component, OnInit, ViewChildren} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import {Router, ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';
import {AppComponentService} from '../../../core/services';
import {RESTService} from '../../../core/services';

@Component({
    selector: 'preferences-form',
    templateUrl: '/app/configurator/components/applicationSettings/application-settings-preferences.componenet.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class PreferencesForm implements OnInit {

    @ViewChildren('licencekey') licencekey;

    insertMode: boolean = false;
    preferencesForm: FormGroup;
    errorMessage: string;
    profileEmail: string;
    licenceKey: string;   
    expirationDate: any;
    units: any;
    companyName: any;
    installType: any;
    licenseType: any;
    public formData = new FormData();

    appComponentService: AppComponentService;
    constructor(
        private formBuilder: FormBuilder,
        private dataProvider: RESTService,
        private router: Router,
        private route: ActivatedRoute, appComponentService: AppComponentService) {

        this.preferencesForm = this.formBuilder.group({
            'company_name': ['', Validators.required],
            'currency_symbol': ['', Validators.required],
            'monitoring_delay': ['', Validators.required],
            'threshold_show': ['', Validators.required],
            'dashboardonly_exec_summary_buttons': [false],
            'bing_key': ['']
        });
       
        this.appComponentService = appComponentService;
    }

    ngOnInit() {
        this.dataProvider.get('/configurator/get_preferences')
            .subscribe(
            response => {
                //console.log(response.data);
                //console.log(response.data.licenseitem.ExpirationDate);
                //console.log(response.data.licenseitem.units);
                //console.log(response.data.licenseitem.CompanyName);
                this.expirationDate = new Date(response.data.licenseitem.ExpirationDate).toDateString();
                this.units = response.data.licenseitem.units;
                this.companyName = response.data.licenseitem.CompanyName;
                this.licenseType = response.data.licenseitem.LicenseType;
                this.installType = response.data.licenseitem.InstallType;
                this.preferencesForm.setValue(response.data.userpreference);
                

                //this.licenseForm.setValue(response.data.licenseInfo);
            },
            (error) => {
                this.errorMessage = <any>error
                this.appComponentService.showErrorMessage(this.errorMessage);
            }
            );
    }

    onSubmit(nameValue: any): void {
        this.dataProvider.put('/configurator/save_preferences', nameValue)
            .subscribe(
            response => {
              
                if (response.status == "Success") {
                        
                        this.appComponentService.showSuccessMessage(response.message);

                    } else {

                        this.appComponentService.showErrorMessage(response.message);
                    }
            }); 
    }

    saveLicence(dialog: wijmo.input.Popup) {
        var licencekey=this.licencekey.first.nativeElement.value;
        if (licencekey == "") {
            alert("error");
        } else
        {
            this.formData = null;
            this.formData = new FormData();
            this.formData.append("licKey", licencekey);
            this.dataProvider.put('/configurator/save_licence', this.formData)
                .subscribe(
                response => {
                    this.dataProvider.get('/configurator/get_preferences')
                        .subscribe(
                        response => {
                            console.log(response.data);
                            console.log(response.data.licenseitem.ExpirationDate);
                            console.log(response.data.licenseitem.units);
                            console.log(response.data.licenseitem.CompanyName);
                            this.expirationDate = response.data.licenseitem.ExpirationDate;
                            this.units = response.data.licenseitem.units;
                            this.companyName = response.data.licenseitem.CompanyName;
                            this.licenseType = response.data.licenseitem.LicenseType;
                            this.installType = response.data.licenseitem.InstallType;
                            //this.preferencesForm.setValue(response.data.userpreference);
                        },
                        (error) => {
                            this.errorMessage = <any>error
                            this.appComponentService.showErrorMessage(this.errorMessage);
                        }
                        );
                });
            
            dialog.hide();
        }
    }
}