using System.Drawing;
using System.Drawing.Imaging;

namespace Application.Common.Helpers;

public class ImageHelper
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    public static string LoadJpeg(string base64String, string productImagePath, string foldarType, string foldarName, long imageName)
    {
        try
        {
            string SourceFoldar = Path.Combine(productImagePath, foldarName, foldarType);
            if (!Directory.Exists(SourceFoldar))
            {
                Directory.CreateDirectory(SourceFoldar);
            }
            string SourcePath = Path.Combine(SourceFoldar, Path.GetFileName("" + imageName + ".jpeg"));

            if (File.Exists(SourcePath))
            {
                // If file  found it delete it and  save it     
                File.Delete(SourcePath);
            }
            byte[] bitmapData = new byte[base64String.Length];
            bitmapData = Convert.FromBase64String(base64String);

            using (var streamBitmap = new MemoryStream(bitmapData))
            {
                using (Image image = Image.FromStream(streamBitmap))
                {
                    image.Save(SourcePath, ImageFormat.Jpeg);
                    return SourcePath;

                    //  image.Save(path);
                }
            }
        }
        catch (Exception ex) { throw ex; }

    }

    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    public static Boolean LoadImage(string base64String, string productImagePath, long imageName, string foldarName)
    {
        try
        {
            string SourceFoldar = Path.Combine(productImagePath, foldarName);
            if (!Directory.Exists(SourceFoldar))
            {
                Directory.CreateDirectory(SourceFoldar);
            }
            string SourcePath = Path.Combine(SourceFoldar, Path.GetFileName("" + imageName + ".jpeg"));

            if (File.Exists(SourcePath))
            {
                // If file  found it delete it and  save it     

                File.Delete(SourcePath);
            }
            Image image = Base64ToImage(base64String);
            image.Save(SourcePath, ImageFormat.Bmp);
            return (true);
        }
        catch (Exception ex) { return false; }

    }
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]

    public static Image Base64ToImage(string base64String)
    {
        //data:image/gif;base64,
        //this image is a single pixel (black)
        byte[] bytes = Convert.FromBase64String(base64String);

        Image image;
        using (MemoryStream ms = new MemoryStream(bytes))
        {
            image = Image.FromStream(ms);
            return image;
        }

    }

}
