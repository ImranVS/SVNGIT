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
    templateUrl: '/app/services/components/microsoft-powershell-scripts-tab.component.html',
    providers: [WidgetService, RESTService, FormsModule, ReactiveFormsModule]
})
export class MicrosoftPowerShellScriptsTab extends WidgetController implements OnInit {
    deviceId: any;
    service: any;
    serviceId: string;
    widgets: WidgetContract[];

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private route: ActivatedRoute) {
        super(resolver, widgetService);
    }

    ngOnInit() {
        this.route.params.subscribe(params => {
            if (params['service'])
                this.deviceId = params['service'];
        });
        this.widgets = [
            {
                id: 'MicrosoftPowerShellScripts',
                title: '',
                name: 'MicrosoftPowerShellScripts',
                css: 'col-xs-12',
            }
        ]
        injectSVG();
        
    }
    
    
}


