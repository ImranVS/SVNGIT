import { Injectable } from '@angular/core';

import { AppComponent } from '../../app.component';

@Injectable()
export class AlertService {
   
    private _appComponent: AppComponent;

    registerAppComponentView(appComponent: AppComponent) {
        this._appComponent = appComponent;
    }

    showAlertMessage(_type: string, _message: string, _dismissOnTimeout: number = 5000) {
        this._appComponent.showAlert(_type, _message, _dismissOnTimeout);
    }

}