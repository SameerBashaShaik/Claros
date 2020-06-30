using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClarosFlute.API.Model
{
    public class ApiAssociatePerReleaseData
    {
        [JsonProperty("Id")]
        public Guid Id { get; set; }

        [JsonProperty("associate")]
        public string Associate { get; set; }

        [JsonProperty("release")]
        public double Release { get; set; }

        [JsonProperty("storypoints")]
        public double TotalStoryPoints { get; set; }
    }
}
