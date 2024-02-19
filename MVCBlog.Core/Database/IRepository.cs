using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using MVCBlog.Core.Entities;

namespace MVCBlog.Core.Database
{
    /// <summary>
    /// Interface for database access.
    /// </summary>
    public interface IRepository
    {

        IDbSet<BlogEntry> BlogEntries { get; }
        IDbSet<BlogEntryComment> BlogEntryComments { get; }
        IDbSet<BlogEntryPingback> BlogEntryPingbacks { get; }
        IDbSet<BlogEntryFile> BlogEntryFiles { get; }
        IDbSet<Image> Images { get; }
        IDbSet<Tag> Tags { get; }
        IDbSet<FeedStatistic> FeedStatistics { get; }

        #region Agregadas
        IDbSet<AspNetUser> AspNetUsers { get; }
        IDbSet<Category> Categories { get; }
        IDbSet<Province> Provinces { get; }
        IDbSet<Locality> Localities { get; }
        IDbSet<SocietyMonthlyFee> SocietyMonthlyFees { get; }

        IDbSet<ServiceFee> ServiceFees { get; }
        IDbSet<ServiceType> ServiceTypes { get; }
        IDbSet<Entities.Service> Services { get; }
        IDbSet<Entities.ServiceUser> ServiceUsers { get; }

        #endregion

        DbSet<T> Set<T>() where T : class;
        DbEntityEntry Entry(object entity);

        /// <summary>
        /// Asynchronously saves all changes made in this context to the underlying database.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous save operation.
        /// The task result contains the number of objects written to the underlying database.
        /// </returns>
        /// <remarks>
        /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
        /// that any asynchronous operations have completed before calling another method on this context.
        /// </remarks>
        Task<int> SaveChangesAsync();
    }
}
