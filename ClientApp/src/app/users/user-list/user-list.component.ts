import { Component, TemplateRef, ViewChild } from '@angular/core';
import { UserService } from '../services/user.service';
import { Observable, merge, startWith, switchMap, catchError, map } from 'rxjs';
import { IButtonContainerSettings } from 'src/app/shared/models/button-container';
import { BsModalRef, BsModalService, ModalContainerComponent, ModalOptions } from 'ngx-bootstrap/modal';
import { IUser } from '../models';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { HttpParams } from '@angular/common/http';
import { Router } from '@angular/router';
import { MatTableDataSource, MatTableDataSourcePaginator } from '@angular/material/table';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent {

  settingsButton = {
    fn: this.goToCreatePage.bind(this),
    icon: 'post_add',
    text: 'Create'
  } as IButtonContainerSettings;

  displayedColumns: string[] = ['id', 'username', 'name', 'email', 'role', 'options'];
  dataSource: MatTableDataSource<IUser> | undefined;


  @ViewChild(MatSort) sort!: MatSort;

  pageSize = '15';


  // https://levelup.gitconnected.com/how-to-pass-data-between-ngx-bootstrap-modal-and-parent-e348cd596cf7
  bsModalRef?: BsModalRef;   // ng add ngx-bootstrap  --component modals

  categoryId: number = 0;


  constructor(
    private userService: UserService,
    private modalService: BsModalService,
    private router: Router) { }


  ngAfterViewInit() {
    this.getUsers();
  }

  getUsers() {


    this.userService.get().subscribe(resp => {

      console.log(resp);
      this.dataSource = new MatTableDataSource(resp.users);
    });

  }



  goToCreatePage() {
    this.router.navigate(['articles/create'])
  }


  goToEditPage(title: string, id: string) {
    this.router.navigate([`articles/${title}/${id}/edit`]);
  }


  goToDetailPage(title: string, id: string) {
    this.router.navigate([`articles/${title}/${id}/`])
  }


  delete(id: string, title: string, template: TemplateRef<any>) {

    const initialState: ModalOptions = { initialState: { module: 'users', title: title, }, class: 'modal-dialog-centered' };

    this.bsModalRef = this.modalService.show(template, initialState);

    this.bsModalRef.content.event.subscribe(() => {

      this.userService.delete(id).subscribe({
        next: (v) => {


          // to refresh the table
          this.getUsers();
        },
        error: (e) => console.log(e)
      });

    });
  }








}
