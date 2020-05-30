using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Model
{
    [DataContract]
    public class Note
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public Guid CategoryId { get; set; }

        [XmlIgnore]
        public List<File> Files { get; set; }
        [XmlIgnore]
        public Category Category { get; set; }

        public Note(string name, string description, Category category)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            Category = category;
            Files = new List<File>();
        }

        public Note()
        {
            Files = new List<File>();
        }

        public void AddFile(File file)
        {
            Files.Add(file);
        }

        public void RemoveFile(File file)
        {
            Files.Remove(file);
        }
    }
}
