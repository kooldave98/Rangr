using App.Integration.Configuration;
using App.Persistence.Models;
using System;

namespace App.Persistence.EF.Infrastructure
{
    /// <summary>
    /// The Unit of Work wrapper class
    /// </summary>
    public class RepositorySet : IDisposable
    {
        private IDatabaseFactory dbFactory;
        private EntityDbContext context;

        public RepositorySet()
        {
            //TODO: Good place for Dependency Injection
            dbFactory = new DatabaseFactory(new WebConnectionSettings());
            context = dbFactory.Get();
        }

        private IRepository<User> users;
        
        private IRepository<Post> posts;        
        public IRepository<User> Users
        {
            get { return this.users ?? (this.users = new BaseRepository<User>(dbFactory)); }
        }

        public IRepository<Post> Posts
        {
            get
            {
                if (this.posts == null)
                    this.posts = new BaseRepository<Post>(dbFactory);
                return this.posts;
            }
        }
        
        public void CommitChanges()
        {
            context.SaveChanges();
        }


        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
