using System;
using System.Threading.Tasks;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.API.Middlewares;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Xunit;

namespace Restaurants.API.Tests.Middlewares;

[TestSubject(typeof(ErrorHandlingMiddleware))]
public class ErrorHandlingMiddlewareTest
{

    [Fact]
    public async Task InvokeAsync_WhenNoExceptionThrown_ShouldCallNextDelegate()
    {
        // Arrange
        
        var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var middleware = new ErrorHandlingMiddleware(loggerMock.Object);
        var context = new DefaultHttpContext();
        var nextDelegateMock = new Mock<RequestDelegate>();

        // Act
        
        await middleware.InvokeAsync(context, nextDelegateMock.Object);
        
        // Assert
        
        nextDelegateMock.Verify(next => next(context), Times.Once);
    }

    [Fact]
    public async Task InvokeAsync_WhenNotFoundExceptionThrown_ShouldSetStatusCode404()
    {
        // Arrange
        
        var context = new DefaultHttpContext();
        var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var middleware = new ErrorHandlingMiddleware(loggerMock.Object);
        var notFoundException = new NotFoundException(nameof(Restaurant), "1");
        
        // Act
        await middleware.InvokeAsync(context, _ => throw notFoundException);
        
        // Assert

        context.Response.StatusCode.Should().Be(404);
    }
    
    [Fact]
    public async Task InvokeAsync_WhenForbidExceptionThrown_ShouldSetStatusCode403()
    {
        // Arrange
        
        var context = new DefaultHttpContext();
        var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var middleware = new ErrorHandlingMiddleware(loggerMock.Object);
        var forbidException = new ForbidException();
        
        // Act
        await middleware.InvokeAsync(context, _ => throw forbidException);
        
        // Assert

        context.Response.StatusCode.Should().Be(403);
    }
    
    [Fact]
    public async Task InvokeAsync_WhenGenericdExceptionThrown_ShouldSetStatusCode500()
    {
        // Arrange
        
        var context = new DefaultHttpContext();
        var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var middleware = new ErrorHandlingMiddleware(loggerMock.Object);
        var genericException = new Exception();
        
        // Act
        await middleware.InvokeAsync(context, _ => throw genericException);
        
        // Assert

        context.Response.StatusCode.Should().Be(500);
    }
}