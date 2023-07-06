import { NgModule } from '@angular/core';
import { UserComponent } from './user/user.component';
import { UserListComponent } from './user-list/user-list.component';
import { SharedModule } from '../shared/shared.module';
import { Route, RouterModule } from '@angular/router';


const routes: Route[] = [
  {
    path     : '',
    component: UserListComponent
  },

  {
    path     : ':id',
    component: UserComponent
  },

  {
    path     : 'create',
    component: UserComponent,
  },
];

@NgModule({
  declarations: [
    UserComponent,
    UserListComponent
  ],
  imports: [
    SharedModule,
    RouterModule.forChild(routes),
  ]
})
export class UsersModule { }
