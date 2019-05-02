using System.Data.Entity;
using App.Library.CodeStructures.Behavioral;
using App.Library.CodeStructures.Creational;

namespace App.Library.EntityFramework.Initialization {

    /// <summary>
    ///     Database initializer that uses injected policies to decide if the 
    /// database should be built or seeded.
    /// </summary>
    /// <typeparam name="C">
    ///     <see cref"DbContext"/> that is to be built.
    /// </typeparam>
    public class StrategyInitializer<C> : IDatabaseInitializer<C> where C : DbContext {

        public StrategyInitializer 
            ( IPolicy<C> should_create_database_policy 
            , IBuilder<C> the_database_builder
            , IPolicy<C> should_seed_database_policy 
            , ISeeder<C> the_database_seeder ) {

            Guard.IsNotNull( should_create_database_policy, "should_create_database_policy" );
            Guard.IsNotNull( the_database_builder, "the_database_builder" );

            Guard.IsNotNull( should_seed_database_policy, "should_seed_database_policy" );
            Guard.IsNotNull( the_database_seeder, "the_database_seeder" );

            should_create_database = should_create_database_policy;
            database_builder = the_database_builder;

            should_seed_database = should_seed_database_policy;
            database_seeder = the_database_seeder;
        }

        /// <summary>
        ///     Initializes the database and seed's it based on the create 
        /// database and seed database policies
        /// </summary>
        /// <remarks>
        ///     The actual creation and seeding seeding are performed by
        /// the builder and seeder. 
        /// </remarks>
        /// <param name="context">
        ///     The context to be initialized.
        /// </param>
        public void InitializeDatabase ( C context ) {
            Guard.IsNotNull( context, "context" );

            if ( should_create_database.decide_for( context ) ) {
                database_builder.build( context );
            }

            if ( should_seed_database.decide_for( context )) {
                database_seeder.seed( context );
            }
        }

        // policy that decides whether to create the database
        private readonly IPolicy<C> should_create_database;
        
        // builder used to create the database if required
        private readonly IBuilder<C> database_builder;

        // policy that decides whether to seed the database
        private readonly IPolicy<C> should_seed_database;

        // seeder that populates the database
        private ISeeder<C> database_seeder;

    }

}