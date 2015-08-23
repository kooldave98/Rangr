namespace App.Persistence.EF.Infrastructure {

    public class DatabaseFactory : Disposable, IDatabaseFactory {

        public DatabaseFactory ( IConnectionSettings the_connection_settings ) {
            this.connection_settings = the_connection_settings;
        }

        public EntityDbContext DataContext {
            get { return _dataContext; }
        }

        public EntityDbContext Get ( ) {
            return _dataContext ?? (_dataContext = new EntityDbContext( connection_settings.ConnectionString )); //_dataContext = new EntityDbContext( "EntityDbContext" ) );
        }

        protected override void DisposeCore ( ) {
            if ( _dataContext != null )
                _dataContext.Dispose( );
        }

        private readonly IConnectionSettings connection_settings;
        private EntityDbContext _dataContext;

    }

}