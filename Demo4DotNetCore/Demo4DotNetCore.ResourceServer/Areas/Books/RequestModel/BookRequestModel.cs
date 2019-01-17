using Demo4DotNetCore.ResourceServer.Model;
using System.Collections.Generic;

namespace Demo4DotNetCore.ResourceServer.Books.Model
{
    public class BookRequestModel : PaginatedRequestModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Book Book { get; set; }
    }
}