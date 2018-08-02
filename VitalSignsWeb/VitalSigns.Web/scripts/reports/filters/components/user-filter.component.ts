import {Component, Output, EventEmitter, ViewChildren, Input} from '@angular/core';
import {Router, ActivatedRoute} from '@angular/router';
import {RESTService} from '../../../core/services';
import {WidgetComponent} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';

@Component({
    selector: 'user-filter',
    templateUrl: '/app/reports/filters/components/user-filter.component.html',
})
export class UserFilter {
    @ViewChildren('multisel1') multisel1: wijmo.input.MultiSelect;
    selectedUsers: any;
    @Input() widgetName: string;
    @Input() widgetURL: string;
    @Input() showTopX: boolean = false;
    @Input() showServerControl: boolean = false;
    @Input() showCommunityControl: boolean = false;
    @Input() showDateRangeControl: boolean = false;

    deviceType = "IBM Connections"
    errorMessage: any;
    topXData = ["1", "3", "5", "10", "25", "50", "100", "All"]
    selectedTopX = "25"

    allCommunitiesData: any[];
    communitiesData: any[];
    allUserData: any[];
    userData: any[];
    deviceNameData: any[];
    usersTimestamp: Date;
    
    constructor(private service: RESTService, private router: Router, private route: ActivatedRoute, private widgetService: WidgetService) { }
    ngOnInit() {
        if (this.showCommunityControl)
            //this.service.get(`/services/ibm_connections_community_list_dropdown?users=true`)
            this.service.get(`/services/ibm_connections_community_list_dropdown?users=false`)
                .subscribe(
                    (response) => {
                        this.allCommunitiesData = response.data;
                    },
                    (error) => this.errorMessage = <any>error
            );

        if (this.showServerControl)
            this.service.get(`/services/server_list_dropdown?type=${this.deviceType}`)
                .subscribe(
                    (response) => {
                        this.deviceNameData = response.data.deviceNameData;
                    },
                    (error) => this.errorMessage = <any>error
                );
        
            this.service.get(`/dashboard/connections/users?server=true`)
                .subscribe(
                    (response) => {
                        this.allUserData = response.data;
                        this.userData = response.data;
                    },
                    (error) => this.errorMessage = <any>error
                );
    }

    applyFilters(multisel1: wijmo.input.MultiSelect, multiSelectServer: wijmo.input.MultiSelect, multiSelectCommunity: wijmo.input.MultiSelect) {
        var selectedUsers = "";
        for (var item of multisel1.checkedItems) {
            if (selectedUsers == "") 
                selectedUsers = item.name;
            else 
                selectedUsers += "," + item.name;
        }
        var URL = ((this.widgetURL.includes("?")) ? (this.widgetURL + "&") : (this.widgetURL + "?")) + `userNames=` + selectedUsers;
        if (this.showTopX)
            URL = URL += "&topX=" + this.selectedTopX;
        if (this.showCommunityControl)
            URL += "&communityIds=" + multiSelectCommunity.checkedItems.map(x => x.id).toString();
        if (this.showServerControl)
            URL += "&deviceIds=" + multiSelectServer.checkedItems.map(x => x.id).toString();
        this.widgetService.refreshWidget(this.widgetName, URL )
            .catch(error => console.log(error));

    }

    serversChanged(e) {
        if (this.showCommunityControl) {
            if (this.deviceNameData.find(x => x.name == "All" && x.selected == true)) {
                var selectedData = this.deviceNameData.map(y => y.id);
                this.communitiesData = this.allCommunitiesData.filter(x => selectedData.indexOf(x.device_id) >= 0 || x.device_id == "")
            } else {
                var selectedData = this.deviceNameData.filter(y => y.selected === true).map(y => y.id);
                this.communitiesData = this.allCommunitiesData.filter(x => selectedData.indexOf(x.device_id) >= 0 || x.device_id == "")
            }
        } 

        this.updateUsers();
        
    }

    communitiesChanged(e) {
        if (this.showCommunityControl) {
            if (this.deviceNameData.find(x => x.name == "All" && x.selected == true)) {
                var selectedData = this.deviceNameData.map(y => y.id);
                this.communitiesData = this.allCommunitiesData.filter(x => selectedData.indexOf(x.device_id) >= 0 || x.device_id == "")
            } else {
                var selectedData = this.deviceNameData.filter(y => y.selected === true).map(y => y.id);
                this.communitiesData = this.allCommunitiesData.filter(x => selectedData.indexOf(x.device_id) >= 0 || x.device_id == "")
            }
        }
        this.updateUsers();
    }

    updateUsers() {
        let queryString: string[]= [];
        if (this.showServerControl) {
            if (this.deviceNameData.find(x => x.name == "All" && x.selected == true) || this.deviceNameData.find(x => x.selected == true) == null) {
                //no filter is applied
            }
            else {
                var selectedData = this.deviceNameData.filter(y => y.selected === true).map(y => y.id);
                queryString.push("deviceIds=" + selectedData.join(','));
            }
        }

        if (this.showCommunityControl) {
            if (this.communitiesData.find(x => x.name == "All" && x.selected == true) || this.communitiesData.find(x => x.selected == true) == null) {
                //no filter is applied
            } else {
                var selectedData = this.communitiesData.filter(y => y.selected === true).map(y => y.id);
                queryString.push("communityIds=" + selectedData.join(','));
            }
        }

        this.usersTimestamp = new Date();
        let currCallDate = new Date(this.usersTimestamp);

        this.service.get(`/dashboard/connections/users${queryString.length != 0 ? '?' : ''}${queryString.join('&')}`)
            .subscribe(
            (response) => {
                if (currCallDate.getTime() >= this.usersTimestamp.getTime()) {
                    console.log("Refreshing");
                    this.userData = response.data;
                }
                else { console.log("Not Refreshing") }
                    
                },
                (error) => this.errorMessage = <any>error
        );

        return;
//The stuff below this can probably be deleted but leaving it for another patch or 2 incase its needed. If you are reading this comment and dont know what its talking about, you can delete it if you wish
        var tempData: any[];
        if (this.showServerControl) {
            if (this.deviceNameData.find(x => x.name == "All" && x.selected == true) || this.deviceNameData.find(x => x.selected == true) == null) {
                var selectedData = this.deviceNameData.map(y => y.id);
                tempData = this.allUserData.filter(x => selectedData.indexOf(x.device_id) >= 0 || x.device_id == "")
            } else {
                var selectedData = this.deviceNameData.filter(y => y.selected === true).map(y => y.id);
                tempData = this.allUserData.filter(x => selectedData.indexOf(x.device_id) >= 0 || x.device_id == "")
            }
        } else {
            tempData = this.allUserData;
        }
        if (this.showCommunityControl) {
            if (this.communitiesData.find(x => x.name == "All" && x.selected == true) || this.communitiesData.find(x => x.selected == true) == null) {
                var selectedData = this.communitiesData.map(y => y.id);
                tempData = tempData.filter(x => x.community_ids && x.community_ids.some(y => selectedData.indexOf(y) >= 0) || x.device_id == "")
            } else {
                var selectedData = this.communitiesData.filter(y => y.selected === true).map(y => y.id);
                tempData = tempData.filter(x => x.community_ids && x.community_ids.some(y => selectedData.indexOf(y) >= 0) || x.device_id == "")
            }
        } else {
            tempData = tempData;
        }
        this.userData = tempData;
    }

}