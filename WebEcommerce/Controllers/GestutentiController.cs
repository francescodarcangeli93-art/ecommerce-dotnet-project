using BLL_Ecommerce.Interfaaces;
using BLL_Ecommerce.Interfaces;
using BLL_Ecommerce.Services;
using DTO_Ecommerce;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Services.Description;
using WebEcommerce.Models;

namespace WebEcommerce.Controllers
{   
    public class GestutentiController : Controller
    {   
        private readonly IUserService _userservices;
        private readonly ICustomerService _customerservices;
        private readonly IRoleService _roleservices;
        private readonly ILogService _logService;
        public GestutentiController ()
        {
            _logService = new LogService ();
            _userservices =  new UserServices ();
            _customerservices = new CustomerServices ();
            _roleservices = new RoleServices();
        }


        // GET: Gestutenti
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public ActionResult ClearPage()
        {
            
            //Session.Clear();
            GestioneUtentiViewModel model = new GestioneUtentiViewModel ();
            model.Filtri = null;
            model.ListaUtenti = null;
            return RedirectToAction("GestioneUtenti");
        }

        [HttpGet]
        public ActionResult GestioneUtenti(int? Id)
        {
            sp_SearchUser_Param param = new sp_SearchUser_Param ();

            //se scade la sessione va al login
            if (Session["Utente"] == null)
            {
                RedirectToAction("Index", "Login");
            }

            int idUser = 0;

            try
            {
                idUser = (int)Session["UserId"];

                // carico il model nella view
                GestioneUtentiViewModel model = new GestioneUtentiViewModel
                {
                
                    ListRuoli = _roleservices.UserRoleList(),
                    ListaUtenti = _userservices.SearchUser(param)

                };

                 ViewData["Id"] = Id;
               
                return View(model);
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return Json(new { success = false });
            }
        }

        //  Gestione utenti 

        [HttpPost]
        public ActionResult GestioneUtenti(sp_SearchUser_Param valori)
        {
            

            if (Session["Role"] == null || Session["Role"].ToString().ToLower() != "admin")
            {
                return View("Error");
            }

            int idUser = 0;
            // istanziare l'oggetto UserServicer che contiene tutti gli oggetti ricevuti dalla proc
            try
            {
                idUser = (int)Session["UserId"];

                GestioneUtentiViewModel model = new GestioneUtentiViewModel
                {
                    Filtri = valori,
                    ListaUtenti = _userservices.SearchUser(valori),
                    ListRuoli = _roleservices.UserRoleList()
                };

                ViewData["Id"] = valori.Id;
                return View(model);
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return View("Error");
            }
        }

        [HttpGet]
        public ActionResult DatiUtente(int id, int? CustomerId)
        {
            
            if (Session["Role"] == null || Session["Role"].ToString().ToLower() != "admin")
            {
                return View("Error");
            }

            int idUser = 0;

            try
            {
                idUser = (int)Session["UserId"];
                GestioneUtentiViewModel model = new GestioneUtentiViewModel
                {
                    Id = id,
                    CustomerId = CustomerId,
                    ListRuoli = _roleservices.UserRoleList(),
                    UserDetail = _userservices.GetUserById(id)
                };

               // sp_GetUserById_Result utente = _userservices.GetUserById(id);


                if (model.UserDetail == null)
                {
                    return View("Error");
                }

                return View(model);
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return Json(new { success = false });
            }
        }

