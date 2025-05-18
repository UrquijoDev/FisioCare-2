const datalistOptions = [...document.querySelectorAll("#PacientesList option")];

document.getElementById("PacienteSearch").addEventListener("input", function () {
    const input = this.value.toLowerCase();
    const match = datalistOptions.find(option => option.value.toLowerCase() === input);

    if (match) {
        document.getElementById("PacienteId").value = match.getAttribute("data-id");
        document.getElementById("PacienteError").classList.add("d-none");
    } else {
        document.getElementById("PacienteId").value = "";
    }
});

function validarPaciente() {
    const pacienteId = document.getElementById("PacienteId").value;
    if (!pacienteId) {
        document.getElementById("PacienteError").classList.remove("d-none");
        return false;
    }
    return true;
}