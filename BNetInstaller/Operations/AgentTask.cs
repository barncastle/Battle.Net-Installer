using System.Runtime.CompilerServices;
using BNetInstaller.Endpoints;

namespace BNetInstaller.Operations;

internal abstract class AgentTask<T>(Options options)
{
    private readonly Options _options = options;
    private TaskAwaiter<T>? _awaiter;

    public TaskAwaiter<T> GetAwaiter() => _awaiter ??= InnerTask().GetAwaiter();

    public T GetResult() => GetAwaiter().GetResult();

    protected abstract Task<T> InnerTask();

    protected async Task<bool> PrintProgress(ProductEndpoint endpoint)
    {
        var locale = _options.Locale.ToString();
        var cursor = (Left: 0, Top: 0);

        if (_options.ConsoleEnvironment)
            cursor = Console.GetCursorPosition();

        static void Print(string label, object value) =>
            Console.WriteLine("{0,-20}{1,-20}", label, value);

        while (true)
        {
            var stats = await endpoint.Get();

            // check for completion
            var complete = stats["download_complete"]?.GetValue<bool?>();

            if (complete == true)
                return true;

            // get progress percentage and playability
            var progress = stats["progress"]?.GetValue<float?>();
            var playable = stats["playable"]?.GetValue<bool?>();

            if (!progress.HasValue)
                return false;

            // some non-console environments don't support
            // cursor positioning or line rewriting
            if (_options.ConsoleEnvironment)
            {
                Console.SetCursorPosition(cursor.Left, cursor.Top);
                Print("Downloading:", _options.Product);
                Print("Language:", locale);
                Print("Directory:", _options.Directory);
                Print("Progress:", progress.Value.ToString("P4"));
                Print("Playable:", playable.GetValueOrDefault());
            }
            else
            {
                Print("Progress:", progress.Value.ToString("P4"));
            }

            await Task.Delay(2000);

            // exit @ 100%
            if (progress == 1f)
                return true;
        }
    }
}
