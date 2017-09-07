using System;

namespace AE.AuditPlanning.Storage.Entities.Graph.Exceptions
{
    [Serializable]
    public class EdgeException : Exception
    {
        public EdgeException(string message)
            : base(message)
        {

        }
    }
}