namespace BNetInstaller.Operations;

internal sealed class RepairProductTask : AgentTask<bool>
{
    private readonly Options _options;
    private readonly AgentApp _app;

    public RepairProductTask(Options options, AgentApp app) : base(options)
    {
        _options = options;
        _app = app;
    }

    protected override async Task<bool> InnerTask()
    {
        // initiate the repair
        _app.RepairEndpoint.Model.Uid = _options.UID;
        await _app.RepairEndpoint.Post();

        // run the repair endpoint
        if (await PrintProgress(_app.RepairEndpoint.Product))
            return true;

        Console.WriteLine("Unable to repair this product.");
        return false;
    }
}
