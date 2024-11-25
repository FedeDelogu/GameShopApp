window.addEventListener('load', () => {
    const menu = document.getElementById('menu'); // Seleziona il menu
    const sidebar = document.getElementById('sidebar'); // Seleziona il sidebar

    // Calcola l'altezza del menu
    const menuHeight = menu.offsetHeight;

    // Imposta il valore di top per il sidebar
    sidebar.style.top = `${menuHeight}px`;
});

window.addEventListener('resize', () => {
    const menu = document.getElementById('menu');
    const sidebar = document.getElementById('sidebar');

    // Ricalcola l'altezza del menu su resize
    const menuHeight = menu.offsetHeight;

    // Aggiorna il valore di top per il sidebar
    sidebar.style.top = `${menuHeight}px`;
});

document.querySelectorAll('a[data-bs-toggle="collapse"]').forEach(function (link) {
    link.addEventListener('click', function (event) {
        // Prevenire il comportamento predefinito del link (facoltativo)
        event.preventDefault();

        // Trova l'elemento target associato
        const target = document.querySelector(this.getAttribute('href'));

        if (target) {
            // Alterna il comportamento del collapse
            const bsCollapse = bootstrap.Collapse.getOrCreateInstance(target);
            bsCollapse.toggle();
        }
    });
});
document.addEventListener("DOMContentLoaded", function () {
    const collapseElement = document.querySelector('#collapseExample');
    if (!collapseElement) {
        console.error("Elemento con ID 'collapseExample' non trovato!");
    } else {
        console.log("Elemento trovato correttamente.");
    }
});
