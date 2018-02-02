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
    
    public parameterForm: FormGroup;

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private route: ActivatedRoute, private service: RESTService, private formBuilder: FormBuilder,
        private appComponentService: AppComponentService, private authService: AuthenticationService) {
    }

    ngOnInit() {
        console.log(this.deviceId);
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
                    console.log("wes")
                    console.log(response)
                    this.devices = response.data.devices;
                    console.log(this.devices)
                    console.log(this.deviceId);
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

                    console.log(this.deviceTypes)
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
            device_id: mainThis.selectedDevice.device_id
        });
    }
    onSubmit(): void {
        var obj = this.parameterForm.getRawValue()
        this.appComponentService.showProgressBar();
        this.service.put("/services/execute_powershell_script", obj)
            .subscribe((response) => {
                this.appComponentService.hideProgressBar();
                if(response.status == "Success") {
                    this.response = response.data;
                } else {
                    this.response = response.message;
                }
            },
            (error) => {
                this.appComponentService.hideProgressBar();
                this.errorMessage = <any>error
            });

    }

    public initValues(settings: initSettings) {
        console.log("in initValues")
        console.log(settings)
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

        this.scriptChanged(null);

        this.response = "";
        
    }

    
    
}


export interface initSettings {
    DeviceType?: string,
    DefaultValues?: Map<string, string>,
    SubTypes?: Array<string>

}

