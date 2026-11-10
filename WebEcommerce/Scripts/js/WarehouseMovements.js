
let $tabellaContainer;

function eseguiRicerca(pageNumber) {

    $tabellaContainer = $('#tabellaContainer');

    const isGlobalVal = $tabellaContainer.data('is-global') !== undefined
        ? $tabellaContainer.data('is-global')
        : true;

    var filtri = {
        StartDate: $('#searchStartDate').val(),
        EndDate: $('#searchEndDate').val(),
        WarehouseId: $('#searchWarehouseId').val(),
        Description: $('#searchDescription').val(),
        PageNumber: pageNumber,
        isGlobal: isGlobalVal
    };


    // sessione per salvare i filtri
    sessionStorage.setItem('filtriMagazzini', JSON.stringify(filtri));
    $('#loading').removeClass('textHidden');

    $.ajax({
        url: searchMovements,
        type: 'GET',
        data: filtri,
        success: function (response) {

            $('#loading').addClass('textHidden');
            $tabellaContainer.html(response).show();

            const nuovoUrl = window.location.pathname + '?StartDate=' + filtri.StartDate + '&EndDate=' + filtri.EndDate + '&WarehouseId=' + filtri.WarehouseId;
            history.pushState(null, '', nuovoUrl);
            $('#tabellaContainer').addClass('table-visible');
        },
        error: function (xhr, status, error) {
            console.error("Errore ricerca:", error);
            $tabellaContainer.html('<tr><td colspan="10" class="text-center text-danger">Errore di connessione durante la ricerca.</td></tr>');
        }
    });
}


/*=============================================================================================================================================
    FUNZIONE CHE CHIAMA IL GET PER LA MODIFICA E CHE VIENE RICHIAMATA PER IL FILE GLOBALE
==============================================================================================================================================*/

function editGet($btn) {

    const $url = $btn.data('edit-url');
    const id = $btn.data('id');


    const model = {
        MovementId: id
    }


    $.ajax({
        url: $url,
        type: 'GET',
        data: model,
        dataType: 'json',

        success: function (response) {
            if (response.success) {

          

                $('#editNote').val(response.data || 'Nessuna nota');

            } else {
                alert("Errore: " + response.message);
            }
        },
        error: function (xhr, status, error) {
            console.error("Errore AJAX:", error);
            alert("Si è verificato un errore di rete durante il recupero dei dati.");
        }

    });

}

// -------------------------------------------------------------------------------------------------------------------------

