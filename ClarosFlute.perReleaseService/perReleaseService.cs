using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ClarosFlute.perRelease.Model;
using ClarosFlute.perStoryService;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace ClarosFlute.perReleaseService
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class perReleaseService : StatefulService, IperReleaseService
    {
        private IperReleaseData _repoRelease;

        public perReleaseService(StatefulServiceContext context)
            : base(context)
        { }

        public async Task<perReleaseData[]> GetAllPerReleaseDataAsync()
        {
            return (await _repoRelease.GetAllPerReleaseData()).ToArray();
        }

        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, Service Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see https://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return new[]
            {
                new ServiceReplicaListener(context =>
                 new FabricTransportServiceRemotingListener(context,this))
            };
        }

        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following sample code with your own logic 
            //       or remove this RunAsync override if it's not needed in your service.

            _repoRelease = new ServiceFabricPerReleaseService(this.StateManager);

            //var perStoryData1 = new perStoryData
            //{
            //    Id = Guid.NewGuid(),
            //    Associate = "Aniket",
            //    StoryNumber = "CLAROS-26557",
            //    Release = 2020.2,
            //    StoryPoints = 5
            //};

            //var perStoryData2 = new perStoryData
            //{
            //    Id = Guid.NewGuid(),
            //    Associate = "Krishna",
            //    StoryNumber = "CLAROS-26559",
            //    Release = 2020.2,
            //    StoryPoints = 5
            //};


            //var perStoryData3 = new perStoryData
            //{
            //    Id = Guid.NewGuid(),
            //    Associate = "Tejaswini",
            //    StoryNumber = "CLAROS-13039",
            //    Release = 2020.2,
            //    StoryPoints = 1
            //};

            //await _repo.AddStoryPointData(perStoryData1);
            //await _repo.AddStoryPointData(perStoryData2);
            //await _repo.AddStoryPointData(perStoryData3);

            IEnumerable<perReleaseData> all = await _repoRelease.GetAllPerReleaseData();
        }
    }
}
