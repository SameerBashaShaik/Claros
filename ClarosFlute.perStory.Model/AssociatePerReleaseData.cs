using System;

namespace ClarosFlute.perStory.Model
{
    public class AssociatePerReleaseData
    {
        public Guid Id { get; set; }

        public string Associate { get; set; }

        public double Release { get; set; }

        public double TotalStoryPoints { get; set; }
    }
}
