namespace Yoyoyopo5.ValueWrapper.Benchmarks;

[Wrapper<double>]
public readonly partial record struct Meters(double Value);

[Wrapper<string>]
public readonly partial record struct ReportStatus
{
    public static ReportStatus Accepted { get; } = new("accepted");
    public static ReportStatus Rejected { get; } = new("rejected");
    public static ReportStatus Unknown { get; } = new("unknown");

    public string Value { get; }
    private ReportStatus(string value) => Value = value;

    internal static ReportStatus Create(string value) => new(value);
}