function valida() {
    let u = document.getElementById("username").value;
    let p = document.getElementById("password").value;

    if (!u.includes("@")) {
        alert("Lo USERNAME deve essere un indirizzo e-mail");
        return false;
    }

    if (p.length < 8) {
        alert("La password deve avere almeno 8 caratteri");
        return false;
    }

    return true;
}

// Si usa quando l'utente si sta registrando per la prima volta
// (i controlli sulla password possono andare bene anche in caso di modifica della stessa)
function valida_registrazione() {
    let u = document.getElementById("username").value;
    let p1 = document.getElementById("password").value;
    let p2 = document.getElementById("passw2").value;

    if (!u.includes("@")) {
        alert("Lo USERNAME deve essere un indirizzo e-mail");
        return false;
    }

    if (p1 != p2) {
        alert("Le password non coincidono");
        return false
    }

    // Ci salvo gli errori nella scrittura della password
    let error = [];
    // Elenco dei caratteri speciali che voglio accettare
    let simboliStrani = [".", "_", "-", "*", "!", "?"];
    // Mi serve per contare quanti caratteri speciali l'utente ha scritto
    let conta = -1;

    if (p1.length < 8)
        error.push("La password deve avere almeno 8 caratteri");
    if (p1.search(/[a-z]/) < 0) // Ritorna -1 se non trova nessun carattere valido in p1
        error.push("La password deve contenere almeno una lettera in minuscolo");
    if (p1.search(/[A-Z]/) < 0)
        error.push("La password deve contenere almeno una lettera in maiuscolo");
    if (p1.search(/[0-9]/) < 0)
        error.push("La password deve contenere almeno un numero");

    for (let i = 0; i < simboliStrani.length; i++)
        for (let j = 0; j < p1.length; j++)
            if (simboliStrani[i] == p1[j])
                conta++;

    //SOLUZIONE alternativa:
    // sfrutto p1.includes(simboliStrani[i]); -> può sostituire il loop a base j e l'if sottostante
    //for (let i = 0; i < simboliStrani.length; i++)
    //    if (p1.includes(simboliStrani[i]))
    //        conta++;

    if (conta < 0)
        error.push("La password deve contenete almeno un carattere tra i seguenti . _ - * ! ?");

    if (error.length > 0) {
        alert(error.join("\n"));
        return false;
    }

    return true;
}