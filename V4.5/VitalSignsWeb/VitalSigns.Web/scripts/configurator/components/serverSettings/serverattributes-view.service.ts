import { Injectable } from '@angular/core';
import { ServersLocation } from '../server-list-location.component';



@Injectable()
export class ServersLocationService {
    
    public _serversLocation: ServersLocation;
    
    registerServerLocation(serversLocation: ServersLocation) {
  
        this._serversLocation = serversLocation;
        

    }

    refreshServerLocations(deviceType:string) {
       
        this._serversLocation.onDeviceListChange(deviceType);

    }

}