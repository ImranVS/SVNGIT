import {Pipe} from '@angular/core';
@Pipe({
    name: "searchserverlist"
})
export class SearchDeviceListPipe{
    transform(value: any, searchText: string, type: string, status: string, location: string) {
        if (value != null) {
            if (searchText!="") {
                value = value.filter((item) => item.name.toLocaleLowerCase().indexOf(searchText.toLocaleLowerCase()) !== -1);
            }
            if (type && type != "-All-") {
                value = value.filter((item) => item.type === type);
            }
            if (status && status != "-All-") {
                value = value.filter((item) => item.status_code === status);;
            }
            if (location && location != "-All-") {
                value = value.filter((item) => item.location === location);
            }
        }
        return value;
      
    }

}