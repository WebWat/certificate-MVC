using ApplicationCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Web.Interfaces;

public interface ICachedPublicViewModelService
{
    Task<List<Certificate>> GetList(string userId);
    Task SetList(string userId);

    Task<Certificate> GetItemAsync(string id, string userId);
    Task SetItemAsync(string id, string userId);
}
