using Generics.Entities;

namespace Generics.Repositories
{
	public class GenericRepository<T> where T:class, IEntity
    {
		private readonly List<T> _items = new();

		public T GetById(int id)
        {
			return _items.Single(item => item.Id == id);
        }

		public void Add(T item)
		{
			item.Id = _items.Any()
				? _items.Max(item => item.Id) + 1
				: 1;
            _items.Add(item);
		}

		public void Remove(T item)
        {
			_items.Remove(item);
        }

		public void Save()
        {
			foreach(var item in _items)
            {
                Console.WriteLine(item);
            }
        }
	}

}

