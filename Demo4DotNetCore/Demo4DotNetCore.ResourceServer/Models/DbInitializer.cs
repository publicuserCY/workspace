using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo4DotNetCore.ResourceServer.Model
{
    public class DbInitializer
    {
        public static void Initialize(ResourceContext context)
        {
            // Look for any students.
            if (context.Books.Any())
            {
                return;
            }

            var books = new Book[]
            {
                new Book{Id="1",Name="Alexander"},
                new Book{Id="2",Name="Gytis"},
                new Book{Id="3",Name="Peggy"}
            };
            foreach (Book s in books)
            {
                context.Books.Add(s);
            }
            context.SaveChanges();
        }
    }
}
