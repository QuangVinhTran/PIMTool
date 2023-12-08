using System;
using AutoFixture;
using Domain.Entities;

namespace Domain.Tests.Customized
{
	public class GroupSpecimenBuilder : ICustomization
	{
		public GroupSpecimenBuilder()
		{
		}

        public void Customize(IFixture fixture)
        {
            fixture.Customize<Group>(composer =>
             composer
                 .With(g => g.GroupLeader, fixture.Create<Employee>()) 
            );
        }
    }
}

