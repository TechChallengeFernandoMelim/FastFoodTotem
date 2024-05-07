using Amazon.Runtime;
using Amazon.SQS.Model;
using Amazon.SQS;
using FastFoodTotem.Logger;
using Amazon;
using Moq;

namespace FastFoodTotem.Tests.Infrastructure;

public class SqsLoggerTest
{
    Mock<BasicAWSCredentials> _credentialsMock;

    public SqsLoggerTest()
    {
        _credentialsMock = new Mock<BasicAWSCredentials>("accesskey", "secretekeys");
    }

    [Fact]
    public async Task Log_ValidInput_Success()
    {
        // Arrange
        var stackTrace = "Test stack trace";
        var message = "Test message";
        var exception = "Test exception";

        var sqsClientMock = new Mock<AmazonSQSClient>(_credentialsMock.Object, RegionEndpoint.USEast1);
        sqsClientMock.Setup(s => s.SendMessageAsync(It.IsAny<SendMessageRequest>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(new SendMessageResponse());

        var sqsService = new SqsLogger(sqsClientMock.Object);

        // Act
        await sqsService.Log(stackTrace, message, exception);

        // Assert
        sqsClientMock.Verify(s => s.SendMessageAsync(It.IsAny<SendMessageRequest>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
