$(document).ready(function () {
    
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


    function eseguiRicerca() {

        var filtri = {
            StartDate: $('#searchStartDate').val(),
            EndDate: $('#searchEndDate').val(),
            WarehouseId: $('#searchWarehouseId').val(),
            Description: $('Description').val(),
      
        };


        // sessione per salvare i filtri
        sessionStorage.setItem('filtri', JSON.stringify(filtri));

        $.ajax({
            url: '@Url.Action("SearchWarehousesPartial", "GestWarehouses")',
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


     /*=============================================================================================================================================
        2 SALVAMOVIMENTO 
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

});