$(document).ready(function () {

    $tabellaContainer = $('#tabellaContainer');
    const $btnCerca = $('#btnCerca');
    const $btnPulisci = $('#btnPulisci');

    const magazzinoDestinazione = $('#rowMagazzinoDestinazione').addClass('d-none');

    // TODO CREARE UNA FUNIONE PER PULIZIA

    const urlParams = new URLSearchParams(window.location.search);
    let id = urlParams.get('WarehouseId');

    $('#searchWarehouseId').val(id);
    eseguiRicerca(1);




    $('#saveCausale').on('change', function () {

        const causale = $(this).val();


        if (causale && causale == 1) {
            $('#lblMagazzino').text('Magazzino di partenza');
            magazzinoDestinazione.removeClass('d-none');
        } else {
            $('#lblMagazzino').text('Magazzino');
            magazzinoDestinazione.addClass('d-none');
        }

    });

   
    /*=============================================================================================================================================
        1 RICERCA 
    ==============================================================================================================================================*/
    // TO DO PER I PRODOTTI


    $($btnCerca).on('click', function () {

        eseguiRicerca(1);

    });

    // EVENTO CLICK CHE PULISCE I FILTRI 

    $('#btnPulisci').on('click', function (e) {
        $('#searchStartDate').val(''),
            $('#searchEndDate').val(''),
            $('#searchWarehouseId').val(''),
            $('#searchDescription').val('')

        /* paginaCorrente = 1;*/
        $btnCerca.click();
        sessionStorage.removeItem("filtriMagazzini");
    });

    /*=============================================================================================================================================
       2 SALVA MOVIMENTO 
   ==============================================================================================================================================*/

    $('#SalvaMovimento').on('click', function () {

        const $url = $(this).data('save-url');

     

        const model = {

            MovementReasonId: $('#saveCausale').val(),
            ProductId: $('#saveProdotto').val(),
            QtyToTransfer: $('#saveQuantita').val(),
            Notes: $('#saveNote').val(),
            WarehouseId: $('#saveMagazzino').val(),
            DestinationWarehouseId: $('#saveMagazzinoDestinazione').val(),

        };

        $.ajax({
            url: $url,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(model),
            dataType: 'json',

            success: function (response) {
                if (response.success) {

                    $('#resultMessageSaveMovement').html('<span style="color:green; font-weight:bold;">Salvataggio completato!</span>');

                    // ricarica la partialview
                    eseguiRicerca(1);

                } else {
                    $('#resultMessageSaveMovement').html('<span style="color:red">' + response.message + '</span>');

                }
            },
            error: function (xhr) {
                $('#resultMessage').html('<span style="color:red">Errore: ' + xhr.responseText + '</span>');
            }
        });

    });

    /*=============================================================================================================================================
      3 DETTAGLIO MOVIMENTO DI MAGAZZINO 
    ==============================================================================================================================================*/


    $tabellaContainer.on('click', '.btn-dettagli', function () {

        const $url = $(this).data('url');

        const model = {
            MovementId: $(this).data('id')
        }

        $.ajax({
            url: $url,
            type: 'GET',
            data: model,
            dataType: 'json',

            success: function (response) {
                if (response.success) {

                    const item = response.data.TheGetWarehouseMovementDetail;

                    // Popoliamo i campi della modale con i dati ricevuti da C#
                    $('#banner-tipo').addClass(item.MovementType === '+' ? 'green-first' : 'error-first');
                  

                    $('#det-movement-id').text(item.MovementId);
                    $('#det-date').text(item.DateFormatted);
                    $('#det-warehouse-name').text(item.WarehouseName);
                    $('#det-warehouse-id').text(item.WarehouseId);
                    $('#det-product-name').text(item.ProductName);
                    $('#det-product-id').text(item.ProductId);
                    $('#det-company-name').text(item.CompanyName || '-');
                    $('#det-description').text(item.Description);
                    $('#det-reason-id').text(item.MovementReasonId);
                    $('#det-detail').text(item.Detail);
                    $('#det-detail-text').text(item.Detail);
                    $('#det-notes').text(item.Notes || 'Nessuna nota');

                    // Quantità e Segno
                    const segno = item.MovementType === '+' ? '+' : '-';
                    $('#det-quantity').html(`${segno}${item.Quantity} <span class="fs-6 fw-normal">pz</span>`);

                    // Apriamo la modale Bootstrap
                    $('#modalDettaglioMovimento').modal('show');
                } else {
                    alert("Errore: " + response.message);
                }
            },
            error: function (xhr, status, error) {
                console.error("Errore AJAX:", error);
                alert("Si è verificato un errore di rete durante il recupero dei dati.");
            }
        });
    });

    $('#backToMovement').on('click', function () {

        $('#banner-tipo').removeClass('green-first error-first');
    });


    /*=============================================================================================================================================
      4 CLICK MODIFCA NOTE 
    ==============================================================================================================================================*/

    $(document).on('click', '#edit-movement', function (e) {
     
        e.preventDefault();

        const id = $('.global-modaledit-id').val();
        const notes = $('#editNote').val();
        console.log(id);

        const $url = $(this).data('edit-url');

        const model = {
            MovementId: id,
            Notes: notes
        }

        $.ajax({
            url: $url,
            type: 'POST',
            data: model,
            dataType: 'json',

            success: function (response) {
                if (response.success) {

                    $('#edit-modal')[0].close();
                    eseguiRicerca(1);

                    // stampa messaggio di successo
                    $('.allert').show().text('modifica effettuata con successo!').addClass('green-first');

                    setTimeout(function () {
                        $('.allert').fadeOut(500); // sparisce in 500ms
                    }, 3000); //aspetta 3 secondi prima di iniziare a sparire

                } else {
                    alert("Errore: " + response.message);
                }
            },
            error: function (xhr, status, error) {
                console.error("Errore AJAX:", error);
                alert("Si è verificato un errore.");
            }
        });

    });

});