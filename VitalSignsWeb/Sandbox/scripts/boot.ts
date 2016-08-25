///<reference path="../node_modules/angular2/typings/browser.d.ts"/> 
import {bootstrap} from 'angular2/platform/browser'
import 'rxjs/Rx';

import {Traveler} from './Traveler/Traveler'
import {MobileDevices} from './Traveler/MobileDevices'
import {CountUserDevicesByType} from './Traveler/Views/CountUserDevicesByType'
import {CountUserDevicesByOS} from './Traveler/Views/CountUserDevicesByOS'

bootstrap(Traveler);