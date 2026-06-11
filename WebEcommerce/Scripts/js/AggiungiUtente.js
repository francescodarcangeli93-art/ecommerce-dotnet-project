function mostraNascondiCustomer() {

    const roleId = document.getElementById('roleId').value;
    const selezioneDati = document.getElementById('sezioneDatiPersonali');
    const inputsDati = selezioneDati.querySelectorAll('input');

    // Se il ruolo selezione è 1 viene mostrato il form dei customer

    if (roleId !== '' && roleId !== '1') {
        selezioneDati.style.display = 'block';
    } else {
        //Altrimenti nascondiamo il form e svuotiamo i campi per sicurezza
        selezioneDati.style.display = 'none';
        inputsDati.forEach(input => input.value = '');
    }
}




const valoriOriginali = {};
document.querySelectorAll('#formModifica input:not([type=hidden]), #formModifica select')
    .forEach(el => {
        valoriOriginali[el.id] = el.type === 'radio' ? el.checked : el.value;
    });

/*
    function annullaModifiche() {
        document.querySelectorAll('#formModifica input:not([type=hidden]), #formModifica select')
            .forEach(el => {
                if (el.type === 'radio') {
                    el.checked = valoriOriginali[el.id];
                } else {
                    el.value = valoriOriginali[el.id];
                }
            });
    }
*/


function confermaModifica() {
    const nome = document.getElementById('nome')?.value || '';
    const cognome = document.getElementById('cognome')?.value || '';
    const nomeCompleto = (nome + ' ' + cognome).trim() || document.getElementById('username')?.value || '';
    document.getElementById('nomeUtenteModal').textContent = nomeCompleto;
    const modal = new bootstrap.Modal(document.getElementById('modalConferma'));
    modal.show();
}

function submitForm() {
    const form = document.getElementById('formAggiungi');
    form.action = '/Gestutenti/AggiungiUtente';
    form.method = 'POST';
    form.submit();
}

function togglePass() {
    const inp = document.getElementById('password');
    const icona = document.getElementById('iconaEye');
    inp.type = inp.type === 'password' ? 'text' : 'password';
    icona.className = inp.type === 'password' ? 'bi bi-eye' : 'bi bi-eye-slash';
}