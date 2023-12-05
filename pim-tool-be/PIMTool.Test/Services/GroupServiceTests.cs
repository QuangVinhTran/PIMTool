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
        }

        [Test]
        public async Task TestGetGroups()
        {
            // Arrange

            // Act
            var entities = await _groupService.GetGroups();

            // Assert
            Assert.That(entities.Count(), Is.GreaterThan(0));
        }

        [Test]
        public async Task TestGetAsync()
        {
            // Arrange
            int id = 1;

            // Act
            var entity = await _groupService.GetAsync(id);

            // Assert
            Assert.That(entity, Is.Not.Null);
        }

        [Test]
        [Ignore("This test is ignored because it is not implemented yet.")]
        public async Task TestAddAsync()
        {
            // Arrange
            var group = new Group
            {
                GroupLeaderId = 9
            };

            // Act
            await _groupService.AddAsync(group);

            // Assert
            Assert.That(group.Id, Is.GreaterThan(0));
        }

        [Test]
        [Ignore("Ignore a test")]
        public async Task TestUpdateAsync()
        {
            // Arrange
            var updateGroup = await _groupService.GetAsync(5);
            updateGroup.GroupLeaderId = 10;
            // Act
            await _groupService.UpdateAsync();
            var entity = await _groupService.GetAsync(5);
            // Assert
            Assert.That(entity.GroupLeaderId, Is.EqualTo(10));
        }

        [Test]
        [Ignore("Ignore a test")]
        public async Task TestDeleteAsync()
        {
            // Arrange
            var delGroup = await _groupService.GetAsync(5);
            // Act
            await _groupService.DeleteAsync(delGroup);
            var entity = await _groupService.GetAsync(5);
            // Assert
            Assert.That(entity, Is.Null);
        }
    }
}
