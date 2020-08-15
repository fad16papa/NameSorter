using Microsoft.AspNetCore.Http;
using NameSorter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NameSorter.Repository.Interface
{
    public interface ITextFileRepository
    {
        List<string> ProcessUploadFile(IFormFile textFileModel);
    }
}
