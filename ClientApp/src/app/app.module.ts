import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';
import {FormsModule} from '@angular/forms';
import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';
import {RouterModule} from '@angular/router';

import {AppComponent} from './app.component';
import {NavMenuComponent} from './nav-menu/nav-menu.component';
import {HomeComponent} from './home/home.component';
import {FetchDataComponent} from './fetch-data/fetch-data.component';
import {CsrfHeaderInterceptor} from "./csrf-header.interceptor";
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {ModalModule} from 'ngx-bootstrap/modal';
import {appRoutes} from "./app.routing";
import {SharedModule} from "./shared/shared.module";

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    FetchDataComponent
  ],
  imports: [
    BrowserModule.withServerTransition({appId: 'ng-cli-universal'}),
    HttpClientModule,
    FormsModule,
    SharedModule,
    RouterModule.forRoot(appRoutes),
    BrowserAnimationsModule,
    ModalModule.forRoot()
  ],
  // Need For Call Apis
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: CsrfHeaderInterceptor,
      multi: true,
    },
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}
