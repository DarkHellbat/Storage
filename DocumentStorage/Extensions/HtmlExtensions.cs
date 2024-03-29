﻿using DocumentStorage.Models;
using DocumentStorage.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Web.UI.WebControls;

namespace DocumentStorage.Extensions
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString SortLink(this HtmlHelper html,
           string linkText,
           string sortExpression,
           string actionName,
           string controllerName, RouteValueDictionary routeValues)
        {
            SortDirection? sort = null;
            var sortDirectionStr = html.ViewContext.HttpContext.Request["SortDirection"];
            if (!string.IsNullOrEmpty(sortDirectionStr)
                && html.ViewContext.HttpContext.Request["SortExpression"] == sortExpression)
            {
                SortDirection s;
                if (Enum.TryParse(sortDirectionStr, out s))
                {
                    sort = s;
                }
            }
            routeValues["SortExpression"] = sortExpression;
            routeValues["SortDirection"] = sort.HasValue && sort.Value == SortDirection.Ascending ?
                SortDirection.Descending : SortDirection.Ascending;
            return html.Partial("SortLink", new SortLinkModel
            {
                ActionName = actionName,
                ControllerName = controllerName,
                SortDirection = sort,
                RouteValues = routeValues,
                LinkText = linkText
            });
        }
        public static bool IsImage(this HtmlHelper html, File file)
        {
            if (string.IsNullOrEmpty(file.Type))
            {
                return false;
            }
            return file.Type.Contains("bmp") ||
                file.Type.Contains("jpeg") ||
                file.Type.Contains("png");
        }
    }
}