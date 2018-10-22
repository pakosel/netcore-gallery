using System;
using System.IO;
using System.Linq;

namespace aspnettcoreapp
{
    public static class FilesyStemHelper
    {
        public static DirectoryInfo GetDirContent(string inputPath, out string errorMsg)
        {
            var drives = Environment.GetLogicalDrives();
            var pathParts = inputPath.Split('/');
            errorMsg = "";

            foreach (var dr in drives)
            {
                var di = new DriveInfo(dr);

                if (!di.IsReady)
                {
                    Console.WriteLine("The drive {0} could not be read", di.Name);
                    continue;
                }

                var currentDir = di.RootDirectory;
                if (di.Name == "/")
                {
                    //for(int i=0; i<pathParts.Length; i++)
                    foreach (var dir in pathParts)
                    {
                        if (dir == "")
                            continue;
                        
                        currentDir = currentDir?.GetDirectories().FirstOrDefault(d => d.Name == dir);
                        if (currentDir == null)
                        {
                            errorMsg = $"Cannot find folder {dir}";
                            break;
                        }
                    }
                    
                    return currentDir;
                }

            }

            errorMsg = $"Cannot find folder {inputPath}";
            return null;
        }

        public static void WalkDirectoryTree(DirectoryInfo root, Action<FileInfo> fileFunc)
        {
            FileInfo[] files = null;
            DirectoryInfo[] subDirs = null;

            // First, process all the files directly under this folder
            try
            {
                files = root.GetFiles("*.*");
            }
            // This is thrown if even one of the files requires permissions greater
            // than the application provides.
            catch (UnauthorizedAccessException e)
            {
                // This code just writes out the message and continues to recurse.
                // You may decide to do something different here. For example, you
                // can try to elevate your privileges and access the file again.
                Console.WriteLine(e.Message);
            }

            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
        
            if (files != null)
            {
                foreach (var fi in files)
                {
                    // In this example, we only access the existing FileInfo object. If we
                    // want to open, delete or modify the file, then
                    // a try-catch block is required here to handle the case
                    // where the file has been deleted since the call to TraverseTree().
                    fileFunc(fi);
                    /*Entries += fi.FullName + "<BR>";
                    if(fi.FullName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase))
                        GraphicsHelper.GenerateThumbnail(fi.FullName, Thumbnails);*/
                }

                // Now find all the subdirectories under this directory.
                subDirs = root.GetDirectories();

                foreach (DirectoryInfo dirInfo in subDirs)
                {
                    // Resursive call for each subdirectory.
                    WalkDirectoryTree(dirInfo, fileFunc);
                }
            }   
        }
    }
}