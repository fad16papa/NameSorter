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
    /// Description: This is the interface for NameSortRepository
    /// </summary>
    public interface INameSortRepository
    {
        List<NamesModel> sortGivenName(List<string> givenNameList);
    }
}
