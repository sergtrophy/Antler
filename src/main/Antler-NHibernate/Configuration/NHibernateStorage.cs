﻿using System;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Tool.hbm2ddl;
using SmartElk.Antler.Core.Common.CodeContracts;
using SmartElk.Antler.Core.Domain;
using SmartElk.Antler.Core.Domain.Configuration;

namespace SmartElk.Antler.NHibernate.Configuration
{
    public class NHibernateStorage : AbstractStorage<NHibernateStorage>
    {        
        protected Action<global::NHibernate.Cfg.Configuration> ActionToApplyOnNHibernateConfiguration;
        
        protected bool GeneratedDatabase { get; set; }
        protected Action TryToCreateDatabaseCommand { get; set; }

        protected IPersistenceConfigurer PersistenceConfigurer { get; set; }

        protected NHibernateStorage()
        {
        }

        public static NHibernateStorage Use
        {
            get { return new NHibernateStorage();}
        }
                
        public NHibernateStorage ApplyOnNHibernateConfiguration(Action<global::NHibernate.Cfg.Configuration> actionToApplyOnNHibernateConfiguration)
        {
            Requires.NotNull(actionToApplyOnNHibernateConfiguration, "actionToApplyOnNHibernateConfiguration");
            
            ActionToApplyOnNHibernateConfiguration = actionToApplyOnNHibernateConfiguration;
            return this;
        }

        public NHibernateStorage WithGeneratedDatabase(Action tryToCreateDatabaseCommand = null)
        {
            GeneratedDatabase = true;
            TryToCreateDatabaseCommand = tryToCreateDatabaseCommand;
            return this;
        }

        public NHibernateStorage WithDatabaseConfiguration(IPersistenceConfigurer persistenceConfigurer)
        {
            Requires.NotNull(persistenceConfigurer, "persistenceConfigurer");
            
            PersistenceConfigurer = persistenceConfigurer;
            return this;
        }

        public override void Configure(IDomainConfigurator configurator)
        {
            Requires.NotNull(configurator, "configurator");

            global::NHibernate.Cfg.Configuration configuration = null;

            var sessionFactory = Fluently.Configure()
                .Database(PersistenceConfigurer)
                .Mappings(x => x.FluentMappings.AddFromAssembly(AssemblyWithMappings)).
                ExposeConfiguration(x =>
                {
                    configuration = x;

                    if (GeneratedDatabase)
                    {
                        if (TryToCreateDatabaseCommand != null)
                        {
                            TryToCreateDatabaseCommand();
                        }

                        var export = new SchemaExport(x);                                                                                                        
                        export.Drop(true, true);                                                
                        export.Execute(false, true, false);
                    }

                    if (ActionToApplyOnNHibernateConfiguration != null)
                    {
                        ActionToApplyOnNHibernateConfiguration(x);
                    }
                })
                .BuildSessionFactory();

            var sessionScopeFactory = new NHibernateSessionScopeFactory(sessionFactory);
            configurator.Configuration.Container.PutWithNameOrDefault<ISessionScopeFactory>(sessionScopeFactory, configurator.Name);

            LatestConfigurationResult = new ConfigurationResult(sessionFactory, configuration);
        }
                
        private static ConfigurationResult LatestConfigurationResult { get; set; }
    }
}
