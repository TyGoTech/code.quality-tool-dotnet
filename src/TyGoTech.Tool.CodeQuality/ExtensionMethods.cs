namespace TyGoTech.Tool.CodeQuality;

public static class ExtensionMethods
{
    public static async Task<bool> TryDownloadFileAsync(this HttpClient client, Uri uri, string fileName)
    {
        try
        {
            using var stream = await client.GetStreamAsync(uri);
            using var file = new FileStream(fileName, FileMode.Create);
            await stream.CopyToAsync(file);

            return true;
        }
#pragma warning disable CA1031
        catch (Exception ex)
        {
            Console.WriteLine($"Failed save content of {uri} to {fileName}. Error: {ex.Message}.");
            return false;
        }
    }

    public static Uri Append(this Uri uri, params string[] paths)
    {
        return new Uri(
            paths.Aggregate(
                uri.AbsoluteUri,
                (current, path) => $"{current.TrimEnd('/')}/{path.TrimStart('/')}"));
    }

    public static FileInfo Combine(this DirectoryInfo directory, string path) =>
        new(Path.Combine(directory.FullName, path));
}