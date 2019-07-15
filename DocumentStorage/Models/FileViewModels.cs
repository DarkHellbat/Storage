using DocumentStorage.Models.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.UI.WebControls;

namespace DocumentStorage.Models
{
    public class FileListViewModel
    {
        public IList<File> File { get; set; }
        public FileListViewModel()
        {
            File = new List<File>();
        }
    }
    public class FileCreationViewModel
    {
        public string Name { get; set; }
        public HttpPostedFileBase File { get; set; }
    }
    public class SortLinkModel
    {
        public string LinkText { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public RouteValueDictionary RouteValues { get; set; }
        public SortDirection? SortDirection { get; set; }
    }
    public class FilterViewModel
    {
        [Display(Name = "Имя файла")]
        public string Name { get; set; }
        [DataType(DataType.DateTime)]
       // [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Дата создания")]
        public DateTime CreationDate { get; set; }
        public DateTime Today { get; set; }
        public FilterViewModel()
        {
            Today = DateTime.Today;
        }
    }
}