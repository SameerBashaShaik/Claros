using ClarosFlute.perRelease.Model;
using ClarosFlute.perStory.Model;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClarosFlute.perStoryService
{
    class ServiceFabricPerReleaseService : IperReleaseData
    {

        private readonly IReliableStateManager _stateManager;
        

        public ServiceFabricPerReleaseService(IReliableStateManager stateManager)
        {
            _stateManager = stateManager;
        }

        public Task<IEnumerable<perRelease.Model.perReleaseData>> GetAllPerReleaseData()
        {
            throw new NotImplementedException();
        }
    }
}
