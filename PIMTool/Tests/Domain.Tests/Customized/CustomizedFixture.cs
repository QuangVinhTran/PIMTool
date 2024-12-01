using System;
using AutoFixture;

namespace Domain.Tests.Customized
{
	public class CustomizedFixture : Fixture
	{
		public CustomizedFixture()
		{
            Behaviors.Remove(new ThrowingRecursionBehavior());
            Behaviors.Add(new OmitOnRecursionBehavior());

            this.Customize(new EmployeeSpecimenBuilder());
            this.Customize(new ProjectSpecimenBuilder());
            this.Customize(new GroupSpecimenBuilder());
        }
	}
}

