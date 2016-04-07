using iLeadsTest.ViewModels;
using Repository.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iLeadsTest.Mappers
{
    public class Mapper
    {
        /// <summary>
        /// Map a model to a ViewModel 
        /// </summary>
        /// <param name="model">the Model to map</param>
        /// <returns>The viewmodel</returns>
        public ClientListViewModel Map(Client model) 
        {
            var viewModel = new ClientListViewModel();
            viewModel.FirstName = model.FirstName;
            viewModel.LastName = model.LastName;
            viewModel.PinNumber = model.PinNumber;
            viewModel.StreetAddress = model.StreetAddress;
            viewModel.City = model.City;
            viewModel.State = model.State;
            viewModel.ZipCode = model.ZipCode;
            return viewModel;
        }

        /// <summary>
        /// Map a ViewModel to a model
        /// </summary>
        /// <param name="viewModel">the ViewModel to map</param>
        /// <returns>The model</returns>
        public Client Map(CsvFileMapper viewModel) 
        {
            var client = new Client();
            client.PinNumber = viewModel.PinNumberProperty;
            client.FirstName = viewModel.FirstNameProperty;
            client.LastName = viewModel.LastNameProperty;
            client.City = viewModel.CityProperty;
            client.State = viewModel.StateProperty;
            client.StreetAddress = viewModel.StreetAddressProperty;
            client.ZipCode = viewModel.ZipCodeProperty;
            return client;
        }
    }
}