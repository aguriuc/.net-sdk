using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace CTCT.Components.Tracking
{
    /// <summary>
    /// Represents a single Bounce Activity class.
    /// </summary>
    [Serializable]
    [DataContract]
    public class BounceActivity : BaseActivity
    {
        /// <summary>
        /// Gets or sets the bounce code.
        /// </summary>
        [DataMember(Name = "bounce_code")]
        public string BounceCode { get; set; }
        /// <summary>
        /// Gets or sets the bounce description.
        /// </summary>
        [DataMember(Name = "bounce_description")]
        public string BounceDescription { get; set; }
        /// <summary>
        /// Gets or sets the bounce message.
        /// </summary>
        [DataMember(Name = "bounce_message")]
        public string BounceMessage { get; set; }
        /// <summary>
        /// Gets or sets the bounce date.
        /// </summary>
        [DataMember(Name = "bounce_date")]
        public string BounceDate { get; set; }

        /// <summary>
        /// Class constructor.
        /// </summary>
        public BounceActivity() { }
    }

    /// <summary>
    /// Bounce codes.
    /// </summary>
    public struct BounceCodes
    {
        /// <summary>
        /// Non-existent address.
        /// </summary>
        public const string B = "B";
        /// <summary>
        /// Undeliverable.
        /// </summary>
        public const string D = "D";
        /// <summary>
        /// Mailbox full.
        /// </summary>
        public const string F = "F";
        /// <summary>
        /// Vacation or autoreply.
        /// </summary>
        public const string V = "V";
        /// <summary>
        /// Blocked.
        /// </summary>
        public const string Z = "Z";
    }
}
