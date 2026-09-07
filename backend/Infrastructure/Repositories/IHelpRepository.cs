using Raras.EMS.API.Models.Entities;

namespace Raras.EMS.API.Repositories;

public interface IHelpRepository
{
    Task<HelpHeader?> FindByFeatureSpecificationAsync(string moduleKey, string featureKey, string featureSpecificationKey);
    Task<HelpHeader?> FindByFeatureAsync(string moduleKey, string featureKey);
    Task<HelpHeader?> FindByModuleAsync(string moduleKey);
}
