using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LoggerPortal.Models;

namespace LoggerPortal.Controllers
{
    public class LogsController : Controller
    {
        Logger_Entities _db;
        public LogsController()
        {
            _db = new Logger_Entities();
        }

        public ActionResult Logs()
        {
            var Clients = from c in _db.Clients
                          select new
                          {
                              ClientId = c.ClientId,
                              ClientName = c.ClientName
                          };

            SelectList listClients = new SelectList(Clients, "ClientId", "ClientName");
            ViewBag.Clients = listClients;

            var Applications = from a in _db.Applications
                               select new
                               {
                                   ApplicationId = a.ApplicationId,
                                   ApplicationName = a.ApplicationName
                               };

            SelectList listApplicationName = new SelectList(Applications, "ApplicationId", "ApplicationName");
            ViewBag.Applications = listApplicationName;

            var LogTypes = from l in _db.LogTypes
                               select new
                               {
                                   LogTypeId = l.LogTypeId,
                                   LogTypeName = l.LogTypeName
                               };

            SelectList listLogTypes = new SelectList(LogTypes, "LogTypeId", "LogTypeName");
            ViewBag.LogTypes = listLogTypes;

            return View();
        }

        [HttpPost]
        public ActionResult LogDetail(string logId)
        {
            return PartialView("LogDetail", _db.LogPayloads.Where(p => p.LogId.ToString() == logId).FirstOrDefault());
        }
    }
}