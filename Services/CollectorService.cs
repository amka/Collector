using Grpc.Core;
using Collector;
using NATS.Client.JetStream;
using NATS.Client;
using System.Text;
using System.Text.Json;
using Collector.Models;

namespace Collector.Services;

public class CollectorService : Collector.CollectorBase
{
    private readonly ILogger<CollectorService> _logger;
    private readonly IJetStream _jetStream;

    public CollectorService(ILogger<CollectorService> logger, IJetStream jetStream)
    {
        _logger = logger;
        _jetStream = jetStream;
    }

    /// <summary>
    /// Initialize new application session.
    /// </summary>
    public override async Task<TrackReply> Init(InitRequest request, ServerCallContext context)
    {
        // Generate new random id. All the Track requests should use this id.
        var sessionId = Guid.NewGuid().ToString();

        var message = new Event {
            SessionId = sessionId,
            MessageId = request.MessageId
        };

        await enqueueMessage(message);

        return new TrackReply
        {
            Ack = true,
            SessionId = sessionId.ToString(),
            MessageId = request.MessageId,
        };
    }

    /// <summary>
    /// Track application events.
    /// </summary>
    public override async Task<TrackReply> Track(TrackRequest request, ServerCallContext context)
    {
        var message = new Event {
            SessionId = request.MessageId,
            MessageId = request.MessageId,
            EventType = EventType.Track,
        };

        await enqueueMessage(message);

        return new TrackReply
        {
            Ack = true,
            SessionId = request.SessionId,
            MessageId = request.MessageId,
        };
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

    async Task enqueueMessage(object message) {
        var msg = new Msg
        {
            Subject = "facta",
            Data = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message))
        };

        await _jetStream.PublishAsync(msg);
    }
}
