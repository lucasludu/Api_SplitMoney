using Ardalis.Specification;
using Domain.Entities;
using System;

namespace Application.Specification._user
{
    public class MemberInGroupSpecification : Specification<GroupMember>, ISingleResultSpecification<GroupMember>
    {
        public MemberInGroupSpecification(Guid groupId, string userId)
        {
            Query.Where(m => m.GroupId == groupId && m.UserId == userId);
        }
    }
}
