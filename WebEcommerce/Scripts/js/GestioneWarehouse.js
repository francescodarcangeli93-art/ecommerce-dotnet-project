$(document).ready(function () {
    const $btnCerca = $('#btnCerca');
    const $btnPulisci = $('#btnPulisci');
    const $btnAdd = $('#btnAggiungi');
    const $tabellaContainer = $('#tabellaContainer');

    function eseguiRicerca() {

        var filtri = {
            Type: $('select[name="Type"]').val(),
            Name: $('input[name="Nome"]').val(),
            City: $('input[name="City"]').val(),
            PostalCode: $('input[name="PostalCode"]').val(),
            IsActive: $('#searchIsActive').val()
        };

        // sessione per salvare i filtri

        $.ajax({
            url: '/GestWarehouses/SearchWarehousesPartial',
            type: 'GET',
            data: filtri,
            success: function (response) {
                $tabellaContainer.html(response).show();
            },
            error: function (xhr, status, error) {
                console.error("Errore ricerca:", error);
                $tabellaContainer.html('<tr><td colspan="10" class="text-center text-danger">Errore di connessione durante la ricerca.</td></tr>');
            }
        });
    }

    $('#btnCerca').on('click', function () {
        eseguiRicerca();
       
    });

    $('#btnSalva').on('click', function () {

        // pulire i filtri 


        // controllo check validità della modale
        const form = document.getElementById('warehouseForm');

        if (!form.checkValidity()) {
            form.reportValidity(); // mostra i popup nativi di errore del browser
            return; // blocca l'invio AJAX
        }


        const model = {

            Type: $('#mdlTipo').val(),
            Name: $('#mdlNome').val(),
            Address: $('#mdlIndirizzo').val(),
            PostalCode: $('#mdlCAP').val(),
            City: $('#mdlCitta').val(),
            IsActive: $('input[name="IsActive"]:checked').val() === "true"

        }

        $.ajax({

            url: '/GestWarehouses/Save',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(model),
            dataType: 'json',

            success: function (response) {
                if (response.success) {

               
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



    $('#tabellaContainer').on('click', '.btn-elimina', function () {

        const id = $(this).data('id');
        const name = $(this).data('name');

        $('#indirizzoDaEliminare').val(id);

        $('#modalElimina').modal('show');
    });

    $('#confermaElimina').on('click', function () {

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

        });

    });


    // FILTRI 
    $('#btnCerca').on('click', function () {


    });
});