using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace SqlKata.Extensions.Tests
{
 
        [ExcludeFromCodeCoverage]
        public class User
        {
            public int UserId { get; set; }
            public string UserName { get; set; }
            public DateTime CreatedAt { get; set; }
        }

        public class Post
        {
            public int PostId { get; set; }
            public int UserId { get; set; }
            public string Title { get; set; }
        }
    
}
