using CsvHelper;
using iLeadsTest.Mappers;
using iLeadsTest.ViewModels;
using Repository;
using Repository.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
            string tempPath = Path.GetTempPath();

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

                                //Save the file to use again in tempdata
                                var guid = Guid.NewGuid();
                                var myTempFile = Path.Combine(Path.GetTempPath(), guid + fileName);
                                file.SaveAs(myTempFile);

                                //Create a ViewModel to send to my view
                                var viewModel = new SaveViewModel() { FileName = fileName, FileClasses = null, Headers = headers, Properties = properties, ClientsCount = linesCount, FileId = guid };
                                var json = new JavaScriptSerializer().Serialize(viewModel);

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
            var path = Path.Combine(Path.GetTempPath(), model.FileId .ToString() + model.FileName);

            //Get the file from session
            using (CsvReader reader = new CsvReader(new StreamReader(path)))
            {
                //variable to count how many Clients has been saved
                var i = 1;
                while (reader.Read())
                {
                    //Init new client
                    var newClient = new Client() { FileName = model.FileName };

                    //Type to use reflection in order to set the properties
                    var type = newClient.GetType();

                    //Loop trou the mapping config provided by the user
                    foreach (var mapping in model.HeadersMapper) 
                    {
                        var value = reader.GetField(mapping.Header);
                        var prop = type.GetProperty(mapping.Property);
                        prop.SetValue(newClient, value, null);
                    }
                    try
                    {
                        //Save the client
                        _unitOfWork.ClientRepository.Insert(newClient);
                        _unitOfWork.Save();
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Theres been a problem:" + e.Message);
                    }
                    i++;
                }
                var json = new JavaScriptSerializer().Serialize(i + " clients have been saved!");
                return json; 
            }            
        }
    }
}
