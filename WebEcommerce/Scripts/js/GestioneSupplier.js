$(document).ready(function () {

    //  prendiamo  gli id; 
    const $btnCerca = $('#btnCerca');
    const $btnPulisci = $('#btnPulisci');
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

            //se  il controller tirona success
            success: function (data) {
                // pulisce la tabella
                $corpoTabella.empty();

                let rows = "";

                if (data.length === 0) {
                    rows = "<tr><td colspan='8' class='text-center text-muted'> Nessun fornitore trovato</td></tr>";
                } else {
                    $.each(data, function (i, item) {
                        rows += `<tr>
                            <td class="text-center">${item.CompanyName ?? ''}</td>
                            <td class="text-center">${item.ContactName ?? ''}</td>
                            <td class="text-center">${item.ContactTitle ?? ''}</td>
                            <td class="text-center">${item.City ?? ''}</td>
                            <td class="text-center">${item.Country ?? ''}</td>
                            <td>${item.Phone ?? ''}</td>
                            <td>${item.Fax ?? ''}</td>
                            <td class="text-end text-center px-4">
                                <a href="#" class="btn btn-sm btn-outline-info py-1 simpleBevel me-1" title="Visualizza Dettagli"><i class="bi bi-search"></i></a>
                                <a href="#" class="btn btn-sm btn-outline-warning py-1 simpleBevel me-1" title="Modifica riga"><i class="bi bi-pencil"></i></a>
                                <button type="button"                      
                                    class="btn btn-sm btn-outline-danger py-1 simpleBevel btn-elimina"
                                    title="Cancella fornitore"
                                    data-id="@sup.Id">
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
    $corpoTabella.on('click','.btn-elimina', function () {
       var id = $(this).data('id');
        $('#indirizzoDaEliminare').val(id);
        $('#modalElimina').modal('show');

    });

    
});