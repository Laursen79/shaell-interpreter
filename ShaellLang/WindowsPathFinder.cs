using System;
using System.Collections.Generic;
using System.IO;

namespace ShaellLang;

public class WindowsPathFinder : IPathFinder
{
    public string GetAbsolutePath(string path)
    {
        if (File.Exists(path))
            return path;
        var pathWithExtensions = GetRelativePathWithExtensions(path);
        
        return GetFirstExisting(pathWithExtensions);
    }

    private string GetFirstExisting(IEnumerable<string> pathWithExtensions)
    {
        var paths = Environment.GetEnvironmentVariable("PATH")?.Split(Path.PathSeparator);
        foreach (var envPath in paths)
        {
            foreach (var pathWithExtension in pathWithExtensions)
            {
                var absolutePath = Path.Join(envPath, pathWithExtension);
                if (File.Exists(absolutePath))
                    return absolutePath;
            }
        }

        return null;
    }

    private IEnumerable<string> GetRelativePathWithExtensions(string path)
    {
        var extensions = Environment.GetEnvironmentVariable("PATHEXT")?.Split(Path.PathSeparator);
        var paths = new List<string>();
        
        paths.Add(path);

        foreach (var ext in extensions)
            paths.Add(path + ext);

        return paths;
    }
}