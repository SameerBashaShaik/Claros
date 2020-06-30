using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ClarosFlute.perStory.Model
{
    public interface IperStoryData
    {
        Task AddStoryPointData(perStoryData perStoryData);

        Task UpdateStoryPointData(Guid storyPointIdtoUpdate, perStoryData perStoryData);

        Task<perStoryData> GetStoryPointData(Guid storypointEntryId);

        Task<IEnumerable<perStoryData>> GetAllStoryPointData();

        Task DeleteStoryPointEntry(Guid entryId);

        Task DeleteAllStoryPointEntries();

    }
}
