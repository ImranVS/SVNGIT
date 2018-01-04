import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';

import { FormBuilder, FormGroup, Validators, FormArray, FormControl } from '@angular/forms';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser'

import {WidgetController, WidgetContainer, WidgetContract} from '../../core/widgets';
import {WidgetService} from '../../core/widgets/services/widget.service';
import {AppNavigator} from '../../navigation/app.navigator.component';


import {ServiceTab} from '../models/service-tab.interface';
import { RESTService, AppComponentService } from '../../core/services';
declare var injectSVG: any;


@Component({
    templateUrl: '/app/services/components/microsoft-powershell-scripts.component.html',
    providers: [WidgetService, RESTService, FormsModule, ReactiveFormsModule]
})
export class MicrosoftPowerShellScripts extends WidgetController implements OnInit {
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

    public parameterForm: FormGroup;

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private route: ActivatedRoute, private service: RESTService, private formBuilder: FormBuilder,
        private appComponentService: AppComponentService) {

        super(resolver, widgetService);
    }

    ngOnInit() {
        this.route.params.subscribe(params => {
            if (params['service'])
                this.deviceId = params['service'];
        });
        this.service.get('/services/get_powershell_scripts')
            .subscribe(
            (response) => {
                

                this.devices = response.data.devices;
                this.scripts = response.data.scripts;
                if (this.deviceId != null || this.deviceId != "") {
                    this.devices = this.devices.filter(x => x.device_id == this.deviceId);
                    this.selectedType = this.devices.find(x => x.device_id == this.deviceId).device_type
                }

                this.deviceTypes = this.devices.map(x => x.device_type).filter(function (e, i, a) {
                    return i === a.indexOf(e);
                });
                
            },
            (error) => this.errorMessage = <any>error
            );
        
    }
    

    deviceTypeChanged(event: wijmo.EventArgs) {
        this.devicesFromType = this.devices.filter(x => x.device_type == this.selectedType);
        this.scriptsFromType = this.scripts.filter(x => x.device_type == this.selectedType);
    }

    deviceChanged(event: wijmo.EventArgs) {
    }

    scriptChanged(event: wijmo.EventArgs) {
        var mainThis = this;
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
            });

    }
}


