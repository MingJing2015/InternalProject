using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace InternalProject.Services
{
    public class ImageService
    {
        public static void UploadImage(HttpPostedFileBase file, string savingPath)
        {
            if (file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(savingPath, fileName);
                if (!Directory.Exists(savingPath))
                {
                    Directory.CreateDirectory(savingPath);
                }
                file.SaveAs(path);
            }
        }
    }
}