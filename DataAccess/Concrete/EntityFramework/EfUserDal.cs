using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;
using Remotion.Linq.Parsing.Structure.IntermediateModel;

namespace DataAccess.Concrete.EntityFramework
{ 
    public class EfUserDal : EfEntityRepositoryBase<User, NorthwindContext>, IUserDal
    {
        public List<OperationClaim> GetClaims(User user)
        {

            using (var context = new NorthwindContext())
            {
                var result = from OperationClaims in context.OperationClaims
                             join UserOperationClaims in context.UserOperationClaims
                             on OperationClaims.Id equals UserOperationClaims.OperationClaimsId
                             where UserOperationClaims.UserId == user.Id
                             select new OperationClaim { Id = OperationClaims.Id, Name = OperationClaims.Name };
                return result.ToList();
            }

        }
    }
}
