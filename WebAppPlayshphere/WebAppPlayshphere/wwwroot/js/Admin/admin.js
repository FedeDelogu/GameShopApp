
// Attesa caricamento pagina
window.addEventListener('load', function () {

    document.getElementById('lista-utenti').addEventListener('click', listaUtenti); // Al click del Pulsante mostra lista utenti

    // LISTA UTENTI NON BANNATI
    function listaUtenti() {
        fetch('/Utenti/ListaUtenti') // Richiama l'action che restituisce un JSON
            .then(response => response.json()) // Converte il risultato in JSON
            .then(data => {
                let content = `<div class="row border-bottom mb-3 ">
                                            <div class="col-1">ID</div>
                                            <div class="col-4">Username</div>
                                            <div class="col-2">Ruolo</div>
                                            <div class="col">Azioni</div>
                                            </div>`;
                data.forEach(utente => {
                    console.log('Dati ricevuti:', JSON.stringify(data, null, 2));
                    // Crea un ID unico per il modale
                    const modalId = `modal-${utente.id}`;
                    const banId = `ban-${utente.id}`;

                    if (utente.ruolo != -1) {

                        content += `<div class="row border-bottom mb-3 py-2">
                                <div class="col-1">${utente.id}</div>
                                <div class="col-4">${utente.username}</div>
                                    <div class="col-2">${utente.ruolo === 0 ? "Admin" : utente.ruolo === -1 ? "BANNATO" : "Utente"}</div>
                                <div class="col">
                                    <button type="button" class="btn btn-info" data-bs-toggle="modal" data-bs-target="#${modalId}">
                                        Mostra Anagrafica
                                    </button>
                                            <button type="button" class="btn btn-danger" data-user-id="${utente.id}" id="${banId}">
                                            Banna Utente
                                    </button>
                                </div>
                            </div>`;

                        // Aggiungi il modale dinamicamente
                        content += `<div class="modal fade" id="${modalId}" tabindex="-1" aria-labelledby="${modalId}Label" aria-hidden="true">
                                    <div class="modal-dialog">
                                      <div class="modal-content">
                                        <div class="modal-header">
                                          <h1 class="modal-title fs-5" id="${modalId}Label">Anagrafica di ${utente.username}</h1>
                                          <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                        </div>
                                        <div class="modal-body">
                                            Nome: ${utente.anagrafica?.nome ?? 'Non presente'} <br />
                                            Cognome: ${utente.anagrafica?.cognome ?? 'Non presente'} <br />
                                            Telefono: ${utente.anagrafica?.telefono ?? 'Non presente'} <br />
                                            Anagrafica: ${utente.anagrafica?.citta ?? 'Non presente'} <br />
                                            CAP: ${utente.anagrafica?.cap ?? 'Non presente'} <br />
                                            Stato: ${utente.anagrafica?.stato ?? 'Non presente'} <br />
                                        </div>
                                        <div class="modal-footer">
                                          <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Chiudi</button>
                                        </div>
                                      </div>
                                    </div>
                                </div>`;
                    }
                });
                document.getElementById('content-dashboard').innerHTML = content;
            })
            .catch(error => console.error('Errore:', error));
    } // Fine listaUtenti

    // Funzione per ban utente
    document.getElementById('content-dashboard').addEventListener('click', function (event) { // Al click del Pulsante banna utente

        if (event.target.classList.contains('btn-danger')) {
            const userId = event.target.getAttribute('data-user-id'); // Ottieni l'ID utente

            if (userId > 0) {
                // Esegui il ban dell'utente con l'ID specificato
                console.log(`Tentativo di bannare l'utente con ID: ${userId}`);

                fetch('/Utenti/BanUtente', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({ id: userId }) // Passa l'ID utente al server - es. ("id": 1)
                })
                    .then(response => response.json())
                    .then(ban => {
                        if (ban.success) {
                            console.log(`Utente con ID ${userId} bannato con successo.`);
                            alert("Utente Bannato con successo!");
                            listaUtenti(); // Aggiorna la lista

                        } else {
                            console.error(`Errore durante il ban dell'utente con ID ${userId}:`, ban.message);
                        }
                    })
                    .catch(error => console.error('Errore nella richiesta:', error));
            }
        }
    }); // Fine ban utente

    document.getElementById('lista-utenti-bannati').addEventListener('click', listaUtentiBannati); // Al click del Pulsante mostra lista utenti BANNATI

    // LISTA UTENTI BANNATI
    function listaUtentiBannati() {
        fetch('/Utenti/ListaUtenti') // Richiama l'action che restituisce un JSON
            .then(response => response.json()) // Converte il risultato in JSON
            .then(data => {
                let content = `<div class="row border-bottom mb-3 ">
                                            <div class="col-1">ID</div>
                                            <div class="col-4">Username</div>
                                            <div class="col-2">Ruolo</div>
                                            <div class="col">Azioni</div>
                                            </div>`;
                data.forEach(utente => {
                    console.log('Dati ricevuti:', JSON.stringify(data, null, 2));
                    // Crea un ID unico per il modale
                    const modalId = `modal-${utente.id}`;
                    const sbanId = `ban-${utente.id}`;

                    if (utente.ruolo == -1) {

                        content += `<div class="row border-bottom mb-3 py-2">
                                <div class="col-1">${utente.id}</div>
                                <div class="col-4">${utente.username}</div>
                                    <div class="col-2">${utente.ruolo === 0 ? "Admin" : utente.ruolo === -1 ? "BANNATO" : "Utente"}</div>
                                <div class="col">
                                    <button type="button" class="btn btn-info" data-bs-toggle="modal" data-bs-target="#${modalId}">
                                        Mostra Anagrafica
                                    </button>
                                            <button type="button" class="btn btn-success" data-blocked-user-id="${utente.id}" id="${sbanId}">
                                            Sblocca Utente
                                    </button>
                                </div>
                            </div>`;

                        // Aggiungi il modale dinamicamente
                        content += `<div class="modal fade" id="${modalId}" tabindex="-1" aria-labelledby="${modalId}Label" aria-hidden="true">
                                    <div class="modal-dialog">
                                      <div class="modal-content">
                                        <div class="modal-header">
                                          <h1 class="modal-title fs-5" id="${modalId}Label">Anagrafica di ${utente.username}</h1>
                                          <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                        </div>
                                        <div class="modal-body">
                                            Nome: ${utente.anagrafica?.nome ?? 'Non presente'} <br />
                                            Cognome: ${utente.anagrafica?.cognome ?? 'Non presente'} <br />
                                            Telefono: ${utente.anagrafica?.telefono ?? 'Non presente'} <br />
                                            Anagrafica: ${utente.anagrafica?.citta ?? 'Non presente'} <br />
                                            CAP: ${utente.anagrafica?.cap ?? 'Non presente'} <br />
                                            Stato: ${utente.anagrafica?.stato ?? 'Non presente'} <br />
                                        </div>
                                        <div class="modal-footer">
                                          <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Chiudi</button>
                                        </div>
                                      </div>
                                    </div>
                                </div>`;
                    }
                });
                document.getElementById('content-dashboard').innerHTML = content;
            })
            .catch(error => console.error('Errore:', error));
    } // Fine listaUtenti

    // Funzione per sblocco utente
    document.getElementById('content-dashboard').addEventListener('click', function (event) { // Al click del Pulsante banna utente

        if (event.target.classList.contains('btn-success')) {
            const userId = event.target.getAttribute('data-blocked-user-id'); // Ottieni l'ID utente

            if (userId > 0) {
                // Esegui il ban dell'utente con l'ID specificato
                console.log(`Tentativo di sbannare l'utente con ID: ${userId}`);

                fetch('/Utenti/SbloccaUtente', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({ id: userId }) // Passa l'ID utente al server - es. ("id": 1)
                })
                    .then(response => response.json())
                    .then(sban => {
                        if (sban.success) {
                            console.log(`Utente con ID ${userId} bannato con successo.`);
                            alert("Utente Sbloccato con successo!");
                            listaUtentiBannati(); // Aggiorna la lista

                        } else {
                            console.error(`Errore durante il ban dell'utente con ID ${userId}:`, sban.message);
                        }
                    })
                    .catch(error => console.error('Errore nella richiesta:', error));
            }
        }
    }); // Fine sblocco utente


    // Lista delle recensioni approvate
    document.getElementById('lista-recensioni').addEventListener('click', listaRecensioni); // Al click del Pulsante mostra lista recensioni

    // JSON LISTA delle RECENSIONI APPROVATE
    function listaRecensioni() {
        fetch('/Recensioni/ListaRecensioni') // Richiama l'action che restituisce un JSON
            .then(response => response.json()) // Converte il risultato in JSON
            .then(data => {

                let content = `<div class="row border-bottom mb-3 ">
                                            <div class="col-1">ID</div>
                                            <div class="col-4">Username</div>
                                            <div class="col-2">Valutazione</div>
                                            <div class="col-2">Stato</div>
                                            <div class="col">Azioni</div>
                                            </div>`;
                data.forEach(recensione => {
                    console.log('Dati ricevuti:', JSON.stringify(data, null, 2));
                    // Crea un ID unico per il modale
                    const modalId = `modal-${recensione.id}`;
                    const disapprovaId = `approva-${recensione.id}`;
                    const deleteId = `elimina-${recensione.id}`;

                    if (recensione.valido == 1) {
                        content += `<div class="row border-bottom mb-3 py-2">
                                <div class="col-1">${recensione.id}</div>
                                <div class="col-4">${recensione.utente?.email ?? 'Non presente'}</div>
                                <div class="col-2">${recensione.valutazione}/5</div>
                                <div class="col-2">${recensione.valido === true ? "Approvata" : "Da approvare"}</div>
                                <div class="col">
                                    <button type="button" class="btn btn-info" data-bs-toggle="modal" data-bs-target="#${modalId}">
                                        Contenuto recensione
                                    </button>
                                            <button type="button" class="btn btn-disapproved" data-review-id="${recensione.id}" id="${disapprovaId}">
                                            Disapprova
                                    </button>
                                    </button>
                                            <button type="button" class="btn btn-delete" data-delete-id="${recensione.id}" id="${deleteId}">
                                            Elimina
                                    </button>
                                </div>
                            </div>`;

                        // Aggiungi il modale dinamicamente
                        content += `<div class="modal fade" id="${modalId}" tabindex="-1" aria-labelledby="${modalId}Label" aria-hidden="true">
                                    <div class="modal-dialog">
                                      <div class="modal-content">
                                        <div class="modal-header">
                                          <h1 class="modal-title fs-5" id="${modalId}Label">Contenuto della recensioni di ${recensione.utente?.username ?? 'Non presente'}</h1>
                                          <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                        </div>
                                        <div class="modal-body">
                                            ${recensione.commento}
                                        </div>
                                        <div class="modal-footer">
                                          <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Chiudi</button>
                                        </div>
                                      </div>
                                    </div>
                                </div>`;
                    }
                });
                document.getElementById('content-dashboard').innerHTML = content;
            })
            .catch(error => console.error('Errore:', error));
    } // Fine lista recensioni approvate

    // Funzione di DISAPPROVAZIONE RECENSIONE
    document.getElementById('content-dashboard').addEventListener('click', function (event) { // Al click del Pulsante 
        // disapprova la recensione
        if (event.target.classList.contains('btn-disapproved')) {
            const reviewId = event.target.getAttribute('data-review-id'); // Ottieni l'ID recensione

            if (reviewId > 0) {
                // Esegui l'approvazione con l'ID specificato
                console.log(`Tentativo di approvare la recensione con ID: ${reviewId}`);

                fetch('/Recensioni/DisapprovaRecensione', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({ id: reviewId }) // Passa l'ID recensione al server - es. ("id": 1)
                })
                    .then(response => response.json()) // trasforma in json la risposta
                    .then(disapproved => { // usa il json
                        if (disapproved.success) {
                            console.log(`Recensione con ID ${reviewId} disapprovata con successo.`);
                            alert("Recensione disapprovata con successo!");
                            listaRecensioni(); // Aggiorna la lista

                        } else {
                            console.error(`Errore durante il ban dell'utente con ID ${reviewId}:`, reviewId.message);
                        }
                    })
                    .catch(error => console.error('Errore nella richiesta:', error));
            }
        }
    }); // Fine Appravozaione recensione

    // Al click del Pulsante mostra lista recensioni da approvare
    document.getElementById('lista-recensioni-da-approvare').addEventListener('click', listaRecensioniDaApprovare);

    // JSON LISTA delle RECENSIONI DA APPROVATE
    function listaRecensioniDaApprovare() {
        fetch('/Recensioni/ListaRecensioni') // Richiama l'action che restituisce un JSON
            .then(response => response.json()) // Converte il risultato in JSON
            .then(data => {

                let content = `<div class="row border-bottom mb-3 ">
                                            <div class="col-1">ID</div>
                                            <div class="col-4">Username</div>
                                            <div class="col-2">Valutazione</div>
                                            <div class="col-2">Stato</div>
                                            <div class="col">Azioni</div>
                                            </div>`;
                data.forEach(recensione => {
                    console.log('Dati ricevuti:', JSON.stringify(data, null, 2));
                    // Crea un ID unico per il modale
                    const modalId = `modal-${recensione.id}`;
                    const approvaId = `approva-${recensione.id}`;
                    const deleteId = `elimina-${recensione.id}`;

                    if (recensione.valido == 0) {
                        content += `<div class="row border-bottom mb-3 py-2">
                                <div class="col-1">${recensione.id}</div>
                                <div class="col-4">${recensione.utente?.email ?? 'Non presente'}</div>
                                <div class="col-2">${recensione.valutazione}/5</div>
                                <div class="col-2">${recensione.valido === true ? "Approvata" : "Da approvare"}</div>
                                <div class="col">
                                    <button type="button" class="btn btn-info" data-bs-toggle="modal" data-bs-target="#${modalId}">
                                        Contenuto recensione
                                    </button>
                                            <button type="button" class="btn btn-approved" data-review-id="${recensione.id}" id="${approvaId}">
                                            Approva
                                    </button>
                                    </button>
                                            <button type="button" class="btn btn-delete" data-delete-id="${recensione.id}" id="${deleteId}">
                                            Elimina
                                    </button>
                                </div>
                            </div>`;

                        // Aggiungi il modale dinamicamente
                        content += `<div class="modal fade" id="${modalId}" tabindex="-1" aria-labelledby="${modalId}Label" aria-hidden="true">
                                    <div class="modal-dialog">
                                      <div class="modal-content">
                                        <div class="modal-header">
                                          <h1 class="modal-title fs-5" id="${modalId}Label">Contenuto della recensioni di ${recensione.utente?.username ?? 'Non presente'}</h1>
                                          <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                        </div>
                                        <div class="modal-body">
                                            ${recensione.commento}
                                        </div>
                                        <div class="modal-footer">
                                          <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Chiudi</button>
                                        </div>
                                      </div>
                                    </div>
                                </div>`;
                    }
                });
                document.getElementById('content-dashboard').innerHTML = content;
            })
            .catch(error => console.error('Errore:', error));
    } // Fine listaUtenti


    // Funzione di approvazione recensione
    document.getElementById('content-dashboard').addEventListener('click', function (event) { // Al click del Pulsante approva la recensione
        if (event.target.classList.contains('btn-approved')) {
            const reviewId = event.target.getAttribute('data-review-id'); // Ottieni l'ID recensione

            if (reviewId > 0) {
                // Esegui l'approvazione con l'ID specificato
                console.log(`Tentativo di approvare la recensione con ID: ${reviewId}`);

                fetch('/Recensioni/ApprovaRecensione', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({ id: reviewId }) // Passa l'ID recensione al server - es. ("id": 1)
                })
                    .then(response => response.json()) // trasforma in json la risposta
                    .then(approved => { // usa il json
                        if (approved.success) {
                            console.log(`Recensione con ID ${reviewId} approvata con successo.`);
                            alert("Recensione approvata con successo!");
                            listaRecensioniDaApprovare(); // Aggiorna la lista

                        } else {
                            console.error(`Errore durante il ban dell'utente con ID ${reviewId}:`, reviewId.message);
                        }
                    })
                    .catch(error => console.error('Errore nella richiesta:', error));
            }
        }
    }); // Fine Appravozaione recensione

    // Funzione Elimina Recensione
    document.getElementById('content-dashboard').addEventListener('click', function (event) { // Al click del Pulsante elimina la recensione
        if (event.target.classList.contains('btn-delete')) {
            const deleteId = event.target.getAttribute('data-delete-id'); // Ottieni l'ID della recensione

            if (deleteId > 0) {
                // Esegui l'eliminazione con l'ID specificato
                console.log(`Tentativo di eliminare la recensione con ID: ${deleteId}`);

                fetch('/Recensioni/EliminaRecensione', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({ id: deleteId }) // Passa l'ID utente al server - es. ("id": 1)
                })
                    .then(response => response.json()) // trasforma in json la risposta
                    .then(deleted => { // usa il json
                        if (deleted.success) {
                            console.log(`Recensione con ID ${deleteId} eliminata con successo.`);
                            alert("Recensione eliminata con successo!");
                            listaRecensioni(); // Aggiorna la lista

                        } else {
                            console.error(`Errore durante il ban dell'utente con ID ${deleteId}:`, deleteId.message);
                        }
                    })
                    .catch(error => console.error('Errore nella richiesta:', error));
            }
        }
    }); // Fine Elimina recensione

    document.getElementById('lista-prodotti').addEventListener('click', elencoProdotti); // Al click del Pulsante mostra lista prodotti


    // Funzione per elenco prodotti
    async function elencoProdotti() {
        try {
            const response = await fetch('/Videogiochi/CatalogoJson');
            if (!response.ok) {
                throw new Error('Errore nel recupero dei dati');
            }

            const prodotti = await response.json();
            console.log('Dati ricevuti:', JSON.stringify(prodotti, null, 2));

            const content = document.getElementById('content-dashboard');
            content.innerHTML = '';

            prodotti.forEach(prodotto => {
                // Formatta la data
                const rilascio = new Date(prodotto.rilascio);
                const opzioni = { day: '2-digit', month: 'long', year: 'numeric' };
                const dataRilascioFormattata = rilascio.toLocaleDateString('it-IT', opzioni);

                const deleteId = `elimina-${prodotto.id}`;
                const prodottoDiv = document.createElement('div');

                prodottoDiv.classList.add('prodotto');
                prodottoDiv.innerHTML = `
                        <span hidden>ID: ${prodotto.id} </span><h2>${prodotto.titolo}</h2>
                        <p>Descrizione: ${prodotto.descrizione}</p>
                        <p>Data rilascio: ${dataRilascioFormattata}</p>
                        <p>Prezzo: ${prodotto.prezzo}</p>
                        <p>Genere: ${prodotto.generi}</p>
                        <p>Pegi: ${prodotto.pegi}</p>
                        <p>Sviluppatori: ${prodotto.sviluppatori}</p>
                        <p>Publisher: ${prodotto.publisher}</p>
                        <p>Quantità: ${prodotto.quantita}</p>
                        <button type="button" class="btn btn-delete" data-delete-id="${prodotto.id}" id="${deleteId}"> Elimina
                        </button>
                        <hr />`;

                content.appendChild(prodottoDiv);
            });
        } catch (error) {
            console.error(error);
        }
    }

    document.getElementById('aggiungi-prodotto').addEventListener('click', formAggiungiProdotto); // Al click del Pulsante mostra form aggiunta prodotto

    // Funzione per il form di aggiunta prodotti
    async function formAggiungiProdotto() {
        try {
            const response = await fetch('/Videogiochi/FormAggiungi');
            if (!response.ok) {
                throw new Error('Errore nel recupero della pagina del form');
            }
            const htmlContent = await response.text();
            document.getElementById('content-dashboard').innerHTML = htmlContent;
        } catch (error) {
            console.error(error);
        }
    }

    // Funzione Elimina Prodotto
    document.getElementById('content-dashboard').addEventListener('click', function (event) { // Al click del Pulsante elimina il prodotto
        if (event.target.classList.contains('btn-delete')) {
            const deleteId = event.target.getAttribute('data-delete-id'); // Ottieni l'ID del prodotto

            if (deleteId > 0) {
                // Esegui l'eliminazione con l'ID specificato
                console.log(`Tentativo di eliminare la recensione con ID: ${deleteId}`);

                fetch('/Videogiochi/EliminaProdotto', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({ id: deleteId }) // Passa l'ID utente al server - es. ("id": 1)
                })
                    .then(response => response.json()) // trasforma in json la risposta
                    .then(deleted => { // usa il json
                        if (deleted.success) {
                            console.log(`Prodotto con ID ${deleteId} eliminata con successo.`);
                            alert("Recensione eliminata con successo!");
                            elencoProdotti(); // Aggiorna la lista

                        } else {
                            console.error(`Errore durante il ban dell'utente con ID ${deleteId}:`, deleteId.message);
                        }
                    })
                    .catch(error => console.error('Errore nella richiesta:', error));
            }
        }
    }); // Fine Elimina Prodotto


}); // FINE LOAD PAGINA