namespace TestInfra.Pom.V3.Infrastructure;

public sealed class PageComponentNotReadyException(string message, Exception innerException)
    : InvalidOperationException(message, innerException);
