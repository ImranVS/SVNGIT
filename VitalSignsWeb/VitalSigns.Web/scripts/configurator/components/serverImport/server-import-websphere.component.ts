import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import {HttpModule}    from '@angular/http';
import {RESTService} from '../../../core/services';



@Component({
    templateUrl: '/app/configurator/components/serverImport/server-import-websphere.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class WebSphereServerImport {

}