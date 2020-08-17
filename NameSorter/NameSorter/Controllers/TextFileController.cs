using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception error was caught TextFileController:Index {ex.Message}");
                throw ex;
            }
        }

        /// <summary>
        /// This will get the sorted list data in memory cache
        /// The view contain sort serach and pagination
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ViewLoadSortedData(string sortOrder, string currentFilter, string searchString, int? pageNumber, int CurrentItemCount)
        {
            try
            {
                ViewData["CurrentSort"] = sortOrder;
                ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "LastName" : "";

                if (searchString != null)
                {
                    pageNumber = 1;
                }
                else
                {
                    searchString = currentFilter;
                }

                //Get the data in memory cache 
                var sortedDataList = _memoryCache.Get<List<NamesModel>>(CacheKeys.Entry);

                //convert the object to Iqueryable 
                var model = sortedDataList.AsQueryable();

                //search the list of users base in searchString 
                ViewData["CurrentFilter"] = searchString;

                //filter by lastname and firstname
                if (!String.IsNullOrEmpty(searchString))
                {
                    model = sortedDataList.AsQueryable()
                    .Where(x => x.LastName.Contains(searchString) || x.GivenName.Contains(searchString));
                }

                //implementing sorting, use switch case for sorting
                switch (sortOrder)
                {
                    case "LastName":
                        model = model.OrderByDescending(s => s.LastName);
                        break;
                    case "GivenName":
                        model = model.OrderByDescending(s => s.GivenName);
                        break;
                    default:
                        model = model.OrderBy(s => s.LastName);
                        break;
                }

                //declare data size populated per page
                int pageSize = 15;

                //return the List NamesModel thru PaginatedList then populate the data in ViewLoadSortedData
                return View(await PaginatedList<NamesModel>.CreateAsync(model, pageNumber ?? 1, pageSize));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception error was caught TextFileController:ViewLoadSoratedData {ex.Message}");

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
                //Get the data from cache memory
                var cacheEntry = _memoryCache.Get<List<NamesModel>>(CacheKeys.Entry);

                //Use memory stream to make stream file from stream writer
                using (MemoryStream stream = new MemoryStream())
                {
                    //Write the data using stream writer
                    StreamWriter streamWriter = new StreamWriter(stream);
                    foreach (var item in cacheEntry)
                    {
                        await streamWriter.WriteLineAsync(String.Format("{0} {1}", item.GivenName, item.LastName));
                    }

                    streamWriter.Flush();
                    streamWriter.Close();

                    //return the download file named sorted-names-list.tx
                    return File(stream.ToArray(), "text/plain", "sorted-names-list.txt");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception error was caught TextFileController:DownloadCacheSortedList {ex.Message}");

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
        /// This will process the uploaded file then sort it by last name ascending 
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

                return RedirectToAction("ViewLoadSortedData");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception error was caught TextFileController:ReadUploadFile {ex.Message}");

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
