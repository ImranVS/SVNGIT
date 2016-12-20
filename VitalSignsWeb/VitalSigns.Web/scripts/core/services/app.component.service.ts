import { Injectable } from '@angular/core';

import { AppComponent } from '../../app.component';

@Injectable()
export class AppComponentService {
   
    public _appComponent: AppComponent;

    registerAppComponentView(appComponent: AppComponent) {
        this._appComponent = appComponent;
    }

    showAlertMessage(_type: string, _message: string, _dismissOnTimeout: number = 5000) {
        this._appComponent.showAlert(_type, _message, _dismissOnTimeout);
    }
    showSuccessMessage( _message: string, _dismissOnTimeout: number = 5000) {
        this._appComponent.showAlert("success", _message, _dismissOnTimeout);
    }

    showErrorMessage(_message: string) {
        this._appComponent.showAlert("danger", _message, 0);
    }
    closeAlertMessage($event) {
        this._appComponent.onClose($event);
    }
    showProgressBar() {
        this._appComponent.showProgress();
    }
    hideProgressBar() {
        this._appComponent.hideProgress();
    }

}