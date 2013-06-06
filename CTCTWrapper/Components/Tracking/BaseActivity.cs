using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace CTCT.Components.Tracking
{
    /// <summary>
    /// Base class for activities.
    /// </summary>
    [DataContract]
    [Serializable]
    public abstract class BaseActivity : Component
    {
        /// <summary>
        /// Gets or sets the activity type.
        /// </summary>
        [DataMember(Name = "activity_type", EmitDefaultValue = false)]
        public string ActivityType { get; set; }
        /// <summary>
        /// Gets or sets the campaign id.
        /// </summary>
        [DataMember(Name = "campaign_id", EmitDefaultValue = false)]
        public string CampaignId { get; set; }
        /// <summary>
        /// Gets or sets the contact id.
        /// </summary>
        [DataMember(Name = "contact_id", EmitDefaultValue = false)]
        public string ContactId { get; set; }
        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        [DataMember(Name = "email_address", EmitDefaultValue = false)]
        public string EmailAddress { get; set; }
    }

    /// <summary>
    /// Activity type.
    /// </summary>
    public struct ActivityType
    {
        /// <summary>
        /// Bounce activity.
        /// </summary>
        public const string Bounce = "EMAIL_BOUNCE";
        /// <summary>
        /// Click activity.
        /// </summary>
        public const string Click = "EMAIL_CLICK";
        /// <summary>
        /// Forward activity.
        /// </summary>
        public const string Forward = "EMAIL_FORWARD";
        /// <summary>
        /// Open activity.
        /// </summary>
        public const string Open = "EMAIL_OPEN";
        /// <summary>
        /// Unsubscribe activity.
        /// </summary>
        public const string Unsubscribe = "EMAIL_UNSUBSCRIBE";
        /// <summary>
        /// Send activity.
        /// </summary>
        public const string Send = "EMAIL_SEND";
    }
}
