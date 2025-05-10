document.addEventListener('DOMContentLoaded', function () {
    // Variables de estado
/*    let currentStep = 1;*/
    let selectedTherapist = null;
    let selectedService = null;
    let selectedDate = null;
    let selectedTime = null;

    // Elementos del DOM
    const steps = document.querySelectorAll('.step');
    const formSections = document.querySelectorAll('.form-section');
    const therapistCards = document.querySelectorAll('.therapist-card');
    const therapistSelect = document.getElementById('therapist-select');
    const serviceSelect = document.getElementById('service');
    const calendarDays = document.getElementById('calendar-days');
    const timeSlotsContainer = document.getElementById('time-slots');
    const currentMonthElement = document.getElementById('current-month');

    //// Botones de navegación
    //const nextButtons = {
    //    1: document.getElementById('next-1'),
    //    2: document.getElementById('next-2'),
    //    3: document.getElementById('next-3')
    //};

    //const prevButtons = {
    //    2: document.getElementById('prev-2'),
    //    3: document.getElementById('prev-3'),
    //    4: document.getElementById('prev-4')
    //};

    // Elementos de resumen
    const summaryTherapist = document.getElementById('summary-therapist');
    const summaryService = document.getElementById('summary-service');
    const summaryDate = document.getElementById('summary-date');
    const summaryTime = document.getElementById('summary-time');

    // Mensajes de error
    const errorMessages = {
        therapist: document.getElementById('therapist-error'),
        service: document.getElementById('service-error'),
        datetime: document.getElementById('datetime-error')
    };

    // Configuración del calendario
    let currentDate = new Date();
    let currentMonth = currentDate.getMonth();
    let currentYear = currentDate.getFullYear();

    // Event listeners para tarjetas de terapeutas
    therapistCards.forEach(card => {
        card.addEventListener('click', function () {
            therapistCards.forEach(c => c.classList.remove('selected'));
            this.classList.add('selected');
            selectedTherapist = {
                id: this.dataset.id,
                name: this.querySelector('.therapist-name').textContent,
                specialty: this.querySelector('.therapist-specialty').textContent,
                photo: this.querySelector('.therapist-photo').src
            };
            therapistSelect.value = this.dataset.id;
            hideError(errorMessages.therapist);
        });
    });

    // Event listener para el select de terapeutas (versión móvil)
    therapistSelect.addEventListener('change', function () {
        if (this.value) {
            const selectedCard = document.querySelector(`.therapist-card[data-id="${this.value}"]`);
            therapistCards.forEach(c => c.classList.remove('selected'));
            if (selectedCard) {
                selectedCard.classList.add('selected');
                selectedTherapist = {
                    id: selectedCard.dataset.id,
                    name: selectedCard.querySelector('.therapist-name').textContent,
                    specialty: selectedCard.querySelector('.therapist-specialty').textContent,
                    photo: selectedCard.querySelector('.therapist-photo').src
                };
            }
            hideError(errorMessages.therapist);
        } else {
            selectedTherapist = null;
        }
    });

    // Event listener para el select de servicios
    serviceSelect.addEventListener('change', function () {
        if (this.value) {
            selectedService = {
                id: this.value,
                name: this.options[this.selectedIndex].text
            };
            hideError(errorMessages.service);
        } else {
            selectedService = null;
        }
    });

    //// Event listeners para botones de siguiente
    //Object.keys(nextButtons).forEach(step => {
    //    nextButtons[step].addEventListener('click', function () {
    //        if (validateStep(parseInt(step))) {
    //            goToStep(parseInt(step) + 1);
    //        }
    //    });
    //});

    //// Event listeners para botones de anterior
    //Object.keys(prevButtons).forEach(step => {
    //    prevButtons[step].addEventListener('click', function () {
    //        goToStep(parseInt(step) - 1);
    //    });
    //});

    // Event listeners para navegación del calendario
    document.getElementById('prev-month').addEventListener('click', function () {
        currentMonth--;
        if (currentMonth < 0) {
            currentMonth = 11;
            currentYear--;
        }
        renderCalendar();
    });

    document.getElementById('next-month').addEventListener('click', function () {
        currentMonth++;
        if (currentMonth > 11) {
            currentMonth = 0;
            currentYear++;
        }
        renderCalendar();
    });

    //// Event listener para confirmar cita
    //document.getElementById('confirm-appointment').addEventListener('click', function () {
    //    // En una aplicación real, aquí enviarías los datos al servidor
    //    document.querySelector('.booking-form').style.display = 'none';
    //    document.getElementById('confirmation').style.display = 'block';
    //    goToStep(5); // Paso de confirmación
    //});

    //// Event listener para nueva cita
    //document.getElementById('new-appointment').addEventListener('click', function () {
    //    resetForm();
    //    document.getElementById('confirmation').style.display = 'none';
    //    document.querySelector('.booking-form').style.display = 'block';
    //    goToStep(1);
    //});

    // Funciones auxiliares
    //function goToStep(step) {
    //    // Actualizar pasos visualmente
    //    steps.forEach(s => {
    //        s.classList.remove('active', 'completed');
    //        if (parseInt(s.dataset.step) < step) {
    //            s.classList.add('completed');
    //        } else if (parseInt(s.dataset.step) === step) {
    //            s.classList.add('active');
    //        }
    //    });

    //    // Ocultar todas las secciones y mostrar la actual
    //    formSections.forEach(section => section.classList.remove('active'));
    //    document.getElementById(`step-${step}`).classList.add('active');

    //    currentStep = step;

    //    // Actualizar resumen si estamos en el paso 4
    //    if (step === 4) {
    //        updateSummary();
    //    }

    //    // Renderizar calendario si estamos en el paso 3
    //    if (step === 3) {
    //        renderCalendar();
    //    }
    //}

    //function validateStep(step) {
    //    let isValid = true;

    //    switch (step) {
    //        case 1:
    //            if (!selectedTherapist) {
    //                showError(errorMessages.therapist);
    //                isValid = false;
    //            }
    //            break;
    //        case 2:
    //            if (!selectedService) {
    //                showError(errorMessages.service);
    //                isValid = false;
    //            }
    //            break;
    //        case 3:
    //            if (!selectedDate || !selectedTime) {
    //                showError(errorMessages.datetime);
    //                isValid = false;
    //            }
    //            break;
    //    }

    //    return isValid;
    //}

    //function showError(element) {
    //    element.classList.add('show');
    //    setTimeout(() => {
    //        element.classList.remove('show');
    //    }, 3000);
    //}

    //function hideError(element) {
    //    element.classList.remove('show');
    //}

    function renderCalendar() {
        // Configurar fecha inicial del mes
        const firstDay = new Date(currentYear, currentMonth, 1);
        const lastDay = new Date(currentYear, currentMonth + 1, 0);
        const daysInMonth = lastDay.getDate();

        // Obtener el día de la semana del primer día (0 = Domingo, 1 = Lunes, etc.)
        const startingDay = firstDay.getDay();

        // Actualizar el título del mes
        const monthNames = ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
            "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"];
        currentMonthElement.textContent = `${monthNames[currentMonth]} ${currentYear}`;

        // Limpiar el calendario
        calendarDays.innerHTML = '';

        // Añadir celdas vacías para los días del mes anterior
        for (let i = 0; i < startingDay; i++) {
            const emptyDay = document.createElement('div');
            emptyDay.classList.add('calendar-day', 'disabled');
            calendarDays.appendChild(emptyDay);
        }

        // Añadir los días del mes
        const today = new Date();
        today.setHours(0, 0, 0, 0);

        for (let day = 1; day <= daysInMonth; day++) {
            const date = new Date(currentYear, currentMonth, day);
            const dayElement = document.createElement('div');
            dayElement.classList.add('calendar-day');
            dayElement.textContent = day;

            // Marcar el día actual
            if (date.getTime() === today.getTime()) {
                dayElement.classList.add('today');
            }

            // Deshabilitar días pasados
            if (date < today) {
                dayElement.classList.add('disabled');
            } else {
                // Permitir selección de días futuros
                dayElement.addEventListener('click', function () {
                    if (!this.classList.contains('disabled')) {
                        document.querySelectorAll('.calendar-day').forEach(d => {
                            d.classList.remove('selected');
                        });
                        this.classList.add('selected');
                        selectedDate = date;
                        selectedTime = null;
                        renderTimeSlots();
                        hideError(errorMessages.datetime);
                    }
                });
            }

            calendarDays.appendChild(dayElement);
        }

        // Renderizar horarios (vacío hasta que se seleccione una fecha)
        renderTimeSlots();
    }

    //function renderTimeSlots() {
    //    timeSlotsContainer.innerHTML = '';

    //    if (!selectedDate) {
    //        return;
    //    }

    //    // Generar horarios disponibles (simulado)
    //    const startHour = 9; // 9 AM
    //    const endHour = 17; // 5 PM
    //    const bookedSlots = []; // En una app real, esto vendría del servidor

    //    // Aleatoriamente marcar algunos horarios como ocupados para simular
    //    for (let i = 0; i < 3; i++) {
    //        const randomHour = Math.floor(Math.random() * (endHour - startHour)) + startHour;
    //        bookedSlots.push(`${randomHour}:00`);
    //    }

    //    for (let hour = startHour; hour < endHour; hour++) {
    //        // Crear slots cada hora
    //        const timeSlot = document.createElement('div');
    //        const timeString = `${hour}:00`;

    //        timeSlot.classList.add('time-slot');
    //        timeSlot.textContent = timeString;

    //        // Marcar slots ocupados
    //        if (bookedSlots.includes(timeString)) {
    //            timeSlot.classList.add('occupied');
    //        } else {
    //            timeSlot.addEventListener('click', function () {
    //                if (!this.classList.contains('occupied')) {
    //                    document.querySelectorAll('.time-slot').forEach(slot => {
    //                        slot.classList.remove('selected');
    //                    });
    //                    this.classList.add('selected');
    //                    selectedTime = timeString;
    //                    hideError(errorMessages.datetime);
    //                }
    //            });
    //        }

    //        timeSlotsContainer.appendChild(timeSlot);
    //    }
    //}

    //function updateSummary() {
    //    summaryTherapist.textContent = selectedTherapist ? `${selectedTherapist.name} - ${selectedTherapist.specialty}` : 'No seleccionado';
    //    summaryService.textContent = selectedService ? selectedService.name : 'No seleccionado';
    //    summaryDate.textContent = selectedDate ? selectedDate.toLocaleDateString() : 'No seleccionado';
    //    summaryTime.textContent = selectedTime ? selectedTime : 'No seleccionado';
    //}

    function resetForm() {
        selectedTherapist = null;
        selectedService = null;
        selectedDate = null;
        selectedTime = null;

        document.querySelectorAll('.form-section').forEach(section => section.classList.remove('active'));
        goToStep(1);
    }

    // Inicializar
    renderCalendar();
});