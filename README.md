# Ecommerce (Full-Stack) - WORK IN PROGRESS

Un'applicazione gestionale full-stack progettata per la gestione di processi aziendali complessi. Il sistema include moduli per l'anagrafica, la gestione dei prodotti, il controllo dei movimenti di magazzino e l'elaborazione di file multimediali.

## 🛠️ Stack Tecnologico
*   **Backend:** C#, ASP.NET MVC
*   **Database:** SQL Server (20+ tabelle relazionali, 20+ Stored Procedure)
*   **Frontend:** HTML, CSS, JavaScript, jQuery
*   **Comunicazione Client-Server:** Viste HTML (Razor) e dati JSON per chiamate AJAX asincrone.

## 🚀 Architettura e Funzionalità Principali
Il sistema è stato sviluppato seguendo il pattern architetturale MVC (Model-View-Controller).

*   **Gestione Anagrafiche (Clienti, Utenti, Magazzini):** Creazione, modifica e visualizzazione di anagrafiche complesse con gestione delle relazioni e dei ruoli.
*   **Movimenti di Magazzino:** Logica di business per il tracciamento delle entrate/uscite merci, controllo dello stock e associazione dei prodotti ai rispettivi magazzini.
*   **Gestione Avanzata delle Immagini:** Modulo per l'upload dei file che include validazione lato server, **ritaglio dinamico (crop)**, ridimensionamento e applicazione di **watermark** per la protezione dei contenuti.
*   **Interazioni Asincrone:** Utilizzo di chiamate AJAX per l'aggiornamento dinamico dell'interfaccia senza ricaricare la pagina.

## 🚧 Stato del Progetto e Work in Progress
Il progetto è attualmente in fase di sviluppo attivo. Il core backend (gestione anagrafiche, database relazionale e movimenti di magazzino) è funzionante e testato.

**Cose su cui sto lavorando (Roadmap e Known Issues):**
*   **Refactoring Architetturale (Paginazione e Ordinamento):** Attualmente la paginazione dei dati avviene lato server (per ottimizzare le performance), mentre l'ordinamento e parte dei filtri sono gestiti lato client (JavaScript). Questo approccio causa un limite: l'ordinamento agisce solo sui record della pagina corrente e non sull'intero dataset. Il prossimo passo prevede lo spostamento della logica di sorting e filtering direttamente nelle Stored Procedure SQL, passando i parametri dal server.
*   **Miglioramento UI/UX:** Revisione dell'interfaccia per rendere più fluide le chiamate AJAX e la gestione degli stati di caricamento.
*   **Testing:** Implementazione di test di integrazione per le chiamate ai controller.

## 🧠 Metodologia di Sviluppo e uso dell'AI
Durante lo sviluppo di questo progetto, ho limitato volontariamente l'uso dell'Intelligenza Artificiale al minimo indispensabile. Il mio obiettivo era imparare a programmare davvero.

*   **Uso per il Boilerplate:** Ho utilizzato l'AI per generare codice ripetitivo o di base (es. classi di configurazione o markup HTML), come se fosse una scorciatoia per il copia-incolla. 
*   **Sviluppo Core e Problem Solving:** Per le cose complesse (come l'implementazione dell'image processing, le query SQL e la logica di magazzino) mi sono divertito a cercare la soluzione e scrivere il codice da solo. 
*   **Debugging:** Ho risolto tutti i bug per conto mio, sfruttando l'AI solo come "tutor" per spiegarmi concetti teorici o darmi una mano a leggere la documentazione ufficiale. Ogni riga di logica in questo repository è frutto del mio studio e della mia sperimentazione.

## 👨‍💻 Autore
**Francesco D'Arcangeli**
