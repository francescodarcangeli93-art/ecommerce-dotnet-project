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


    $(document).on('click', '.btn-elimina', function () {

        const id = $(this).data('id');
        const name = $(this).data('name');

        $('#idProdottoDaEliminare').val(id);

        $('#ProdottoDaCancellare').text(name);

        $('#modalElimina').addClass('active2')
    });

    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    $(document).on('click', function () {


       

        const $deleteUrl = $(this).data('delete-url');


        const model = {
            id: $('#idProdottoDaEliminare').val()
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

    //-------------------------------------------------------------------------------------------------------------

    $('.no-btn').on('click', function () {

        $('#modalElimina').removeClass('active2');
    });


    /* ====================================================================================================
   APERTURA E POPOLAMENTO MODALE MODIFICA PRODOTTO
==================================================================================================*/
    $(document).on('click', '.btn-modifica', function (e) {
        e.preventDefault();

        const $btn = $(this);
        const id = $btn.data('id');
        const url = $btn.data('url');

        $.ajax({
            url: url,
            type: 'GET',
            data: { id: id },
            success: function (response) {
                if (response.success) {
                    // Estraiamo il prodotto dal model inviato dal Controller (response.data.TheProduct)
                    const product = response.data.TheProduct;

                    // Popolamento 1:1 dei campi con prefisso "edit"
                    $('#editId').val(product.ProductId);
                    $('#editProductName').val(product.ProductName);
                    $('#editSupplierId').val(product.SupplierId);
                    $('#editCategoryId').val(product.CategoryId);
                    $('#editBasedUnitPrice').val(product.BasedUnitPrice);
                    $('#editPackage').val(product.Package);

                    // Conversione booleano in stringa per la select
                    $('#editIsDiscontinued').val(String(product.IsDiscontinued));

                    // Checkbox visibile HP
                    $('#editVisibleInHP').prop('checked', product.VisibleInHP);

                    // Pulizia vecchi messaggi ed apertura modale
                    $('#resultMessageEdit').empty();
                    $('#modalEdit').modal('show');
                } else {
                    alert('Errore: ' + response.message);
                }
            },
            error: function (xhr) {
                console.error('Errore chiamata AJAX:', xhr.responseText);
            }
        });
    });

    /* ====================================================================================================
   SALVATAGGIO MODIFICHE PRODOTTO
==================================================================================================*/
    $(document).on('click', '#btnEditModifica', function () {

        const model = {
            Id: $('#editId').val(),
            ProductName: $('#editProductName').val(),
            SupplierId: $('#editSupplierId').val(),
            CategoryId: $('#editCategoryId').val(),
            BasedUnitPrice: $('#editBasedUnitPrice').val(),
            Package: $('#editPackage').val(),
            IsDiscontinued: $('#editIsDiscontinued').val() === 'true',
            VisibleInHP: $('#editVisibleInHP').is(':checked')
        };

        $.ajax({
            url: '/GestProductFile/Update', // oppure l'azione del tuo Controller
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(model),
            dataType: 'json',
            success: function (response) {
                if (response.success) {
                    $('#resultMessageEdit').html('<span style="color:green; font-weight:bold;">Modifica completata con successo!</span>');

                    // Ricarica la tabella prodotti
                    $btnCerca.click();

                    setTimeout(function () {
                        $('#modalEdit').modal('hide');
                    }, 1000);
                } else {
                    $('#resultMessageEdit').html('<span style="color:red">' + response.message + '</span>');
                }
            },
            error: function (xhr) {
                $('#resultMessageEdit').html('<span style="color:red">Errore durante il salvataggio: ' + xhr.responseText + '</span>');
            }
        });
    });


});