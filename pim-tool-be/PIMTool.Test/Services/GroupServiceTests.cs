using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Interfaces.Services;
using PIMTool.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMTool.Test.Services
{
    public class GroupServiceTests : BaseTest
    {
        private IGroupService _groupService = null!;

        [SetUp]
        public void SetUp()
        {
            _groupService = ServiceProvider.GetRequiredService<IGroupService>();
            if (Context.Database.IsInMemory())
            {
                var emps = new List<Employee>()
                {
                    new Employee
                    {
                       FirstName = "Dung",
                       LastName = "Nguyen",
                       Visa = "NTD",
                       BirthDate = new DateTime(2003, 03, 09),
                       Version = new byte[0]
                    },
                    new Employee
                    {
                       FirstName = "Van",
                       LastName = "Thanh",
                       Visa = "VTV",
                       BirthDate = new DateTime(2003, 07, 14),
                       Version = new byte[0]
                    },
                    new Employee
                    {
                       FirstName = "Dat",
                       LastName = "Do",
                       Visa = "DTD",
                       BirthDate = new DateTime(2003, 10, 23),
                       Version = new byte[0]
                    },
                    new Employee
                    {
                       FirstName = "Hao",
                       LastName = "Nguyen",
                       Visa = "NVH",
                       BirthDate = new DateTime(1968, 08, 15),
                       Version = new byte[0]
                    },
                };
                Context.Employees.AddRangeAsync(emps);
                Context.SaveChangesAsync();
            }
        }

        [Test]
        public async Task TestGetGroups()
        {
            // Arrange
            var groups = new List<Group>()
                {
                    new Group
                    {
                        GroupLeaderId = 1
                    },
                    new Group
                    {
                        GroupLeaderId = 2
                    },
                    new Group
                    {
                        GroupLeaderId = 3
                    },
                    new Group
                    {
                        GroupLeaderId = 4
                    },
                };
            await Context.Groups.AddRangeAsync(groups);
            await Context.SaveChangesAsync();

            // Act
            var entities = await _groupService.GetGroups();

            // Assert
            Assert.That(entities.Count(), Is.GreaterThan(0));
        }

        [Test]
        public async Task TestGetAsync()
        {
            // Arrange
            var groups = new List<Group>()
                {
                    new Group
                    {
                        GroupLeaderId = 1
                    },
                    new Group
                    {
                        GroupLeaderId = 2
                    },
                    new Group
                    {
                        GroupLeaderId = 3
                    },
                    new Group
                    {
                        GroupLeaderId = 4
                    },
                };
            await Context.Groups.AddRangeAsync(groups);
            await Context.SaveChangesAsync();
            int id = groups[0].Id;

            // Act
            var entity = await _groupService.GetAsync(id);

            // Assert
            Assert.That(entity, Is.Not.Null);
        }

        [Test]
        public async Task TestAddAsync()
        {
            // Arrange
            var groups = new List<Group>()
                {
                    new Group
                    {
                        GroupLeaderId = 1
                    },
                    new Group
                    {
                        GroupLeaderId = 2
                    },
                    new Group
                    {
                        GroupLeaderId = 3
                    },
                    new Group
                    {
                        GroupLeaderId = 4
                    },
                };
            await Context.Groups.AddRangeAsync(groups);
            await Context.SaveChangesAsync();

            var newEmp = new Employee
            {
                FirstName = "Chau",
                LastName = "Tran",
                Visa = "TMC",
                BirthDate = new DateTime(2003, 03, 09),
                Version = new byte[0]
            };
            await Context.Employees.AddAsync(newEmp);
            await Context.SaveChangesAsync();

            var group = new Group
            {
                GroupLeaderId = newEmp.Id
            };


            // Act
            await Context.Groups.AddAsync(group);
            await Context.SaveChangesAsync();
            var entities = await _groupService.GetGroups();

            // Assert
            Assert.That(entities.Count(), Is.EqualTo(5));
        }

        [Test]
        public async Task TestUpdateAsync()
        {
            // Arrange
            var groups = new List<Group>()
                {
                    new Group
                    {
                        GroupLeaderId = 1
                    },
                    new Group
                    {
                        GroupLeaderId = 2
                    },
                    new Group
                    {
                        GroupLeaderId = 3
                    },
                    new Group
                    {
                        GroupLeaderId = 4
                    },
                };
            await Context.Groups.AddRangeAsync(groups);
            await Context.SaveChangesAsync();

            var emp = new Employee
            {
                FirstName = "Chau",
                LastName = "Tran",
                Visa = "TMC",
                BirthDate = new DateTime(2003, 03, 09),
                Version = new byte[0]
            };
            await Context.Employees.AddAsync(emp);
            await Context.SaveChangesAsync();

            var updateGroup = await _groupService.GetAsync(1);
            updateGroup.GroupLeaderId = emp.Id;
            // Act
            await _groupService.UpdateAsync();
            var entity = await _groupService.GetAsync(1);
            // Assert
            Assert.That(entity.GroupLeaderId, Is.EqualTo(emp.Id));
        }

        [Test]
        public async Task TestDeleteAsync()
        {
            // Arrange
            var groups = new List<Group>()
                {
                    new Group
                    {
                        GroupLeaderId = 1
                    },
                    new Group
                    {
                        GroupLeaderId = 2
                    },
                    new Group
                    {
                        GroupLeaderId = 3
                    },
                    new Group
                    {
                        GroupLeaderId = 4
                    },
                };
            await Context.Groups.AddRangeAsync(groups);
            await Context.SaveChangesAsync();
            var delGroup = await _groupService.GetAsync(4);
            // Act
            await _groupService.DeleteAsync(delGroup);
            var entities = await _groupService.GetGroups();
            // Assert
            Assert.That(entities.Count(), Is.EqualTo(3));
        }
    }
}
