import {Pipe} from '@angular/core';
@Pipe({
    name: "filterwidgets"
})
export class FilterWidgetsPipe{
    transform(value: any, searchField: string, searchText: string) {
        if (value != null) {
            if (searchText != "") {
                value = value.filter((item) => item[searchField] != null && item[searchField].toLocaleLowerCase().indexOf(searchText.toLocaleLowerCase()) == -1);
            }
        }
        return value;      
    }

}