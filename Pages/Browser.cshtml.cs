using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace aspnettcoreapp.Pages
{
    public class BrowserModel : PageModel
    {
        public string Message { get; set; }
        public string Entries { get; set; }
        public Dictionary<string, byte[]> Thumbnails = new Dictionary<string, byte[]>();

        public void OnGet()
        {
            var path = "/home/pawel/Pictures/zdj/2018-04-29";

            var dirInfo = FilesyStemHelper.GetDirContent(path, out var errStr);
            if (dirInfo != null)
                FilesyStemHelper.WalkDirectoryTree(dirInfo, fi =>
                {
                    Entries += fi.FullName + "<BR>";
                    if(fi.FullName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase))
                        GraphicsHelper.GenerateThumbnail(fi.FullName, Thumbnails);
                });
            else
                Message = errStr;
        }
    }
}
