using ClarosFlute.API.Model;
using ClarosFlute.perStory.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClarosFlute.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class perReleaseController : ControllerBase
    {
        private readonly IperStoryService _service;

        public perReleaseController()
        {
            var proxyFactory = new ServiceProxyFactory(
                 c => new FabricTransportServiceRemotingClientFactory());

            _service = proxyFactory.CreateServiceProxy<IperStoryService>(
                new Uri("fabric:/ClarosFlute/ClarosFlute.perReleaseService"),
                new ServicePartitionKey(0));
        }

        [HttpGet]
        public async Task<IEnumerable<ApiAssociatePerReleaseData>> GetAsync()
        {
            IEnumerable<perStoryData> allStoryPointData = await _service.GetAllStoryPointDataAsync();

            throw new NotImplementedException();

            //return allStoryPointData.Select(p => new ApiPerStoryData
            //{
            //    Id = p.Id,
            //    Associate = p.Associate,
            //    StoryNumber = p.StoryNumber,
            //    Release = p.Release,
            //    StoryPoints = p.StoryPoints
            //});
        }

    }
}