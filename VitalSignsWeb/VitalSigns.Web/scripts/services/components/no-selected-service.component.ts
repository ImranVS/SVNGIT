import {Component} from '@angular/core';

import {ServiceTab} from '../models/service-tab.interface';

@Component({
    template: `
<div id="zeusContext">
    <div id="zeusContextNavigation">
        <div id="zeusContextNavigationIcon">
            <img class="svgInject" src="img/menu/servers.svg" title="Dashboard" alt="Dashboard" />
        </div>
        <div id="zeusContextNavigationText">
            <h2>
                Servers                
            </h2>
            <p>Manage your servers</p>
        </div>
    </div>
 <div class="clearfix"></div>
</div>
<div id="noServerSelectedWrapper">
    <div id="noServerSelected">
        <img id="noServerImg" class="svgInject" src="img/menu/servers.svg" title="Servers" alt="Servers" />
        <h2>No server selected</h2>
        <p>Choose a server by clicking a server on the list or click on add server button   <button type="button" class="btn btn-primary" >
                            <span class="glyphicon glyphicon-plus"></span>
                            Add Server
                        </button> &nbsp; from configurator </p>
        
    </div>
</div>
`,
})
export class NoSelectedService implements ServiceTab {
    serviceId: string;
}