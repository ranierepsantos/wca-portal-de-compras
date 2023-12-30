export const tipoContratatacao =  [
    {id: 1, nome: "CLT"},
    {id: 2, nome: "Prazo Indeterminado"},
    {id: 3, nome: "Temporário"}
]

export const motivoTrabalhoTemporario = [
    {id: 1, nome: 'Substituição de Quadro Permanente'},
    {id: 2, nome: 'Acréscimo de Demanda de Serviços'}
]

export const motivoSubstituicaoQuadro = [
    {id: 1, nome: 'Acidente de Trabalho', capturarMotivo: false},
    {id: 2, nome: 'Auxílio Doença', capturarMotivo: false},
    {id: 3, nome: 'Cobertura de férias', capturarMotivo: false},
    {id: 4, nome: 'Licença Maternidade', capturarMotivo: false},
    {id: 5, nome: 'Outros', capturarMotivo: true}
]

const enumAprovador = {
    None: 0,
    Cliente: 1,
    WCA: 2
}


export const statusVaga = [
    {id: 1, statusCliente: 'Em análise', statusWca: 'Aguardando Aprovação', aprovadorPor: enumAprovador.WCA },
    {id: 2, statusCliente: 'Orçamento pendente', statusWca: 'Orçamento em análise', aprovadorPor: enumAprovador.Cliente},
    {id: 3, statusCliente: 'Aguardando Ordem de Compra', statusWca: 'Aguardando Ordem de Compra', aprovadorPor: enumAprovador.None},
    {id: 3, statusCliente: 'Seleção em Andamento', statusWca: 'Orçamento Aprovado', aprovadorPor: enumAprovador.None},
]