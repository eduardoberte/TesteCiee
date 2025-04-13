import { Component, OnInit } from '@angular/core';
import { ApiService } from 'src/app/service/api.service';
import { Router } from '@angular/router';
import { LoginService } from 'src/app/service/login.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  constructor(private router: Router, private apiService: ApiService,private loginService:LoginService) {}
  showAnimalModal = false;
  showCareModal = false;
  showCuidadosModal = false;
  modoEdicao = false;

  animais: any[] = [];
  animalComCuidados: any = null;
  novoAnimal: any = this.criarAnimalVazio();
  animalSelecionado: any = null;
  novoCuidado = { nome: '', descricao: '', frequencia: '' };

  ngOnInit(): void {

    if (!this.loginService.getUserLogado()) {
      this.router.navigate(['/']);
      return;
    }

    this.carregarAnimais();
  }

  logOut(){
    this.loginService.logOut();
}

  criarAnimalVazio() {
    return {
      id: null,
      nome: '',
      descricao: '',
      dataNascimento: '',
      especie: '',
      habitat: '',
      paisOrigem: '',
      cuidados: []
    };
  }

  carregarAnimais() {
    this.apiService.getAnimais().subscribe({
      next: data => this.animais = data,
      error: err => console.error('Erro ao buscar animais:', err)
    });
  }

  openAnimalModal() {
    this.modoEdicao = false;
    this.novoAnimal = this.criarAnimalVazio();
    this.showAnimalModal = true;
  }
//
  editarAnimal(animal: any) {
    this.modoEdicao = true;
    this.novoAnimal = { ...animal };
    this.showAnimalModal = true;
  }

  closeModals() {
    this.showAnimalModal = false;
    this.showCareModal = false;
    this.showCuidadosModal = false;
    this.animalComCuidados = null;
    this.novoAnimal = this.criarAnimalVazio();
    this.modoEdicao = false;
  }

  cadastrarAnimal() {
    if (this.modoEdicao) {
      this.apiService.atualizarAnimal(this.novoAnimal.id, this.novoAnimal).subscribe({
        next: () => {
          this.carregarAnimais();
          this.closeModals();
        },
        error: err => console.error('Erro ao atualizar animal:', err)
      });
    } else {
      this.apiService.cadastrarAnimal(this.novoAnimal).subscribe({
        next: () => {
          this.carregarAnimais();
          this.closeModals();
        },
        error: err => console.error('Erro ao cadastrar animal:', err)
      });
    }
  }

  excluirAnimal(animal: any) {
    if (confirm("Confirmar a exclusão?")) {
      this.apiService.excluirAnimal(animal.id).subscribe({
        next: () => this.carregarAnimais(),
        error: err => console.error('Erro ao excluir animal:', err)
      });
    }
  }

  cadastrarCuidado() {
    if (this.animalSelecionado && this.novoCuidado.nome) {
      const cuidadoParaApi = {
        ...this.novoCuidado,
        animalIds: [this.animalSelecionado.id]
      };

      this.apiService.cadastrarCuidado(cuidadoParaApi).subscribe({
        next: () => {
          this.carregarAnimais();
          this.novoCuidado = { nome: '', descricao: '', frequencia: '' };
          this.animalSelecionado = null;
          this.closeModals();
        },
        error: err => console.error('Erro ao cadastrar cuidado:', err)
      });
    }
  }

  abrirCuidadosModal(animal: any) {
    this.animalComCuidados = {
      nome: animal.nome,
      cuidados: []
    };

    this.apiService.getCuidadosPorAnimalId(animal.id).subscribe({
      next: cuidados => {
        this.animalComCuidados.cuidados = cuidados;
        this.showCuidadosModal = true;
      },
      error: err => console.error('Erro ao buscar cuidados:', err)
    });
  }
}