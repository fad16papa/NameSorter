using Microsoft.AspNetCore.Http;
using NameSorter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NameSorter.Repository.Interface
{
    /// <summary>
    /// Author: Francis Decena
    /// Date: 16/8/2020
    /// Description: These are the interface for TextFileReposirtory 
    /// </summary>
    public interface ITextFileRepository
    {
        List<string> ProcessUploadFile(IFormFile textFileModel);

        Task WriteSortedNames(List<NamesModel> namesModels);
    }
}
