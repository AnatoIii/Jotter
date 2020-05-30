using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Model
{
    [DataContract]
    public class NoteFile 
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public Guid NoteId { get; set; }
        [DataMember]
        public Guid FileId { get; set; }

        [XmlIgnore]
        public Note Note { get; set; }
        [XmlIgnore]
        public File File { get; set; }
    }
}
