﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class BookDTO
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string Description { get; set; }

        public string AuthorId { get; set; }

        public int Year { get; set; }

        public string Path { get; set; }
    }
}
