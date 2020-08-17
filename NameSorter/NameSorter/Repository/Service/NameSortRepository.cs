using Microsoft.Extensions.Logging;
using NameSorter.Models;
using NameSorter.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NameSorter.Repository.Service
{
    /// <summary>
    /// Author: Franis Decena 
    /// Date 15/8/2020
    /// Description: This will sort the processed uploaded file
    /// </summary>
    public class NameSortRepository : INameSortRepository
    {
        #region Properties
        private readonly ILogger<NameSortRepository> _logger;
        #endregion

        #region Constructor
        public NameSortRepository(ILogger<NameSortRepository> logger)
        {
            _logger = logger;
        }
        #endregion

        /// <summary>
        /// This will sort the givenNameList object and return a List NamesModel order by lastName
        /// </summary>
        /// <param name="givenNameList"></param>
        /// <returns></returns>
        public List<NamesModel> sortGivenName(List<string> givenNameList)
        {

            try
            {
                //Create a List Object of NamesModel 
                var namesModelList = new List<NamesModel>();

                //populate the List<NameModel>
                foreach (var item in givenNameList)
                {
                    //sort the item base on genericList object
                    //split each item then store it to an array object string
                    string[] splitObject = item.Split(" ");

                    //count the splitObject 
                    int countSplitObject = splitObject.Count();

                    //convert splitObject array to list string object
                    //then remove specific element of list by subtructing the countSplitObject -1 
                    //to locate the exact location of last name 
                    var givenNameObjectList = new List<string>(splitObject);
                    givenNameObjectList.RemoveAt(countSplitObject - 1);

                    //Instantiate NamesModel 
                    var namesModel = new NamesModel
                    {
                        //get the length on splitObject then subtract by -1 to locate 
                        //the exact element then assigned the value to the property of lastName
                        LastName = splitObject[splitObject.Length - 1].ToString(),
                        //use string join to create single string object from givenNameObjectList array object
                        //then assign the value to the property of FirstName
                        GivenName = string.Join(" ", givenNameObjectList.ToArray())
                    };

                    //Then add the namesModel to List NamesModel Object 
                    namesModelList.Add(namesModel);

                    //Order by LastName
                    namesModelList = namesModelList.AsQueryable().OrderBy(x => x.LastName).ToList();
                }

                return namesModelList;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception error was caught NameSortRepository:sortGivenName: {ex.Message}");
                throw ex;
            }      
        }
    }
}
