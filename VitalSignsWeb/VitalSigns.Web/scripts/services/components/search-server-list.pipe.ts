import {Pipe} from '@angular/core';
@Pipe({
    name: "searchserverlist"
})
export class SearchDeviceListPipe{
    transform(value: any, searchText: string, type: string, status: string, location: string) { 

        if (searchText) {  
            console.log(searchText);       
           value= value.filter((item) => item.name.toLocaleLowerCase().indexOf(searchText.toLocaleLowerCase()) !== -1);
        }
        if (type && type != "-All-") {     
            console.log(type);        
            value = value.filter((item) => item.type === type);
        }
        if (status && status != "-All-") {
            console.log(status);  
            value = value.filter((item) => item.status_code === status);
        }
        if (location && location != "-All-") {
            console.log(location);  
            value = value.filter((item) => item.location === location);
        }      
            return value;
      
    }

}