namespace BNetInstaller.Operations;

internal sealed class InstallProductTask(Options options, AgentApp app) : AgentTask<bool>(options)
{
    private readonly Options _options = options;
    private readonly AgentApp _app = app;

    protected override async Task<bool> InnerTask()
    {
        // initiate the download
        _app.UpdateEndpoint.Model.Uid = _options.UID;
        await _app.UpdateEndpoint.Post();

        // first try the install endpoint
        if (await PrintProgress(_app.InstallEndpoint.Product))
            return true;

        // then try the update endpoint instead
        if (await PrintProgress(_app.UpdateEndpoint.Product))
            return true;

        // failing that another agent or the BNet app has
        // probably taken control of the install
        Console.WriteLine("Another application has taken over. Launch the Battle.Net app to resume installation.");
        return false;
    }
}
