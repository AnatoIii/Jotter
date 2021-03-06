﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Model.DTO
{
	public class NoteData
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid CategoryId { get; set; }

        public List<File> Files { get; set; }
        public Category Category { get; set; }
    }
}
