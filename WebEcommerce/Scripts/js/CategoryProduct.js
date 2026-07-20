$(document).ready(function () {

    const $btnCerca = $('#btnCerca');
    const $btnPulisci = $('#btnPulisci');
    const $btnAdd = $('#btnAggiungi');
    const $tabellaContainer = $('#tabellaContainer');


    /* ====================================================================================================
      1. RESETTA I FILTRI
      ================================================================================================== */
    $btnAdd.on('click', function () {
   
        $('#mdlTipo').val('STOCK');
        $('#mdlNome').val('');
        $('#mdlIndirizzo').val('');
        $('#mdlCAP').val('');
        $('#mdlCitta').val('');
        $('#mdlTrue').prop('checked', true);

    });

    /* ====================================================================================================
    2. SESSIONE PER SALVARE I FILTRI 
    ================================================================================================== */

    function eseguiRicerca() {

        const $searchUrl = $btnCerca.data('search-url');


        var filtri = {
            Name: $('input[name="Name"]').val(),
            IsActive: $('select[name="IsActive"]').val(),
            Description: $('input[name="Description"]').val()
            
        };

        // SESSIONE PER SALVARE I FILTRI
        sessionStorage.setItem('filtriCategorie', JSON.stringify(filtri));
       

        $.ajax({
            url: $searchUrl,
            type: 'GET',
            data: filtri,
            success: function (response) {
                $tabellaContainer.html(response).show();

                const nuovoUrl = window.location.pathname + '?Name=' + filtri.Name + '&IsActive=' + filtri.IsActive + '&Description=' + filtri.Description;
                history.pushState(null, '', nuovoUrl);

            },
            error: function (xhr, status, error) {
              
                $tabellaContainer.html('<tr><td colspan="10" class="text-center text-danger">Errore di connessione durante la ricerca.</td></tr>');
            }
        });

    }

   

    $('#btnCerca').on('click', function () {
        eseguiRicerca();

    });

     
    const filtriSalvati = sessionStorage.getItem('filtriCategorie');

    if (filtriSalvati) {

        const filtri = JSON.parse(filtriSalvati);
        let haFiltri = false;

        $('#searchNome').val(filtri.Type);
        $('#searchIsActive').val(filtri.Name);
        $('#searchDescription').val(filtri.City);
    
        haFiltri = true;

        if (haFiltri) {
            $btnCerca.click();
        }

    }

    // funzione per pulire i filtri
    $btnPulisci.on('click', function (e) {

      
        $('input[name="Name"]').val('');
        $('input[name="Description"]').val('');
    
        $('select[name="IsActive"]').val('');
        /* paginaCorrente = 1;*/
        $btnCerca.click();
        sessionStorage.removeItem("filtriMagazzini");
    });

    /* =================================================================================================
   3. EVENTO CLICK SU SALVA
   ======================================================================================================*/

    $('#btnSalva').on('click', function () {

        // pulire i filtri 
        const $addUrl = $('#btnSalva').data('save-url');

        // controllo check validità della modale
        const form = document.getElementById('form-add');

        if (!form.checkValidity()) {
            form.reportValidity(); // mostra i popup nativi di errore del browser
            return; // blocca l'invio AJAX
        }


        const model = {

            Name: $('#addNome').val(),
            Description: $('#addDescrizione').val(),
            IsActive: $('#addIsActive').val(),
        }

        $.ajax({

            url: $addUrl,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(model),
            dataType: 'json',

            success: function (response) {
                if (response.success) {


                    $('#btnCerca').click();

                    $('#resultMessage').html('<span style="color:green">Nuovo Listino Aggiunto</span>');

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

    /* ==========================================================
    4. APERTURA MODALE ELIMINA
    ==========================================================*/

    $('#tabellaContainer').on('click', '.btn-elimina', function () {

        const id = $(this).data('id');
        const name = $(this).data('name');

        $('#CategoriaDaEliminare').val(id);
        
        $('#CategoriaDaCancellare').text(name);

        $('#modalElimina').addClass('active2')
    });

    $('.no-btn').on('click', function () {

        $('#modalElimina').removeClass('active2');
    });

    /* =========================================================================================================
    5. EVENTO CLiCK SUL BOTTONE DELLA MODALE ELIMINA
    ===========================================================================================================*/

    $('#del-category').on('click', function () {

    
        const $deleteUrl = $('#del-category').data('delete-url');


        const model = {
            id: $('#CategoriaDaEliminare').val()
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
                    $('#resultMessage').html('<span style="color:red">' + response.message + '</span>');
                }
            },

        });

    });

/* ==========================================================
         4. APERTURA MODALE MODIFICA
  ==========================================================*/

    $('#tabellaContainer').on('click', '.btn-modifica', function () {

        const id = $(this).data('id');
        const name = $(this).data('name');

        $('#CategoriaDaEliminare').val(id);

        $('#CategoriaDaModificare').text(name);

        $('#modalEdit').addClass('active2')
    });

    $('.no-btn').on('click', function () {

        $('#modalEdit').removeClass('active2');
    });

    /* ==========================================================
         5. MODIfICA PRODOTTO
    ==========================================================*/

    $('#tabellaContainer').on('click', '.btn-modifica', function () {

      const  $loadUrl = $(this).data('load-url');

        const model = {
            id: $(this).data('id')
        }

        $.ajax({
            url: $loadUrl,
            type: 'GET',
            data: model,
            dataType: 'json',

            success: function (response) {
                if (response.success) {

                    $('#editId').val(response.data.Id);
                    $('#editNome').val(response.data.Name);
                    $('#editDescrizione').val(response.data.Description);
                    $('#editIsActive').val(response.data.IsActive ? "true" : "false");
                    
      

                    //$('input[name="editIsActive"][value="' + response.data.IsActive + '"]').prop('checked', true);


                 /*   $('#modalEdit').modal('show');*/

                } else {
                    $('#resultMessage').html('<span style="color:red">' + response.message + '</span>');
                }
            },

            error: function (xhr, status, error) {
                $('#resultMessage').html('<span style="color:red">Errore: ' + xhr.responseText + '</span>');
            }

        });

    });

    $('#edit-category').on('click', function () {

     
        $delUrl = $('#edit-category').data('edit-url');

        const model = {
            Id: $('#editId').val(),
            Name: $('#editNome').val(),
            Description: $('#editIsActive').val(),
            IsActive: $('#editIsActive').val(),
       
        }

        $.ajax({

            url: $delUrl,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(model),
            dataType: 'json',

            success: function (response) {
                if (response.success) {
                    $('#modalEdit').removeClass('active2');
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