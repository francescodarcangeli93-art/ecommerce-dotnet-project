$(document).ready(function () {

    //  prendiamo  gli id; 
    const $btnCerca = $('#btnCerca');
    const $btnPulisci = $('#btnPulisci');
    const $btnAdd = $('#btnAggiungi');
    const $corpoTabella = $('#corpoTabella');
    
    const $searchUrl = $btnCerca.attr('data-url');

    //  alla funzione click
    $btnCerca.click(function () {


        const param = {

            CompanyName: $('#txtCompany').val(),
            ContactName: $('#txtReferente').val(),
            ContactTitle: $('#txtRuolo').val(),
            City: $('#txtCitta').val(),
            CountryId: $('#txtPaese').val(),
            Phone: $('#txtTelefono').val(),
            Fax: $('#txtFax').val(),
        };

        $.ajax({

            url: '/GestSupplier/SearchSupplierPartial',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(param),
           

            //se  il controller ritorna success
            success: function (tabella) {
               
                $('#tabellaContainer').html(tabella);
            },

            error: function (xhr, status, error) {
                alert("Errore durante la ricerca: " + xhr.responseText);
            }

        });

    });

    //  funzione che pulisce i filtri 
    $('#btnPulisci').click(function () {

        $('#txtCompany').val('');
        $('#txtReferente').val('');
        $('#txtRuolo').val('');
        $('#txtCitta').val('');
        $('#txtPaese').val('');
        $('#txtTelefono').val('');
        $('#txtFax').val('');

        // click sul pulsate cerca
        $('#btnCerca').click();
    });



    /* ==========================================================
    2. APERTURA MODALE ELIMINA
    ==========================================================*/

    $('#tabellaContainer').on('click', '.btn-elimina', function () {

        const id = $(this).data('id');
        const name = $(this).data('companyname');

        $('#fornitoreDaEliminare').val(id);
        $('#supplierNameMessage').text(name);

        $('#modalElimina').addClass('active2')
    });


    $('.no-btn').on('click', function () {

        $('#modalElimina').removeClass('active2');
    });


    $('#confermaElimina').on('click', function () {

        console.log("stampa delete");
        console.log($('#fornitoreDaEliminare').val());

        const $deleteUrl = $(this).data('delete-url');

        const model = {
            id: $('#fornitoreDaEliminare').val()
        }

        $.ajax({
            url: $deleteUrl,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(model),
            dataType: 'json',

            success: function (response) {
                if (response.success) {

                    $('#modalElimina').removeClass('active2');

                    // rimuovo la riga della tabella corrispondente all'indirizzo eliminato
                    $('tr[data-id="' + model.id + '"]').remove();
                  
                } else {
                    $('#resultMessageDel').html('<span style="color:red">' + response.message + '</span>');
                }
            },

            error: function (xhr, status, error) {
                $('#resultMessage').html('<span style="color:red">Errore: ' + xhr.responseText + '</span>');
            }

        });

    });

    $('#tabellaContainer').on('click', '.btn-dettagli', function () {
       
        const model = {
            id: $(this).data('id')
        }

        $.ajax({
            url: '/GestSupplier/Details',
            type: 'GET',
            data: model,
            dataType: 'json',

            success: function (response) {
                if (response.success) {

                    $('#showCompany').val(response.data.CompanyName);
                    $('#showReferente').val(response.data.ContactName);
                    $('#showRuolo').val(response.data.ContactTitle);
                    $('#showCitta').val(response.data.City);
                    $('#showPaese').val(response.data.Country);
                    $('#showTelefono').val(response.data.Phone);
                    $('#showFax').val(response.data.Fax);

                    $('#modalDettagli').modal('show');

                } else {
                    $('#resultMessage').html('<span style="color:red">' + response.message + '</span>');
                }
            },

            error: function (xhr, status, error) {
                $('#resultMessage').html('<span style="color:red">Errore: ' + xhr.responseText + '</span>');
            }

        });

        
    });

    // evento click aggiungi
    $btnAdd.on('click', function () {

        $('#modalAggiungi').modal('show');

        //puliamo i campi della modale aggiungi
        $('#Company').val('');
        $('#Referente').val('');
        $('#Ruolo').val('');
        $('#Citta').val('');
        $('#Paese').val('');
        $('#Telefono').val('');
        $('#Fax').val('');

    });

    $('#btnSalva').on('click', function () {
  
        const model = {
            CompanyName: $('#Company').val(),
            ContactName: $('#Referente').val(),
            ContactTitle: $('#Ruolo').val(),
            City: $('#Citta').val(),
            CountryId: $('#Paese').val(),
            Phone: $('#Telefono').val(),
            Fax: $('#Fax').val()
        }

        $.ajax({
            url: '/GestSupplier/Save',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(model),
            dataType: 'json',

            success: function (response) {
                if (response.success) {
                    $('#modalAggiungi').modal('hide');
                    $('#btnCerca').click();

                    $('#resultMessage').html('<span style="color:green">Nuovo fornitore aggiunto con successo!</span>');

                    setTimeout(function () {
                        $('#resultMessage').html('');
                    }, 3000);

                } else {
                    $('#resultMessage').html('<span style="color:red">' + response.message + '</span>');
                }
            },

            error: function (xhr, status, error) {
                $('#resultMessage').html('<span style="color:red">Errore: ' + xhr.responseText + '</span>');
            }

        });
    });

    // modifica fornitori
    $('#tabellaContainer').on('click','.btn-modifica', function () {

        const model = {
            id: $(this).data('id')
        }

        $.ajax({
            url: '/GestSupplier/Edit',
            type: 'GET',
            data: model,
            dataType: 'json',

            success: function (response) {
                if (response.success) {
                    $('#editId').val(response.data.Id);
                    $('#editCompany').val(response.data.CompanyName);
                    $('#editReferente').val(response.data.ContactName);
                    $('#editRuolo').val(response.data.ContactTitle);
                    $('#editCitta').val(response.data.City);
                    $('#editPaese').val(response.data.CountryId);
                    $('#editTelefono').val(response.data.Phone);
                    $('#editFax').val(response.data.Fax);

                    $('#modalEdit').modal('show'); 



                } else {
                    $('#resultMessage').html('<span style="color:red">' + response.message + '</span>');
                }
            },

            error: function (xhr, status, error) {
                $('#resultMessage').html('<span style="color:red">Errore: ' + xhr.responseText + '</span>');
            }

        });

    });

    $('#btnModifica').on('click', function () {

        const model = {
            Id: $('#editId').val(),
            CompanyName: $('#editCompany').val(),
            ContactName: $('#editReferente').val(),
            ContactTitle: $('#editRuolo').val(),
            City: $('#editCitta').val(),
            CountryId: $('#editPaese').val(),
            Phone: $('#editTelefono').val(),
            Fax: $('#editFax').val(),
        }

        $.ajax({
            url: '/GestSupplier/Edit',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(model),
            dataType: 'json',

            success: function (response) {
                if (response.success) {
                    $('#modalEdit').modal('hide');
                    $('#btnCerca').click();

                } else {
                    $('#resultMessage').html('<span style="color:red">' + response.message + '</span>');
                }
            },

            error: function (xhr, status, error) {
                $('#resultMessage').html('<span style="color:red">Errore: ' + xhr.responseText + '</span>');
            }

        });
    });
});

