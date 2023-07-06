import {Route} from "@angular/router";
import {HomeComponent} from "./home/home.component";

import {FetchDataComponent} from "./fetch-data/fetch-data.component";

export const appRoutes: Route[] = [
  {path: '', component: HomeComponent, pathMatch: 'full'},
  {path: 'fetch-data', component: FetchDataComponent},
  {path: 'users', loadChildren: () => import ('src/app/users/users.module').then(m => m.UsersModule)}
]
