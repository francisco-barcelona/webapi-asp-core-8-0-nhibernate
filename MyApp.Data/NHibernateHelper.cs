using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Cfg;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyApp.Data.Mappings;
using MyApp.Data.Entities;
using NHibernate.Tool.hbm2ddl;

namespace MyApp.Data
{
    public static class NHibernateHelper
    {
        public static ISessionFactory CreateSessionFactory(string connectionString)
        {
            return Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012.ConnectionString(connectionString))
                .Mappings(m => {
                    m.FluentMappings.AddFromAssemblyOf<ProductMap>();
                    m.FluentMappings.AddFromAssemblyOf<ClientMap>();
                    m.FluentMappings.AddFromAssemblyOf<SalesMap>();
                })
                .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
                .BuildSessionFactory();
        }
    }
}
