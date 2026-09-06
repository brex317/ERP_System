using Raras.EMS.API.Models.Entities;

namespace Raras.EMS.API.Repositories;

public interface IHelpRepository
{
    Task<HelpContext?> FindByFunctionalityAsync(string moduleKey, string pageKey, string functionalityKey);
    Task<HelpContext?> FindByPageAsync(string moduleKey, string pageKey);
    Task<HelpContext?> FindByModuleAsync(string moduleKey);
}
