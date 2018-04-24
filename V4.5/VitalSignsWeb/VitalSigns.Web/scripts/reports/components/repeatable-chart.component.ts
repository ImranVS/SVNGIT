import { Component, Input, Output, AfterViewInit, EventEmitter } from '@angular/core';

@Component({
    selector: 'repeatable-chart',
    template: `
<div id="{{prefix}}-{{id}}"></div>
`,
})
export class RepeatableChart implements AfterViewInit {

    private _id: string;

    @Input() prefix: string;

    @Input() set id(id: string) {

        this._id = id;
        
    }

    get id() : string {

        return this._id;

    }

    @Output() render: EventEmitter<any> = new EventEmitter<any>();

    ngAfterViewInit() {

        this.render.emit({
            prefix: this.prefix,
            id: this._id,
            clientId: `${this.prefix}-${this._id}`
        });

    }

}