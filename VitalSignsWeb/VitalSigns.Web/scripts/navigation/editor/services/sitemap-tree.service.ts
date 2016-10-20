import {Injectable} from '@angular/core';

@Injectable()
export class SiteMapTreeService {

    private _containers: any[] = [];
    private _models: any[] = [];

    public registerNode(container: any, model: any) {

        if (this._containers.indexOf(container) >= 0) {

            this._models[this._containers.indexOf(container)] = model;

        } else {

            this._containers.push(container);
            this._models.push(model);

        }

    }

    public getContainerModel(container: any) {

        return this._models[this._containers.indexOf(container)];

    }

    public removeNode(container: any) {

        let index = this._containers.indexOf(container);
        
        if (index >= 0) {

            this._containers.splice(index, 1);
            this._models.splice(index, 1);

        }

    }
}