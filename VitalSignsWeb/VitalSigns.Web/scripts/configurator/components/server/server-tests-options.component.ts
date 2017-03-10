import {Component, OnInit, ViewChildren} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import {Router, ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';
import {RESTService} from '../../../core/services';
import {AppComponentService} from '../../../core/services';

@Component({
    selector: 'server-tests-list',
    templateUrl: '/app/configurator/components/server/server-tests-options.component.html',
    providers: [
        HttpModule,
        RESTService
        ]
})
export class TestsOptions implements OnInit {
    @ViewChildren('name') inputName;
    deviceId: any;
    insertMode: boolean = false;
    tests: FormGroup;
    errorMessage: string;
    profileEmail: string;
    formTitle: string;
    appComponentService: AppComponentService;

    constructor(
        private formBuilder: FormBuilder,
        private dataProvider: RESTService,
        private router: Router,
        private route: ActivatedRoute, appComponentService: AppComponentService ) {
        this.tests = this.formBuilder.group({
            'id': [],
            'smtp': [false],
            'auto_discovery': [false],
            'create_calendar': [false],
            'imap': [false],
            'pop3': [false],
            'mapi_connectivity': [false],
            'inbox': [false],
            'owa': [false],
            'mail_flow': [false],
            'create_folder': [false],
            'create_site': [false],
            'onedrive_upload': [false],
            'onedrive_download': [false],
            'mail_flow_threshold': [],
            'create_folder_threshold': [],
            'create_site_threshold': [],
            'onedrive_upload_treshold': [],
            'onedrive_download_threshold': []
        });
        this.appComponentService = appComponentService;
}

    ngOnInit() {
        this.route.params.subscribe(params => {
            this.deviceId = params['service'];
         });
        this.dataProvider.get(`/configurator/${this.deviceId}/get_tests`)
            .subscribe(
            response => {
                this.tests.setValue(response.data);
            },
            (error) => {
                this.errorMessage = <any>error
                this.appComponentService.showErrorMessage(this.errorMessage);
            });
    }
    onSubmit(nameValue: any): void {
        this.dataProvider.put('/configurator/save_tests', nameValue)
            .subscribe(
            response => {

                if (response.status == "Success") {

                    this.appComponentService.showSuccessMessage(response.message);

                } else {

                    this.appComponentService.showErrorMessage(response.message);
                }
            }); 
    }
}