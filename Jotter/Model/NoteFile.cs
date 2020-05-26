using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class NoteFile : Entity
    {
        public Guid NoteId { get; set; }
        public Guid FileId { get; set; }

        public Note Note { get; set; }
        public File File { get; set; }
    }
}
