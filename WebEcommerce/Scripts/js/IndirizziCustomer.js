$(document).ready(function () {

    /*-----------------------------------------------------------------------------------------------------
     ------------------------------------------------------------------------------------------------------
                 ******* CLICK BOTTONE ELIMINA *******
      -----------------------------------------------------------------------------------------------------
      -----------------------------------------------------------------------------------------------------
    */

    $('.btnElimina').on('click', function () {

        const id = $(this).data('id');
        $('#indirizzoDaEliminare').val(id);

        var name = $(this).data('name');
        $('#indirizzoDaMostrare').text(name);

        $('#modalElimina').addClass('active2');

       /* const test = document.getElementById('interf').classList.add('no-pointer-events');*/
     
    });

    $('.no-btn').on('click', function () {


        $('#modalElimina').removeClass('active2');
    });


    
    // click conferma elimina viene lanciato questo metodo
   
    $('.si-btn').on('click', function () {

        const interface = document.getElementById('interf');
       
        // credo il modello da passare al controller con l'id dell'indirizzo da eliminare
        const model = {
            id: $('#indirizzoDaEliminare').val()
        }

        $.ajax({
            url: urls.eliminaIndirizzo,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(model),
            dataType: 'json',

            success: function (response) {
                if (response.success) {

                    var messaggio = response.message || response.Message || "Indirizzo ";

                    $('#resultMessageDel').html('<span style="color:green; font-weight:bold;">' + messaggio + '</span>');

                    // 2. Attendiamo 1.5 secondi (1500 ms) prima di far sparire tutto
                    setTimeout(function () {
                        // 3. Chiudiamo la modal
                        $('#modalElimina').removeClass('active2');

                        

                        // 4. Rimuoviamo la riga della tabella (con un effetto di sfumatura soft!)
                        $('tr[data-id="' + $('#indirizzoDaEliminare').val() + '"]').remove();

                        // Ripristiniamo lo stile dell'interfaccia 
                      
                        if (typeof interface !== 'undefined') {
                            interface.classList.remove('no-pointer-events');
                        } else if (document.getElementById('interf')) {
                            document.getElementById('interf').classList.remove('no-pointer-events');
                        }

                        // Puliamo il messaggio per la prossima volta che si aprirà la modal
                        $('#resultMessageDel').html('');

                    }, 500);
                } else {
                    $('#resultMessage').html('<span style="color:red">' + response.message + '</span>');
                }
            },

            error: function (xhr, status, error) {
                // Se il server risponde con un errore, lo mostriamo in rosso
                $('#resultMessage').html('<span style="color:red">' + messaggio + '</span>');
            }

        });
    });


    /*------------------------------------------------------------------------------------------------------
      ------------------------------------------------------------------------------------------------------
                 ******* CLICK BOTTONE AGGIUNGI *******
      ------------------------------------------------------------------------------------------------------
      ------------------------------------------------------------------------------------------------------
    */

  
    document.getElementById('btnAggiungi').addEventListener('click', function () {

        const hide = document.getElementById('interf').classList.add('no-pointer-events');
    
    });

    document.getElementById('addAnnulla').addEventListener('click', function () {

        const show = document.getElementById('interf').classList.remove('no-pointer-events');
 
    });

    // METODO CHE SI ATTIVA QUANDO IL TASTO DELLA MODALE SALVA VIENE CLICCATO
    $('#btnSalvaIndirizzo').on('click', function () {

        const form = document.getElementById('formAggiungiIndirizzo');
        // creo il modello da passare al controller con i dati del nuovo indirizzo

        if (!form.checkValidity()) {
            form.reportValidity(); // mostra i popup nativi di errore del browser
            return; // blocca l'invio AJAX
        }

        var model = {
            // passiamo l'id del customer per associarlo al nuovo indirizzo
            CustomerId: $('#CustomerId').val(),
            Address: $('#txtAddress').val(),
            PostalCode: $('#txtPostalcode').val(),
            City: $('#txtCity').val(),
            IsDefault: $('#txtIsDefault').is(':checked'),
            IsBilling: $('#txtIsBilling').is(':checked')
        };

        $.ajax({
            url: urls.aggiungiIndirizzo,
            type: 'POST',
            // contentType: 'application/json; charset=utf-8',
            data: model,
            dataType: 'json',
            success: function (response) {

                var messaggio = response.message || response.Message || "Operazione completata";

                if (response.success) {

                    $('#resultMessageAdd').html('<span style="color:green; font-weight:bold;">' + messaggio + '</span>');

                    // 2. Aspettiamo 1.5 secondi (1500 ms) prima di chiudere la modal e ricaricare
                    setTimeout(function () {
                        $('#mdlAggiungi').modal('hide');

                        document.getElementById('interf').classList.remove('no-pointer-events');;

                        // 3. Ora che l'utente ha letto il messaggio, ricarichiamo la pagina
                        location.reload();
                    }, 500);
                } else {
                    $('#resultMessageAdd').html('<span style="color:red">' + response.message + '</span>');
                }
            },
            error: function (xhr, status, error) {
                $('#resultMessageAdd').html('<span style="color:red">Errore: ' + xhr.responseText + '</span>');
            }
        });

    });

    /*------------------------------------------------------------------------------------------------------
      ------------------------------------------------------------------------------------------------------
                 ******* CLICK BOTTONE MODIFICA *******
      ------------------------------------------------------------------------------------------------------
      ------------------------------------------------------------------------------------------------------
    */


    // quando viene cliccato il bottone della modale annulla si attiva questo  evento
    document.getElementById('editAnnulla').addEventListener('click', function () {

        document.getElementById('interf').classList.remove('no-pointer-events');;
    });


    $('.btnModifica').on('click', function () {

        document.getElementById('interf').classList.add('no-pointer-events');;

        var bottone = $(this);
        $('#editId').val(bottone.data('id'));
        $('#editAddress').val(bottone.data('address'));
        $('#editPostalcode').val(bottone.data('postalcode'));
        $('#editCity').val(bottone.data('city'));

        $('#editIsDefault').prop('checked', bottone.data('isdefault') === true);
        $('#editIsBilling').prop('checked', bottone.data('isbilling') === true);

    });


    $('#btnModificaIndirizzo').on('click', function () {

        var model = {
            Id: $('#editId').val(),
            CustomerId: $('#editCustomerId').val(),
            Address: $('#editAddress').val(),
            PostalCode: $('#editPostalcode').val(),
            City: $('#editCity').val(),
            IsDefault: $('#editIsDefault').is(':checked'),
            IsBilling: $('#editIsBilling').is(':checked')
        };

        $.ajax({
            url: urls.modificaIndirizzo,
            type: 'POST',

            data: model,
            dataType: 'json',
            success: function (response) {
                var messaggio = response.message || response.Message || "Modificato con successo!";

                if (response.success) {

                    $('#resultMessageEdit').html('<span style="color:green; font-weight:bold;">' + messaggio + '</span>');

                   setTimeout(function () {
                    $('#mdlModifica').modal('hide');

                       document.getElementById('interf').classList.remove('no-pointer-events');;
                    
                    // 4. Ricarichiamo la pagina per mostrare l'indirizzo aggiornato in tabella
                    location.reload();
                }, 1500);
                } else {
                    $('#resultMessageAdd').html('<span style="color:red">' + response.message + '</span>');
                }
            }
        })

    });
  
});



////  METODO CHE COLORA LA RIGA CHE HA ISDEFAULT A TRUE
//document.addEventListener('DOMContentLoaded', function () {

//    const righe = document.querySelectorAll('tr[data-isdefault="true"]');

//    righe.forEach(riga => {
//        riga.classList.add('table-success');
//    });

//});