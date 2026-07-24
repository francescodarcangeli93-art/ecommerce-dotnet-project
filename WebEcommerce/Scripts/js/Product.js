$(document).ready(function () {

    const $btnCerca = $('#btnCerca');
    const $btnPulisci = $('#btnPulisci');
    const $btnAdd = $('#btnAggiungi');
    const $tabellaContainer = $('#tabellaContainer');


    /* ====================================================================================================
        2. FUNZIONE PER SALVARE I FILTRI 
    ================================================================================================== */

    function eseguiRicerca() {

        const $searchUrl = $btnCerca.data('search-url');
        const test = $('#searchProductName').val();


        var filtri = {
            ProductName: $('#searchProductName').val(),
            BasedUnitPriceDa: $('#searchBasedUnitPriceDa').val(),
            BasedUnitPriceA: $('#searchBasedUnitPriceA').val(),
            IsDiscontinued: $('#searchIsDiscontinued').val(),
            CategoryId: $('#searchCategoryId').val(),
            SupplierId: $('#searchSupplierId').val()
        };

        // SESSIONE PER SALVARE I FILTRI
        sessionStorage.setItem('filtriProdotti', JSON.stringify(filtri));


        $.ajax({
            url: $searchUrl,
            type: 'GET',
            data: filtri,
            success: function (response) {
                $tabellaContainer.html(response).show();

                const nuovoUrl = window.location.pathname
                    + '?ProductName=' + encodeURIComponent(filtri.ProductName || '')
                    + '&BasedUnitPriceDa=' + encodeURIComponent(filtri.BasedUnitPriceDa || '')
                    + '&BasedUnitPriceA=' + encodeURIComponent(filtri.BasedUnitPriceA || '')
                    + '&IsDiscontinued=' + encodeURIComponent(filtri.IsDiscontinued || '')
                    + '&CategoryId=' + encodeURIComponent(filtri.CategoryId || '')
                    + '&SupplierId=' + encodeURIComponent(filtri.SupplierId || '');

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


    /* ====================================================================================================
       2. RIPRISTINO FILTRI SALVATI IN SESSIONE (al caricamento pagina)
       ==================================================================================================*/

    const filtriSalvati = sessionStorage.getItem('filtriProdotti');

    if (filtriSalvati) {

        const filtri = JSON.parse(filtriSalvati);
        let haFiltri = false;

        $('#searchProductName').val(filtri.ProductName);
        $('#searchBasedUnitPriceDa').val(filtri.BasedUnitPriceDa);
        $('#searchBasedUnitPriceA').val(filtri.BasedUnitPriceA);
        $('#searchIsDiscontinued').val(filtri.IsDiscontinued);
        $('#searchCategoryId').val(filtri.CategoryId);
        $('#searchSupplierId').val(filtri.SupplierId);

        haFiltri = true;

        if (haFiltri) {
            $btnCerca.click();
        }
    }

    /* ====================================================================================================
       3. PULISCI FILTRI
       ==================================================================================================*/
    $btnPulisci.on('click', function (e) {

        $('#searchProductName').val('');
        $('#searchBasedUnitPriceDa').val('');
        $('#searchBasedUnitPriceA').val('');
        $('#searchIsDiscontinued').val('');
        $('#searchCategoryId').val('');
        $('#searchSupplierId').val('');

        sessionStorage.removeItem('filtriProdotti');
        $btnCerca.click();
    });



    /* ====================================================================================================
       5. EVENTO CLICK CHE SALVA IL PRODOTTO
       ==================================================================================================*/

    $(document).on('click', '#btnSalvaProdotto', function () {
  

        const model = {
            ProductName: $('#addProductName').val(),
            SupplierId: $('#addSupplierId').val(),
            CategoryId: $('#addCategoryId').val(),
            
            BasedUnitPrice: $('#addBasedUnitPrice').val(),
            Package: $('#addPackage').val(),
            IsDiscontinued: $('#addIsDiscontinued').val(),
            VisibleInHP: $('#addVisibleInHP').is(':checked')
        };

        $.ajax({
            url: '/GestProductFile/Save',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(model),
            dataType: 'json',

            success: function (response) {
                if (response.success) {

                    $('#resultMessage').html('<span style="color:green; font-weight:bold;">Salvataggio completato!</span>');

                    $('#btnCerca').click(); // ricarica la lista prodotti
                    setTimeout(function () {
                        $('#aggiungi').modal('hide');
                        // opzionale: svuoti il form o ricarichi la tabella qui
                    }, 1000);
                    // chiusura modale 
                   


                } else {
                    $('#resultMessage').html('<span style="color:red">' + response.message + '</span>');
                }
            },
            error: function (xhr) {
                $('#resultMessage').html('<span style="color:red">Errore: ' + xhr.responseText + '</span>');
            }
        });
    });


    /* ====================================================================================================
       5. EVENTO CLICK CHE  APRE LA MODALE ELIMINA
       ==================================================================================================*/


    $('#tabellaContainer').on('click', '.btn-elimina', function () {

        const id = $(this).data('id');
        const name = $(this).data('name');

        $('#idProdottoDaEliminare').val(id);

        $('#ProdottoDaCancellare').text(name);

        $('#modalElimina').addClass('active2')
    });

    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    $('#delList-btn').on('click', function () {


       

        const $deleteUrl = $(this).data('delete-url');


        const model = {
            id: $('#idProdottoDaEliminare').val()
        }
        console.log($id);
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

    //-------------------------------------------------------------------------------------------------------------

    $('.no-btn').on('click', function () {

        $('#modalElimina').removeClass('active2');
    });

});