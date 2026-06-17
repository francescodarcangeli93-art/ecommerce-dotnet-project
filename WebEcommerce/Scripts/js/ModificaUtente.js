
    document.addEventListener('DOMContentLoaded', function () {
        mostraNascondiCustomer();
    });

    function mostraNascondiCustomer() {
        const roleId = document.getElementById('roleId').value;
       

        const selezioneDati = document.getElementById('DatiPersonali');
        const inputsDati = selezioneDati.querySelectorAll('input');
        if (roleId !== '' && roleId != '1') {
            selezioneDati.style.display = 'block';
        } else {
                // nasconde la sezione dati personali dall'te
            selezioneDati.style.display = 'none';
              //  inputsDati.forEach(input => input.value = ''); questo comando pulisce 
            }
    }

    //function confermaModifica() {
    //    const nome = document.getElementById('nome')?.value || '';
    //    const cognome = document.getElementById('cognome')?.value || '';
    //    const nomeCompleto = (nome + ' ' + cognome).trim() || document.getElementById('username')?.value || '';
    //    document.getElementById('nomeUtenteModal').textContent = nomeCompleto;
    //    const modal = new bootstrap.Modal(document.getElementById('modalConferma'));
    //    modal.show();
    //}

    function confermaModifica() {
        const Modal = document.getElementById('modalConferma');
        const istanza = new bootstrap.Modal(Modal, {
            backdrop: 'static',
            keyboard: false
        });
        istanza.show();
    }

    function submitForm() {
        document.getElementById('formModifica').submit();
    }

 

setTimeout(function () {
    var successo = document.getElementById('successo');

    if (successo)
    {
        successo.style.transition = 'opacity 5s';
        successo.style.opacity = '0';
    }
   
    
},1000)