using FluentNHibernate.Mapping;
using MyApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Data.Mappings
{
    public class SalesMap : ClassMap<Sales>
    {
        public SalesMap()
        {
            Table("Sales");
            Id(x => x.Id).GeneratedBy.Identity();
            References(x => x.Client, "ClientId").Not.Nullable();
            References(x => x.Product, "ProductId").Not.Nullable();
        }
    }
}
