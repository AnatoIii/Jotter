using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Model
{
    [DataContract]
    public class User
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Password { get; set; }
        [XmlIgnore]
        public string PasswordSalt { get; set; }

        [XmlIgnore]
        public List<Note> Notes { get; set; }

        public User(string name, string email, string password, string passwordSalt)
        {
            Id = Guid.NewGuid();
            Name = name;
            Email = email;
            Password = password;
            PasswordSalt = passwordSalt;
            Notes = new List<Note>();
        }

        public User()
        {
            Id = Guid.NewGuid();
            Notes = new List<Note>();
        }

        public void AddNote(Note note)
        {
            Notes.Add(note);
        }

        public void RemoveNote(Note note)
        {
            Notes.Remove(note);
        }
    }
}
