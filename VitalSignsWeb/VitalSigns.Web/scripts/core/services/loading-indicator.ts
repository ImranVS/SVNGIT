import { Component, OnInit, OnDestroy, Input } from '@angular/core';

@Component({
    selector: 'loading-indicator',
    template: `
        <div *ngIf='isLoading' style="min-width: 65px; min-height: 65px">
            <div style="position: absolute; width: 100%; height:100%; z-index: 100100; filter:alpha(opacity=50);opacity: 0.5;">
                <div style="border-radius: 50% 50% 50% 50%;box-shadow: 5px 5px 150px 50px #FFFFFF, -5px -5px 150px 50px #FFFFFF;display: inline-block;height: 64px;width: 64px;"><img src='/img/loading-64.gif'/> </div>
            </div>
        </div>

    `
})

export class LoadingIndicator implements OnInit, OnDestroy {
    @Input() isLoading: boolean = false;
    //private isLoading = true;

    showOrHideLoadingIndicator(loading) {
        this.isLoading = loading;
        //if(this.isLoading)
    }

    ngOnInit() {

    }

    ngOnDestroy() {

    }
}