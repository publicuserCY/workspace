using System.Collections.ObjectModel;
using Demo4DotNetCore.ResourceServer.Model;

namespace Demo4DotNetCore.ResourceServer.Books.Model
{
    public class Book : BaseModel<string>
    {
        public Book() : base(true) { }

        public string Name { get; set; }

        public ObservableCollection<Attachment> Attachments { get; set; }
    }
}
