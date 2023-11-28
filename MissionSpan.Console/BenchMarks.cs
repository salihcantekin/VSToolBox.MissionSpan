using MissionSpan.Console;

namespace ConsoleApp;

[MemoryDiagnoser]
[SimpleJob(BenchmarkDotNet.Jobs.RuntimeMoniker.Net60)]
[SimpleJob(BenchmarkDotNet.Jobs.RuntimeMoniker.Net70)]
[SimpleJob(BenchmarkDotNet.Jobs.RuntimeMoniker.Net80)]
public class BenchMarks
{
    private readonly string[] logLines;

    public BenchMarks()
    {
        logLines = File.ReadLines("request_logs.txt").ToArray();
    }

    [Benchmark]
    public List<LogModel> ProcessLogs_Span()
    {
        return LogParser.ProcessLogs_Span(logLines);
    }

    [Benchmark]
    public List<LogModel> ProcessLogs_String()
    {
        return LogParser.ProcessLogs_String(logLines);
    }


    [Benchmark]
    public List<LogModel> ProcessLogs_Split()
    {
        return LogParser.ProcessLogs_Split(logLines);
    }

    [Benchmark]
    public List<LogModel> ProcessLogs_Regex()
    {
        return LogParser.ProcessLogs_Split(logLines);
    }
}
