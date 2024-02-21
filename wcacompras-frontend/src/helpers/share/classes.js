export class Evento {
    nota = "";
    entidade = ""
    entidadeId = ""
    
    constructor(data = null){
        this.nota = data ? data.nota: "";
        this.entidade = data? data.entidade: ""
        this.entidadeId = data? data.entidade: 0
    }
}