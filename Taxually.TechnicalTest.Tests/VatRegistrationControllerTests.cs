using Microsoft.AspNetCore.Mvc;
using Moq;
using Taxually.TechnicalTest.Controllers;
using Taxually.TechnicalTest.Models;

namespace Taxually.TechnicalTest.Tests;

public class VatRegistrationControllerTests
{
    private readonly Mock<ITaxuallyHttpClient> _httpClientMock = new();
    private readonly Mock<ITaxuallyQueueClient> _queueClientMock = new();

    private VatRegistrationController _controller;

    public VatRegistrationControllerTests()
    {
        _controller = new(_httpClientMock.Object, _queueClientMock.Object);
    }

    [Test]
    public async Task Call_API_with_GB_Success()
    {
        var request = new VatRegistrationRequest
        {
            Country = "GB",
            CompanyId = Guid.NewGuid().ToString(),
            CompanyName = "GB Test Company"
        };

        var result = await _controller.Post(request);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.GetType(), Is.EqualTo(typeof(OkResult)));
        _httpClientMock.Verify(x => x.PostAsync(It.IsAny<string>(), It.IsAny<VatRegistrationRequest>()), Times.Once);
    }

    [Test]
    public async Task Call_API_with_FR_Success()
    {
        var request = new VatRegistrationRequest
        {
            Country = "FR",
            CompanyId = Guid.NewGuid().ToString(),
            CompanyName = "FR Test Company"
        };

        var result = await _controller.Post(request);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.GetType(), Is.EqualTo(typeof(OkResult)));
        _queueClientMock.Verify(x => x.EnqueueAsync(It.IsAny<string>(), It.IsAny<object>()));
    }

    [Test]
    public async Task Call_API_with_DE_Success()
    {
        var request = new VatRegistrationRequest
        {
            Country = "DE",
            CompanyId = Guid.NewGuid().ToString(),
            CompanyName = "DE Test Company"
        };

        var result = await _controller.Post(request);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.GetType(), Is.EqualTo(typeof(OkResult)));
        _queueClientMock.Verify(x => x.EnqueueAsync(It.IsAny<string>(), It.IsAny<string>()));
    }

    [Test]
    public async Task Call_API_Unsuccess()
    {
        var request = new VatRegistrationRequest
        {
            Country = "Other",
            CompanyId = Guid.NewGuid().ToString(),
            CompanyName = "Other Test Company"
        };

        var ex = Assert.ThrowsAsync<Exception>(() => _controller.Post(request));
        Assert.That(ex.Message, Is.EqualTo("Country not supported"));
    }
}