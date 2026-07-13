$(document).ready(function () {

    const $btnCerca = $('#btnCerca');
    const $btnPulisci = $('#btnPulisci');
    const $btnAdd = $('#btnAggiungi');
    const $tabellaContainer = $('#tabellaContainer');


    /* ====================================================================================================
        2. SESSIONE PER SALVARE I FILTRI 
    ================================================================================================== */

    function eseguiRicerca() {

        const $searchUrl = $btnCerca.data('search-url');
        const test = $('#searchProductName').val();


        var filtri = {
            ProductName: $('#searchProductName').val(),
            BasedUnitPrice: $('#searchBasedUnitPrice').val(),
            IsDiscontinued: $('#searchIsDiscontinued').val(),
        };

        // SESSIONE PER SALVARE I FILTRI
        sessionStorage.setItem('filtriCategorie', JSON.stringify(filtri));


        $.ajax({
            url: $searchUrl,
            type: 'GET',
            data: filtri,
            success: function (response) {
                $tabellaContainer.html(response).show();

                const nuovoUrl = window.location.pathname + '?ProductName=' + filtri.ProductName + '&BasedUnitPrice=' + filtri.BasedUnitPrice + '&IsDiscontinued=' + filtri.IsDiscontinued;
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

        $('#searchProductName').val(filtri.ProductName);
        $('#searchBasedUnitPrice').val(filtri.BasedUnitPrice);
        $('#searchIsDiscontinued').val(filtri.IsDiscontinued);

        haFiltri = true;

        if (haFiltri) {
            $btnCerca.click();
        }

    }

    // funzione per pulire i filtri
    $btnPulisci.on('click', function (e) {


        $('#searchProductName').val('');
        $('#searchBasedUnitPrice').val('');

        $('#searchIsDiscontinued').val('');

        $btnCerca.click();
        sessionStorage.removeItem("filtriMagazzini");
    });



});