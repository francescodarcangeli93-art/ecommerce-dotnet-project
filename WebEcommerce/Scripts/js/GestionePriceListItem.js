$(document).ready(function () {

    function ricaricaModalDettaglio(idListino) {
        $.ajax({
            url: '/GestPriceList/GetDetailsModal/' + idListino,
            type: 'GET',
            success: function (partialViewHtml) {

                // 1. Distruggi l'istanza vecchia PRIMA di rimuovere l'HTML
                var vecchioModalEl = document.getElementById('modalDettaglioListino');
                if (vecchioModalEl) {
                    var vecchiaIstanza = bootstrap.Modal.getInstance(vecchioModalEl);
                    if (vecchiaIstanza) {
                        vecchiaIstanza.dispose();
                    }
                }

                // 2. Ora sostituisci l'HTML
                $('#modalContainer').html(partialViewHtml);

                // 3. Rimuovi backdrop rimasti orfani
                $('.modal-backdrop').remove();
                $('body').removeClass('modal-open').css({
                    'overflow': '',
                    'padding-right': ''
                });

                // 4. Crea la nuova istanza pulita
                var nuovoModalEl = document.getElementById('modalDettaglioListino');
                var nuovaIstanza = new bootstrap.Modal(nuovoModalEl);
                nuovaIstanza.show();
            },
            error: function (xhr, status, error) {
                alert('Errore durante il recupero del dettaglio listino: ' + xhr.responseText);
            }
        });
    }

    /* ==========================================================
    1. APERTURA MODALE SALVA PRODOTTO
    ==========================================================*/


    $(document).on('click', '.btn-dettagli', function () {
        var idListino = $(this).data('id');
        ricaricaModalDettaglio(idListino);
    });

    $(document).on('click', '#btnAggiungiProdotto', function () {
        if ($('#priceListFormItem').length > 0) {
            $('#priceListFormItem')[0].reset();
        }
        var modalEl = document.getElementById('ModalAggiungiProdotto');
        var modal = new bootstrap.Modal(modalEl);
        modal.show();
    });



    /* ==========================================================
    2. AJAX PER SALVARE IL PRODOTTO
    ==========================================================*/
    $(document).on('click', '#chkEnablePrice', function () {

        if ($(this).is(':checked')) {
            // Abilita l'input
            $('#set-UnitPrice').prop('hidden', false);

            $('#item_UnitPrice').focus(); // Sposta il cursore direttamente nel campo
        } else {
            // Disabilita l'input e pulisce il valore
            $('#set-UnitPrice').prop('hidden', true);
            $('#set-UnitPrice').val('');
        }
    });

    /* ==========================================================
    3. AJAX PER SALVARE IL PRODOTTO
    ==========================================================*/


    $(document).on('click', '#Item-btnSalva', function () {

        const $btn = $(this);
        const saveUrl = $btn.data('save-url');
        const form = document.getElementById('priceListFormItem');

        if (!form.checkValidity()) {
            form.reportValidity();
            return;
        }

        const idListino = $('#PriceListId').val();

        const model = {
            PriceListId: idListino,
            ProductId: $('#item_ProductId').val(),
            Price: $('#item_UnitPrice').val(),
            StartDate: $('#addStartDateItem').val() || null,
            EndDate: $('#addEndDateItem').val() || null
        };

        $.ajax({
            url: saveUrl,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(model),
            dataType: 'json',

            success: function (response) {
                if (response.success) {

                    var modalEl = document.getElementById('ModalAggiungiProdotto');
                    var modalInstance = bootstrap.Modal.getInstance(modalEl);

                    if (modalInstance) {
                        modalEl.addEventListener('hidden.bs.modal', function handler() {
                            modalEl.removeEventListener('hidden.bs.modal', handler);
                            ricaricaModalDettaglio(idListino);
                        });
                        modalInstance.hide();
                    } else {
                        ricaricaModalDettaglio(idListino);
                    }
                } else {
                    $('#resultMessageItem').html('<span style="color:red">' + (response.message || 'Errore durante il salvataggio.') + '</span>');
                }
            },

            error: function (xhr, status, error) {
                $('#resultMessageItem').html('<span style="color:red">Errore del server: ' + xhr.responseText + '</span>');
            }
        });
    });



    // Apri il modal custom di conferma eliminazione prodotto
    $(document).on('click', '.btn-elimina', function () {
        const id = $(this).data('id');
        const name = $(this).data('name');

        $('#idProdottoDaEliminare').val(id);
        $('#nomeProdottoDaEliminare').text(name);

        $('#modalEliminaItem').addClass('active2');
    });

    $(document).on('click', '.btn-elimina', function () {

        const id = $(this).data('id');
        const name = $(this).data('name');

        $('#ItemDaEliminare').val(id);


    });

    // Annulla
    $(document).on('click', '.no-btn-item', function () {
        $('#modalEliminaItem').removeClass('active2');
    });

    // Conferma eliminazione
    $(document).on('click', '.si-btn-item', function () {
       
      
        const $deleteUrl = $(this).data('delete-url');
   
        const model = {
            id: $('#ItemDaEliminare').val()
        }
       

        $.ajax({
            url: $deleteUrl,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(model),
            dataType: 'json',
            success: function (response) {
                if (response.success) {
                    $('#modalEliminaItem').removeClass('active2');

                    $('tr[data-id="' + model.id + '"]').remove();

                } else {
                    $('#resultMessageDelItem').html('<span style="color:red">' + (response.message || 'Errore durante l\'eliminazione.') + '</span>');
                }
            },
            error: function (xhr) {
                $('#resultMessageDelItem').html('<span style="color:red">Errore del server.</span>');
            }
        });
    });

});