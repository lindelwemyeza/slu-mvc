//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace StudentLinkUp_MVC_
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserMessage
    {
        public int messageID { get; set; }
        public int senderID { get; set; }
        public int recieverID { get; set; }
        public string containedText { get; set; }
        public System.DateTime messageDate { get; set; }
        public System.TimeSpan messageTime { get; set; }
    
        public virtual UserProfile UserProfile { get; set; }
        public virtual UserProfile UserProfile1 { get; set; }
    }
}