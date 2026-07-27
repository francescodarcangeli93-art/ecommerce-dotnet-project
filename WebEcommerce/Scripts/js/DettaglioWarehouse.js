$(document).ready(function () {
   
    var timerRicerca;

    document.getElementById('ricercaProdotto').addEventListener('input', function (event) {

        // 2. Ogni volta che premi un tasto, cancelli il timer del tasto precedente
        clearTimeout(timerRicerca);

        // Salviamo subito il valore scritto dall'utente
        let filtro = event.target.value.toLowerCase();
        console.log(typeof filtro);
        // 3. Diciamo al computer di aspettare 300 millisecondi prima di fare qualsiasi calcolo
        timerRicerca = setTimeout(function () {

            let righe = document.querySelectorAll('#tabellaGiacenze tbody tr');
            
            let isMatch = false;

            // Il tuo ciclo FOR originale, intatto:
            //for (var i = 0; i < righe.length; i++) {
            //    var riga = righe[i];

            //    if (isMatch) {
            //        riga.style.display = 'none';
            //        continue;
            //    }

            //    if (riga.cells[0].innerText.toLowerCase() === filtro) {
            //        riga.style.display = '';
            //        isMatch = true;
            //    }
            //    else if (filtro.length > 3 && riga.cells[1].innerText.toLowerCase().includes(filtro)) {
            //        riga.style.display = '';
            //    }
            //    else if (filtro === '') {
            //        riga.style.display = '';
            //    }
            //    else {
            //        riga.style.display = 'none';
            //    }
            //}


            for (riga of righe) {

                let editedRow = riga.innerText.toLowerCase().trim();
                

                if (editedRow.includes(filtro)) {
                   
                    riga.style.display = '';
                } else {
                    riga.style.display = 'none';
                }
            }

        }, 300); // <-- 300 millisecondi di ritardo

    });

    $('#formImpostazioni').on('submit', function (e) {
        e.preventDefault(); // evita il refresh della pagina dato il submit nativo del form

        const model = {
            Id: $('#inputId').val(), 
            Type: $('#inputTipo').val(),
            Name: $('#inputNome').val(),
            Address: $('#inputIndirizzo').val(),
            PostalCode: $('#inputCap').val(),
            City: $('#inputCitta').val(),
            IsActive: $('#inputIsActive').is(':checked')
        }

        $.ajax({
            url: '/GestWarehouses/Edit',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(model),
            dataType: 'json',

            success: function (response) {
                if (response.success) {
                    // qui non c'è modale da chiudere, magari un messaggio di conferma
                    $('#resultMessage').html('<span style="color:green">Modifiche salvate con successo!</span>');
                } else {
                    $('#resultMessage').html('<span style="color:red">' + response.message + '</span>');
                }
            },

            error: function (xhr, status, error) {
                $('#resultMessage').html('<span style="color:red">Errore: ' + xhr.responseText + '</span>');
            }

        });
    });


    /*=============================================================================================================================================
        MODAL: CLICK CONFERMA CARICO 
    ==================================================================================================================================================*/

    $('#btnConfermaCarico').on('click', function () {

        const $url = $(this).data('carico-url');
  


        $.ajax({
            url: '/Warehouse/MoveProducts',
            type: 'POST',
            data: formData,
            success: function (response) {
                // 1. Chiudi la modal
                $('#modalCaricaProdotto').modal('hide');

                // 2. Ricarica SOLO la tabella tramite Partial View
                $('#contenitoreTabellaGiacenze').load('/Warehouse/GetTabellaGiacenze?warehouseId=' + magazzinoId);
            },
            error: function (xhr) {
                alert(xhr.responseText); // Mostra l'errore del RAISERROR/WarehouseOperationException
            }
        });

    });



    function safelyTrim(input) {
        // Add a "guard clause" to check if the input is a string.
        if (typeof input !== 'string') {
            console.error('Error: The provided input is not a string.');
            // Return a predictable value or the original input on failure.
            return input;
        }

        return input.trim();
    }
});