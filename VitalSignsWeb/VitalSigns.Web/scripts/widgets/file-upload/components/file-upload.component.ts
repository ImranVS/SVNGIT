import {Component, Input, OnInit} from '@angular/core';
import {HttpModule}    from '@angular/http';
import {WidgetComponent} from '../../../core/widgets';
import {WidgetController, WidgetContract, WidgetService} from '../../../core/widgets';
import {Router, ActivatedRoute} from '@angular/router';
import {RESTService} from '../../../core/services/rest.service';
import {Http, Response, Headers} from '@angular/http';
declare var injectSVG: any;
declare var bootstrapNavigator: any;


@Component({
    templateUrl: './app/widgets/file-upload/components/file-upload.component.html',
    providers: [
        WidgetService,
        RESTService
    ]
})

export class FileUpload implements WidgetComponent, OnInit {
    @Input() settings: any;
    public formData = new FormData();
    public file: any;
    public url: string;
    selectedFiles: any;
    constructor(private service: RESTService, private router: Router, private route: ActivatedRoute, public http: Http) {
        //set the header as multipart        
        this.url = '/configurator/upload_file';
    }

    ngOnInit() {
    }
    uploadFiles(fileInput: any) {
        this.service.post(this.url, this.formData);
        var v2 = <HTMLDivElement>document.getElementById("dvSelectedFiles");
        v2.innerHTML = "Uploaded Selected Files(s)!";
        this.formData = null;
    }
    changeListener(fileInput: any) {
        this.postFile(fileInput);
        
    }
    //send post file to server 
    postFile(inputValue: any): void {
        var formData = new FormData();
        var v2 = <HTMLDivElement>document.getElementById("dvSelectedFiles");
        for (let i = 0; i < inputValue.target.files.length; i++) {
            formData.append("file-" + i.toString(), inputValue.target.files[i]);
            this.formData.append("file-" + i.toString(), inputValue.target.files[i]);
            v2.innerHTML += inputValue.target.files[i].name +"<br/>";
        }
        
       // this.service.post(this.url, formData);
    }
}