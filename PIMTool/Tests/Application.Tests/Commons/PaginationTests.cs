using Application.Commons;
using AutoFixture;
using Domain.Tests;
using FluentAssertions;

namespace Application.Tests.Commons;

public class PaginationTests : SetupTest
{
    [Fact]
        public void Pagination_PaginationFirstPage_ShouldReturnExpectedObject()
        {
            //arrange
            var mockItems = _fixture.Build<string>().CreateMany(100).ToList();
            
            //act
            var pagination = new Pagination<string>()
            {
                Items = mockItems,
                PageSize = 10,
                PageIndex = 0,
                TotalItemsCount = mockItems.Count,
            };

            //assert
            pagination.Previous.Should().BeFalse();
            pagination.Next.Should().BeTrue();
            pagination.Items.Should().NotBeNullOrEmpty();
            pagination.TotalItemsCount.Should().Be(100);
            pagination.TotalPagesCount.Should().Be(10);
            pagination.PageIndex.Should().Be(0);
            pagination.PageSize.Should().Be(10);
        }

        [Fact]
        public void Pagination_PaginationSecondPage_ShouldReturnExpectedObject()
        {
            //arrange
            var mockItems = _fixture.Build<string>().CreateMany(100).ToList();

            //act
            var pagination = new Pagination<string>
            {
                Items = mockItems,
                PageSize = 10,
                PageIndex = 1,
                TotalItemsCount = mockItems.Count,
            };

            //assert
            pagination.Previous.Should().BeTrue();
            pagination.Next.Should().BeTrue();
            pagination.Items.Should().NotBeNullOrEmpty();
            pagination.TotalItemsCount.Should().Be(100);
            pagination.TotalPagesCount.Should().Be(10);
            pagination.PageIndex.Should().Be(1);
            pagination.PageSize.Should().Be(10);
        }

        [Fact]
        public void Pagination_PaginationLastPage_ShouldReturnExpectedObject()
        {
            //arrange
            var mockItems = _fixture.Build<string>().CreateMany(101).ToList();

            //act
            var pagination = new Pagination<string>
            {
                Items = mockItems,
                PageSize = 10,
                PageIndex = 10,
                TotalItemsCount = mockItems.Count,
            };

            //assert
            pagination.Previous.Should().BeTrue();
            pagination.Next.Should().BeFalse();
            pagination.Items.Should().NotBeNullOrEmpty();
            pagination.TotalItemsCount.Should().Be(101);
            pagination.TotalPagesCount.Should().Be(11);
            pagination.PageIndex.Should().Be(10);
            pagination.PageSize.Should().Be(10);
        }
}