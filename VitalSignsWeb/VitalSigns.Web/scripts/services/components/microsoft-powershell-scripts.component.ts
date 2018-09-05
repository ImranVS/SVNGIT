import {Component, ComponentFactoryResolver, OnInit, Input, ViewChild} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import { AuthenticationService } from '../../profiles/services/authentication.service';
import { FormBuilder, FormGroup, Validators, FormArray, FormControl } from '@angular/forms';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser'

import {WidgetController, WidgetContainer, WidgetContract} from '../../core/widgets';
import {WidgetService} from '../../core/widgets/services/widget.service';
import {AppNavigator} from '../../navigation/app.navigator.component';


import {ServiceTab} from '../models/service-tab.interface';
import { RESTService, AppComponentService } from '../../core/services';
declare var injectSVG: any;
import { WidgetComponent } from '../../core/widgets';


@Component({
    selector: 'powershell-scripts',
    templateUrl: '/app/services/components/microsoft-powershell-scripts.component.html',
    providers: [WidgetService, RESTService, FormsModule, ReactiveFormsModule]
})
export class MicrosoftPowerShellScripts implements WidgetComponent, OnInit  {
    @Input() settings: any;
    deviceId: any;
    widgets: WidgetContract[];
    errorMessage: any;

    scripts: any;
    devices: any;
    deviceTypes: any;
    scriptsFromType: any;
    devicesFromType: any;

    selectedType: any;
    selectedDevice: any;
    selectedScript: any;

    response: string = "";

    showPowerScripts: boolean = false;

    subTypes:Array<string> = [];
    defaultValues: Map<string, string>;
    disabledFields: Array<string> = [];

    access: boolean = false;
    userId: string = "";
    password: string = "";
    useServerCreds: boolean = true;

    private isLoading: boolean = false;
    
    public parameterForm: FormGroup;

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private route: ActivatedRoute, private service: RESTService, private formBuilder: FormBuilder,
        private appComponentService: AppComponentService, private authService: AuthenticationService) {
    }

    ngOnInit() {
        this.route.params.subscribe(params => {
            if (params['service'])
                this.deviceId = params['service'];
        });

        if (this.authService.isCurrentUserInRole("PowerScripts")) {
            this.access = true
        } else {
            return;
        }

        this.service.get('/services/get_powershell_scripts')
            .subscribe(
            (response) => {
                try {
                    this.devices = response.data.devices;
                    this.scripts = response.data.scripts;
                    if (this.deviceId != null && this.deviceId != "") {
                        this.devices = this.devices.filter(x => x.device_id == this.deviceId);
                        this.selectedType = this.devices.find(x => x.device_id == this.deviceId).device_type
                    }
                    else {
                        
                    }

                    this.deviceTypes = this.devices.map(x => x.device_type).filter(function (e, i, a) {
                        return i === a.indexOf(e);
                    });
                    
                } catch (ex){
                    console.log(ex)
                }
            },
            (error) => this.errorMessage = <any>error
            );
        
    }
    

    deviceTypeChanged(event: wijmo.EventArgs) {
        this.devicesFromType = this.devices.filter(x => x.device_type == this.selectedType);
        this.scriptsFromType = this.scripts.filter(x => x.device_type == this.selectedType);
        var outterThis = this;
        
        if (this.subTypes && this.subTypes.length > 0)
            this.scriptsFromType = this.scriptsFromType.filter(x => x.sub_types.some(function (y) {
                return outterThis.subTypes.indexOf(y) !== -1;
            }));
    }

    deviceChanged(event: wijmo.EventArgs) {
        if (this.selectedDevice.credential && this.selectedDevice.credential.user_id)
            this.userId = this.selectedDevice.credential.user_id;
        else
            this.userId = "";
    }

    scriptChanged(event: wijmo.EventArgs) {
        console.log("in scriptChanged")
        var mainThis = this; 
        if (this.defaultValues)
            this.defaultValues.forEach((value: string, key: string) => {
                var param = mainThis.selectedScript.parameters.find(x => x.name == key);
                if (param) {
                    param.value = value;
                }
            });
        
        this.parameterForm = this.formBuilder.group({
            path: mainThis.selectedScript.path,
            parameters: mainThis.formBuilder.array(mainThis.selectedScript.parameters.map(function (x) { return mainThis.formBuilder.group({ name: x.name, value: x.value }); })),
            device_id: mainThis.selectedDevice.device_id,
            name: mainThis.selectedScript.name,
            user_id: "",
            password: "",
            server_credentials: this.useServerCreds 
        });
    }
    onSubmit(): void {
        this.parameterForm.patchValue({
            user_id: this.userId,
            password: this.password,
            server_credentials: this.useServerCreds
        });
        var obj = this.parameterForm.getRawValue()
        this.isLoading = true;
        //this.buttonnativeElement.visability = false;
        this.service.put("/services/execute_powershell_script", obj)
            .subscribe((response) => {
                try{
                    this.isLoading = false;
                    if (response.status == "Success") {
                        this.response = response.data;
                    } else {
                        this.response = response.message;
                    }
                }catch (ex) {
                    console.log(ex);
                }
            },
            (error) => {
                this.isLoading = false;
                //this.button.disabled = false;
                this.errorMessage = <any>error
            });

    }

    public initValues(settings: initSettings) {
        if (settings.DeviceType) {
            this.selectedType = settings.DeviceType;
            this.disabledFields.push("DeviceType");
        }

        if (settings.DefaultValues) {
            this.defaultValues = settings.DefaultValues;
        } else {
            this.defaultValues = new Map<string, string>();
        }


        if (settings.SubTypes) {
            this.subTypes = settings.SubTypes
        }

        this.deviceTypeChanged(null)
        this.scriptChanged(null);

        this.response = "";
        
    }

    
    
}


export interface initSettings {
    DeviceType?: string,
    DefaultValues?: Map<string, string>,
    SubTypes?: Array<string>

}

