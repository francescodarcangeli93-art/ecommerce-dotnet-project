function confermaEliminazione(id, username) {
    document.getElementById('idUtenteDaEliminare').value = id;
    document.getElementById('nomeUtenteDaEliminare').textContent = username;

    const Modal = document.getElementById('modalElimina');
    const istanza = new bootstrap.Modal(Modal, {
        backdrop: 'static',
        keyboard: false
    });

    istanza.show();
}

setTimeout(function () {
    var alertSuccesso = document.getElementById('alertSuccesso');
    var alerErrore = document.getElementById('alertErrore');

    if (alertSuccesso) {
        alertSuccesso.style.transition = 'opacity 3s';
        alertSuccesso.style.opacity = '0';
    }

    if (alerErrore) {
        alerErrore.style.transition = 'opacity 3s';
        alerErrore.style.opacity = '0';
    }

}, 1000);

$(document).ready(function () {

    const $btnCerca = $('#btnCerca');
    const $searchUrl = $btnCerca.attr('data-url');
    const $tabellaContainer = $('#tabellaUtentiContainer');

    // FUNZIONE DI RICERCA
    function eseguiRicerca(param) {
        // FACCIAMO APPARIRE IL LOADER E ASSICURIAMOCI CHE IL CONTENITORE SIA VISIBILE
        $tabellaContainer.show().html('<div class="text-center py-5"><div class="spinner-border text-primary" role="status"><span class="visually-hidden">Caricamento...</span></div></div>');

        $.ajax({
            url: $searchUrl,
            type: 'POST',
            data: param,
            success: function (html) {
                $tabellaContainer.html(html);
            },
            error: function (xhr) {
                alert("Errore durante la ricerca: " + xhr.responseText);
                $tabellaContainer.show();
            }
        });
    }

    // ORDINAMENTO COLONNE
    $('#tabellaUtentiContainer').on('click', '#tblUser th', function () {
        var table = $("#tblUser");
        var tbody = table.find("tbody");
        var columnIndex = $(this).data("column");

        var currentDirection = $(this).data("direction") || "ASC";
        var newDirection = currentDirection === "ASC" ? "DESC" : "ASC";

        $("#tblUser th").data("direction", null);
        $("#tblUser th span").text("");

        $(this).data("direction", newDirection);
        $(this).find("span").text(newDirection === "ASC" ? " ▲" : " ▼");

        var rows = tbody.find("tr").toArray();

        rows.sort(function (a, b) {
            var valueA = $(a).children("td").eq(columnIndex).text().trim();
            var valueB = $(b).children("td").eq(columnIndex).text().trim();

            var numberA = parseFloat(valueA.replace(",", "."));
            var numberB = parseFloat(valueB.replace(",", "."));

            if (!isNaN(numberA) && !isNaN(numberB)) {
                return newDirection === "ASC" ? numberA - numberB : numberB - numberA;
            }

            return newDirection === "ASC" ? valueA.localeCompare(valueB) : valueB.localeCompare(valueA);
        });

        tbody.empty().append(rows);
    });

    // CLICK SUL BOTTONE CERCA
    $btnCerca.click(function () {
        const param = {
            Username: $('input[name="Username"]').val(),
            RoleId: $('select[name="RoleId"]').val(),
            LastName: $('input[name="LastName"]').val(),
            FirstName: $('input[name="FirstName"]').val(),
            Email: $('input[name="Email"]').val(),
            IsActive: $('#isActive').val()
        };

        sessionStorage.setItem('filtriRicercaUtenti', JSON.stringify(param));
        eseguiRicerca(param);
    });

    // RIPRISTINO DEI FILTRI AL CARICAMENTO
    const filtriSalvati = sessionStorage.getItem('filtriRicercaUtenti');
    if (filtriSalvati) {
        const f = JSON.parse(filtriSalvati);

        $('input[name="Username"]').val(f.Username);
        $('select[name="RoleId"]').val(f.RoleId);
        $('input[name="LastName"]').val(f.LastName);
        $('input[name="FirstName"]').val(f.FirstName);
        $('input[name="Email"]').val(f.Email);
        $('#isActive').val(f.IsActive);

        eseguiRicerca(f);
    } else {
        // Se NON ci sono filtri salvati, l'HTML è stato nascosto per niente dal trucco inline? 
        // No, lo script inline lo nasconde SOLO se ci sono i filtri. Ma per sicurezza lo forziamo visibile.
        $tabellaContainer.show();
    }
});

function clearFilters() {
    $('input[name="Username"]').val('');
    $('select[name="RoleId"]').val('');
    $('input[name="LastName"]').val('');
    $('input[name="FirstName"]').val('');
    $('input[name="Email"]').val('');
    $('#isActive').val('');

    sessionStorage.removeItem('filtriRicercaUtenti');

    setTimeout(function () {
        $('#btnCerca').click();
    }, 50);
}