using FluentNHibernate.Mapping;
using MyApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Data.Mappings
{
    public class ClientMap : ClassMap<Client>
    {
        public ClientMap()
        {
            Table("Clients");
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Name).Not.Nullable();
            HasMany(x => x.Sales)
                .Cascade.All()
                .Inverse()
                .KeyColumn("ClientId"); ;
        }
    }
}
