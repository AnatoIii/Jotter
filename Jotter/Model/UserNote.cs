using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class UserNote : Entity
    {
        public Guid UserId { get; set; }
        public Guid NoteId { get; set; }

        public User User { get; set; }
        public Note Note { get; set; }
    }
}
