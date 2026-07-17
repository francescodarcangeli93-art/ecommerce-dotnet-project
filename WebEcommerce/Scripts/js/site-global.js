$(document).ready(function () {

    /*===================================================================================================================
     1 FUNZIONE PER SALVARE LA CATEORIA E FARE IL REDIRECT  ALLA NUOVA PAGINA
    =====================================================================================================================*/

    $(document).on("click", '.filtro-categoria', function () {

        let $filtroId = $(this).data('filtro');
        let $url = $(this).data('filtri-url') + '?Id=' + $filtroId;
       
        window.location.href = $url;

    });

    $(document).ready(function () {

        const urlParams = new URLSearchParams(window.location.search);

        let id = urlParams.get('Id');

        $('#searchCategoryId').val(id);
        $('#btnCerca').click();

    });

});

