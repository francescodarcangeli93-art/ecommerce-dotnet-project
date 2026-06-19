$(document).ready(function () {

    //  prendiamo  gli id; 
    const $btnCerca = $('#btnCerca');
    const $btnPulisci = $('#btnPulisci');
    const $corpoTabella = $('#corpoTabella');

    const $searchUrl = $btnCerca.attr('data-url');

    //  alla funzione click
    $btnCerca.click(function () {

        // 
        const param = {

            CompanyName: $('#txtCompany').value,
            txtCompany: $('#txtReferente').value,
            txtCompany: $('#txtRuolo').value,
            txtCompany: $('#txtCitta').value,
            txtCompany: $('#txtPaese').value,
            txtCompany: $('#txtTelefono').value,
            txtCompany: $('#txtFax').value,
        };

        $.ajax({

            url: $searchUrl,
            Type: 'POST',

            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(param),

            dataType: 'Json',

            // TODO: da finire la  parte del success e della stampa della tabella  sicuramente modificare il controller

        });

    });


    // TODO: funzione che al click del pulsante pulisci resetta tutti i filtri

});