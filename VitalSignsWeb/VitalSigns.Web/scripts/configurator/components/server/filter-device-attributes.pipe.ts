import {Pipe} from '@angular/core';
@Pipe({
    name: "filterDeviceAttributes"
})
export class FilterDeviceAttributesPipe {
    transform(value: any, category: string) {
        console.log(value);  
        if (category) {
            console.log(category);  
            value = value.filter((item) => item.category.toLocaleLowerCase().indexOf(category.toLocaleLowerCase()) !== -1);
        }
        console.log(value);  
        return value;
    }

}