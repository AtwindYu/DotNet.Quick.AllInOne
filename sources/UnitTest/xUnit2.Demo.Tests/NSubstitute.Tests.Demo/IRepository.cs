using System.Linq;

namespace Sunkong.CheckingInSystem.Test.NSubstitute.Tests.Demo
{
    public interface IRepository<T>
    {
        T GetById(object id);

        void Insert(T entity);

        void Update(T entity);

        void Delete(T entity);

        IQueryable<T> Table { get; }

        IQueryable<T> TableNoTracking { get; }

        void Attach(T entity);
    }
}