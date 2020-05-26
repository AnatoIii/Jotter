using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Model
{
    public class User : Entity
    {
        [DataMember]
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }

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
