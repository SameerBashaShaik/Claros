using ClarosFlute.API.Model;
using ClarosFlute.perStory.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ClarosFlute.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class perStoryController : ControllerBase
    {
        private readonly IperStoryService _service;

        public perStoryController()
        {
            var proxyFactory = new ServiceProxyFactory(
                 c => new FabricTransportServiceRemotingClientFactory());

            _service = proxyFactory.CreateServiceProxy<IperStoryService>(
                new Uri("fabric:/ClarosFlute/ClarosFlute.perStoryService"),
                new ServicePartitionKey(0));
        }

        #region Get Methods

        [HttpGet]
        public async Task<IEnumerable<ApiPerStoryData>> GetAsync()
        {
            IEnumerable<perStoryData> allStoryPointData = await _service.GetAllStoryPointDataAsync();

            return allStoryPointData.Select(p => new ApiPerStoryData
            {
                Id = p.Id,
                Associate = p.Associate,
                StoryNumber = p.StoryNumber,
                Release = p.Release,
                StoryPoints = p.StoryPoints
            });
        }

        [HttpGet("{storyEntryId}")]
        public async Task<ApiPerStoryData> GetStoryDataAsync(Guid storyEntryId)
        {
            perStoryData storyPointEntry = await _service.GetStoryPointDataAsync(storyEntryId);

            if (storyPointEntry != null)
            {
                return (new ApiPerStoryData()
                {
                    Associate = storyPointEntry.Associate,
                    StoryNumber = storyPointEntry.StoryNumber,
                    Release = storyPointEntry.Release,
                    StoryPoints = storyPointEntry.StoryPoints,
                    Id = storyPointEntry.Id
                });
            }
            else
            {
                return new ApiPerStoryData();
            }

        }

        [Route("associateperrelease")]
        [HttpGet]
        public async Task<IEnumerable<ApiAssociatePerReleaseData>> Get()
        {
            IEnumerable<perStoryData> allStoryPointData = await _service.GetAllStoryPointDataAsync();

            var groups = from e in allStoryPointData
                         group e by new
                         {
                             e.Associate,
                             e.Release
                         } into gcs
                         select new AssociatePerReleaseData()
                         {
                             Associate = gcs.Key.Associate,
                             Release = gcs.Key.Release,
                             TotalStoryPoints = gcs.Sum(x => x.StoryPoints),
                             Id = Guid.NewGuid()
                         };


            return groups.Select(p => new ApiAssociatePerReleaseData
            {
                Associate = p.Associate,
                Release = p.Release,
                TotalStoryPoints = p.TotalStoryPoints,
                Id = Guid.NewGuid()
            });

        }

        [Route("release")]
        [HttpGet]
        public async Task<ApiPerReleaseData> GetReleaseData(double Release)
        {
            IEnumerable<perStoryData> allStoryPointData = await _service.GetAllStoryPointDataAsync();

            double storyPointsperReleaseData = allStoryPointData.Where(x => x.Release == Release).Sum(x => x.StoryPoints);

            return new ApiPerReleaseData
            {
                Id = Guid.NewGuid(),
                Release = Release,
                TotalStoryPoints = storyPointsperReleaseData

            };

        }

        #endregion

        #region Post Methods

        [HttpPost]
        public async Task PostAsync([FromBody] ApiPerStoryData newstorypointdata)
        {
            var newStoryPointData = new perStoryData()
            {
                Id = Guid.NewGuid(),
                Associate = newstorypointdata.Associate,
                StoryNumber = newstorypointdata.StoryNumber,
                Release = newstorypointdata.Release,
                StoryPoints = newstorypointdata.StoryPoints
            };

            await _service.AddStoryPointsAsync(newStoryPointData);

        }

        [HttpPost]
        [Route("multipleStoryPointData")]
        public async Task PostMultipleDataAsync([FromBody] List<ApiPerStoryData> newstorypointdatacollection)
        {
            foreach (ApiPerStoryData newStoryPointData in newstorypointdatacollection)
            {
                var newStoryPointDataforPost = new perStoryData()
                {
                    Id = Guid.NewGuid(),
                    Associate = newStoryPointData.Associate,
                    StoryNumber = newStoryPointData.StoryNumber,
                    Release = newStoryPointData.Release,
                    StoryPoints = newStoryPointData.StoryPoints
                };
                await _service.AddStoryPointsAsync(newStoryPointDataforPost);

            }
        }

        #endregion

        #region Put Method

        [HttpPut("{storyPointIdtoUpdate}")]
        public async Task UpdateStoryDataAsync(Guid storyPointIdtoUpdate, [FromBody] ApiPerStoryData updatedstorypointdata)
        {
            var updatedStoryPointData = new perStoryData()
            {
                Id = storyPointIdtoUpdate,
                Associate = updatedstorypointdata.Associate,
                StoryNumber = updatedstorypointdata.StoryNumber,
                Release = updatedstorypointdata.Release,
                StoryPoints = updatedstorypointdata.StoryPoints
            };

            await _service.UpdateStoryPointsAsync(storyPointIdtoUpdate, updatedStoryPointData);
        }

        #endregion

        #region Delete Method

        [HttpDelete("{storyPointIdtoDelete}")]
        public async Task DeleteAsync(Guid storyPointIdtoDelete)
        {
            await _service.DeleteStoryPointEntryAsync(storyPointIdtoDelete);
        }

        [HttpDelete]
        public async Task DeleteAllDataAsync()
        {
            await _service.DeleteAllStoryPointDataAsync();
        }

        #endregion
    }
}