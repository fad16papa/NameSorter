using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using NameSorter.Helper;
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
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<TextFileController> _logger;
        #endregion

        #region Constructor
        public TextFileController(ITextFileRepository textFileRepository, INameSortRepository nameSortRepository, IMemoryCache memoryCache, ILogger<TextFileController> logger)
        {
            _textFileRepository = textFileRepository;
            _nameSortRepository = nameSortRepository;
            _memoryCache = memoryCache;
            _logger = logger;
        }
        #endregion

        /// <summary>
        /// This will return the index page of TextFile
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// This will process the uploaded file then sort it by last name ascending and display it to view ReadUploadFile
        /// </summary>
        /// <param name="sortViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ReadUploadFile(SortViewModel sortViewModel)
        {
            try
            {
                //create object List of NamesModel
                var namesModels = new List<NamesModel>();

                //create object List of string
                var genericList = new List<string>(); 

                if (ModelState.IsValid)
                {
                    //call the textfilerespository then return the List<NameModel>
                    genericList = _textFileRepository.ProcessUploadFile(sortViewModel.TextFile);
                    _logger.LogInformation($"Done processing upload file {sortViewModel.TextFile}");

                    //call the nameSortRepository to process the genericList 
                    namesModels = _nameSortRepository.sortGivenName(genericList);
                    _logger.LogInformation("Done sorting name list");

                    //store the namesModels object lsit to a memorycache
                    _textFileRepository.CreateCacheMemory(namesModels);

                }
                else
                {
                    return View("Index");
                } 

                //assigned the value to NamesModels property of SortViewModel
                sortViewModel.NamesModels = namesModels;
                return View("ReadUploadFile", sortViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception error was caught: {ex.Message}");

                ErrorModel errorModel = new ErrorModel
                {
                    ErrorTitle = "System Error!",
                    ErrorMessage = string.Format("We have encountered an Error. \nPlease contact your System Administrator."),
                    RedirectAction = "Index",
                    RedirectController = "TextFile"
                };

                return RedirectToAction("Index", "Error", errorModel);
            }
        }

        /// <summary>
        /// This will download the file name sorted-names-list.txt from the memory cache
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> DownloadCacheSortedList()
        {
            try
            {
                var cacheEntry = _memoryCache.Get<List<NamesModel>>(CacheKeys.Entry);

                using (MemoryStream stream = new MemoryStream())
                {
                    StreamWriter streamWriter = new StreamWriter(stream);
                    foreach (var item in cacheEntry)
                    {
                        await streamWriter.WriteLineAsync(String.Format("{0} {1}", item.FirstName, item.LastName));
                    }

                    streamWriter.Flush();
                    streamWriter.Close();
                    //return File(stream.ToArray(), "text/plain", "file.txt");
                    return File(stream.ToArray(), "text/plain", "sorted-names-list.txt");
                }  
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception error was caught: {ex.Message}");

                ErrorModel errorModel = new ErrorModel
                {
                    ErrorTitle = "System Error!",
                    ErrorMessage = string.Format("We have encountered an Error. \nPlease contact your System Administrator."),
                    RedirectAction = "Index",
                    RedirectController = "TextFile"
                };

                return RedirectToAction("Index", "Error", errorModel);
            }
        }
    }
}
