import { Injectable } from '@angular/core';

import { AppComponent } from '../../app.component';

@Injectable()
export class AppComponentService {
   
    public _appComponent: AppComponent;

    registerAppComponentView(appComponent: AppComponent) {
        console.log(appComponent);
        this._appComponent = appComponent;
    }

    showAlertMessage(_type: string, _message: string, _dismissOnTimeout: number = 5000) {
        console.log(this._appComponent);
        this._appComponent.showAlert(_type, _message, _dismissOnTimeout);
    }
    showSuccessMessage( _message: string, _dismissOnTimeout: number = 5000) {
        console.log(this._appComponent);
        this._appComponent.showAlert("success", _message, _dismissOnTimeout);
    }

    showErrorMessage(_message: string) {
        console.log(this._appComponent);
        this._appComponent.showAlert("danger", _message, 0);
    }

}