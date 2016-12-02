import {Component, OnInit, ViewChildren} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import {Router, ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';
import {RESTService} from '../../../core/services';
import {AppComponentService} from '../../../core/services';

@Component({
    selector: 'server-simulation-list',
    templateUrl: '/app/configurator/components/server/server-simulation-tests.compoment.html',
    providers: [
        HttpModule,
        RESTService
        ]
})
export class SimulationTests implements OnInit {
    @ViewChildren('name') inputName;
    deviceId: any;
    insertMode: boolean = false;
    simulationTests: FormGroup;
    errorMessage: string;
    profileEmail: string;
    formTitle: string;
    appComponentService: AppComponentService;

    constructor(
        private formBuilder: FormBuilder,
        private dataProvider: RESTService,
        private router: Router,
        private route: ActivatedRoute, appComponentService: AppComponentService ) {
        this.simulationTests = this.formBuilder.group({
            'id':[],
            'create_activity_threshold': [],
            'create_blog_threshold': [],
            'create_bookmark_threshold': [],
            'create_community_threshold': [],
            'create_file_threshold': [],
            'create_wiki_threshold': [],
            'search_profile_threshold': [],
            'create_activity': [false],
            'create_blog': [false],
            'create_bookmark': [false],
            'create_community': [false],
            'create_file': [false],
            'create_wiki': [false],
            'search_profile': [false]
        });
        this.appComponentService = appComponentService;
}

    ngOnInit() {
        this.route.params.subscribe(params => {
            this.deviceId = params['service'];
         });
        this.dataProvider.get('/configurator/' + this.deviceId + '/get_simulationtests')
            .subscribe(
            response => {
                this.simulationTests.setValue(response.data);
            },
            (error) => {
                this.errorMessage = <any>error
                this.appComponentService.showErrorMessage(this.errorMessage);
            });
    }
    onSubmit(nameValue: any): void {
        this.dataProvider.put('/configurator/save_simulationtests', nameValue)
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