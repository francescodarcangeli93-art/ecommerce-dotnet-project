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

    let paginaCorrente = 1;

    function eseguiRicerca() {

        var filtri = {
            Username: $('input[name="Username"]').val(),
            RoleId: $('select[name="RoleId"]').val(),
            LastName: $('input[name="LastName"]').val(),
            FirstName: $('input[name="FirstName"]').val(),
            Email: $('input[name="Email"]').val(),
            IsActive: $('#isActive').val(),
            page: paginaCorrente
        };

        sessionStorage.setItem('filtriUtenti', JSON.stringify(filtri));

        $.ajax({
            url: $searchUrl,
            type: 'GET',
            data: filtri,
            success: function (response) {
                $tabellaContainer.html(response).show();
            },
            error: function (xhr, status, error) {
                console.error("Errore ricerca:", error);
                $tabellaContainer.html('<tr><td colspan="10" class="text-center text-danger">Errore di connessione durante la ricerca.</td></tr>');
            }
        });
    }

    // Click sul bottone: esegue la ricerca, sempre da pagina 1
    $btnCerca.on('click', function (e) {
        e.preventDefault();
        paginaCorrente = 1;
        eseguiRicerca();
    });

    // RIPRISTINO: eseguito SOLO al caricamento della pagina, una volta
    const filtriSalvati = sessionStorage.getItem('filtriUtenti');

    if (filtriSalvati) {
        const filtri = JSON.parse(filtriSalvati);
        let haFiltriReali = false;

        Object.keys(filtri).forEach(key => {
            if (key !== 'page' && filtri[key] !== null && filtri[key] !== undefined && filtri[key] !== "") {
                if (key === 'IsActive') {
                    $('#isActive').val(filtri[key]);
                } else {
                    $(`[name="${key}"]`).val(filtri[key]);
                }
                haFiltriReali = true;
            }
        });

        paginaCorrente = filtri.page ? parseInt(filtri.page) : 1;

        if (haFiltriReali) {
            $tabellaContainer.hide();
            eseguiRicerca(); // chiama direttamente la funzione, non il click
        }
    }

    // funzione per pulire i filtri
    $('#btnPulisci').on('click', function (e) {

        $('input[name="Username"]').val('');
        $('select[name="RoleId"]').val('');
        $('input[name="LastName"]').val('');
        $('input[name="FirstName"]').val('');
        $('input[name="Email"]').val('');
        $('#isActive').val('');
        paginaCorrente = 1;
        sessionStorage.removeItem("filtriUtenti");
        $btnCerca.click();
    });

    // Gestione click sui link di paginazione (Precedente / Successiva)
    // I link nella view puntano a GestioneUtenti con ?page=N: qui viene intercettato
    // il click, viene bloccata la navigazione reale, e si estrae il numero di pagina
    // dall'href per rilanciare la ricerca via AJAX con i filtri correnti.
    $tabellaContainer.on('click', '.page-link', function (e) {
        e.preventDefault();

        if ($(this).closest('li').hasClass('disabled')) {
            return;
        }

        var paginaRichiesta = parseInt($(this).data('page'));

        if (paginaRichiesta >= 1) {
            paginaCorrente = paginaRichiesta;
            eseguiRicerca();
        }
    });

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

});