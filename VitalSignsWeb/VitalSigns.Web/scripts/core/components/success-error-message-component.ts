import {Component, Input, trigger, state, animate, transition, style} from '@angular/core';

@Component({
    selector: 'success-error-message',
    providers: [],
    animations: [
        trigger('visibility', [
            state('shown', style({
                opacity: 1
            })),
            state('hidden', style({
                opacity: 0
            })),
            transition('* => *', animate('.5s'))
        ])
    ],
    template: `
<div [ngSwitch]="showDiv">
  <div [@visibility]="success_visibility" #successDiv id="successDiv" *ngSwitchCase="'success'" class="alert alert-success col-lg-12">
      {{message}}
      <button type="button" tabindex="-1" class="close wj-hide" (click)="toggleVisibility(false,'')" name="cancel">&times;</button>
  </div>
  <div [@visibility]="error_visibility" #successDiv id="successDiv" *ngSwitchCase="'error'" class="alert alert-danger col-lg-12">
      {{message}}
      <button type="button" tabindex="-1" class="close wj-hide" (click)="toggleVisibility(true,'')" name="cancel">&times;</button>
  </div>
</div>
  `
})
export class SuccessErrorMessageComponent {
    @Input() message: any;
    showDiv: string;
    success_visibility: string = 'hidden';
    error_visibility: string = 'hidden';

    toggleVisibility(isError: boolean, msg) {
        if (!isError) {
            this.showDiv = "success";
            this.message = msg;
            this.success_visibility = this.success_visibility == 'shown' ? 'hidden' : 'shown';
            this.error_visibility = 'hidden';
        }
        else {
            this.showDiv = "error";
            this.message = msg;
            this.error_visibility = this.error_visibility == 'shown' ? 'hidden' : 'shown';
            this.success_visibility = 'hidden';
        }
    }
}