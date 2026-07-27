$(document).ready(function () {
    
    const magazzinoDestinazione = $('#saveMagazzinoDestinazione').addClass('d-none');

    $('#saveCausale').on('change', function () {

        const causale = $(this).val();
   

        if (causale && causale == 1) {
       
            magazzinoDestinazione.removeClass('d-none');
        } 

    });

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