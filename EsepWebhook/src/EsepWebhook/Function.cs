using System;
using Amazon.Lambda.Core;
using Newtonsoft.Json;

public class Function
{
    private static readonly HttpClient client = new HttpClient();
    private const string SlackUrl = Environment.GetEnvironmentVariable("SLACK_URL");

    public async Task FunctionHandler(SQSEvent sqsEvent, ILambdaContext context)
    {
        foreach (var record in sqsEvent.Records)
        {
            var payload = JsonConvert.DeserializeObject<dynamic>(record.Body);
            var issueUrl = payload.issue.html_url.ToString();

            var slackMessage = new
            {
                text = $"A new issue was created: {issueUrl}"
            };

            var content = new StringContent(JsonConvert.SerializeObject(slackMessage), Encoding.UTF8, "application/json");
            await client.PostAsync(SlackUrl, content);
        }
    }
}
