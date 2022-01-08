using ApplicationCore.Entities;
using System.Collections.Generic;

namespace Web.Interfaces;

public interface IFilterService
{
    IEnumerable<Certificate> FilterOut(IEnumerable<Certificate> list, string year, string find, Stage? stage);
}