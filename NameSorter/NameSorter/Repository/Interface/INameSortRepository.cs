using NameSorter.Models;
using System.Collections.Generic;

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
