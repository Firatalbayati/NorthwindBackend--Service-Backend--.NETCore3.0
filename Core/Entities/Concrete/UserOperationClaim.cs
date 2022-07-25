using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities;

namespace Core.Entities.Concrete
{
    public class UserOperationClaims : IEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int OperationClaimsId { get; set; }
    }
}
