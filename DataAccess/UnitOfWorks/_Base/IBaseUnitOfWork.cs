namespace DataAccess.UnitOfWorks._Base
{
    public interface IBaseUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync(string transactionBy);
    }
}