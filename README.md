# Gestionale E-commerce (Full-Stack) - PROTOTIPO

Un'applicazione gestionale full-stack progettata per la gestione di processi aziendali complessi. Il sistema include moduli per l'anagrafica, la gestione dei prodotti, il controllo dei movimenti di magazzino e l'elaborazione di file multimediali.

## 🔑 Credenziali di Accesso (Demo)
Per testare le diverse funzionalità del gestionale, è possibile utilizzare le seguenti credenziali demo:

*   **Amministratore (Admin):** 
    *   username: `admin`
    *   Password: `1234`
*   **Utente Standard (Cliente):** 
    *   Email: `luv`
    *   Password: `abcd`

## 🛠️ Stack Tecnologico
*   **Backend:** C#, ASP.NET, MVC ADO.NET
*   **Database:** SQL Server (20+ tabelle relazionali, 20+ Stored Procedure)
*   **Frontend:** HTML, CSS, JavaScript, jQuery
*   **Comunicazione Client-Server:** Viste HTML (Razor) e dati JSON per chiamate AJAX asincrone.

## 🚀 Architettura e Funzionalità Principali
Il sistema è stato sviluppato seguendo il pattern architetturale MVC (Model-View-Controller).

*   **Gestione Anagrafiche (Clienti, Utenti, Magazzini):** Creazione, modifica e visualizzazione di anagrafiche complesse con gestione delle relazioni e dei ruoli.
*   **Gestione Avanzata delle Immagini:** Modulo per l'upload dei file che include validazione lato server, **ritaglio dinamico (crop)**, ridimensionamento e applicazione di **watermark** per la protezione dei contenuti.
*   **Interazioni Asincrone:** Utilizzo di chiamate AJAX per l'aggiornamento dinamico dell'interfaccia senza ricaricare la pagina.

## 🚧 Stato del Progetto
Il progetto è nato durante il tirocinio ed è attualmente in pausa. Le funzionalità sono implementate, ma non ho eseguito test sistematici né verificato tutti i casi limite: possono esserci bug.

* **Anagrafiche e database:** implementati; flussi principali provati manualmente.
* **Movimenti di magazzino:** implementati; non verificati a fondo, quindi la logica di stock potrebbe contenere errori.
* **Gestione immagini (upload, ritaglio, ridimensionamento, watermark):** implementata; flussi principali provati manualmente.

**Limiti noti e possibili sviluppi:**
*   **Paginazione e ordinamento:** attualmente la paginazione dei dati avviene lato server (per ottimizzare le performance), mentre l'ordinamento e parte dei filtri sono gestiti lato client (JavaScript). Questo approccio causa un limite: l'ordinamento agisce solo sui record della pagina corrente e non sull'intero dataset. La soluzione prevista è spostare la logica di sorting e filtering direttamente nelle stored procedure SQL, passando i parametri dal server.
*   **UI/UX:** rendere più fluide le chiamate AJAX e la gestione degli stati di caricamento.
*   **Testing:** aggiungere test di integrazione per le chiamate ai controller.

## 🧠 Metodologia di Sviluppo e uso dell'AI
Ho scritto io il codice e la logica del progetto, con l'obiettivo di imparare a programmare davvero. Ho usato l'AI come tutor per chiarire concetti e valutare alternative di implementazione, e per generare codice ripetitivo (markup HTML, classi di configurazione), che ho poi riletto e capito prima di tenerlo.

## 👨‍💻 Autore
**Francesco D'Arcangeli**
