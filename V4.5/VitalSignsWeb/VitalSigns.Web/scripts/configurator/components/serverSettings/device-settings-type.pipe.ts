import {Pipe} from '@angular/core';
@Pipe({
    name: "FilterByDeviceType"
})
export class FilterByDeviceTypePipe {
    transform(value: any,  type: string) {       
        if (type) {
          
            value = value.filter((item) => item.device_type === type);
        }        
        return value;
    }

}