using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClarosFlute.perRelease.Model
{
    public interface IperReleaseData
    {
        Task<IEnumerable<perReleaseData>> GetAllPerReleaseData();
    }
}
