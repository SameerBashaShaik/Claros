using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ClarosFlute.perRelease.Model
{
    public interface IperReleaseService : IService
    {
        Task<perReleaseData[]> GetAllPerReleaseDataAsync();
    }
}
