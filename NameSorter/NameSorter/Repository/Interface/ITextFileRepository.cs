using Microsoft.AspNetCore.Http;
using NameSorter.Models;
using System.Collections.Generic;

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

        void CreateCacheMemory(List<NamesModel> namesModels);
    }
}
