import {Pipe} from '@angular/core';
@Pipe({
    name: "filterDeviceAttributes"
})
export class FilterDeviceAttributesPipe {
    transform(value: any, category: string) {
        if (category) {
            value = value.filter((item) => item.catogory.toLocaleLowerCase().indexOf(category.toLocaleLowerCase()) !== -1);
        }       
        return value;
    }

}