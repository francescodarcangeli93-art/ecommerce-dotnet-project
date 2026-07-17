
$(document).ready(function () {

    function ricaricaImmaginiProdotto(productId) {
        console.log('entrato');

        $.ajax({
            url: '/GestProductFile/GetProductImagesPartial',
            type: 'GET',
            data: { ProductId: productId },
            success: function (html) {
                console.log('successo');
                $('#imagesList').replaceWith(html);
            },
            error: function (xhr) {
                console.error('Errore ricaricamento immagini:', xhr.responseText);
            }
        });
    }

    /*=========================================================================================================
       EVENTO CHE CHIUDE LA MODALE
    =========================================================================================================*/
    $('#btnAggiungiFile').on('click', function () {

        $('#uploadFiles').val('');
        $('#uploadTitle').val('');
        $('#uploadIsMain').val('');
        $('#uploadDescription').val('');

    });

    $(document).on('click','#btnUploadFile', function () {
       
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

    $(document).on('click mouseenter', '.product-img', function () {
        const $url = $(this).data('full-src');

        $('#mainProductImage').attr('src',$url);

    });

});