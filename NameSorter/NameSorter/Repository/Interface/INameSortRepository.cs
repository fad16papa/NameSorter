using NameSorter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NameSorter.Repository.Interface
{
    public interface INameSortRepository
    {
        List<NamesModel> sortGivenName(List<string> givenNameList);
    }
}
