using Generics.Entities;

namespace Generics.Repositories
{
    public interface IWriteRepository<in T>
    {
        void Add(T item);
        void Remove(T item);
        void Save();
    }

    public interface IReadRepository<out T>
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
    }

    public interface IRepository<T> : IReadRepository<T>, IWriteRepository<T>
        where T : IEntity
    {

    }
}


// Covariance - allowing the use of less specifc type (IEntity) on the generic interface
/*

namespace Generics.Repositories
{
    public interface IReadRepository<out T>
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
    }

    public interface IRepository<T> : IReadRepository<T> where T : IEntity
    {
        void Add(T item);
        void Remove(T item);
        void Save();
    }
}

*/

//Contravariance - allowing the use of more specifc type (Manager) on the generic interface
/*
namespace Generics.Repositories
{
    public interface IWriteRepository<in T>
    {
        void Add(T item);
        void Remove(T item);
        void Save();
    }

    public interface IReadRepository<out T>
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
    }

    public interface IRepository<T> : IReadRepository<T>, IWriteRepository<T>
        where T : IEntity
    {

    }
}

*/