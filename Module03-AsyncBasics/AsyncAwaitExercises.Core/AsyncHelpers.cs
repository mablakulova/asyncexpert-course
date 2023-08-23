using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwaitExercises.Core
{
    public class AsyncHelpers
    {
        public static async Task<string> GetStringWithRetries(HttpClient client, string url, int maxTries = 3, CancellationToken token = default)
        {
            if (maxTries < 2)
                throw new ArgumentException($"{nameof(maxTries)} must be at least 2");

            TimeSpan nextDelay = TimeSpan.FromSeconds(1);

            for (int i = 0; i < maxTries - 1; i++)
            {
                try
                {
                    return await GetResponseAsync(client, url, token);
                }
                catch
                {
                    await Task.Delay(nextDelay, token);
                    nextDelay *= 2;
                }
            }

            await Task.Delay(nextDelay, token);
            return await GetResponseAsync(client, url, token);
        }

        static async Task<string> GetResponseAsync(HttpClient client, string url, CancellationToken token)
        {
            var response = await client.GetAsync(url, token);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
