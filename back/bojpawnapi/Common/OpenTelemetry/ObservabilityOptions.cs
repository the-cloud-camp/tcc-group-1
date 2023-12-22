namespace bojpawnapi.Common.OpenTelemetry;

public class ObservabilityOptions
{
    public string ServiceName { get; set; } = default!;
    public string CollectorUrl { get; set; } = @"http://localhost:4317";
    public string CollectorProtocol { get; set; } = "Grpc";
    public Uri CollectorUri => new(this.CollectorUrl);
}
