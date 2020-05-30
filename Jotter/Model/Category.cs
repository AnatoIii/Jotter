using System;
using System.Runtime.Serialization;

namespace Model
{
    [DataContract]
    public class Category
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Password { get; set; }

        public bool IsPrivate { get; set; }

        public Category(string name, string password = null)
        {
            Id = Guid.NewGuid();
            Name = name;
            Password = password;
        }

        public Category()
        {
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
