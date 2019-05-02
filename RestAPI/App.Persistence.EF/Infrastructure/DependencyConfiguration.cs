using System;
using System.Data.Entity;
using Ninject;
using Ninject.Activation;
using App.Library.EntityFramework.Contexts.ConnectionStringProviders;
using App.Library.Persistence;
using App.Library.Persistence.EF;
using App.Library.EntityFramework.Configuration;
using App.Library.Ninject.Configuration;
using App.Persistence.EF.Domain;

namespace App.Persistence.EF.Infrastructure {

    public class DependencyConfiguration : ADependencyConfiguration {

        public override void configure ( IKernel kernel, Func<IContext, object> scope ) {

            kernel
                .Bind( typeof( IEntityRepository<> ))
                .To( typeof(EntityRepository<> ))
                ;

            kernel
                .Bind( typeof( IQueryRepository<> ) )
                .To( typeof( QueryRepository<> ))
                ;

            kernel
                .Bind<IUnitOfWork>(  )
                .To<UnitOfWork >(  )
                ;

            kernel
                .Bind<DbContext>(  )
                .To<AppContext>(  )
                .InScope( scope )
                ;


            kernel
                .Bind<IConnectionStringProvider>(  )
                .To<AppConnectionStringProvider>()
                ;


            kernel
                .Bind<IModelConfiguration>()
                .To<ModelBuilder>()
                ;
        }

    }

}