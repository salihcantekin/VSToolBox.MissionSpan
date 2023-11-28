using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissionSpan.Console;
public class LogParser
{
    public static List<LogModel> ProcessLogs_Span(string[] logLines)
    {
        foreach (var line in logLines)
        {
            var lineSpan = line.AsSpan();

            var httpMethod = lineSpan.Slice(20, 3);
            if (!httpMethod.SequenceEqual("GET"))
                continue;

            var createdDate = DateTime.ParseExact(lineSpan[..19], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

            var durationIndex = lineSpan.LastIndexOf(' ');
            var durationStr = lineSpan.Slice(durationIndex + 1, line.Length - durationIndex - 3);
            int duration = int.Parse(durationStr);

            _ = new LogModel(duration, httpMethod.ToString(), createdDate);
        }

        return null;
    }

    public static List<LogModel> ProcessLogs_String(string[] logLines)
    {
        foreach (var line in logLines)
        {
            string httpMethod = line.Substring(20, 3);
            if (httpMethod != "GET")
                continue;

            string dateStr = line.Substring(0, 19);
            var createdDate = DateTime.ParseExact(dateStr, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

            var durationIndex = line.LastIndexOf(' ');
            string durationStr = line.Substring(durationIndex + 1, line.Length - durationIndex - 3);
            int duration = int.Parse(durationStr);

            _ = new LogModel(duration, httpMethod, createdDate);
        }

        return null;
    }

    public static List<LogModel> ProcessLogs_Split(string[] logLines)
    {
        foreach (var line in logLines)
        {
            string[] parts = line.Split([' '], 6);
            if (parts[2] != "GET")
                continue;

            var date = DateTime.ParseExact(parts[0] + parts[1], "yyyy-MM-ddHH:mm:ss", CultureInfo.InvariantCulture);
            var httpMethod = parts[2];
            int duration = int.Parse(parts[5].Replace("ms", ""));

            _ = new LogModel(duration, httpMethod, date);
        }

        return null;
    }

    public static List<LogModel> ProcessLogs_Regex(string[] logLines)
    {
        string pattern = @"(\d{4}-\d{2}-\d{2}) (\d{2}:\d{2}:\d{2}) (\w+) (/.+) (\d+) (\d+)ms";

        foreach (var line in logLines)
        {
            Match match = Regex.Match(line, pattern);

            if (match.Success)
            {
                var httpMethod = match.Groups[3].Value;

                if (httpMethod != "GET")
                    continue;

                var date = DateTime.ParseExact(match.Groups[1].Value + match.Groups[2].Value, "yyyy-MM-ddHH:mm:ss", CultureInfo.InvariantCulture);
                int duration = int.Parse(match.Groups[6].Value);

                _ = new LogModel(duration, httpMethod, date);
            }
        }

        return null;
    }
}
