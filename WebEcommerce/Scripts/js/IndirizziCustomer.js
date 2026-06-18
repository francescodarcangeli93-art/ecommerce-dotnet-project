$(document).ready(function () {

    /*-----------------------------------------------------------------------------------------------------
     ------------------------------------------------------------------------------------------------------
                 ******* CLICK BOTTONE ELIMINA *******
      -----------------------------------------------------------------------------------------------------
      -----------------------------------------------------------------------------------------------------
    */

    $('.btnElimina').on('click', function () {

        var id = $(this).data('id');
        $('#indirizzoDaEliminare').val(id);
        $('#modalElimina').modal('show');

        const test = document.getElementById('interf');
        test.style = 'pointer-events: none';
    });

    // 
    document.getElementById('btnAnnulla').addEventListener('click', function () {
        const test = document.getElementById('interf');
        test.removeAttribute('style');

    });

    
    // click conferma elimina viene lanciato questo metodo
   
    $('#btnConfermaElimina').on('click', function () {

        const test = document.getElementById('interf');
       
        // credo il modello da passare al controller con l'id dell'indirizzo da eliminare
        var model = {
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

                    $('#modalElimina').modal('hide');
                   
                    // rimuovo la riga della tabella corrispondente all'indirizzo eliminato
                    $('tr[data-id="' + $('#indirizzoDaEliminare').val() + '"]').remove();
                    test.removeAttribute('style');
                } else {
                    $('#resultMessage').html('<span style="color:red">' + response.message + '</span>');
                }
            },

            error: function (xhr, status, error) {
                $('#resultMessage').html('<span style="color:red">Errore: ' + xhr.responseText + '</span>');
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

        const hide = document.getElementById('interf').style = "pointer-events: none";
    
    });

    document.getElementById('addAnnulla').addEventListener('click', function () {

        const show = document.getElementById('interf').removeAttribute('style');
 
    });

    // METODO CHE SI ATTIVA QUANDO IL TASTO DELLA MODALE SALVA VIENE CLICCATO
    $('#btnSalvaIndirizzo').on('click', function () {

        // creo il modello da passare al controller con i dati del nuovo indirizzo
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
                if (response.success) {
                    $('#mdlAggiungi').modal('hide');
                    // ricarico la pagina per mostrare il nuovo indirizzo
                    document.getElementById('interf').removeAttribute('style');

                    location.reload();
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

        document.getElementById('interf').removeAttribute('style');
    });


    $('.btnModifica').on('click', function () {

        document.getElementById('interf').style = 'pointer-events: none';

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
                if (response.success) {
                    $('#mdlModifica').modal('hide');

                    document.getElementById('interf').removeAttribute('style');
                    // ricarico la pagina per mostrare il nuovo indirizzo
                    location.reload();
                } else {
                    $('#resultMessageAdd').html('<span style="color:red">' + response.message + '</span>');
                }
            }
        })

    });
  
});

//  METODO CHE COLORA LA RIGA CHE HA ISDEFAULT A TRUE
document.addEventListener('DOMContentLoaded', function () {

    const righe = document.querySelectorAll('tr[data-isdefault="true"]');

    righe.forEach(riga => {
        riga.classList.add('table-success');
    });

});