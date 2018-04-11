import 'core-js/es6';
import 'core-js/es7/reflect';
import 'zone.js/dist/zone';
import 'core-js/client/shim.min.js';
//import 'systemjs/dist/system.src.js';

import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app.module';
import { enableProdMode } from '@angular/core';


enableProdMode();
platformBrowserDynamic().bootstrapModule(AppModule);
