namespace Purchasely.Application.Interfaces;

public interface IBus
{
    Task PublishAsync<T>(T message, CancellationToken cancellationToken) where T : class;
}