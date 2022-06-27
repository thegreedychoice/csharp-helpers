using System;
using Generics.Entities;

namespace Generics.Repositories
{
	public static class RepositoryExtensions
	{
        public static void AddBatch<T>(this IWriteRepository<T> repository, T[] items)
            where T : IEntity
        {
            foreach (var item in items)
            {
                repository.Add(item);
            }
            repository.Save();
        }
    }
}

