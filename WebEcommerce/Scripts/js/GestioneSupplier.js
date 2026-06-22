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
            Country: $('#txtPaese').val(),
            Phone: $('#txtTelefono').val(),
            Fax: $('#txtFax').val(),
        };

        $.ajax({

            url: $searchUrl,
            type: 'POST',

            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(param),

            dataType: 'json',

            //se  il controller ritorna success
            success: function (data) {
                // pulisce la tabella
                $corpoTabella.empty();

                let rows = "";

                if (data.length === 0) {
                    rows = "<tr><td colspan='8' class='text-center text-muted'> Nessun fornitore trovato</td></tr>";
                } else {
                    $.each(data, function (i, item) {
                        rows += `<tr data-id="${item.Id}">
                            <td class="text-center">${item.CompanyName ?? ''}</td>
                            <td class="text-center">${item.ContactName ?? ''}</td>
                            <td class="text-center">${item.ContactTitle ?? ''}</td>
                            <td class="text-center">${item.City ?? ''}</td>
                            <td class="text-center">${item.Country ?? ''}</td>
                            <td>${item.Phone ?? ''}</td>
                            <td>${item.Fax ?? ''}</td>
                            <td class="text-end text-center px-4">

                                <a class="btn btn-sm btn-outline-info py-1 simpleBevel me-1 btn-dettagli" data-id="${item.Id}" title="Visualizza Dettagli"><i class="bi bi-search"></i></a>

                                <a class="btn btn-sm btn-outline-warning py-1 simpleBevel me-1 btn-modifica" title="Modifica riga"><i class="bi bi-pencil"></i></a>
                                
                                 <button type="button"                      
                                    class="btn btn-sm btn-outline-danger py-1 simpleBevel btn-elimina"
                                    title="Cancella fornitore"
                                    data-id="${item.Id}"
                                    data-companyname="${item.CompanyName}">

                                <i class="bi bi-trash"></i>
                            </button>

                            </td>
                        </tr>`;
                    });
                }

                $corpoTabella.html(rows);
            },

            error: function (xhr, status, error) {
                alert("Errore durante la ricerca: " + xhr.responseText);
            }

        });

    });

    //  funzoine che pulisce i filtri 
    $btnPulisci.click(function () {

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

    //gestione evento cancella con la delega al corpo della tabella
    $corpoTabella.on('click', '.btn-elimina', function () {
        const id = $(this).data('id');
        const name = $(this).data('companyname');

        $('#indirizzoDaEliminare').val(id);
        $('#nomeFornitore').text(name);

        $('#modalElimina').modal('show');
        

    });

    $('#confermaElimina').on('click', function () {
        console.log('CLICK RICEVUTO');

        const $btnElimina = $('#confermaElimina');
        const $deleteUrl = $btnElimina.attr('data-url');

        const model = {
            id: $('#indirizzoDaEliminare').val()
        }

        $.ajax({
            url: $deleteUrl,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(model),
            dataType: 'json',

            success: function (response) {
                if (response.success) {

                    $('#modalElimina').modal('hide');

                    // rimuovo la riga della tabella corrispondente all'indirizzo eliminato
                    $('tr[data-id="' + model.id + '"]').remove();
                  
                } else {
                    $('#resultMessage').html('<span style="color:red">' + response.message + '</span>');
                }
            },

            error: function (xhr, status, error) {
                $('#resultMessage').html('<span style="color:red">Errore: ' + xhr.responseText + '</span>');
            }

        });

    });

    $corpoTabella.on('click', '.btn-dettagli', function () {
       
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


        $('#btnSalva').on('click', function () {

            const model = {
                CompanyName: $('#Company').val(),
                ContactName: $('#Referente').val(),
                ContactTitle: $('#Ruolo').val(),
                City: $('#Citta').val(),
                Country: $('#Paese').val(),
                Phone: $('#Telefono').val(),
                Fax: $('#Fax').val(),
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

    // modifica fornitori

});