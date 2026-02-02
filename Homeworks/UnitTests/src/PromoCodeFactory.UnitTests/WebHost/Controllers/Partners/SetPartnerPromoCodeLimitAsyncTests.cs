using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Controllers;
using PromoCodeFactory.WebHost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PromoCodeFactory.UnitTests.WebHost.Controllers.Partners
{
    public class SetPartnerPromoCodeLimitAsyncTests
    {
        private readonly Mock<IRepository<Partner>> _partnersRepositoryMock;
        private readonly PartnersController _controller;

        public SetPartnerPromoCodeLimitAsyncTests()
        {
            _partnersRepositoryMock = new Mock<IRepository<Partner>>();
            _controller = new PartnersController(_partnersRepositoryMock.Object);
        }

        [Fact]
        public async Task GetPartnersAsync_ReturnsListOfPartners()
        {
            // Arrange
            var partnerId = Guid.NewGuid();
            var partners = new List<Partner>
            {
                new Partner
                {
                    Id = partnerId,
                    Name = "Partner 1",
                    NumberIssuedPromoCodes = 10,
                    PartnerLimits = new List<PartnerPromoCodeLimit>()
                }
            };

            _partnersRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(partners.AsEnumerable());
            
            // Act
            var result = await _controller.GetPartnersAsync();

            // Assert
            var actionResult = Assert.IsType<ActionResult<List<PartnerResponse>>>(result);
            var okResult = actionResult.Result as OkObjectResult;

            Assert.NotNull(okResult);
            var responseValue = Assert.IsType<List<PartnerResponse>>(okResult.Value);

            // Формируем ожидаемый ответ
            var expectedResponse = partners.Select(p => new PartnerResponse
            {
                Id = p.Id,
                Name = p.Name,
                NumberIssuedPromoCodes = p.NumberIssuedPromoCodes,
                IsActive = true,
                PartnerLimits = p.PartnerLimits.Select(l => new PartnerPromoCodeLimitResponse
                {
                    Id = l.Id,
                    PartnerId = l.PartnerId,
                    Limit = l.Limit,
                    CreateDate = l.CreateDate.ToString("dd.MM.yyyy HH:mm:ss"),
                    EndDate = l.EndDate.ToString("dd.MM.yyyy HH:mm:ss"),
                    CancelDate = l.CancelDate?.ToString("dd.MM.yyyy HH:mm:ss"),
                }).ToList()
            }).ToList();

            responseValue.Should().BeEquivalentTo(expectedResponse);
        }


        [Fact]
        public async Task GetPartnerLimitAsync_PartnerNotFound_ReturnsNotFound()
        {
            // Arrange
            var partnerId = Guid.NewGuid();
            var limitId = Guid.NewGuid(); // Этот ID не имеет значения, так как партнера нет.

            // Настраиваем мок, чтобы он возвращал null для указанного ID
            _partnersRepositoryMock.Setup(repo => repo.GetByIdAsync(partnerId)).ReturnsAsync((Partner)null);

            // Act
            var result = await _controller.GetPartnerLimitAsync(partnerId, limitId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task SetPartnerPromoCodeLimitAsync_PartnerNotFound_ReturnsNotFound()
        {
            // Arrange
            var partnerId = Guid.NewGuid();
            var request = new SetPartnerPromoCodeLimitRequest { Limit = 5 };
            _partnersRepositoryMock.Setup(repo => repo.GetByIdAsync(partnerId)).ReturnsAsync((Partner)null);

            // Act
            var result = await _controller.SetPartnerPromoCodeLimitAsync(partnerId, request);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task SetPartnerPromoCodeLimitAsync_PartnerInactive_ReturnsBadRequest()
        {
            // Arrange
            var partnerId = Guid.NewGuid();
            var partner = new Partner { Id = partnerId, IsActive = false };
            _partnersRepositoryMock.Setup(repo => repo.GetByIdAsync(partnerId)).ReturnsAsync(partner);
            var request = new SetPartnerPromoCodeLimitRequest { Limit = 5 };

            // Act
            var result = await _controller.SetPartnerPromoCodeLimitAsync(partnerId, request);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task SetPartnerPromoCodeLimitAsync_NewLimit_Success()
        {
            // Arrange
            var partnerId = Guid.NewGuid();
            var partner = new Partner { Id = partnerId, IsActive = true, PartnerLimits = new List<PartnerPromoCodeLimit>() };
            _partnersRepositoryMock.Setup(repo => repo.GetByIdAsync(partnerId)).ReturnsAsync(partner);
            var request = new SetPartnerPromoCodeLimitRequest { Limit = 10, EndDate = DateTime.Now.AddDays(30) };

            // Act
            var result = await _controller.SetPartnerPromoCodeLimitAsync(partnerId, request);

            // Assert
            result.Should().BeOfType<CreatedAtActionResult>();
            partner.PartnerLimits.Should().HaveCount(1);
            partner.PartnerLimits.First().Limit.Should().Be(10);
        }

        [Fact]
        public async Task CancelPartnerPromoCodeLimitAsync_PartnerNotFound_ReturnsNotFound()
        {
            // Arrange
            var partnerId = Guid.NewGuid();
            _partnersRepositoryMock.Setup(repo => repo.GetByIdAsync(partnerId)).ReturnsAsync((Partner)null);

            // Act
            var result = await _controller.CancelPartnerPromoCodeLimitAsync(partnerId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task CancelPartnerPromoCodeLimitAsync_PartnerInactive_ReturnsBadRequest()
        {
            // Arrange
            var partnerId = Guid.NewGuid();
            var partner = new Partner { Id = partnerId, IsActive = false };
            _partnersRepositoryMock.Setup(repo => repo.GetByIdAsync(partnerId)).ReturnsAsync(partner);

            // Act
            var result = await _controller.CancelPartnerPromoCodeLimitAsync(partnerId);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task CancelPartnerPromoCodeLimitAsync_Success()
        {
            // Arrange
            var partnerId = Guid.NewGuid();
            var limit = new PartnerPromoCodeLimit { Id = Guid.NewGuid(), CancelDate = null };
            var partner = new Partner { Id = partnerId, IsActive = true, PartnerLimits = new List<PartnerPromoCodeLimit> { limit } };
            _partnersRepositoryMock.Setup(repo => repo.GetByIdAsync(partnerId)).ReturnsAsync(partner);

            // Act
            var result = await _controller.CancelPartnerPromoCodeLimitAsync(partnerId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            limit.CancelDate.Should().NotBeNull();
        }
    }
}