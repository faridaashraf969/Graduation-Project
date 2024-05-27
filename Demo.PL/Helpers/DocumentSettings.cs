using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;

namespace Demo.PL.Helpers
{
    public class DocumentSettings
    {
        public static string UploadFille(IFormFile file, string foldername)
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot//Files", foldername);

            string filename = $"{Guid.NewGuid()}{file.FileName}";

            string filepath = Path.Combine(folderPath, filename);

            using var Fs = new FileStream(filepath, FileMode.Create);
            file.CopyTo(Fs);

            return filename;
        }
        public static void DeleteFiles(string FileName, string FolderName)
        {
            string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot//Files", FolderName, FileName);

            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);

            }


        }
    }
}
