namespace TyGoTech.Tool.CodeQuality;

using System.Threading.Tasks;

public sealed class ResourceMapDownloader : IDisposable
{
    private readonly Uri _resources;

    private readonly DirectoryInfo _repo;

    private readonly HttpClient _client;

    public ResourceMapDownloader(Uri resources, DirectoryInfo repo)
    {
        this._resources = resources;
        this._repo = repo;
        this._client = new();
    }

    public async Task DownloadAsync(ResourceMap map)
    {
        var local = map.GetLocal(this._repo);
        if (local.Exists && map.Preserve)
        {
            Console.WriteLine($"File '{local}' already exists, skipping.");
            return;
        }

        var remote = map.GetRemote(this._resources);
        try
        {
            using var stream = await this._client.GetStreamAsync(this._resources);
            using var file = local.Open(FileMode.Create, FileAccess.Write, FileShare.None);
            await stream.CopyToAsync(file);

            Console.WriteLine($"Saved the content of '{remote} to '{local}'.");
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Failed to save the content of '{remote} to '{local}'. Error: {ex.Message}.",
                ex);
        }
    }

    public void Dispose() => ((IDisposable)this._client).Dispose();
}