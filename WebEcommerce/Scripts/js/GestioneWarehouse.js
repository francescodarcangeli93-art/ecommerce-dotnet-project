$(document).ready(function () {
    const $btnCerca = $('#btnCerca');
    const $btnPulisci = $('#btnPulisci');
    const $btnAdd = $('#btnAggiungi');
    const $tabellaContainer = $('#tabellaContainer');

    $btnAdd.on('click', function () {
        // resettare i filtri 

        $('#mdlTipo').val('STOCK');
        $('#mdlNome').val('');
        $('#mdlIndirizzo').val('');
        $('#mdlCAP').val('');
        $('#mdlCitta').val('');
        $('#mdlTrue').prop('checked', true);

    });

    /*=============================================================================================================================================
        1 RICERCA 
    ==============================================================================================================================================*/

    function eseguiRicerca() {

        var filtri = {
            Type: $('select[name="Type"]').val(),
            Name: $('input[name="Name"]').val(),
            City: $('input[name="City"]').val(),
            PostalCode: $('input[name="PostalCode"]').val(),
            IsActive: $('#searchIsActive').val()
        };

        // sessione per salvare i filtri
        sessionStorage.setItem('filtriMagazzini', JSON.stringify(filtri));

        $.ajax({
            url: '/GestWarehouses/SearchWarehousesPartial',
            type: 'GET',
            data: filtri,
            success: function (response) {
                $tabellaContainer.html(response).show();

                const nuovoUrl = window.location.pathname + '?Type=' + filtri.Type + '&Nome=' + filtri.Name + '&City=' + filtri.City + '&PostalCode=' + filtri.PostalCode + '&IsActive=' + filtri.IsActive; 
                history.pushState(null, '', nuovoUrl);
                
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

    //---------------------------------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------


    const filtriSalvati = sessionStorage.getItem('filtriMagazzini');


    // se ci sono i filtri 
    if (filtriSalvati) {

        const filtri = JSON.parse(filtriSalvati);
        let haFiltri = false;

        $('#searchType').val(filtri.Type);
        $('#searchNome').val(filtri.Name);
        $('#searchCitta').val(filtri.City);
        $('#searchPostalCode').val(filtri.PostalCode);
        $('#searchIsActive').val(filtri.IsActive);
       

        haFiltri = true;

        if (haFiltri) {
            $btnCerca.click();
        }
        
    }

    //---------------------------------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------


    // funzione per pulire i filtri
    $('#btnPulisci').on('click', function (e) {

        $('select[name="Type"]').val('');
        $('input[name="Name"]').val('');
        $('input[name="City"]').val('');
        $('input[name="PostalCode"]').val('');
        $('select[name="IsActive"]').val('');
       /* paginaCorrente = 1;*/
        $btnCerca.click();
        sessionStorage.removeItem("filtriMagazzini");
    });

    //---------------------------------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------


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
                    $('#resultMessageElimina').html('<span style="color:red">' + response.message + '</span>');
                }
            },

        });

    });

    /* ==========================================================
       3. MODIFICA  WAREHOUSE
       ==========================================================*/

    $('#tabellaContainer').on('click', '.btn-modifica', function () {

        const model = {
            id: $(this).data('id')
        }

        $.ajax({
            url: '/GestWarehouses/Edit',
            type: 'GET',
            data: model,
            dataType: 'json',

            success: function (response) {
                if (response.success) {
                    $('#editId').val(response.data.Id);
                    $('#editTipo').val(response.data.Type);
                    $('#editNome').val(response.data.Name);
                    $('#editIndirizzo').val(response.data.Address);
                    $('#editCAP').val(response.data.PostalCode);
                    $('#editCitta').val(response.data.City);
                 
                    $('input[name="editIsActive"][value="' + response.data.IsActive + '"]').prop('checked', true);
                  

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

    $('#btnEditModifica').on('click', function () {

        const model = {
            Id: $('#editId').val(),
            Type: $('#editTipo').val(),
            Name: $('#editNome').val(),
            Address: $('#editIndirizzo').val(),
            PostalCode: $('#editCAP').val(),
            City: $('#editCitta').val(),
            IsActive: $('input[name=editIsActive]:checked').val()
            
        }

        

        $.ajax({
            url: '/GestWarehouses/Edit',
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