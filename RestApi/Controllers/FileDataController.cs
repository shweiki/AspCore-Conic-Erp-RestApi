using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace RestApi.Controllers;


public class FileDataController : Controller
{
    private ConicErpContext DB;
    public IConfiguration Configuration { get; }

    public FileDataController(ConicErpContext dbcontext, IConfiguration configuration)
    {
        DB = dbcontext;
        Configuration = configuration;
    }

    [Authorize]
    [Route("Files/GetProfilePictureByObjId")]
    [HttpGet]
    public IActionResult GetProfilePictureByObjId(string TableName, long ObjId)
    {

        var file = DB.FileData.Where(i => i.TableName == TableName && i.Fktable == ObjId && i.Type == "ProfilePicture").Select(x => new
        {
            x.Id,
            x.Type,
            x.File,
            x.FileType
        }).FirstOrDefault();

        if (file != null)
            return Ok(file);

        return Ok(false);
    }
    [Route("Files/SetTypeByObjId")]
    [HttpPost]
    public IActionResult SetTypeByObjId(long Id, string type)
    {

        var file = DB.FileData.Where(i => i.Id == Id).SingleOrDefault();

        if (file != null)
        {
            file.Type = type;
            DB.SaveChanges();
            return Ok(true);
        }

        return Ok(false);
    }
    [Route("Files/GetFileByObjId")]
    [HttpGet]
    public IActionResult GetFileByObjId(string TableName, long ObjId)
    {

        var file = DB.FileData.Where(i => i.TableName == TableName && i.Fktable == ObjId).Select(x => new
        {
            x.Id,
            x.Type,
            x.File,
            x.FileType
        }).ToList().LastOrDefault();

        if (file != null)
            return Ok(file);

        return Ok(false);
    }
    [Route("Files/GetFilesByObjId")]
    [HttpGet]
    public IActionResult GetFilesByObjId(string TableName, long ObjId)
    {

        var files = DB.FileData.Where(i => i.TableName == TableName && i.Fktable == ObjId).Select(x => new
        {
            x.Id,
            x.Type,
            x.File,
            x.FileType
        }).ToList();

        return Ok(files);

    }
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    [Route("Files/Create")]
    [HttpPost]
    public IActionResult Create(FileDatum collection)
    {
        if (ModelState.IsValid)
        {

            if (collection.FileType == "image")
            {
                string base64String = Regex.Replace(collection.File, @"^data:image\/[a-zA-Z]+;base64,", string.Empty);

                string path = LoadJpeg(base64String, collection.Fktable, collection.TableName);
                if (!string.IsNullOrWhiteSpace(path))
                {
                    collection.FilePath = path;
                    collection.File = string.Empty;
                    DB.FileData.Add(collection);

                    DB.SaveChanges();
                    return Ok(true);
                }
                //  LoadImage(collection.File, collection.Fktable, collection.TableName) &&
                //  collection.File = "data:image/jpeg;base64," + collection.File;
                return Ok(false);
            }
            else
                return Ok(false);
        }

        return Ok(false);
    }
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    [Route("Files/FixBase64ToPathWithLoaded")]
    [HttpGet]
    public IActionResult FixBase64ToPathWithLoaded()
    {
        try
        {
            var files = DB.FileData.ToList();
            foreach (var file in files)
            {
                if (file.FileType != "image")
                {
                    continue;
                }
                string base64String = file.File;// Regex.Replace(file.File, @"^data:image\/[a-zA-Z]+;base64,", string.Empty);

                string path = LoadJpeg(base64String, file.Fktable, file.TableName);
                if (!string.IsNullOrWhiteSpace(path))
                {
                    file.FilePath = path;
                    file.File = string.Empty;
                }
            }
            DB.SaveChanges();
            return Ok(true);

        }
        catch (Exception ex)
        {
            return Ok(false);
        }
    }
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    public string LoadJpeg(string base64String, long imageName, string foldarName)
    {
        try
        {
            string productImagePath = Path.GetFullPath(Configuration.GetConnectionString("ProductImagePath") ?? "C:\\ConicImages");
            string SourceFoldar = Path.Combine(productImagePath, foldarName);
            if (!Directory.Exists(SourceFoldar))
            {
                Directory.CreateDirectory(SourceFoldar);
            }
            string SourcePath = Path.Combine(SourceFoldar, Path.GetFileName("" + imageName + ".jpeg"));

            if (System.IO.File.Exists(SourcePath))
            {
                // If file  found it delete it and  save it     

                System.IO.File.Delete(SourcePath);
            }
            Image image = Base64ToImage(base64String);
            image.Save(SourcePath, ImageFormat.Jpeg);
            return SourcePath;
        }
        catch (Exception ex) { return null; }

    }

    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    public Boolean LoadImage(string base64String, long imageName, string foldarName)
    {
        try
        {
            string productImagePath = Path.GetFullPath(Configuration.GetConnectionString("ProductImagePath") ?? "C:\\ConicImages");
            string SourceFoldar = Path.Combine(productImagePath, foldarName);
            if (!Directory.Exists(SourceFoldar))
            {
                Directory.CreateDirectory(SourceFoldar);
            }
            string SourcePath = Path.Combine(SourceFoldar, Path.GetFileName("" + imageName + ".jpeg"));

            if (System.IO.File.Exists(SourcePath))
            {
                // If file  found it delete it and  save it     

                System.IO.File.Delete(SourcePath);
            }
            Image image = Base64ToImage(base64String);
            image.Save(SourcePath, ImageFormat.Bmp);
            return (true);
        }
        catch (Exception ex) { return false; }

    }
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]

    public Image Base64ToImage(string base64String)
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
