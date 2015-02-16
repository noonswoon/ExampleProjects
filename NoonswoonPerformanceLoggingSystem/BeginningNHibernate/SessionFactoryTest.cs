using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using BeginningNHibernate.NhSessionFactory;
namespace BeginningNHibernate
{
    [TestFixture]
    public class SessionFactoryTest
    {

        [Test]
        public void CreateTable_ValidConfig_TableCreated()
        {
            SessionFactory.Init();
            var config = SessionFactory.Config;
            
            //create table
            var schemaExport = new SchemaExport(config);
            schemaExport.SetOutputFile("db_create.sql");
            schemaExport.Create(true, true);
        }

    }
}
