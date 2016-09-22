import {Pipe} from '@angular/core';
@Pipe({
    name: "search"
})
export class SearchDevicePipe{
    transform(value, [searchText]) {
       
        if (searchText) {
           // alert(searchText);
            return value.filter((item) => item.name.startsWith(searchText));
        }
        else {
            return value;
        }
    }

}