
$(document).ready(function () {

    function ricaricaImmaginiProdotto(productId) {


        $.ajax({
            url: '/GestProductFile/GetProductImagesPartial',
            type: 'GET',
            data: { ProductId: productId },
            success: function (html) {

                $('#imagesList').html(html);
            },
            error: function (xhr) {
                console.error('Errore ricaricamento immagini:', xhr.responseText);
            }
        });
    }

    // quando apre la modale di upload pulisce i filtri
    $('#btnAggiungiFile').on('click', function () {

        $('#uploadFiles').val('');
        $('#uploadTitle').val('');
        $('#uploadIsMain').val('');
        $('#uploadDescription').val('');

    });

    /*=========================================================================================================
       UPLOAD IMMAGINI
    =========================================================================================================*/


    $(document).on('click', '#btnUploadFile', function () {

        const id = $(this).data('product-id');
        const fileInput = document.getElementById('uploadFiles');


        if (!fileInput.files || fileInput.files.length === 0) {
            $('#resultMessageUpload').html('<span style="color:red">Seleziona un file.</span>');
            return;
        }

        const formData = new FormData();
        formData.append('ProductId', id);
        formData.append('Description', $('#uploadDescription').val() || '');
        formData.append('Title', $('#uploadTitle').val());
        formData.append('IsMain', $('#uploadIsMain').is(':checked'));

        formData.append('uploadFiles', fileInput.files[0]);

        $.ajax({
            url: '/GestProductFile/UploadImage',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            dataType: 'json',

            success: function (response) {
                if (response.success) {
                    var modalEl = document.getElementById('modalAggiungiFile');
                    var modalInstance = bootstrap.Modal.getOrCreateInstance(modalEl);
                    if (modalInstance) modalInstance.hide();

                    $('#uploadFiles').val('');
                    ricaricaImmaginiProdotto(id);
                } else {
                    $('#resultMessageUpload').html('<span style="color:red">' + response.message + '</span>');
                }
            },
            error: function (xhr) {
                $('#resultMessageUpload').html('<span style="color:red">Errore: ' + xhr.responseText + '</span>');
            }
        });

    });

    /*========================================================================================================================
        GESTIONE DEL CAMBIO IMMAGINI QUANDO PASSO SOPRA CON IL MOUSE
    ==========================================================================================================================*/


    $(document).on('mouseenter', '.product-img', function () {

        const $url = $(this).data('full-src');

        $('#mainProductImage').attr('src', $url);

    });

    $(document).on('mouseleave', '.product-img', function () {
        const $originalSrc = $('#mainProductImage').data('full-src') || '';
        $('#mainProductImage').attr('src', $originalSrc);
    });

    /*========================================================================================================================
        EVENTO CLICK SULL'IMMAGINE PER APRIRE IL DETTAGLIO DELL'IMMAGINE
    ==========================================================================================================================*/

    $(document).on('click', '.test-click', function () {
        let $id = $(this).data('id');
        let $productId = $(this).data('product-id');
        const $url = $(this).data('url');
        console.log('entrato nel click');
        // questo serve per prendere l'immagine ad alta risoluzione 
        let $fullSrc = $(this).data('full-src');



        console.log($fullSrc);

        caricaMiniatureModale($productId, $id, $url);

        const model = {
            id: $id,
            productId: $productId
        }

        $('#modalImage').attr('src', $fullSrc);

        $.ajax({
            url: $url,
            type: 'GET',
            data: model,
            dataType: 'json',

            success: function (response) {
                if (response.success) {

                    $('#ImageTitle').text(response.data.Title);

                    // Popola la modale con la tua logica esistente
                    popolaDettagliModale(response.data);

                    // blocca lo sfondo e apre la finestra
                    $('#modalShowImageDetail').addClass('active2')

                } else {
                    $('#resultMessage').html('<span style="color:red">' + response.message + '</span>');
                }
            },

            error: function (xhr, status, error) {
                $('#resultMessage').html('<span style="color:red">Errore: ' + xhr.responseText + '</span>');
            }

        });


    });

   //----------------------------------------------------------------------------------------------------------------------------------------------------------------------

    // Evento click su una miniatura DENTRO la modale
    $(document).on('click', '.modal-thumb-item', function () {

        // Evidenzia la miniatura cliccata
        $('.modal-thumb-item').removeClass('active');
        $(this).addClass('active');

        // Simula il click invocando l'aggiornamento dei dettagli via AJAX
        let $id = $(this).data('id');
        let $productId = $(this).data('product-id');
        const $url = $(this).data('url');
        let $fullSrc = $(this).data('full-src');

        $('#modalImage').attr('src', $fullSrc);

        $.ajax({
            url: $url,
            type: 'GET',
            data: { id: $id, productId: $productId },
            dataType: 'json',
            success: function (response) {
                if (response.success) {
                    popolaDettagliModale(response.data);
                }
            }
        });
    });

//----------------------------------------------------------------------------------------------------------------------------------------------------------------------

    $('.mybtn-close').on('click', function () {

        $('#modalShowImageDetail').removeClass('active2');
    });

    /* ===========================================================================================================================================================================
        1. APERTURA MODALE ELIMINA
    ===========================================================================================================================================================================*/

    $('#tabellaContainer').on('click', '.btn-elimina', function () {

        const id = $(this).data('id');
        const name = $(this).data('name');

        $('#CategoriaDaEliminare').val(id);

        $('#CategoriaDaCancellare').text(name);

        $('#modalElimina').addClass('active2')



    });

    $('.no-btn').on('click', function () {

        $('#modalElimina').removeClass('active2');
    });

    /* ===========================================================================================================================================================================
       1. FUNZIONI JAVASCRIPT
   ===========================================================================================================================================================================*/

    function popolaDettagliModale(data) {
        $('#ImageTitle').text(data.Title);
        $('#Detail-Description').text(data.Description || 'Nessuna Descrizione disponibile');
        $('#Detail-FileType').text(data.FileType);
        $('#Detail-FileName').text(data.FileName);
        $('#Detail-FileSizeKb').text(data.FileSizeKb);
        $('#Detail-CreatedDate').text(data.CreatedDateString || '');
        $('#Detail-IsActive').text(data.IsActiveString || '');
        $('#Detail-IsMain').text(data.IsMainString || '');
    }

    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------

    // Funzione che crea la griglia di miniature dentro la modale
    function caricaMiniatureModale(productId, activeId, url) {

        const $container = $('#ModalSmallImageConteiner');
        $container.empty(); // Pulisce le vecchie miniature

        // Cerca tutte le immagini della pagina che hanno lo stesso productId
        $(`.product-img[data-product-id="${productId}"]`).each(function () {
            let imgId = $(this).data('id');
            let fullSrc = $(this).data('full-src');
            let thumbSrc = $(this).attr('src'); // o data-src
            let isActive = (imgId === activeId) ? 'active' : '';

            // Crea il tag <img> per la miniatura nella modale
            let $thumb = $('<img>', {
                src: thumbSrc,
                class: `modal-thumb-item ${isActive}`,
                'data-id': imgId,
                'data-product-id': productId,
                'data-full-src': fullSrc,
                'data-url': url
            });

            $container.append($thumb);
        });
    }


});