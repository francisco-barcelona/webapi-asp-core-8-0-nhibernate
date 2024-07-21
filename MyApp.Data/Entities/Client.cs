using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Data.Entities
{
    public class Client : IEntity
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual ICollection<Sales> Sales { get; set; }
    }
}
