using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using LoggerAPI.Database;

namespace LoggerAPI.Controllers
{
    public class LogPayloadsController : ApiController
    {
        private LoggingEntities db = new LoggingEntities();

        // GET: api/LogPayloads
        public IQueryable<LogPayload> GetLogPayloads()
        {
            return db.LogPayloads;
        }

        // GET: api/LogPayloads/5
        [ResponseType(typeof(LogPayload))]
        public IHttpActionResult GetLogPayload(int id)
        {
            LogPayload logPayload = db.LogPayloads.Find(id);
            if (logPayload == null)
            {
                return NotFound();
            }

            return Ok(logPayload);
        }

        // PUT: api/LogPayloads/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutLogPayload(int id, LogPayload logPayload)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != logPayload.PayloadId)
            {
                return BadRequest();
            }

            db.Entry(logPayload).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LogPayloadExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/LogPayloads
        [ResponseType(typeof(LogPayload))]
        public IHttpActionResult PostLogPayload(LogPayload logPayload)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.LogPayloads.Add(logPayload);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = logPayload.PayloadId }, logPayload);
        }

        // DELETE: api/LogPayloads/5
        [ResponseType(typeof(LogPayload))]
        public IHttpActionResult DeleteLogPayload(int id)
        {
            LogPayload logPayload = db.LogPayloads.Find(id);
            if (logPayload == null)
            {
                return NotFound();
            }

            db.LogPayloads.Remove(logPayload);
            db.SaveChanges();

            return Ok(logPayload);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LogPayloadExists(int id)
        {
            return db.LogPayloads.Count(e => e.PayloadId == id) > 0;
        }
    }
}