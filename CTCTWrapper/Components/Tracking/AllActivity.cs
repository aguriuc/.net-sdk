using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace CTCT.Components.Tracking
{
    /// <summary>
    /// Represents a single All Activity class.
    /// </summary>
    [Serializable]
    [DataContract]
    public class AllActivity : BaseActivity
    {
        /// <summary>
        /// Gets or sets the open date.
        /// </summary>
        [DataMember(Name = "open_date", EmitDefaultValue=false)]
        public string OpenDate { get; set; }
        /// <summary>
        /// Gets or sets the unsubscribe date.
        /// </summary>
        [DataMember(Name = "unsubscribe_date", EmitDefaultValue = false)]
        public string UnsubscribeDate { get; set; }
        /// <summary>
        /// Gets or sets the send date.
        /// </summary>
        [DataMember(Name = "send_date", EmitDefaultValue = false)]
        public string SendDate { get; set; }
        /// <summary>
        /// Gets or sets the forward date.
        /// </summary>
        [DataMember(Name = "forward_date", EmitDefaultValue = false)]
        public string ForwardDate { get; set; }
        /// <summary>
        /// Gets or sets the number of opens.
        /// </summary>
        [DataMember(Name = "opens", EmitDefaultValue = false)]
        public int? Opens { get; set; }
        /// <summary>
        /// Gets or sets the link identification.
        /// </summary>
        [DataMember(Name = "link_id", EmitDefaultValue = false)]
        public string LinkId { get; set; }
        /// <summary>
        /// Gets or sets the link uri the contact clicked on.
        /// </summary>
        [DataMember(Name = "link_uri", EmitDefaultValue = false)]
        public string LinkUri { get; set; }
        /// <summary>
        /// Gets or sets the number of bounces.
        /// </summary>
        [DataMember(Name = "bounces", EmitDefaultValue = false)]
        public int? Bounces { get; set; }
        /// <summary>
        /// Gets or sets the unsubscribe reason.
        /// </summary>
        [DataMember(Name = "unsubscribe_reason", EmitDefaultValue = false)]
        public string UnsubscribeReason { get; set; }
        /// <summary>
        /// Gets or sets the number of forwards.
        /// </summary>
        [DataMember(Name = "forwards", EmitDefaultValue = false)]
        public int? Forwards { get; set; }
        /// <summary>
        /// Gets or sets the bounce description.
        /// </summary>
        [DataMember(Name = "bounce_description", EmitDefaultValue = false)]
        public string BounceDescription { get; set; }
        /// <summary>
        /// Gets or sets the unsubscribe source.
        /// </summary>
        [DataMember(Name = "unsubscribe_source", EmitDefaultValue = false)]
        public string UnsubscribeSource { get; set; }
        /// <summary>
        /// Gets or sets the bounce message.
        /// </summary>
        [DataMember(Name = "bounce_message", EmitDefaultValue = false)]
        public string BounceMessage { get; set; }
        /// <summary>
        /// Gets or sets the bounce code.
        /// </summary>
        [DataMember(Name = "bounce_code", EmitDefaultValue = false)]
        public string BounceCode { get; set; }
        /// <summary>
        /// Gets or sets the number of clicks.
        /// </summary>
        [DataMember(Name = "clicks", EmitDefaultValue = false)]
        public int? Clicks { get; set; }
        /// <summary>
        /// Gets or sets the bounce date.
        /// </summary>
        [DataMember(Name = "bounce_date", EmitDefaultValue = false)]
        public string BounceDate { get; set; }
        /// <summary>
        /// Gets or sets the click date.
        /// </summary>
        [DataMember(Name = "click_date", EmitDefaultValue = false)]
        public string ClickDate { get; set; }

        /// <summary>
        /// Class constructor.
        /// </summary>
        public AllActivity() { }
    }
}
