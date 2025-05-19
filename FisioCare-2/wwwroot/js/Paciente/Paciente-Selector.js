// Obtener elementos clave
const therapistSelect = document.getElementById("therapist-select");
const therapistCards = document.querySelectorAll(".therapist-card");
const selectedInput = document.getElementById("selectedTherapistId");
const errorMsg = document.getElementById("therapist-error");

// Cuando cambian el select
therapistSelect.addEventListener("change", function () {
    const selectedId = this.value;
    selectedInput.value = selectedId;

    therapistCards.forEach(card => {
        if (card.dataset.id === selectedId) {
            card.setAttribute("data-selected", "true");
        } else {
            card.removeAttribute("data-selected");
        }
    });

    errorMsg.style.display = "none";
});

// Cuando hacen click en una tarjeta
therapistCards.forEach(card => {
    card.addEventListener("click", function () {
        const selectedId = this.dataset.id;
        selectedInput.value = selectedId;
        therapistSelect.value = selectedId;

        therapistCards.forEach(c => c.removeAttribute("data-selected"));
        this.setAttribute("data-selected", "true");

        errorMsg.style.display = "none";
    });
});

// Validación al dar siguiente
function validateTherapistSelection() {
    if (!selectedInput.value) {
        errorMsg.style.display = "block";
        return false;
    }
    return true;
}