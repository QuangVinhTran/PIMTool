using System;
using AutoFixture;
using Domain.Entities;

namespace Domain.Tests.Customized
{
    public class EmployeeSpecimenBuilder : ICustomization
    {
        public EmployeeSpecimenBuilder()
        {
        }
        public void Customize(IFixture fixture)
        {
            fixture.Customize<Employee>(composer =>
            composer
                .Without(emp => emp.ProjectEmployees)
            );
        }
    }
}
