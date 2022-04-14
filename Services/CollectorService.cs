using Grpc.Core;
using Collector;

namespace Collector.Services;

public class CollectorService : Collector.CollectorBase
{
    private readonly ILogger<CollectorService> _logger;
    public CollectorService(ILogger<CollectorService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Initialize new application session.
    /// </summary>
    public override Task<TrackReply> Init(InitRequest request, ServerCallContext context)
    {
        // Generate new random id. All the Track requests should use this id.
        var sessionId = Guid.NewGuid().ToString();

        return Task.FromResult(new TrackReply
        {
            Ack = true,
            SessionId = new Guid().ToString(),
            MessageId = request.MessageId,
        });
    }

    /// <summary>
    /// Track application events.
    /// </summary>
    public override Task<TrackReply> Track(TrackRequest request, ServerCallContext context)
    {
        return Task.FromResult(new TrackReply
        {
            Ack = true,
            SessionId = request.SessionId,
            MessageId = request.MessageId,
        });
    }

    /// <summary>
    /// Track application's error. Stora traceback and other information.
    ///
    /// Client application doesn't need to call this method directly.
    /// </summary>
    public override Task<TrackReply> TrackError(TrackErrorRequest request, ServerCallContext context)
    {
        return Task.FromResult(new TrackReply
        {
            Ack = true,
            SessionId = request.SessionId,
            MessageId = request.MessageId,
        });
    }
}
