using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NameSorter.Models;
using NameSorter.Repository.Interface;
using NameSorter.ViewModel;

namespace NameSorter.Controllers
{
    /// <summary>
    /// Author: Francis Decena 
    /// Date 15/8/2020
    /// Description: This will call the TextFileRepository and NameSortRepository. 
    /// To process the uploaded text file and sort the names base on the uploaded files. 
    /// </summary>
    public class TextFileController : Controller
    {
        #region Properties
        private readonly ITextFileRepository _textFileRepository;
        private readonly INameSortRepository _nameSortRepository;
        #endregion

        #region Constructor
        public TextFileController(ITextFileRepository textFileRepository, INameSortRepository nameSortRepository)
        {
            _textFileRepository = textFileRepository;
            _nameSortRepository = nameSortRepository;
        }
        #endregion

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ReadUploadFile(SortViewModel sortViewModel)
        {
            try
            {
                //instantiate List NamesModel
                var namesModels = new List<NamesModel>();

                //create object List of string
                var genericList = new List<string>(); 

                if (ModelState.IsValid)
                {
                    //call the textfilerespository then return the List<NameModel>
                    genericList = _textFileRepository.ProcessUploadFile(sortViewModel.TextFile);

                    //call the nameSortRepository to process the genericList 
                    namesModels = _nameSortRepository.sortGivenName(genericList);
                }
                else
                {
                    return View("Index");
                }

                //assigned the value to NamesModels property 
                sortViewModel.NamesModels = namesModels;
                return View("ReadUploadFile", sortViewModel);
            }
            catch (Exception ex)
            {
                ErrorModel errorModel = new ErrorModel
                {
                    ErrorTitle = "System Error!",
                    ErrorMessage = string.Format("We have encountered an Error. \nPlease contact your System Administrator. \n Error: {0}",  ex.Message),
                    RedirectAction = "Index",
                    RedirectController = "TextFile"
                };

                return RedirectToAction("Index", "Error", errorModel);
            }
        }
    }
}
