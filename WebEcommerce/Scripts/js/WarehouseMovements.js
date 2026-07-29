$(document).ready(function () {

    const $tabellaContainer = $('#tabellaContainer');
    const $btnCerca = $('#btnCerca');
    const $btnPulisci = $('#btnPulisci');

    const magazzinoDestinazione = $('#saveMagazzinoDestinazione').addClass('d-none');

    $('#saveCausale').on('change', function () {

        const causale = $(this).val();
   

        if (causale && causale == 1) {
       
            magazzinoDestinazione.removeClass('d-none');
        } 

    });

    /*=============================================================================================================================================
        1 RICERCA 
    ==============================================================================================================================================*/
    // TO DO PER I PRODOTTI


    function eseguiRicerca(pageNumber) {

        var filtri = {
            StartDate: $('#searchStartDate').val(),
            EndDate: $('#searchEndDate').val(),
            WarehouseId: $('#searchWarehouseId').val(),
            Description: $('#searchDescription').val(),
            PageNumber: pageNumber   
        };


        // sessione per salvare i filtri
        sessionStorage.setItem('filtri', JSON.stringify(filtri));

        $.ajax({
            url: searchMovements,
            type: 'GET',
            data: filtri,
            success: function (response) {
                $tabellaContainer.html(response).show();

                const nuovoUrl = window.location.pathname + '?StartDate=' + filtri.StartDate + '&EndDate=' + filtri.EndDate + '&WarehouseId=' + filtri.WarehouseId;
                history.pushState(null, '', nuovoUrl);

            },
            error: function (xhr, status, error) {
                console.error("Errore ricerca:", error);
                $tabellaContainer.html('<tr><td colspan="10" class="text-center text-danger">Errore di connessione durante la ricerca.</td></tr>');
            }
        });
    }

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
        const $partialUrl = $('#tabellaContainer').data('partial-url');
       

        const model = {

            MovementReasonId: $('#saveCausale').val(),
            ProductId: $('#saveProdotto').val(),
            QtyToTransfer: $('#saveQuantita').val(),

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
                    $('#tabellaContainer').load($partialUrl);

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
       3 PAGINAZINONE 
   ==============================================================================================================================================*/

    $tabellaContainer.on('click', '.page-link', function (e) {
        e.preventDefault();

        if ($(this).closest('li').hasClass('disabled')) {
            return;
        }

        var paginaRichiesta = parseInt($(this).data('page'));

        if (paginaRichiesta >= 1) {
          
            eseguiRicerca(paginaRichiesta);
        }
    });


    /*=============================================================================================================================================
       4 DETTAGLIO MOVIMENTO DI MAGAZZINO 
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
                    console.log("Dati ricevuti:", response);
                    const item = response.data.TheGetWarehouseMovementDetail;
                    
                    // Popoliamo i campi della modale con i dati ricevuti da C#
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



    //TO DO DA cancellare 

    //function apriDettaglio() {
    //    $.ajax({
    //        url: '/NomeTuoController/Details', 
    //        type: 'GET',
    //        data: { movementId: movementId },
    //        success: function (response) {
    //            if (response.success) {
    //                const item = response.data;

    //                // Popoliamo i campi della modale con i dati ricevuti dal C#
    //                $('#det-movement-id').text(item.MovementId);
    //                $('#det-date').text(item.Date);
    //                $('#det-warehouse-name').text(item.WarehouseName);
    //                $('#det-warehouse-id').text(item.WarehouseId);
    //                $('#det-product-name').text(item.ProductName);
    //                $('#det-product-id').text(item.ProductId);
    //                $('#det-company-name').text(item.CompanyName || '-');
    //                $('#det-description').text(item.Description);
    //                $('#det-reason-id').text(item.MovementReasonId);
    //                $('#det-detail').text(item.Detail);
    //                $('#det-detail-text').text(item.Detail);
    //                $('#det-notes').text(item.Notes || 'Nessuna nota');

    //                // Quantità e Segno
    //                const segno = item.MovementType === '+' ? '+' : '-';
    //                $('#det-quantity').html(`${segno}${item.Quantity} <span class="fs-6 fw-normal">pz</span>`);

    //                // Apriamo la modale Bootstrap
    //                $('#modalDettaglioMovimento').modal('show');
    //            } else {
    //                alert("Errore: " + response.message);
    //            }
    //        },
    //        error: function (xhr, status, error) {
    //            console.error("Errore AJAX:", error);
    //            alert("Si è verificato un errore di rete durante il recupero dei dati.");
    //        }
    //    });
    //}

});