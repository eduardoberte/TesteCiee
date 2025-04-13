export interface CuidadoModel {
    id: number;
    nome: string;
    descricao: string;
  }
export interface AnimalModel {
    id: number;
    nome: string;
    raca: string;
    especie: string;
    cuidados: CuidadoModel[];
  }