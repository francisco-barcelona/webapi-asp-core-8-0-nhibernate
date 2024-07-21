using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Data.Entities
{
    public class Sales : IEntity
    {
        public virtual int Id { get; set; }
        public virtual Client Client { get; set; }
        public virtual Product Product { get; set; }

    }
}
