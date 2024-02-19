export const TipoSolicitacao = [
    {text: 'Desligamento', value: 1},
    {text: 'Comunicado', value: 2},
    {text: 'Férias', value: 3},
    {text: 'Mudança de base', value: 4},
]

//FUNCTIONS

export function getTextFromListByCodigo(list, codigo) {
    let _data = list.find((x) => x.value ==  codigo);
    return _data ? _data.text : "";
}
