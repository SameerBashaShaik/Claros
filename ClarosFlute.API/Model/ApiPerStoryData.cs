using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClarosFlute.API.Model
{
    public class ApiPerStoryData
    {
        [JsonProperty("Id")]
        public Guid Id { get; set; }

        [JsonProperty("associate")]
        public string Associate { get; set; }

        [JsonProperty("storynumber")]
        public string StoryNumber { get; set; }

        [JsonProperty("release")]
        public double Release { get; set; }

        [JsonProperty("storypoints")]
        public double StoryPoints { get; set; }
    }
}
