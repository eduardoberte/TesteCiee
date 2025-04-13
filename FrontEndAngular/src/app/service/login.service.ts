import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  constructor(private router:Router) {
  }

  login(email:string,password:any){

    const users = JSON.parse(localStorage.getItem('users') || '[]');
    const found = users.find(
      (u: any) =>
        u.email === email && u.password === password
    );

    if (found) {
      localStorage.setItem('logado', JSON.stringify(found));
      return true;
    } 
    return false;
  }

  logOut(){
    localStorage.removeItem('logado')
    this.router.navigate(['/']);
  }

  getUserLogado(){
    return JSON.parse(localStorage.getItem('logado')||'');
  }

  createUser(newUser:any){
    let users = JSON.parse(localStorage.getItem('users') || '[]');
    const exists = users.some((u: any) => u.email === newUser.email);
  
    if (exists) {
      alert('Este email já está cadastrado.');
      return;
    }

    users.push(newUser);
    localStorage.setItem('users', JSON.stringify(users));
    alert('Usuário criado com sucesso!');
  }
}