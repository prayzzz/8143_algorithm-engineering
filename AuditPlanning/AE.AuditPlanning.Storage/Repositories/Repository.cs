using System;
using System.Collections.Generic;
using System.Linq;
using AE.AuditPlanning.Storage.Entities;

namespace AE.AuditPlanning.Storage.Repositories
{
    public class Repository : IRepository
    {
        private readonly Dictionary<Type, IList<Entity>> repository;

        private static IRepository instance;

        private Repository()
        {
            this.repository = new Dictionary<Type, IList<Entity>>();
        }

        public static IRepository Current
        {
            get
            {
                return instance ?? (instance = new Repository());
            }
        }

        public int? Add<T>(T item) where T : Entity
        {
            if (item.IsStored)
            {
                return null;
            }

            IList<Entity> items;

            if (this.repository.TryGetValue(typeof(T), out items) && items != null)
            {
                item.Id = items.Any() ? items.Max(x => x.Id) + 1 : 1;
                items.Add(item);
                return item.Id;
            }

            item.Id = 1;
            this.repository.Add(typeof(T), new List<Entity> { item });

            return item.Id;
        }

        public void Delete<T>(int? id) where T : Entity
        {
            if (id == null)
            {
                return;
            }

            IList<Entity> items;

            if (this.repository.TryGetValue(typeof(T), out items) && items != null)
            {
                var itemToRemove = items.FirstOrDefault(x => x.Id == id);

                if (itemToRemove != null)
                {
                    items.Remove(itemToRemove);
                }
            }
        }

        public void Clear<T>() where T : Entity
        {
            this.repository.Remove(typeof(T));
        }

        public T GetById<T>(int? id) where T : Entity
        {
            if (id == null)
            {
                return null;
            }

            IList<Entity> items;

            if (this.repository.TryGetValue(typeof(T), out items) && items != null)
            {
                return items.FirstOrDefault(x => x.Id == id) as T;
            }

            return null;
        }

        public IEnumerable<T> GetList<T>() where T : Entity
        {
            IList<Entity> items;

            if (this.repository.TryGetValue(typeof(T), out items) && items != null)
            {
                return items.Cast<T>();
            }

            return new List<T>();
        }

        public IQueryable<T> GetQueryable<T>() where T : Entity
        {
            IList<Entity> items;

            if (this.repository.TryGetValue(typeof(T), out items) && items != null)
            {
                return items.Cast<T>().AsQueryable();
            }

            return new List<T>().AsQueryable();
        }
    }
}