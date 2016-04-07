using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Model
{
    public class Client
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string PinNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string FileName { get; set; }


        public Client() 
        {
            CreateDate = DateTime.Now;
        }

        public string[] GetPropertiesToMap() 
        {
            return new string[] { "PinNumber", "FirstName", "LastName", "StreetAddress", "City", "State", "ZipCode" };
        }
    }
}
