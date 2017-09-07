using System.Collections.Generic;
using System.Linq;
using AE.AuditPlanning.Storage.Entities;

namespace AE.AuditPlanning.Storage.Repositories
{
    public interface IRepository
    {
        int? Add<T>(T item) where T : Entity;

        void Delete<T>(int? id) where T : Entity;

        T GetById<T>(int? id) where T : Entity;

        IEnumerable<T> GetList<T>() where T : Entity;

        IQueryable<T> GetQueryable<T>() where T : Entity;

        void Clear<T>() where T : Entity;
    }
}