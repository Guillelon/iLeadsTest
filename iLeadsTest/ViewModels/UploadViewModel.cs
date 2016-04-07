using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iLeadsTest.ViewModels
{
    public class UploadViewModel
    {
        public HttpPostedFileBase MyFile { get; set; }
    }

    public class HeaderMappingViewModel 
    {
        public string Header { get; set; }
        public string Property { get; set; }
    }

    public class SaveViewModel 
    {
        public string FileName { get; set; }
        public List<CsvFileMapper> FileClasses { get; set; }
        public string[] Properties { get; set; }
        public string[] Headers { get; set; }
        public int ClientsCount { get; set; }

        public List<HeaderMappingViewModel> HeadersMapper { get; set; }

        public SaveViewModel() 
        {
            HeadersMapper = new List<HeaderMappingViewModel>();
        }
    }    
}