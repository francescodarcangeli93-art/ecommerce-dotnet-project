
function confermaEliminazione(id, username)
{

    document.getElementById('idUtenteDaEliminare').value = id;
    document.getElementById('nomeUtenteDaEliminare').textContent = username;

    const Modal = document.getElementById('modalElimina');
    const istanza = new bootstrap.Modal(Modal, {
        backdrop: 'static',
        keyboard: false
    });

    istanza.show();
}

setTimeout(function ()
{
    var alertSuccesso = document.getElementById('alertSuccesso');
    var alerErrore = document.getElementById('alertErrore');

    if (alertSuccesso)
    {
        alertSuccesso.style.transition = 'opacity 1s';
        alertSuccesso.style.opacity = '0';
    }

    if (alerErrore) {
        alerErrore.style.transition = 'opacity 1s';
        alerErrore.style.opacity = '0';
    }

}, 1000)

function clearFilters()
{
    const filtri = document.querySelectorAll('#filtri input, #filtri select');

    filtri.forEach(filtro => {
        filtro.value = '';
    });

    document.getElementById('filtri').submit();
}

    $(document).ready(function () {

        $("#tblUser th").click(function () {

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
                return newDirection === "ASC"
                    ? numberA - numberB
                    : numberB - numberA;
            }

            return newDirection === "ASC"
                ? valueA.localeCompare(valueB)
                : valueB.localeCompare(valueA);
        });

        tbody.empty().append(rows);
    });

});