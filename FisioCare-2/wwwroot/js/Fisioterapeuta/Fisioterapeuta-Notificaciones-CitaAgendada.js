<script>
    setTimeout(() => {
        const alertNode = document.querySelector('.alert');
    if (alertNode) {
        alertNode.classList.remove('show');
    alertNode.classList.add('fade');
            setTimeout(() => {
        alertNode.remove();
            }, 300); // Espera breve para que se vea el fade antes de eliminar
        }
    }, 5000); // 5 segundos antes de iniciar la eliminación
</script>