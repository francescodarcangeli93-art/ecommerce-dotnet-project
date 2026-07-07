
$(document).ready(function () {

    const $btnCerca = $('#btnCerca');
    const $btnPulisci = $('#btnPulisci');
    const $btnAdd = $('#btnAggiungi');
    const $tabellaContainer = $('#tabellaContainer');


    const allSelect = document.querySelectorAll('.is-placeholder');

    allSelect.forEach(function (select) {

        select.addEventListener('change', function () {

           
            this.classList.toggle('is-placeholder', this.value === '');
        });
    });

    function eseguiRicerca() {

        const searchUrl = $('#btnCerca').data('search-url');
 
        var filtri = {

            PriceListName: $('input[name="PriceListName"]').val(),
            Description: $('input[name="Description"]').val(),
            StartDate: $('input[name="StartDate"]').val(),
            EndDate: $('input[name="EndDate"]').val(),
            CurrencyCode: $('select[name="CurrencyCode"]').val(),
            IsDefault: $('select[name="IsDefault"]').val(),

            IsActive: $('select[name="IsActive"]').val()
        };

        // sessione per salvare i filtri
        sessionStorage.setItem('filtriListini', JSON.stringify(filtri));

        $.ajax({
            url: searchUrl,
            type: 'GET',
            data: filtri,
            success: function (response) {

                    $tabellaContainer.html(response).show();
              
                const nuovoUrl = window.location.pathname + '?PriceListName=' + filtri.PriceListName + '&Description=' + filtri.Description
                    + '&StartDate=' + filtri.StartDate + '&EndDate=' + filtri.EndDate + '&CurrencyCode=' + filtri.CurrencyCode
                    + '&IsDefault=' + filtri.IsDefault + '&IsActive=' + filtri.IsActive;

                history.pushState(null, '', nuovoUrl);

            },
            error: function (xhr, status, error) {
                console.error("Errore ricerca:", error);
                $tabellaContainer.html('<tr><td colspan="10" class="text-center text-danger">Errore di connessione durante la ricerca.</td></tr>');
            }
        });
    }

    $btnCerca.on('click', function () {

        eseguiRicerca();
    });

    $btnPulisci.on('click', function () {

        $('input[name="PriceListName"]').val('');
        $('input[name="Description"]').val('');
        $('input[name="StartDate"]').val('');
        $('input[name="EndDate"]').val('');
        $('select[name="CurrencyCode"]').val('');
        $('select[name="IsDefault"]').val('');
        $('select[name="IsActive"]').val('');
        $btnCerca.click();
        sessionStorage.removeItem("filtriListini");
    });

    /* ==========================================================
      3. SALVA LISTINO
      ==========================================================*/

    $('#btnSalva').on('click', function () {

        // pulire i filtri 

        const savehUrl = $('#btnSalva').data('save-url');

        // controllo check validità della modale
        const form = document.getElementById('priceListForm');

        if (!form.checkValidity()) {
            form.reportValidity(); // mostra i popup nativi di errore del browser
            return; // blocca l'invio AJAX
        }


        const model = {

            PriceListName: $('#addPriceListName').val(),
            Description: $('#addDescription').val(),
            StartDate: $('#addStartDate').val(),
            EndDate: $('#addEndDate').val(),
            CurrencyCode: $('#addCurrencyCode').val(),
            IsDefault: $('#addIsDefault').val(),
            IsActive: $('#addIsActive').val()
           
        }

        $.ajax({

            url: savehUrl,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(model),
            dataType: 'json',

            success: function (response) {
                if (response.success) {


                    $('#btnCerca').click();

                    $('#resultMessage').html('<span style="color:green">Nuovo listino aggiunto con successo!</span>');

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
       3. ELIMINA LISTINO
       ==========================================================*/

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
                    $('#resultMessage').html('<span style="color:red">' + response.message + '</span>');
                }
            },

        });

    });

});


