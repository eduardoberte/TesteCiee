
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private baseUrl = 'https://localhost:7150';

  usuarioLogado: any

  constructor(private http: HttpClient) {}

  getAnimais(): Observable<any> {
    return this.http.get(`${this.baseUrl}/animais`);
  }

  cadastrarAnimal(animal: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/animais`, animal);
  }

  atualizarAnimal(id: string, animal: any): Observable<any> {
    return this.http.put(`${this.baseUrl}/animais/${id}`, animal);
  }

  excluirAnimal(id: string): Observable<any> {
    return this.http.delete(`${this.baseUrl}/animais/${id}`);
  }

  getCuidados(): Observable<any> {
    return this.http.get(`${this.baseUrl}/cuidados`);
  }

  cadastrarCuidado(cuidado: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/cuidados`, cuidado);
  }

  atualizarCuidado(id: string, cuidado: any): Observable<any> {
    return this.http.put(`${this.baseUrl}/cuidados/${id}`, cuidado);
  }

  excluirCuidado(id: string): Observable<any> {
    return this.http.delete(`${this.baseUrl}/cuidados/${id}`);
  }

  getCuidadosPorAnimalId(id: string): Observable<any> {
    return this.http.get(`${this.baseUrl}/cuidados/animal/${id}`);
  }
 
}


