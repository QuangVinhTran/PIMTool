using System;
using AutoFixture;
using Domain.Entities;

namespace Domain.Tests.Customized
{
	public class ProjectSpecimenBuilder : ICustomization
	{
		public ProjectSpecimenBuilder()
		{
		}

        public void Customize(IFixture fixture)
        {
            fixture.Customize<Project>(composer =>
           composer
               .Without(proj => proj.Group)
               .Without(proj => proj.ProjectEmployees)
            );
        }
    }
}

