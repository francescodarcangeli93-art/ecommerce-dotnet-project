function togglePass() {
    const inp = document.getElementById('password');
    const icona = document.getElementById('iconaEye');
    inp.type = inp.type === 'password' ? 'text' : 'password';
    icona.className = inp.type === 'password' ? 'bi bi-eye' : 'bi bi-eye-slash';
}