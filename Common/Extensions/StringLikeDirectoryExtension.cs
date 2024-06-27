using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Common.Extensions;

public static class StringLikeDirectoryExtension
{
    public static void CreateDirectoryIfNotExist(this string dir)
    {
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);
    }

    public static void DeleteDirectoryIfExist(this string dir)
    {
        if (Directory.Exists(dir))
            Directory.Delete(dir, true);
    }

    public static void DeleteDirectoryFiles(this string dir)
    {
        dir.DeleteDirectoryIfExist();
        dir.CreateDirectoryIfNotExist();
    }

    public static void MoveFilesToDirectory(this string destDir, IEnumerable<string> paths)
    {
        foreach (var x in paths)
            File.Move(x, $"{destDir}\\{Path.GetFileName(x)}");
    }

    public static (IEnumerable<string> DelList, IEnumerable<string> AddList, IEnumerable<(string OrigPath, string NewPath)> EditList)
        MergeDirectoryFiles(this string newDir, string origDir)
    {
        var newFiles = newDir.DirectoryFiles()
            .Select(x => x.FileInfo())
            .ToList();

        var origFiles = origDir.DirectoryFiles()
            .Select(x => x.FileInfo())
            .ToList();

        var newFileDict = newFiles
            .ToDictionary(x => x.Name, x => x.Path);
        var origFileNameSet = new HashSet<string>(origFiles.Select(x => x.Name));

        var delList = origFiles
            .Where(x => !newFileDict.ContainsKey(x.Name))
            .Select(x => x.Path);

        var addList = newFiles
            .Where(x => !origFileNameSet.Contains(x.Name))
            .Select(x => x.Path);

        var editList = origFiles
            .Where(x => newFileDict.ContainsKey(x.Name) && (x.Path.Size() != newFileDict[x.Name].Size()
                                                            || !x.Path.Md5().IsEqual(newFileDict[x.Name].Md5())))
            .Select(x => (x.Path, newFileDict[x.Name]));

        return (delList, addList, editList);
    }

    public static void CopyDirectoryFiles(this string sourceDir, string destDir)
    {
        foreach (var x in sourceDir.DirectoryFiles())
        {
            var dest = $"{destDir}\\{Path.GetFileName(x)}";
            File.Copy(x, dest);
            File.SetCreationTime(dest, x.CreationTime());
        }
    }

    public static IList<string> DirectoryFiles(this string dir)
    {
        return !Directory.Exists(dir) ? new string[] { } : Directory.GetFiles(dir);
    }

    public static bool DirectoryIsEmpty(this string dir)
    {
        return !Directory.EnumerateFileSystemEntries(dir).Any();
    }
}
