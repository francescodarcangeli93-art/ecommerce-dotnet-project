
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

        resetCropModal();
      

    });

    function resetCropModal() {
        // 1. Svuota l'input file vero e proprio (questo accetta .val(''))
        $('#uploadFiles').val('');
        $('#uploadTitle').val('');
        $('#uploadIsMain').val('');
        $('#uploadDescription').val('');
        $('#cropCanvas').val('');
        // 2. Cancella la grafica dal Canvas
        const canvas = document.getElementById('cropCanvas');
        if (canvas) {
            const ctx = canvas.getContext('2d');
            ctx.clearRect(0, 0, canvas.width, canvas.height);
        }

        // 3. Azzera lo stato e nascondi il contenitore del crop
        cropState = null;
        $('#cropContainer').hide();
    }


    /*=========================================================================================================
       FUNZIONE PER GESTIRE IL CONTROLLO DELLE IMMAGINI NELL'UPLOAD
   =========================================================================================================*/

    // 1. Selezioni il tuo tag <input type="file"> (sostituisci "fileInput" col tuo ID reale)
    const fileInput = document.getElementById('uploadFiles');

   

    /*=========================================================================================================
    ANTEPRIMA RITAGLIO
   =========================================================================================================*/
    let cropState = null;

    // Leggi le dimensioni dall'input file esistente
    const targetW = parseInt($('#uploadFiles').data('width')) || 1200;
    const targetH = parseInt($('#uploadFiles').data('height')) || 1200;

    // Calcola il rapporto dinamico
    const TARGET_RATIO = targetW / targetH;

    $(document).on('change', '#uploadFiles', function () {

        const file = this.files[0];

        // se  il file non è un immagine mandalo cosi come è 

        if (!file || !file.type.startsWith('image/')) {
            $('#cropContainer').hide();
            cropState = null;
            return;
        }

        const reader = new FileReader();
        reader.onload = function (e) {

            const img = new Image();

            img.onload = function () {

                const canvas = document.getElementById('cropCanvas');
                const maxW = 500;
                const scale = Math.min(1, maxW / img.width);

                //calcola la grandezza dell'immagine per la scala cosi che il canvas non sara mai più grande di 500
                canvas.width = img.width * scale;
                canvas.height = img.height * scale

                // Logica identica all'algoritmo in c#
                const sourceRatio = img.width / img.height;

                let cropWidth = img.width;
                let cropHeight = img.height;
                let offsetX = 0, offsetY = 0;

                if (sourceRatio > TARGET_RATIO) {
                    cropWidth = Math.round(img.height * TARGET_RATIO);
                    offsetX = Math.round((img.width - cropWidth) / 2);
                } else {
                    cropHeight = Math.round(img.width / TARGET_RATIO);
                    offsetY = Math.round((img.height - cropHeight) / 2);
                }
                // ------------------------------------------------------------------------

                cropState = {
                    img: img,
                    scale: scale,
                    // coordinate in pixel REALI dell'immagine originale
                    x: offsetX,
                    y: offsetY,
                    w: cropWidth,
                    h: cropHeight,
                    dragging: false
                };

                $('#cropContainer').show();
                drawCrop();
            };

            img.onerror = function () {
                $('#cropContainer').hide();
                cropState = null; // Forza a null così non andrà in crash dopo
            };

            img.src = e.target.result;
        };

        reader.readAsDataURL(file);
    });

    // funzione che disegna il ritaglio
    function drawCrop() {

        const canvas = document.getElementById('cropCanvas');
        const ctx = canvas.getContext('2d');
        const s = cropState.scale;

        // pulire il canvas
        ctx.clearRect(0, 0, canvas.width, canvas.height);
        ctx.drawImage(cropState.img, 0, 0, canvas.width, canvas.height);

        // riquadro  con le coordinate adattate alla scala calcolata in precedenza
        const rx = cropState.x * s, ry = cropState.y * s;
        const rw = cropState.w * s, rh = cropState.h * s

        ctx.fillStyle = 'rgba(0,0,0,0.5)';
        ctx.fillRect(0, 0, canvas.width, canvas.height);
        ctx.clearRect(rx, ry, rw, rh);
        ctx.drawImage(cropState.img, cropState.x, cropState.y, cropState.w, cropState.h, rx, ry, rw, rh);

        ctx.strokeStyle = '#fff';
        ctx.lineWidth = 2;
        ctx.strokeRect(rx, ry, rw, rh);

    }

    // Trascinamento del riquadro (solo spostamento, la dimensione resta quella calcolata dall'aspect ratio)
    $(document).on('mousedown', '#cropCanvas', function (e) {

        if (!cropState) return;

        // capire dove si trova il canvas 
        const rect = this.getBoundingClientRect();

        const mx = (e.clientX - rect.left) / cropState.scale;
        const my = (e.clientY - rect.top) / cropState.scale;

        // controllo  se il punto cliccato dal mouse si trova alll'intenrno dei confini del rettangolo di ritaglio attuale 
        if (mx >= cropState.x && mx <= cropState.x + cropState.w &&
            my >= cropState.y && my <= cropState.y + cropState.h) {
            // se il click è all'intenro si imposta lo stato a true
            cropState.dragging = true;

            cropState.offsetMX = mx - cropState.x;
            cropState.offsetMY = my - cropState.y;
        }
    });

    // renderizza la posizione del riquadro quando si muove
    $(document).on('mousemove', '#cropCanvas', function (e) {

        if (!cropState || !cropState.dragging) return;

        //trovare dove sta il mouse e convertire la posizoine nei pixel effettivi della foto originale 
        const rect = this.getBoundingClientRect();
        const mx = (e.clientX - rect.left) / cropState.scale;
        const my = (e.clientY - rect.top) / cropState.scale;

        //calcolare la nuova posizione  del canvas
        let newX = mx - cropState.offsetMX;
        let newY = my - cropState.offsetMY;

        // 1. Controllo Bordo Sinistro: non può andare sotto lo 0
        if (newX < 0) {
            newX = 0;
        }
        // 2. Controllo Bordo Destro: non può superare la fine della foto
        else if (newX > cropState.img.width - cropState.w) {
            newX = cropState.img.width - cropState.w;
        }
        // (Se non entra in nessuno dei due if, newX rimane esattamente com'è!)

        // 1. Controllo Bordo Superiore: non può andare sotto lo 0
        if (newY < 0) {
            newY = 0;
        }
        // 2. Controllo Bordo Inferiore: non può superare il fondo della foto
        else if (newY > cropState.img.height - cropState.h) {
            newY = cropState.img.height - cropState.h;
        }

        cropState.x = newX;
        cropState.y = newY;
        drawCrop();
    });

    $(document).on('mouseup mouseleave', '#cropCanvas', function () {
        if (cropState) cropState.dragging = false;
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
        function inviaFile(fileToSend) {
            const formData = new FormData();
            formData.append('ProductId', id);
            formData.append('Description', $('#uploadDescription').val() || '');
            formData.append('Title', $('#uploadTitle').val());
            formData.append('IsMain', $('#uploadIsMain').is(':checked'));

            formData.append('uploadFiles', fileToSend, file.name);

            $.ajax({
                url: '/GestProductFile/UploadProductImage',
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
        }


        const file = fileInput.files[0];

        if (!cropState) {
            if (file) {
                // Opzione A (Consigliata per il TEST):
                // Invia il file originale grezzo al server così da far scattare la validazione C#
                inviaFile(file);
                return;
            } else {
                alert("Seleziona prima un'immagine valida.");
                return;
            }
        }



        // Ritaglia alla risoluzione REALE del rettangolo scelto (niente resize, mantiene qualità)
        const outputCanvas = document.createElement('canvas');
        outputCanvas.width = cropState.w;
        outputCanvas.height = cropState.h;

        const ctx = outputCanvas.getContext('2d');
        ctx.drawImage(cropState.img,
            cropState.x,
            cropState.y,
            cropState.w,
            cropState.h,
            0, 0, cropState.w,
            cropState.h);

        outputCanvas.toBlob(function (blob) {
            inviaFile(blob); // sostituisce il file originale con quello già ritagliato
        }, file.type === 'image/png' ? 'image/png' : 'image/jpeg', 0.92);
    });

   

      /*=========================================================================================================
       UPLOAD MANUALI
    =========================================================================================================*/
  
    $(document).on('click', '#btnUploadDocument', function () {

        const id = $(this).data('product-id');
        const fileInput = document.getElementById('uploadDocumentFile'); // Input file specifico per PDF

        $('#resultMessageUploadDoc').empty();

        if (!fileInput.files || fileInput.files.length === 0) {
            $('#resultMessageUploadDoc').html('<span class="text-danger">Seleziona un file PDF.</span>');
            return;
        }

        const file = fileInput.files[0];

        // Controllo rapido estensione lato client
        if (!file.name.toLowerCase().endsWith('.pdf')) {
            $('#resultMessageUploadDoc').html('<span class="text-danger">È possibile caricare solo documenti PDF.</span>');
            return;
        }

        const formData = new FormData();
        formData.append('ProductId', id);
        formData.append('Title', $('#uploadDocTitle').val() || '');
        formData.append('Description', $('#uploadDocDescription').val() || '');
        formData.append('IsMain', $('#uploadDocIsMain').is(':checked'));
        formData.append('uploadFiles', file);

        $.ajax({
            url: '/GestProductFile/UploadProductDocument',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            dataType: 'json',
            success: function (response) {
                if (response.success) {
                    // Chiudi modale e pulisci form
                    var modalEl = document.getElementById('modalAggiungiDocumento');
                    var modalInstance = bootstrap.Modal.getOrCreateInstance(modalEl);
                    if (modalInstance) modalInstance.hide();

                    $('#formAggiungiDocumento')[0].reset();

                    // 🌟 RICARICA SOLO LA PARTIAL DEI MANUALI
                    ricaricaManualiProdotto(id);
                } else {
                    $('#resultMessageUploadDoc').html('<span class="text-danger">' + response.message + '</span>');
                }
            },
            error: function (xhr) {
                $('#resultMessageUploadDoc').html('<span class="text-danger">Errore durante il caricamento del file.</span>');
            }
        });
    });

    // Funzione helper per ricaricare la partial view dei file
    function ricaricaManualiProdotto(productId) {
        $('.file-conteiner').load('/GestProductFile/GetProductFilesPartial?ProductId=' + productId);
    }

    /*========================================================================================================================
        GESTIONE DEL CAMBIO IMMAGINI QUANDO PASSO SOPRA CON IL MOUSE
    ==========================================================================================================================*/


    $(document).on('mouseenter', '.product-img', function () {
       
        const fullResUrl = $(this).data('full-src');

        $('#mainProductImage').attr('src', fullResUrl);

    });

    $(document).on('mouseleave', '.product-img', function () {
        const originalUrl = $('#mainProductImage').data('full-src') || '';
        console.log(originalUrl);
        $('#mainProductImage').attr('src', originalUrl);
    });

    /*========================================================================================================================
        EVENTO CLICK SULL'IMMAGINE PER APRIRE IL DETTAGLIO DELL'IMMAGINE
    ==========================================================================================================================*/

    $(document).on('click', '.test-click', function () {

       

        let $id = $(this).data('id');
        let $productId = $(this).data('product-id');
        const $url = $(this).data('url');
      
        // questo serve per prendere l'immagine ad alta risoluzione 
        let $fullSrc = $(this).data('hires-url');

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

                    // Popola la modale con la  logica esistente
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
        let $fullSrc = $(this).data('hires-url');
        console.log($fullSrc);
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

    $('#fileConteiner').on('click', '.btn-elimina-file', function () {

        const id = $(this).data('id');
        const name = $(this).data('name');

        $('#idFileDaEliminare').val(id);

        $('#FileDaCancellare').text(name);

        $('#modalEliminaPDF').addClass('active2');

    });

    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    $('#delFile-btn').on('click', function () {


        const $id = $('#idFileDaEliminare').val();

        const $deleteUrl = $(this).data('delete-url');


        const model = {
            id: $('#idFileDaEliminare').val()
        }
      
        $.ajax({

            url: $deleteUrl,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(model),
            dataType: 'json',

            success: function (response) {
                if (response.success) {

                    $('#modalEliminaPDF').removeClass('active2');

                    // rimuovo la riga della tabella corrispondente all'indirizzo eliminato
                    $('tr[data-id="' + model.id + '"]').remove();

                } else {
                    $('#resultMessage').html('<span style="color:red">' + response.message + '</span>');
                }
            },

        });

    });

    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    $('.no-btn').on('click', function () {

        $('#modalEliminaPDF').removeClass('active2');
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
            //prende l'url dell'hires
            let fullSrc = $(this).data('hires-url');

            let thumbSrc = $(this).attr('src'); // o data-src
            let isActive = (imgId === activeId) ? 'active' : '';

            // Crea il tag <img> per la miniatura nella modale
            let $thumb = $('<img>', {
                src: thumbSrc,
                class: `modal-thumb-item ${isActive}`,
                'data-id': imgId,
                'data-product-id': productId,
                'data-hires-url': fullSrc,
                'data-url': url
            });

            $container.append($thumb);
        });
    }

    /* ===========================================================================================================================================================================
      1.CANCELLA IMMAGINE
  ===========================================================================================================================================================================*/

    $(document).on('click', '.btn-delete-img', function () {
  
        const id = $(this).data('id');
   

        $('#idImmagineDaEliminare').val(id);



        $('#modalEliminaImmagine').addClass('active2');
    });

    // 

    $('#delImage-btn').on('click', function () {

 
        const $deleteUrl = $(this).data('delete-Image-url');

       

        const model = {
            id: $('#idImmagineDaEliminare').val()
        }

        $.ajax({

            url: '/GestProductFile/DeleteProductFile',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(model),
            dataType: 'json',

            success: function (response) {
                if (response.success) {

                    $('#modalEliminaImmagine').removeClass('active2');

                    // rimuovo dell'immagine nella lista
                    $('[data-id="' + model.id + '"]').closest('li').fadeOut(300, function () {
                        $(this).remove();
                    });

                } else {
                    $('#resultMessage').html('<span style="color:red">' + response.message + '</span>');
                }
            },

        });
       

    });

    /* ===========================================================================================================================================================================
    IMMAGINE PRINCIPALE
===========================================================================================================================================================================*/

    $('.immagine-no-btn').on('click', function () {
    
        $('#modalEliminaImmagine').removeClass('active2');
    });


    /* ===========================================================================================================================================================================
        IMMAGINE PRINCIPALE
    ===========================================================================================================================================================================*/

    $(document).on('click', '.btn-set-main', function (e) {
        e.stopPropagation();

        const $btn = $(this);

        // 1. Definiamo il MODEL FUORI dall'AJAX
        const model = {
            id: $btn.data('id'),
            productId: $btn.data('product-id'),
            fileType: 'Image'
        };

        if (!model.id || !model.productId) {
            alert("Dati immagine non validi.");
            return;
        }

        // 2. Chiamata AJAX
        $.ajax({
            url: '/GestProductFile/SetMainProductFile',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(model), // 👈 Usa il model
            dataType: 'json',

            success: function (response) {
                if (response.success) {

                    $('#imagesList').load('/GestProductFile/GetProductImagesPartial?ProductId=' + model.productId);


                } else {
                    alert('Errore: ' + response.message);
                }
            },
            error: function (xhr, status, error) {
                alert('Errore di rete: ' + error);
            }
        });
    });


    /* ===========================================================================================================================================================================
        ORDINAMENTO IMMAGINI
    ===========================================================================================================================================================================*/

    $(document).ready(function () {
        let $draggedItem = null;

        // 1. Inizio trascinamento del <li>
        $('#imagesList-2').on('dragstart', 'li', function (e) {
            $draggedItem = $(this);
            $draggedItem.addClass('dragging');

            // Necessario per abilitare il drag-and-drop su Firefox e Safari
            e.originalEvent.dataTransfer.setData('text/plain', '');
            e.originalEvent.dataTransfer.effectAllowed = 'move';
        });

        // 2. Fine trascinamento
        $('#imagesList-2').on('dragend', 'li', function () {
            if ($draggedItem) {
                $draggedItem.removeClass('dragging');
                $draggedItem = null;
            }
        });

        // 3. Permette il dragover (FONDAMENTALE per il drop)
        $('#imagesList-2').on('dragover', 'li', function (e) {
            e.preventDefault();
            e.originalEvent.dataTransfer.dropEffect = 'move';
        });

        // 4. Rilascio (Drop)
        $('#imagesList-2').on('drop', 'li', function (e) {
            e.preventDefault();
            e.stopPropagation();

            let $target = $(this);

            if ($draggedItem && $draggedItem[0] !== $target[0]) {
                let draggedIndex = $draggedItem.index();
                let targetIndex = $target.index();

                // Spostamento visivo 
                if (draggedIndex < targetIndex) {
                    $target.after($draggedItem);
                } else {
                    $target.before($draggedItem);
                }

                // Recuperiamo il ProductId dal <li>
                let productId = $draggedItem.attr('data-product-id');

                if (productId) {
                    salvaNuovoOrdine(productId);
                }
            }
        });
    });

    function salvaNuovoOrdine(productId) {
        let orderData = [];

        // Scorriamo la lista <li> aggiornata
        $('#imagesList-2 li').each(function (index) {
            let imageId = $(this).attr('data-id');

            if (imageId) {
                orderData.push({
                    Id: parseInt(imageId),
                    DisplayOrder: index + 1 // Nuova posizione
                });
            }
        });

        // Invio dati tramite AJAX
        $.ajax({
            url: '/GestProductFile/UpdateImageOrder',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({
                productId: parseInt(productId),
                items: orderData
            }),
            success: function (response) {
                console.log('Ordine aggiornato con successo!');
            },
            error: function (xhr) {
                alert('Errore durante il salvataggio: ' + xhr.responseText);
            }
        });
    }

   

});