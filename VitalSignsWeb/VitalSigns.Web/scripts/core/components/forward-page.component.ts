import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
    template: ''
})
export class ForwardPage implements OnInit {

    constructor(
        private router: Router,
        private route: ActivatedRoute) { }

    ngOnInit(): void {
    
        this.router.navigateByUrl(this.route.snapshot.params['ref']);
        
    }
}