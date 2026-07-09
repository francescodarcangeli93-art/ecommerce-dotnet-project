$(document).ready(function () {

   
    $('#logout').on('click', function () {

        $('#logOut').addClass('active2');
    });

    $('.no-btn').on('click', function () {

        $('#logOut').removeClass('active2');
    });

    $('#si-btn').on('click', function () {

        let url = $(this).data('url');

        if (url) {
            window.location.href = url;
        }

        //$('#logOut').removeClass('active2');
    });

});



