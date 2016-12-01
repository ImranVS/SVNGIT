
import {Component, OnInit, AfterViewInit, ViewChild, ViewChildren} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import {Router, ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';
import {AppComponentService} from '../../../core/services';
import {RESTService} from '../../../core/services';

@Component({
    selector: 'ibmDominoSettings-form',
    templateUrl: '/app/configurator/components/ibmDomino/Ibm-domino-settings.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class IbmDominoSettingsForm implements OnInit, AfterViewInit {

    @ViewChildren('name') inputName;
    @ViewChild('message') message;
    insertMode: boolean = false;
    ibmDominoSettingsForm: FormGroup;
    errorMessage: string;
    successMessage: string;
    profileEmail: string;
    formTitle: string;
    isModified: boolean = false;
    protected appComponentService: AppComponentService;
    constructor(
        private formBuilder: FormBuilder,
        private dataProvider: RESTService,
        private router: Router,
        private route: ActivatedRoute,
     appComponentService: AppComponentService) {

        this.ibmDominoSettingsForm = this.formBuilder.group({
            'notes_program_directory': [''],
            'notes_user_id': [''],
            'notes_ini': [''],
            'notes_password': [''],
            'enableex_journal': [''],
            'enable_domino_console_commands': [''],
            'exjournal_threshold': [''],
            'consecutive_telnet': [''],
             'is_modified':['']

        });
        this.appComponentService = appComponentService;

    }

    ngOnInit() {
      
      
        this.route.params.subscribe(params => {

          
           
            this.formTitle = "IBM Domino Settings";
        
            this.dataProvider.get('/Configurator/get_ibm_domino_settings')
                    .subscribe(
                    (data) => this.ibmDominoSettingsForm.setValue(data.data),
                    (error) => {
                        this.errorMessage = <any>error
                        this.appComponentService.showErrorMessage(this.errorMessage);
                    }
                   
                    );             
        });

    }

    ngAfterViewInit() {

    }

    onSubmit(nameValue: any): void {
        this.errorMessage = "";
        this.successMessage = "";
        console.log(nameValue.is_modified);
        nameValue.is_modified = this.isModified;
        this.dataProvider.put('/Configurator/save_ibm_domino_settings', nameValue)
            .subscribe(
            response => {
              
                   if (response.status == "OK") {
                        
                        this.appComponentService.showSuccessMessage(response.message);

                    } else {

                        this.appComponentService.showErrorMessage(response.message);
                    }
            });
      


    }
    valuechange(newValue, form) {
    
      //  console.log(ids);
        console.log(form.notes_password);
        if (form.notes_password== "****") {
            console.log("**")
            this.isModified = false;
            console.log(this.isModified)
        }
        else {
            console.log("yessss*")
            this.isModified= true;
            console.log(this.isModified)

        }

    }
}