using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AssociateActor.Interfaces;
using ClarosFlute.perStory.Model;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace ClarosFlute.perStoryService
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class perStoryService : StatefulService, IperStoryService
    {
        private IperStoryData _repo;

        public perStoryService(StatefulServiceContext context)
            : base(context)
        {

        }
       

        private IAssociateActor GetAssociateActor(string associateId)
        {
            return ActorProxy.Create<IAssociateActor>(
                new ActorId(associateId),
                new Uri("fabric:/ClarosFlute/AssociateActorService"));
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

        public async Task AddStoryPointsAsync(perStoryData perStoryData)
        {
            await _repo.AddStoryPointData(perStoryData);
        }

        public async Task<perStoryData[]> GetAllStoryPointDataAsync()
        {
            return (await _repo.GetAllStoryPointData()).ToArray();
        }

        public async Task<perStoryData> GetStoryPointDataAsync(Guid storypointEntryId)
        {
            return await _repo.GetStoryPointData(storypointEntryId);
        }

        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following sample code with your own logic 
            //       or remove this RunAsync override if it's not needed in your service.

            _repo = new ServiceFabricPerStoryService(this.StateManager);

           


            //await _repo.AddStoryPointData(perStoryData1);
            //await _repo.AddStoryPointData(perStoryData2);
            //await _repo.AddStoryPointData(perStoryData3);

            IEnumerable<perStoryData> all = await _repo.GetAllStoryPointData();
        }

        public async Task DeleteStoryPointEntryAsync(Guid entryId)
        {
            await _repo.DeleteStoryPointEntry(entryId);
        }

        public async Task UpdateStoryPointsAsync(Guid storyPointIdtoUpdate, perStoryData perStoryData)
        {
            await _repo.UpdateStoryPointData(storyPointIdtoUpdate, perStoryData);
        }

        public async Task DeleteAllStoryPointDataAsync()
        {
            await _repo.DeleteAllStoryPointEntries();
        }
    }
}
