
$(document).ready(function () {

    $('.btn-dettagli').on('click', function () {

        // 1. Prendi l'ID del listino dal pulsante
        var idListino = $(this).data('id');

        // 2. Fai la chiamata AJAX al tuo Controller GetDetailsModal
        $.ajax({
            url: '/GestPriceList/GetDetailsModal/' + idListino,
            type: 'GET',
            success: function (partialViewHtml) {
                // 3. Inserisci l'HTML della Partial View dentro il contenitore
                $('#modalContainer').html(partialViewHtml);

                // 4. Apre la modale (deve corrispondere all'ID definito dentro la tua Partial View)
                $('#modalDettaglioListino').modal('show');
            },
            error: function (xhr, status, error) {
                alert('Errore durante il recupero del dettaglio listino: ' + xhr.responseText);
            }
        });
    });


    $(document).on('click', '#btnAggiungiProdotto', function () {
        // 1. Reset del form
        if ($('#priceListForm').length > 0) {
            $('#priceListForm')[0].reset();
        }
        $('#item_Id').val(0);

        // 2. IMPORTANTISSIMO: Spostiamo la modale nel body e la mostriamo
        $('#aggiungi').appendTo('body').modal('show');
    });



});


