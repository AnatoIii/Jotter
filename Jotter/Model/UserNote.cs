using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Model
{
    [DataContract]
    public class UserNote
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public Guid UserId { get; set; }
        [DataMember]
        public Guid NoteId { get; set; }

        [XmlIgnore]
        public User User { get; set; }
        [XmlIgnore]
        public Note Note { get; set; }

        public UserNote() { }
    }
}
