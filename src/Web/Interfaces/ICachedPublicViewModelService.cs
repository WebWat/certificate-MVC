using ApplicationCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Web.Interfaces;

public interface ICachedPublicViewModelService
{
    Task<List<Certificate>> GetList(string userId);
    Task SetList(string userId);

    Task<Certificate> GetItemAsync(int id, string userId);
    Task SetItemAsync(int id, string userId);
}
