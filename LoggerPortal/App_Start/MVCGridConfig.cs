[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(LoggerPortal.MVCGridConfig), "RegisterGrids")]

namespace LoggerPortal
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Linq;
    using System.Collections.Generic;

    using MVCGrid.Models;
    using MVCGrid.Web;
    using LoggerPortal.Models;

    public static class MVCGridConfig
    {
        public static void RegisterGrids()
        {
            MVCGridDefinitionTable.Add("LogsDetailGrid", new MVCGridBuilder<LogsDetail>()
            .WithAuthorizationType(AuthorizationType.AllowAnonymous)
            .AddColumns(cols =>
            {
                cols.Add("LogID").WithVisibility(false)
                    .WithSorting(false)
                    .WithFiltering(false)
                    .WithValueExpression(p => p.LogId.ToString());
                cols.Add("ClientId").WithVisibility(false)
                    .WithValueExpression(p => p.ClientId.ToString())
                    .WithFiltering(true);
                cols.Add("ApplicationId").WithVisibility(false)
                    .WithValueExpression(p => p.ApplicationId.ToString())
                    .WithFiltering(true);
                cols.Add("LogTypeId").WithVisibility(false)
                    .WithValueExpression(p => p.LogType.ToString())
                    .WithFiltering(true);
                cols.Add("ClientName").WithHeaderText("Client")
                    .WithValueExpression(p => p.ClientName)
                    .WithFiltering(true)
                    .WithSorting(true);
                cols.Add("ApplicationName").WithHeaderText("Application")
                    .WithValueExpression(p => p.ApplicationName)
                    .WithFiltering(true)
                    .WithSorting(true);
                cols.Add("LogTypeName").WithHeaderText("Log Type")
                    .WithValueExpression(p => p.LogTypeName)
                    .WithFiltering(true)
                    .WithSorting(true);
                cols.Add("LogDateTime").WithHeaderText("Log DateTime")
                    .WithValueExpression(p => p.LogDateTime.ToString())
                    .WithFiltering(true)
                    .WithSorting(true);
                cols.Add("ViewLink").WithSorting(false)
                    .WithHeaderText("")
                    .WithHtmlEncoding(false)
                    .WithValueExpression(p => p.LogId.ToString())
                    .WithValueTemplate("<a id='{Value}' class='LogsDetailGridClass' href='javascript:;'>View</a>");
                //.WithValueTemplate("<a id=" + p.LogId + "class='LogsDetailGridClass' href='javascript:;'>View</a>");
                //.WithValueExpression((p, c) => c.UrlHelper.Action("LogDetail", "Logs", new { id = p.LogId }))
                //.WithValueTemplate("<a href='{Value}'>View</a>");
            })
            .WithSorting(true, "LogDateTime", SortDirection.Dsc)
            .WithPaging(true, 10, true, 50)
            .WithFiltering(true)
            //.WithQueryOnPageLoad(false)
            //.WithPreloadData(false)
            .WithRetrieveDataMethod((context) =>
            {
                var options = context.QueryOptions;
                int totalRecords;
                var repo = new LogsDetailRepository();
                string sortColumn = options.GetSortColumnData<string>();
                var items = repo.GetData(out totalRecords,
                    options.GetFilterString("ClientId"),
                    options.GetFilterString("ApplicationId"),
                    options.GetFilterString("LogTypeId"),
                    options.GetLimitOffset(), options.GetLimitRowcount(),
                    sortColumn, options.SortDirection == SortDirection.Dsc);
                return new QueryResult<LogsDetail>()
                {
                    Items = items,
                    TotalRecords = totalRecords
                };
            }));
        }
    }
}