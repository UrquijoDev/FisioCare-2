function filtrarPacientes() {
    const input = document.getElementById("busquedaPaciente").value.toLowerCase();
    const items = document.querySelectorAll(".paciente-item");

    console.log("🔍 Valor de búsqueda:", input);
    console.log("📋 Elementos encontrados:", items.length);

    items.forEach(item => {
        const nombre = item.dataset.nombre;
        const correo = item.dataset.correo;

        const coincideBusqueda = nombre.includes(input) || correo.includes(input);

        console.log("👤 Paciente:", nombre);
        console.log("✉️ Correo:", correo);
        console.log("✅ Coincide con búsqueda:", coincideBusqueda);

        if (coincideBusqueda) {
            item.style.setProperty("display", "flex", "important");
            console.log("✅ MOSTRAR", item.dataset.nombre);
        } else {
            item.style.setProperty("display", "none", "important");
            console.log("❌ OCULTAR", item.dataset.nombre);
        }
    });
}



function filtrarHistorial() {
    const filtro = document.getElementById("filtroHistorial").value;
    const filas = document.querySelectorAll("tbody tr");

    filas.forEach(fila => {
        const estado = fila.getAttribute("data-estado");

        if (filtro === "Todos" || estado === filtro) {
            fila.style.display = "";
        } else {
            fila.style.display = "none";
        }
    });
}