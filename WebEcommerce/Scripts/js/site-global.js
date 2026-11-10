$(document).ready(function () {

    /*=============================================================================================================================================
     2 PAGINAZIONE (GLOBALE, RIUSABILE DA PIÙ TAB/CONTROLLER)
     ==============================================================================================================================================*/

    $(document).on('click', '.page-link', function (e) {
        e.preventDefault();

        if ($(this).closest('li').hasClass('disabled')) {
            return;
        }

        let paginaRichiesta = parseInt($(this).data('page'));

        if (paginaRichiesta >= 1) {
            eseguiRicerca(paginaRichiesta);
        }
    });

    /*=============================================================================================================================================
     3 MODALE MODIFICA
     ==============================================================================================================================================*/
    let modalDialog;


    $(document).on('click', '.btn-modifica', function () {

  
        const target = $(this).data('target');

        if (!target) {
            console.warn('Bottone senza data-target:', this);
            return;
        }

        // scatola che viene riempita con la variabile presa dal data-target  edit-modal//
        const modalId = '#' + target; // -> #edit-modal
      
        modalDialog = $(modalId); // $(#edit-modal)
       
        // salva l'id dal bottone  modifica
        const id = $(this).data('id'); 
       
        editGet($(this));

        if (modalDialog.length > 0) {
            // Assegna l'ID all'input hidden con la classe standard
            $('.global-modaledit-id').val(id);
            // Apri la modale
            modalDialog[0].showModal();
        }

    });

    $(document).on('click', '.btn-close-modal', function () {

        if (modalDialog) {
            modalDialog[0].close();
            modalDialog = null;

            // Metodo con ciclo esplicito .each()
            $('input, textarea').each(function () {
                // $(this) si riferisce al singolo elemento corrente
                $(this).val('');
            });
        }
        
    });


});
