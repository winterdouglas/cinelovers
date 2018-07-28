using System;
using System.Collections.Generic;
using System.Text;

namespace Cinelovers.Core.Services.Models
{
    public class Review
    {
        public string Id { get; }

        public string Content { get; }

        public string Author { get; }

        public Review(string id, string author, string content)
        {
            Id = id;
            Author = author;
            Content = content;
        }
    }
}
