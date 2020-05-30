using System;
using System.Runtime.Serialization;

namespace Model
{
    [DataContract]
    public class File
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Path { get; set; }

        public File(string name, string path)
        {
            Id = Guid.NewGuid();
            Name = name;
            Path = path;
        }
    }
}
