
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



