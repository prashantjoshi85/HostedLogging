using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoggerPortal.Models
{
    public interface ILogsDetailRepository
    {
        IEnumerable<LogsDetail> GetData(out int totalRecords, string globalSearch, int? limitOffset, int? limitRowCount, string orderBy, bool desc);
        IEnumerable<LogsDetail> GetData(out int totalRecords, int? limitOffset, int? limitRowCount, string orderBy, bool desc);
        IEnumerable<LogsDetail> GetData(out int totalRecords, string filterClientId, string filterApplicationId, int? limitOffset, int? limitRowCount, string orderBy, bool desc);
    }
    public class LogsDetailRepository : ILogsDetailRepository
    {
        public IEnumerable<LogsDetail> GetData(out int totalRecords, string filterClientId, string filterApplicationId, int? limitOffset, int? limitRowCount, string orderBy, bool desc)
        {
            return GetData(out totalRecords, null, filterClientId, filterApplicationId, limitOffset, limitRowCount, orderBy, desc);
        }

        public IEnumerable<LogsDetail> GetData(out int totalRecords, string globalSearch, int? limitOffset, int? limitRowCount, string orderBy, bool desc)
        {
            return GetData(out totalRecords, globalSearch, null, null, limitOffset, limitRowCount, orderBy, desc);
        }

        public IEnumerable<LogsDetail> GetData(out int totalRecords, string globalSearch, string filterClientId, string filterApplicationId, int? limitOffset, int? limitRowCount, string orderBy, bool desc)
        {
            using (var db = new Logger_Entities())
            {
                var query = db.LogsDetails.AsQueryable();

                if (!String.IsNullOrWhiteSpace(filterClientId))
                {
                    query = query.Where(p => p.ClientId.ToString() == filterClientId);
                }
                if (!String.IsNullOrWhiteSpace(filterApplicationId))
                {
                    query = query.Where(p => p.ApplicationId.ToString() == filterApplicationId);
                }

                if (!String.IsNullOrWhiteSpace(globalSearch))
                {
                    query = query.Where(p => (p.ClientId + " " + p.ApplicationId).Contains(globalSearch));
                }

                totalRecords = query.Count();

                if (!String.IsNullOrWhiteSpace(orderBy))
                {
                    switch (orderBy)
                    {
                        case "ClientName":
                            if (!desc)
                                query = query.OrderBy(p => p.ClientName);
                            else
                                query = query.OrderByDescending(p => p.ClientName);
                            break;
                        case "ApplicationName":
                            if (!desc)
                                query = query.OrderBy(p => p.ApplicationName);
                            else
                                query = query.OrderByDescending(p => p.ApplicationName);
                            break;
                        case "LogTypeName":
                            if (!desc)
                                query = query.OrderBy(p => p.LogTypeName);
                            else
                                query = query.OrderByDescending(p => p.LogTypeName);
                            break;
                        case "LogDateTime":
                            if (!desc)
                                query = query.OrderBy(p => p.LogDateTime);
                            else
                                query = query.OrderByDescending(p => p.LogDateTime);
                            break;
                    }
                }

                if (limitOffset.HasValue)
                {
                    query = query.Skip(limitOffset.Value).Take(limitRowCount.Value);
                }

                return query.ToList();
            }
        }

        public IEnumerable<LogsDetail> GetData(out int totalRecords, int? limitOffset, int? limitRowCount, string orderBy, bool desc)
        {
            return GetData(out totalRecords, null, null, null, limitOffset, limitRowCount, orderBy, desc);
        }
    }
}