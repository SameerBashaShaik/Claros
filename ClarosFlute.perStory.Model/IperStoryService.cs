using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ClarosFlute.perStory.Model
{
    public interface IperStoryService : IService
    {
        Task<perStoryData[]> GetAllStoryPointDataAsync();

        Task AddStoryPointsAsync(perStoryData perStoryData);

        Task UpdateStoryPointsAsync(Guid storyPointIdtoUpdate, perStoryData perStoryData);

        Task<perStoryData> GetStoryPointDataAsync(Guid storypointEntryId);

        Task DeleteStoryPointEntryAsync(Guid entryId);

        Task DeleteAllStoryPointDataAsync();
    }
}