        [HttpGet]
        public ActionResult ModificaUtente(int Id)
        {
            int idUser = 0;

            try
            {
                idUser = (int)Session["UserId"];
                sp_GetUserById_Result utente = _userservices.GetUserById(Id);

                if (utente == null)
                {
                    return View("Error");
                }

                GestioneUtentiViewModel model = new GestioneUtentiViewModel
                {
                    ListRuoli = _roleservices.UserRoleList(),
                    Id = utente.Id,
                    RoleId = utente.RoleId,
                    RoleName = utente.RoleName,
                    Username = utente.Username,
                    Email = utente.Email,
                    Password = utente.Password,
                    CreatedDate = utente.CreatedDate,
                    IsActive = utente.IsActive,

                    CustomerId = utente.CustomerId,
                    FirstName = utente.FirstName,
                    LastName = utente.LastName,
                    City = utente.City,
                    Country = utente.Country,
                    Phone = utente.Phone

                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public ActionResult ModificaUtente(GestioneUtentiViewModel model)
        {

            int idUser = 0;

            if (Session["Role"] == null || Session["Role"].ToString().ToLower() != "admin")
            {
                return View("Error");
            }


            model.ListRuoli = _roleservices.UserRoleList();

      
            // recupero l'utente dal database 
            sp_GetUserById_Result user = _userservices.GetUserById(model.Id);
            if (user == null)
            {
                TempData["Errore"] = "Impossibile aggiornare i dati: utente non trovato.";
                return View(model);
            }

            // prendiamo il customer id
            int? CustomerId = 0;
            
            try
            {
                idUser = (int)Session["UserId"];

                CustomerId = user.CustomerId;

                // CASO 1 in cui bisogna settare il customer o un ADMIN-CUSTOMER  ad admin  
                // fare un operazione di delete nella tabella customer 
                if (model.RoleId == 1 && CustomerId != null)
                {
                    // fare un update sull'CustomerID della tabella User
                    sp_ManageUser_Param updateCustomerID = new sp_ManageUser_Param
                    {
                        Operation = "U",
                        Id = user.Id,
                        RoleId = model.RoleId,
                        Username = model.Username,
                        Email = model.Email,
                        Password = model.Password,
                        IsActive = model.IsActive,
                        CustomerId = null
                    };

                    // eseguo l'update
                    _userservices.sp_ManageUser(updateCustomerID);


                    sp_ManageCustomer_Param delete = new sp_ManageCustomer_Param
                    {
                        Operation = "D",
                        Id = CustomerId
                    };

                    _customerservices.sp_ManageCustomer(delete);

                    TempData["Successo"] = "Utente modificato con successo!";

                    return RedirectToAction("ModificaUtente", new { Id = model.Id });
                }

                // CASO 2
                // creo la classe param per salvare i dati che arrivano dal form della sezione customer
                sp_ManageCustomer_Param spCustomerParam = new sp_ManageCustomer_Param
                {

                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    City = model.City,
                    Country = model.Country,
                    Phone = model.Phone,

                };

                // se il customer id è nullo vuol dire che è un Admin 
                if (CustomerId == null)
                {
                    spCustomerParam.Operation = "I";
                   // fare una insert nella tabella customer  e recuper l'id

                    // eseguo la insert 
                   CustomerId = _customerservices.sp_ManageCustomer(spCustomerParam);

                    // e modificare il customerid nella tabella user dell'utente
                    sp_ManageUser_Param edit = new sp_ManageUser_Param
                    {
                        Operation = "U",
                        Id = user.Id,
                        RoleId = model.RoleId,
                        Username = model.Username,
                        Email = model.Email,
                        Password = model.Password,
                        IsActive = model.IsActive,
                        CustomerId = CustomerId
                       
                    };

                    // eseguire l'update
                    _userservices.sp_ManageUser(edit);

                   
                }
                // se invece è ha un CustomerId  non null  deve fare un apdate o alla tabella customer o all user
                else
                {    
                    // inserisco i dati nell'user
                    sp_ManageUser_Param mangeUser = new sp_ManageUser_Param
                    {
                        Operation = "U",
                        Id = model.Id,
                        RoleId = model.RoleId,
                        Username = model.Username,
                        Email = model.Email,
                        Password = model.Password,
                        CustomerId = CustomerId,
                        IsActive = model.IsActive,
                    };

                    _userservices.sp_ManageUser(mangeUser);

                    // poi i dati nella tabella customer
                     spCustomerParam.Operation = "U";
                     spCustomerParam.Id = CustomerId;
                    _customerservices.sp_ManageCustomer(spCustomerParam);

                 
                    
                }
            }

            // aggiornare  i filtri  da mettere nel model 
            catch (Exception ex)
            {
                ViewData["Errore"] = ex.Message;
                model.ListRuoli = _roleservices.UserRoleList();

                _logService.WriteLogProcessError(idUser, ex.Message);
                return View(model);
            }

            TempData["Successo"] = "Utente modificato con successo!";

            return  RedirectToAction("ModificaUtente", new { Id = model.Id });

        }

        [HttpGet]
        public ActionResult EliminaUtente (int? id)
        {   

           // return RedirectToAction("GestioneUtenti", "Gestutenti");

           return View();
        }



        [HttpPost]
        public ActionResult CambiaStatoInTabella(int id, bool? isActive, int? customerId)
        {
            int idUser = 0;

            try
            {
                idUser =(int)Session["UserId"];
                _userservices.sp_ManageUser(new sp_ManageUser_Param
                {
                    Operation = "U",
                    Id = id,
                    IsActive = isActive,
                    CustomerId = customerId
                });

               

                TempData["Successo"] = "Stato utente aggiornato con successo.";
            }
            catch (Exception ex)
            {

                _logService.WriteLogProcessError(idUser, ex.Message);

                TempData["MessaggioErrore"] = "Errore durante il cambio stato: " + ex.Message;
            }

            // 5. Ricarica la pagina di gestione utenti mostrando i nuovi dati aggiornati
            return RedirectToAction("GestioneUtenti");
        }



        // AGGIUNGI UTENTE 

        [HttpGet]
        public ActionResult AggiungiUtente()
        {

            int? idUser = Session["UserId"] != null ? (int?)Session["UserId"] : null;

            try
            {

                // Carichiamo i ruoli così la dropdown nel form non è vuota
                GestioneUtentiViewModel model = new GestioneUtentiViewModel();

                RoleServices role = new RoleServices();
                model.ListRuoli = role.UserRoleList();

                return View(model);
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return View("Error");
            }
        }
        
        [HttpPost] 
        public ActionResult AggiungiUtente(GestioneUtentiViewModel model)
        {
            int idUser = 0;

            //  istanziamo il role model 
            RoleServices role = new RoleServices();
            // riempiamo il model  per i ruoli 
            model.ListRuoli = role.UserRoleList();

            try
            {
                idUser = (int)Session["UserId"];
                int? customerId = null;

                if (model.RoleId != 1)
                {
                    
                    // 1. Salvo il Cliente e prendo l'ID 
                    sp_ManageCustomer_Param cliente = new sp_ManageCustomer_Param
                    {
                        Operation = "I",
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        City = model.City,
                        Country = model.Country,
                        Phone = model.Phone
                    };

                    customerId = _customerservices.sp_ManageCustomer(cliente);

                }

               
                sp_ManageUser_Param userParam = new sp_ManageUser_Param
                {
                    Operation = "I",
                    Username = model.Username,
                    Email = model.Email,
                    Password = model.Password,
                    RoleId = model.RoleId,
                    CustomerId = customerId, 
                    IsActive = true,
                   
                };


                _userservices.sp_ManageUser(userParam);

                ViewBag.Successo = "Utente aggiunto con successo!";

          
                // aggiornare la sessione
                if (Session["Filtri"] != null)
                {
                    sp_SearchUser_Param filtri = Session["Filtri"] as sp_SearchUser_Param;
                    List < sp_SearchUser_Result > listaAggiornata = _userservices.SearchUser(filtri);
                    Session["ListaValori"] = listaAggiornata;
                }
                else
                {
                    Session.Remove("ListaValori");
                }

                return View(model);
            }
            catch (Exception ex)
            {
                
                model.ListRuoli = role.UserRoleList();

                ViewBag.Errore = "Errore durante il salvataggio: " + ex.Message;
          
                return View(model); // Ritorna alla view col modello per mostrare l'errore
            }
        }


        //[HttpGet]
        //public ActionResult EliminaUtente(int? id)
        //{

        //     return RedirectToAction("GestioneUtenti", "Gestutenti");
        //    return View();
        //}


        [HttpPost]
        public ActionResult EliminaUtente(int Id)
        {
            int? idUser = Session["UserId"] != null ? (int?)Session["UserId"] : null;

            try
            {
                // Impedisci l'eliminazione del proprio account
                if (Session["UserId"] != null && (int)Session["UserId"] == Id)
                {
                    TempData["Errore"] = "Non puoi eliminare il tuo account.";
                    return RedirectToAction("GestioneUtenti");
                }


                // ritirare l'utente dal database e salvarlo dentro un oggetto
                  sp_GetUserById_Result user  = _userservices.GetUserById(Id);

                // verificare che l'utente esista
                if(user == null)
                {
                    TempData["Errore"] = "Utente non trovato";

                  

                    return RedirectToAction("GestioneUtenti");
                }
                //  salvare il customerId dentro una variabile
                int? CustomerId = user.CustomerId;

                // eliminiare  prima l'utente 
                sp_ManageUser_Param userParam = new sp_ManageUser_Param
                {
                    Operation = "D",// cancella
                    Id = Id
                };

                // richiamo il service  per cancellare 
                _userservices.sp_ManageUser(userParam);

                // se il customer id non è null quindi non è vuoto 
                if (CustomerId != null)
                {
                    sp_ManageCustomer_Param customerParam = new sp_ManageCustomer_Param
                    {
                        Operation = "D",
                        Id = CustomerId
                    };

                    // eliminare i dati dalla tabella customer
                    _customerservices.sp_ManageCustomer(customerParam);
                }

                // se tutto è andato bene arriva qui 
                TempData["Successo"] = "Utente eliminato con successo.";
                return RedirectToAction("GestioneUtenti");
            }
            catch (Exception ex)
            {
                TempData["Errore"] = ex.Message;

                _logService.WriteLogProcessError(idUser, ex.Message);
                return RedirectToAction("GestioneUtenti");
            }
        }

  
    }
}