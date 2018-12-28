using Demo4OAuth.Tools;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Collections.ObjectModel;

namespace Demo4OAuth.ResourceServer.Model
{
    public class Book: BaseModel
    {     
        public Book() : base(true) { }
        
        public string Name { get; set; }

        public ObservableCollection<Attachment> Attachments { get; set; }
    }
}
