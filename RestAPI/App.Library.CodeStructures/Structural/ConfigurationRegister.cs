using System.Collections.Generic;
using App.Library.CodeStructures.Behavioral;

namespace App.Library.CodeStructures.Structural {

    public class ConfigurationRegister<T,C> 
                : IConfiguration<T> 
        where C : IConfiguration<T> {

        public void configure ( T target_to_configure ) {
            
            Guard.IsNotNull( target_to_configure, "target_to_configure" );

            foreach ( var configuration in configurations ) {
                configuration.configure( target_to_configure );
            }             
        }

        public void register ( C configuration ) {
            Guard.IsNotNull( configuration, "configuration" );

            configurations.Add( configuration );
        }

        private readonly List<C> configurations = new List<C>( );

    }

}