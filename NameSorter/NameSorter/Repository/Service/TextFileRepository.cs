using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NameSorter.Models;
using NameSorter.Repository.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

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
        #endregion

        #region Contructor
        public TextFileRepository(ILogger<TextFileRepository> logger)
        {
            _logger = logger;
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

        /// <summary>
        /// This will write the sorted name list in a text file.
        /// </summary>
        /// <param name="namesModels"></param>
        /// <returns></returns>
        public async Task WriteSortedNames(List<NamesModel> namesModels)
        {
            try
            {
                string path = @"C:\Temp\sorted-names-list.txt";

                if(!File.Exists(path))
                {
                    //Create a file to write to
                    using (StreamWriter streamWriter = File.CreateText(path))
                    {
                        foreach (var item in namesModels)
                        {
                            await streamWriter.WriteLineAsync(String.Format("{0} {1}", item.FirstName, item.LastName));
                        }
                    }
                }
                else
                {
                    //Create a file to write to
                    using (StreamWriter streamWriter = File.CreateText(path))
                    {
                        foreach (var item in namesModels)
                        {
                            await streamWriter.WriteLineAsync(String.Format("{0} {1}", item.FirstName, item.LastName));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception error was caught TextFileRepository:WriteSortedNames: {ex.Message}");
                throw ex;
            }
        }
    }
}
