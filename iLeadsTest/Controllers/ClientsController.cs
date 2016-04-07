using CsvHelper;
using iLeadsTest.Mappers;
using iLeadsTest.ViewModels;
using Repository;
using Repository.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace iLeadsTest.Controllers
{
    public class ClientsController : Controller
    {
        private UnitOfWork _unitOfWork;
        private Mapper _mapper;
        private List<CsvFileMapper> _dataFromFile;
      
        public ClientsController() 
        {
            _unitOfWork = new UnitOfWork();
            _mapper = new Mapper();
            _dataFromFile = new List<CsvFileMapper>();
        }

        public ActionResult Index()
        {
            //Get all the clients
            var clients = _unitOfWork.ClientRepository.GetAll().ToList();
            var viewModels = new List<ClientListViewModel>();

            //Mapped them to ViewModels
            foreach (var client in clients) 
            {
                var viewModel = _mapper.Map(client);
                viewModels.Add(viewModel);

            }
            return View(viewModels);
        }

        [HttpPost]
        public string PreUpload()
        {
            string result = Path.GetTempPath();

            //Check if request has files
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                //Check if files have values
                if (file != null && file.ContentLength > 0)
                {
                    if (file.ContentType.Equals("application/vnd.ms-excel"))
                    {
                        try
                        {
                            //Get the file
                            var fileName = Path.GetFileName(file.FileName);
                            //Process the file using CsvHelper
                            using (var sr = new StreamReader(file.InputStream))
                            {                                
                                //Initialize the CsvReader
                                var reader = new CsvReader(sr);
                                reader.Read();
                                
                                //Get the Headers
                                var headers = reader.FieldHeaders;

                                //Get the properties
                                var client = new Client();
                                var properties = client.GetPropertiesToMap();

                                //get how many lines does the file has to inform the user
                                sr.DiscardBufferedData();
                                sr.BaseStream.Seek(0, System.IO.SeekOrigin.Begin); 
                                var lines = sr.ReadToEnd().Split(new char[] { '\n' });
                                var linesCount = lines.Count() - 1;

                                //Create a ViewModel to send to my view
                                var viewModel = new SaveViewModel() { FileName = fileName, FileClasses = null, Headers = headers, Properties = properties, ClientsCount = linesCount };
                                var json = new JavaScriptSerializer().Serialize(viewModel);
                                //Session["file"] = file.InputStream.
                                //Return my viewModel in a json
                                return json;
                            }
                        }
                        catch (Exception e)
                        {
                            throw new Exception("There is been a problem:" + e.Message);
                        }
                    }
                    else 
                    {
                        throw new Exception("Format not supported.");
                    }
                }
                else 
                {
                    throw new Exception("File is empty.");
                }
            }
            else 
            {
                throw new Exception("There is no file.");
            }
        }

        [HttpPost]
        public string Save(SaveViewModel model) 
        {
            //Get the file from session
            var file = Session["file"] as string;
            var hola = 1;
            //using (var sr = new StreamReader(file.InputStream))
            //{
            //    var reader = new CsvReader(sr);
            //    while(reader.Read())
            //    {
            //        var algo = reader.GetField("PIN Number", reader.Row);
            //    }
            //    var stringField = reader.;
            //    var dataFromFile = reader.GetRecords<CsvFileMapper>().ToList();

            //}

            try
            {
                foreach (var viewModel in model.FileClasses)
                {
                    var client = _mapper.Map(viewModel);
                    client.FileName = model.FileName;
                    _unitOfWork.ClientRepository.Insert(client);
                    _unitOfWork.Save();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Theres been a problem:" + e.Message);
            }
            var json = new JavaScriptSerializer().Serialize(model.FileClasses.Count + " clients have been saved!");
            return json;
        }
    }
}
