using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using NameSorter.Helper;
using NameSorter.Models;
using NameSorter.Repository.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NameSorter.Repository.Service
{
    /// <summary>
    /// Author: Francis Decena 
    /// Date: 15/8/2020
    /// Description: This will process the uploaded text file.
    /// </summary>
    public class TextFileRepository : ITextFileRepository
    {
        #region Properties
        private readonly ILogger<TextFileRepository> _logger;
        private readonly IMemoryCache _memoryCache;
        #endregion

        #region Contructor
        public TextFileRepository(ILogger<TextFileRepository> logger, IMemoryCache memoryCache)
        {
            _logger = logger;
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// This will create a cache memory for the sorted list names
        /// </summary>
        /// <param name="namesModels"></param>
        /// <returns></returns>
        public void CreateCacheMemory(List<NamesModel> namesModels)
        {
           var namesModelsList = new List<NamesModel>();

            try
            {
                //Look for cache key 
                if (!_memoryCache.TryGetValue(CacheKeys.Entry, out namesModelsList))
                {
                    //Key not in cache, so get data
                    namesModelsList = namesModels;

                    // Set cache options.
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        // Keep in cache for this time, reset time if accessed.
                        .SetSlidingExpiration(TimeSpan.FromMinutes(60));

                    // Save data in cache.
                    _memoryCache.Set(CacheKeys.Entry, namesModelsList, cacheEntryOptions);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception error was caught at TextFileRepository:CreateCacheMemory: {ex.Message}");
                throw ex;
            }
        }
        #endregion


        /// <summary>
        /// Description: This will read the uploaded text file and return an IEnumerable<NameModel> object
        /// </summary>
        /// <param name="textFileModel"></param>
        /// <returns></returns>
        public List<string> ProcessUploadFile(IFormFile textFile)
        {
            try
            {
                //store the read lines of text file
                List<string> genericListNames = new List<string>();

                var result = new StringBuilder();

                //Open the text file using a stream reader. 
                using (var streamFile = new StreamReader(textFile.OpenReadStream()))
                {
                    //Read the stream as a string 
                    while(streamFile.Peek() >= 0)
                    {
                        genericListNames.Add(streamFile.ReadLine());
                    }
                }
                return genericListNames;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception error was caught at TextFileRepository:ProcessUploadFile: {ex.Message}");
                throw ex;
            }
        }
    }
}
