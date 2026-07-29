
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
        $('#uploadFiles').val('');
        $('#uploadTitle').val('');
        $('#uploadDescription').val('');
        $('#resultMessageUpload').text('');
        $('#multiCropContainer').empty(); //svuota tutte le anteprime generate dinamicamente

        cropStates = []; //resetta l'array, non la variabile singola inesistente
    }


    /*=========================================================================================================
       FUNZIONE PER GESTIRE IL CONTROLLO DELLE IMMAGINI NELL'UPLOAD
   =========================================================================================================*/

    // 1. Selezioni il tuo tag <input type="file"> (sostituisci "fileInput" col tuo ID reale)
    const fileInput = document.getElementById('uploadFiles');



 /*=========================================================================================================
    ANTEPRIMA RITAGLIO
 =========================================================================================================*/
    let cropStates = [];

    // Leggi le dimensioni dall'input file esistente
    const targetW = parseInt($('#uploadFiles').data('width')) || 1200;
    const targetH = parseInt($('#uploadFiles').data('height')) || 1200;

    // Calcola il rapporto dinamico
    const TARGET_RATIO = targetW / targetH;

    $(document).on('change', '#uploadFiles', function () {

        const files = Array.from(this.files);
        cropStates = [];

        $('#multiCropContainer').empty();

        files.forEach(function (file, index) {

            // se il file non è un'immagine, saltalo
            if (!file || !file.type.startsWith('image/')) return;

            const reader = new FileReader();
            reader.onload = function (e) {

                const img = new Image();

                img.onload = function () {

                    const canvasId = 'cropCanvas_' + index;
                    const radioId = 'isMain_' + index;

                    $('#multiCropContainer').append(`
                    <div class="crop-item text-center">
                        <canvas id="${canvasId}" style="border:1px solid #ccc;  max-width:100%; cursor: move; display:block;"></canvas>
                        <small class="text-muted d-block mt-1">${file.name}</small>

                        <div class="form-check form-check-inline mt-1">
                            <input type="checkbox" name="isMainRadio" class="form-check-input isMain-radio" id="${radioId}" value="${index}">
                            <label class="form-check-label" for="${radioId}">Principale</label>
                        </div>
                    </div>
                `);

                    const canvas = document.getElementById(canvasId);
                    const maxW = 500;
                    const scale = Math.min(1, maxW / img.width);

                    // calcola la grandezza dell'immagine per la scala cosi che il canvas non sara mai più grande di 500
                    canvas.width = img.width * scale;
                    canvas.height = img.height * scale;

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

                    cropStates[index] = {
                        img: img,
                        scale: scale,
                        canvasId: canvasId,
                        file: file,
                        // coordinate in pixel REALI dell'immagine originale
                        x: offsetX,
                        y: offsetY,
                        w: cropWidth,
                        h: cropHeight,
                        dragging: false
                    };

                    drawCrop(index);
                };

                img.onerror = function () {
                    cropStates[index] = null;
                };

                img.src = e.target.result;
            };

            reader.readAsDataURL(file);
        });
    });

    $(document).on('change', '.isMain-radio', function () {
        if (this.checked) {
            // Deseleziona tutti gli altri checkbox "Principale"
            $('.isMain-radio').not(this).prop('checked', false);
        }
    });

    // funzione che disegna il ritaglio
    function drawCrop(index) {

        const state = cropStates[index];
        if (!state) return;

        const canvas = document.getElementById(state.canvasId);
        const ctx = canvas.getContext('2d');
        const s = state.scale;

        // pulire il canvas
        ctx.clearRect(0, 0, canvas.width, canvas.height);
        ctx.drawImage(state.img, 0, 0, canvas.width, canvas.height);

        // riquadro con le coordinate adattate alla scala calcolata in precedenza
        const rx = state.x * s, ry = state.y * s;
        const rw = state.w * s, rh = state.h * s;

        ctx.fillStyle = 'rgba(0,0,0,0.5)';
        ctx.fillRect(0, 0, canvas.width, canvas.height);
        ctx.clearRect(rx, ry, rw, rh);
        ctx.drawImage(state.img, state.x, state.y, state.w, state.h, rx, ry, rw, rh);

        ctx.strokeStyle = '#fff';
        ctx.lineWidth = 2;
        ctx.strokeRect(rx, ry, rw, rh);
    }

    function getStateIndexFromCanvas(canvasEl) {
        return cropStates.findIndex(s => s && s.canvasId === canvasEl.id);
    }

    // Trascinamento del riquadro (solo spostamento, la dimensione resta quella calcolata dall'aspect ratio)
    $(document).on('mousedown', 'canvas[id^="cropCanvas_"]', function (e) {

        const index = getStateIndexFromCanvas(this);
        const state = cropStates[index];
        if (!state) return;

        // capire dove si trova il canvas
        const rect = this.getBoundingClientRect();

        const mx = (e.clientX - rect.left) / state.scale;
        const my = (e.clientY - rect.top) / state.scale;

        // controllo se il punto cliccato dal mouse si trova all'interno dei confini del rettangolo di ritaglio attuale
        if (mx >= state.x && mx <= state.x + state.w &&
            my >= state.y && my <= state.y + state.h) {
            // se il click è all'interno si imposta lo stato a true
            state.dragging = true;

            state.offsetMX = mx - state.x;
            state.offsetMY = my - state.y;
        }
    });

    // renderizza la posizione del riquadro quando si muove
    $(document).on('mousemove', 'canvas[id^="cropCanvas_"]', function (e) {

        const index = getStateIndexFromCanvas(this);
        const state = cropStates[index];
        if (!state || !state.dragging) return;

        // trovare dove sta il mouse e convertire la posizione nei pixel effettivi della foto originale
        const rect = this.getBoundingClientRect();
        const mx = (e.clientX - rect.left) / state.scale;
        const my = (e.clientY - rect.top) / state.scale;

        // calcolare la nuova posizione del canvas
        let newX = mx - state.offsetMX;
        let newY = my - state.offsetMY;

        // 1. Controllo Bordo Sinistro: non può andare sotto lo 0
        if (newX < 0) {
            newX = 0;
        }
        // 2. Controllo Bordo Destro: non può superare la fine della foto
        else if (newX > state.img.width - state.w) {
            newX = state.img.width - state.w;
        }

        // 1. Controllo Bordo Superiore: non può andare sotto lo 0
        if (newY < 0) {
            newY = 0;
        }
        // 2. Controllo Bordo Inferiore: non può superare il fondo della foto
        else if (newY > state.img.height - state.h) {
            newY = state.img.height - state.h;
        }

        state.x = newX;
        state.y = newY;
        drawCrop(index);
    });

    $(document).on('mouseup mouseleave', 'canvas[id^="cropCanvas_"]', function () {
        const index = getStateIndexFromCanvas(this);
        if (cropStates[index]) cropStates[index].dragging = false;
    });

    /*=========================================================================================================
       UPLOAD IMMAGINI
    =========================================================================================================*/

    $(document).on('click', '#btnUploadFile', async function () {

        const id = $(this).data('product-id');
        const fileInput = document.getElementById('uploadFiles');

        if (!fileInput.files || fileInput.files.length === 0) {
            $('#resultMessageUpload').html('<span style="color:red">Seleziona un file.</span>');
            return;
        }

        // Determina quale immagine è "principale"
        const checkboxSelezionato = $('.isMain-radio:checked').val();
        let mainIndex;

        if (checkboxSelezionato !== undefined) {
            mainIndex = parseInt(checkboxSelezionato);
        } else if (fileInput.files.length === 1) {
            mainIndex = null;
        } else {
            mainIndex = 0;
        }

        const files = Array.from(fileInput.files);

        try {
            // Ritaglia TUTTE le immagini in parallelo (lavoro locale, nessun rischio)
            const blobsRitagliati = await Promise.all(
                files.map((file, index) => ritagliaImmagine(file, cropStates[index]))
            );

            // Invia le immagini una alla volta, in ordine (sicuro lato server)
            for (let index = 0; index < files.length; index++) {
                await inviaFile(blobsRitagliati[index], index, files[index].name, id, mainIndex);
            }

            var modalEl = document.getElementById('modalAggiungiFile');
            var modalInstance = bootstrap.Modal.getOrCreateInstance(modalEl);
            if (modalInstance) modalInstance.hide();

            $('#uploadFiles').val('');
            cropStates = [];
            $('#multiCropContainer').empty();

            ricaricaImmaginiProdotto(id);

        } catch (errMsg) {
            $('#resultMessageUpload').html('<span style="color:red">Errore: ' + errMsg + '</span>');
        }
    });

    // Ritaglia l'immagine secondo lo stato di crop scelto dall'utente (o restituisce il file grezzo se non c'è crop)
    function ritagliaImmagine(file, state) {
        return new Promise(function (resolve) {
            if (!state) {
                resolve(file); // nessun crop disponibile, invia il file così com'è
                return;
            }

            const outputCanvas = document.createElement('canvas');
            outputCanvas.width = state.w;
            outputCanvas.height = state.h;

            const ctx = outputCanvas.getContext('2d');
            ctx.drawImage(state.img,
                            state.x,
                            state.y,
                            state.w,
                            state.h, 0, 0,
                            state.w,
                            state.h);

            outputCanvas.toBlob(function (blob) {
                resolve(blob);
            }, file.type === 'image/png' ? 'image/png' : 'image/jpeg', 0.92);
        });
    }

    // Invia un singolo file al server
    function inviaFile(fileToSend, index, fileName, productId, mainIndex) {
        return new Promise(function (resolve, reject) {
            const formData = new FormData();
            formData.append('ProductId', productId);
            formData.append('Description', $('#uploadDescription').val() || '');
            formData.append('Title', $('#uploadTitle').val());
            formData.append('IsMain', index === mainIndex);
            formData.append('uploadFiles', fileToSend, fileName);

            $.ajax({
                url: '/GestProductFile/UploadProductImage',
                type: 'POST',
                data: formData,
                processData: false,
                contentType: false,
                dataType: 'json',

                success: function (response) {
                    if (response.success) resolve();
                    else reject(response.message);
                },
                error: function (xhr) {
                    reject(xhr.responseText);
                }
            });
        });
    }



   /*=========================================================================================================
     UPLOAD PDF/MANUALI
  =========================================================================================================*/

    $(document).on('click', '#btnUploadDocument', function () {

        const productId = $(this).data('product-id');
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
        formData.append('ProductId', productId);
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

                    // RICARICA SOLO LA PARTIAL DEI MANUALI

                    $('#fileConteiner').load('/GestProductFile/GetProductFilesPartial?ProductId=' + productId);

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
  

    /*========================================================================================================================
        GESTIONE DEL CAMBIO IMMAGINI QUANDO PASSO SOPRA CON IL MOUSE
    ==========================================================================================================================*/


    $(document).on('mouseenter', '.product-img', function () {

        const fullResUrl = $(this).data('full-src');

        $('#mainProductImage').attr('src', fullResUrl);

    });

    $(document).on('mouseleave', '.product-img', function () {
        const originalUrl = $('#mainProductImage').data('full-src') || '';
    
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

    /* ===========================================================================================================================================================================
          1. EVENTO CLICK CHE ELIMINA IL FILE
      ===========================================================================================================================================================================*/

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

    // scorrimento immagini
   let currentImageIndex = 0;

/* ===========================================================================================================================================================================
     1. CAMBIO IMMAGINI CON LE FRECCETTE NEL DETTAGLIO IMMAGINI
 ===========================================================================================================================================================================*/

 function showImageAtIndex(index) {


  let $thumbs = $('#ModalSmallImageConteiner img');
  let totalThumbs = $thumbs.length;


  if (totalThumbs === 0) return;

  // Ciclo infinito
  if (index < 0) {
    currentImageIndex = totalThumbs - 1; // se l'indice è minore di zero -> le immagini sono 13 l'indice arriva 12
  } else if (index >= totalThumbs) {
    currentImageIndex = 0; //
  } else {
    currentImageIndex = index;
  }

  // Prendo l'immagine corrispondente dentro #ModalSmallImageConteiner
  let $selectedThumb = $thumbs.eq(currentImageIndex);
  let newSrc = $selectedThumb.data('hires-url'); //  mi serve il hiresurl

  // Aggiorno l'immagine principale
    $('#modalImage').attr('src', newSrc);

  // Aggiorno il bordo blu sulla miniatura
  $thumbs.removeClass('active');
  $selectedThumb.addClass('active');
}
// --------------------------------------------------------------
// Click sulla Freccia Sinistra
$(document).on('click', '#btnPrevImage', function (e) {
    e.preventDefault();

    let activeIndex = $('#ModalSmallImageConteiner img.active').index();
    let currentImageIndex = 0;

    if (activeIndex >= 0) {
        currentImageIndex = activeIndex;
    } else {
        currentImageIndex
    }

  showImageAtIndex(currentImageIndex - 1);
});

// Click sulla Freccia Destra
    $(document).on('click', '#btnNextImage', function (e) {
   
        e.preventDefault();
        let activeIndex = $('#ModalSmallImageConteiner img.active').index();
        let currentImageIndex = 0;


        if (activeIndex >= 0) {
            currentImageIndex = activeIndex;
        } else {
            currentImageIndex
        }
  showImageAtIndex(currentImageIndex + 1);
});

// Click su una miniatura dentro il tuo #ModalSmallImageConteiner
$(document).on('click', '#ModalSmallImageConteiner img', function () {
  let index = $(this).index(); // Prende la posizione dell'immagine nella griglia
  showImageAtIndex(index);
});

// Navigazione da tastiera con le frecce
$(document).on('keyup', function (e) {
  if ($('#ModalSmallImageConteiner').is(':visible')) {
    if (e.key === 'ArrowLeft') showImageAtIndex(currentImageIndex - 1);
    if (e.key === 'ArrowRight') showImageAtIndex(currentImageIndex + 1);
  }
});


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


        const $deleteUrl = $(this).data('delete-image-url');
        const $productId = $(this).data('product-id');


        const model = {
            id: $('#idImmagineDaEliminare').val(),
            productId: $productId
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
                    //$('[data-id="' + model.id + '"]').closest('li').fadeOut(300, function () {
                    //    $(this).remove();
                    //});
                    $('#imagesList').load('/GestProductFile/GetProductImagesPartial?ProductId=' + model.productId);
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
        const $url = $(this).data('url');

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
            url: $url,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(model), 
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
       EVENTO CLICK CHE SETTA UN FILE A FILE PRINCIPALE
   ===========================================================================================================================================================================*/
    
    $('#fileConteiner').on('click', '.fileIsActive', function () {
        const $btn = $(this);
        const $url = $(this).data('url');

        console.log($url);

        // 1. Definiamo il MODEL FUORI dall'AJAX
        const model = {
            id: $btn.data('id'),
            productId: $btn.data('product-id'),
            fileType: 'Manual'
        };

        $.ajax({
            url: $url,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(model),
            dataType: 'json',

            success: function (response) {
                if (response.success) {

                    $('#fileConteiner').load('/GestProductFile/GetProductFilesPartial?ProductId=' + model.productId);


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

        // Delegazione globale per elementi correnti e futuri
        $(document)
            .off('dragstart dragend dragover drop', '#imagesList-2 li') // Pulisce eventuali vecchi listener

            .on('dragstart', '#imagesList-2 li', function (e) {
                $draggedItem = $(this);
                $draggedItem.addClass('dragging');
                e.originalEvent.dataTransfer.setData('text/plain', '');
                e.originalEvent.dataTransfer.effectAllowed = 'move';
            })

            .on('dragend', '#imagesList-2 li', function () {
                if ($draggedItem) {
                    $draggedItem.removeClass('dragging');
                    $draggedItem = null;
                }
            })

            .on('dragover', '#imagesList-2 li', function (e) {
                e.preventDefault();
                e.originalEvent.dataTransfer.dropEffect = 'move';
            })

            .on('drop', '#imagesList-2 li', function (e) {
                e.preventDefault();
                e.stopPropagation();

                let $target = $(e.target).closest('li');

                if ($draggedItem && $target.length && $draggedItem[0] !== $target[0]) {
                    let draggedIndex = $draggedItem.index();
                    let targetIndex = $target.index();

                    if (draggedIndex < targetIndex) {
                        $target.after($draggedItem);
                    } else {
                        $target.before($draggedItem);
                    }

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