using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Collections.ObjectModel;

namespace Demo4DotNetCore.ResourceServer.Model
{
    public class Book: BaseModel
    {     
        public Book() : base(true) { }
        
        public string Name { get; set; }

        public ObservableCollection<Attachment> Attachments { get; set; }
    }
}
