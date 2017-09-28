﻿using System;
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
    public class LogEntriesController : ApiController
    {
        private LoggingEntities db = new LoggingEntities();

        // GET: api/LogEntries
        public IQueryable<LogEntry> GetLogEntries()
        {
            return db.LogEntries;
        }

        // GET: api/LogEntries/5
        [ResponseType(typeof(LogEntry))]
        public IHttpActionResult GetLogEntry(int id)
        {
            LogEntry logEntry = db.LogEntries.Find(id);
            if (logEntry == null)
            {
                return NotFound();
            }

            return Ok(logEntry);
        }

        // PUT: api/LogEntries/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutLogEntry(int id, LogEntry logEntry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != logEntry.LogId)
            {
                return BadRequest();
            }

            db.Entry(logEntry).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LogEntryExists(id))
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

        // POST: api/LogEntries
        [ResponseType(typeof(LogEntry))]
        public IHttpActionResult PostLogEntry(LogEntry logEntry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.LogEntries.Add(logEntry);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = logEntry.LogId }, logEntry.LogId  );
        }

        // DELETE: api/LogEntries/5
        [ResponseType(typeof(LogEntry))]
        public IHttpActionResult DeleteLogEntry(int id)
        {
            LogEntry logEntry = db.LogEntries.Find(id);
            if (logEntry == null)
            {
                return NotFound();
            }

            db.LogEntries.Remove(logEntry);
            db.SaveChanges();

            return Ok(logEntry);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LogEntryExists(int id)
        {
            return db.LogEntries.Count(e => e.LogId == id) > 0;
        }
    }
}