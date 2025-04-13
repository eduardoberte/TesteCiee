import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ApiService } from 'src/app/service/api.service';
import { LoginService } from 'src/app/service/login.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {

  showModal = false;
  usuarioLogado:any;

  loginData = {
    email: '',
    password: ''
  };

  newUser = {
    email: '',
    password: '',
    confirmPassword: ''
  };

  constructor(private router: Router,private apiService: ApiService,private loginService:LoginService) {
  }

  get passwordMismatch(): boolean {
    return this.newUser.password !== this.newUser.confirmPassword;
  }

  openModal() {
    this.showModal = true;
  }

  closeModal() {
    this.showModal = false;
  }

  createUser(){
    if (this.passwordMismatch) {
      return;
    }
    const newUser = {
      email: this.newUser.email,
      password: this.newUser.password
    };

    this.loginService.createUser(newUser);
    this.showModal = false;
    this.newUser = { email: '', password: '', confirmPassword: '' };
  };
  
  login() {
    
    if(this.loginService.login(this.loginData.email, this.loginData.password)){
      this.router.navigate(['/home']);    
    }else {
      alert('Email ou senha inválidos ou usuário não cadastrado.');
    }
  } 
}