namespace Collector.Models;

public enum EventType
{
    Init,
    Track,
    TrackError,
    Shutdown,
}

public class Event
{
    public string MessageId { get; set; }

    public string SessionId { get; set; }

    public EventType EventType { get; set; }

    public string? Name { get; set; }

    public string ApplicationId { get; set; }

    public string? ApplicationVersion { get; set; }

    public string ModuleName { get; set; }

    public string ModuleVersion { get; set; }

    public string? OSName { get; set; }

    public string? OSVersion { get; set; }

    public string? Locale { get; set; }

    public long? RAM { get; set; }

    public string? CPU { get; set; }

    public string? GPU { get; set; }

    public string? Arch { get; set; }

    public DateTime DateTime { get; set; }
}
