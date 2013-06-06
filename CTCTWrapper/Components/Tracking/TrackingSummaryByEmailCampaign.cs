using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace CTCT.Components.Tracking
{
    /// <summary>
    /// Represents a Tracking Summary class.
    /// </summary>
    [Serializable]
    [DataContract]
    public class TrackingSummaryByEmailCampaign : Component
    {
        /// <summary>
        /// Gets or sets the number of opens.
        /// </summary>
        [DataMember(Name="opens")]
        public int Opens { get; set; }
        /// <summary>
        /// Gets or sets the number of clicks.
        /// </summary>
        [DataMember(Name="clicks")]
        public int Clicks { get; set; }
        /// <summary>
        /// Gets or sets the number of forwards.
        /// </summary>
        [DataMember(Name="forwards")]
        public int Forwards { get; set; }
        /// <summary>
        /// Gets or sets the number of bounces.
        /// </summary>
        [DataMember(Name="bounces")]
        public int Bounces { get; set; }

        /// <summary>
        /// Class constructor.
        /// </summary>
        public TrackingSummaryByEmailCampaign() { }
    }
}
