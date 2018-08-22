import { Pipe } from '@angular/core';
import { Tab } from '../models/tab.interface';
@Pipe({
    name: "tabVisiblePipe"
})
export class TabVisiblePipe{
    transform(value: Tab[]) {
        if (value != null) {
            return value.filter(x => x.visible == undefined || x.visible);
        }
        return value;
    }
}