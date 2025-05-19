    function validateServiceSelection() {
        const selected = document.getElementById("service").value;
        if (!selected) {
            document.getElementById("service-error").style.display = "block";
            return false;
        }
        return true;
    }