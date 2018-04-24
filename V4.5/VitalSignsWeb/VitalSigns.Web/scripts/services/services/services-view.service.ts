import { Injectable } from '@angular/core';

import { ServicesView } from '../components/services-view.component';

@Injectable()
export class ServicesViewService {

    private _servicesView: ServicesView;

    registerServicesView(servicesView: ServicesView) {

        this._servicesView = servicesView;

    }

    refreshServicesList() {

        this._servicesView.loadData();

    }
    
}