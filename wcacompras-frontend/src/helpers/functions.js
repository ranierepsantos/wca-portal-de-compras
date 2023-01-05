export const status = [
    { value: -1, text: "Todos" },
    { value: 0, text: "Aguardando", color: "warning" },
    { value: 1, text: "Aprovado", color: "success" },
    { value: 2, text: "Rejeitado", color: "error" },
    { value: 3, text: "Finalizado", color: "success" },
    { value: 4, text: "Cancelado", color: "error" }
]

export const compararValor = function (key, order = "asc")
{
    return function(a, b) {
        if (!a.hasOwnProperty(key) || !b.hasOwnProperty(key)) {
            // property doesn't exist on either object
            return 0;
        }

        const varA = typeof a[key] === "string" ? a[key].toUpperCase() : a[key];
        const varB = typeof b[key] === "string" ? b[key].toUpperCase() : b[key];

        let comparison = 0;
        if (varA > varB) {
            comparison = 1;
        } else if (varA < varB) {
            comparison = -1;
        }
        return order == "desc" ? comparison * -1 : comparison;
    };
};

export const toBase64 = function(arquivo) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.readAsDataURL(arquivo);
        reader.onload = () => resolve(reader.result);
        reader.onerror = error => reject(error);
    });
};

export const realizarDownload = async function(response, nomearquivo, fileType="application/pdf") {
    // It is necessary to create a new blob object with mime-type explicitly set
    // otherwise only Chrome works like it should
    var newBlob = new Blob([response.data], {
        type: fileType
    });

    // IE doesn't allow using a blob object directly as link href
    // instead it is necessary to use msSaveOrOpenBlob
    if (window.navigator && window.navigator.msSaveOrOpenBlob) {
        window.navigator.msSaveOrOpenBlob(newBlob);
        return;
    }

    // For other browsers:
    // Create a link pointing to the ObjectURL containing the blob.
    const data = window.URL.createObjectURL(newBlob);
    var link = document.createElement("a");
    link.href = data;
    link.download = nomearquivo;
    link.click();
    setTimeout(function() {
        // For Firefox it is necessary to delay revoking the ObjectURL
        window.URL.revokeObjectURL(data);
    }, 100);
};