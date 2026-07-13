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



    /* ==============================================================================================
    2. ABILITAZIONE DEL PULSTANE PER AGGIUNGERE IL PREZZO PERSONALIZZATO (NON DI DEFAULT)
    ================================================================================================*/
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
    3. AJAX PER SALVARE IL LIST ITEM
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

    $(document).on('click', '.btn-eliminaItem', function () {
        const id = $(this).data('id');
        const name = $(this).data('name');

        $('#ItemDaEliminare').val(id);
        $('#nomeProdottoDaEliminare').text(name);

        $('#modalEliminaItem').addClass('active2');
    });

    

    // Annulla
    $(document).on('click', '.no-btn-item', function () {
        $('#modalEliminaItem').removeClass('active2');
    });

    // Conferma eliminazione
    $(document).on('click', '#delItem-btn', function () {
        
      
        const $deleteUrl = $(this).data('delete-url');
        console.log($deleteUrl);
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

    /* ==========================================================
        4. APERTURA MODALE MODIFICA PRICE LIST ITEM
     ==========================================================*/

    $(document).on('click', '.btn-modificaItem', function () {

        


        const $loadUrl = $(this).data('load-url');
       
        const model = {
            id: $(this).data('id')
        }

        $.ajax({
            url: $loadUrl,
            type: 'GET',  
            data: model,
            dataType: 'json',

            success: function (response) {
                if (response.success) {

                    $('#editItemId').val(response.data.Id);
                    $('#editPriceListId').val(response.data.PriceListId);
                    $('#editProductId').val(response.data.ProductId);
                    $('#editUnitPriceItem').val(response.data.Price);
                    $('#editStartDateItem').val(response.data.StartDateString || '');
                    $('#editEndDateItem').val(response.data.EndDateString || '');

                    // prendere la data e aggiungere un giorno

                    const date = aggiungiGiorno(response.data.StartDateString);
                 
                    
                    $('#editEndDateItem').attr('min', date);
               
                    modalEditOpen();

                    //$('input[name="editIsActive"][value="' + response.data.IsActive + '"]').prop('checked', true);

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
   5. SALVATAGGIO MODIFICA PRICE LIST ITEM
========================================================== */

    $(document).on('click', '#edit-category', function () {

      
        const $loadUrl = $(this).data('load-url');

        const model = {
            Id: $('#editItemId').val(),
            PriceListId: $('#editPriceListId').val(),
            ProductId: $('#editProductId').val(),
            Price: $('#editUnitPriceItem').val(),
            StartDate: $('#editStartDateItem').val() || null,
            EndDate: $('#editEndDateItem').val() || null
        };

        $.ajax({
            url: $loadUrl,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(model),
            dataType: 'json',

            success: function (response) {
                if (response.success) {
                   
                   
                    const idListino = $('#PriceListId').val();
                    ricaricaModalDettaglio(idListino); // ricarica la tabella prodotti aggiornata

                } else {
                    $('#resultMessageEditItem').html('<span style="color:red">' + response.message + '</span>');
                }
            },

            error: function (xhr, status, error) {
              
                $('#resultMessageEditItem').html('<span style="color:red">Errore: ' + xhr.responseText + '</span>');
            }
        });
    });

    $(document).on('change', '#editStartDateItem', function () {

        const startDate = $('#editStartDateItem').val();
        const newDate = aggiungiGiorno(startDate);
        $('#editEndDateItem').attr('min', newDate);
    });

    $(document).on('click', '.no-edit-btn' , function () {
   
        $('#modalEditItem').removeClass('active2');
    });

    /* ==========================================================
        6. FUNZIONE PER CONVERTIRE LA DATA
    ========================================================== */

    function aggiungiGiorno(dataString) {
        const [anno, mese, giorno] = dataString.split('-').map(Number);
        const data = new Date(anno, mese - 1, giorno); 

        data.setDate(data.getDate() + 1);

        // ricostruzione della formattazione 
        const y = data.getFullYear();
        const m = String(data.getMonth() + 1).padStart(2, '0');
        const d = String(data.getDate()).padStart(2, '0');

        return `${y}-${m}-${d}`; 
    }


    function modalEditOpen() {

        $('#ItemListDaModificare').text($(this).data('name'));

        $('#modalEditItem').addClass('active2');
        
    }
});