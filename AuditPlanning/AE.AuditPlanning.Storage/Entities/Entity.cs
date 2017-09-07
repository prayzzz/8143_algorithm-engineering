using System;

using Newtonsoft.Json;

namespace AE.AuditPlanning.Storage.Entities
{
    [Serializable]
    public class Entity
    {
        public Entity(int id)
        {
            this.Id = id;
        }

        protected Entity()
        {
        }

        [JsonIgnore]
        public int? Id { get; set; }

        [JsonIgnore]
        public bool IsStored
        {
            get
            {
                return this.Id.HasValue && this.Id > 0;
            }
        }
    }
}