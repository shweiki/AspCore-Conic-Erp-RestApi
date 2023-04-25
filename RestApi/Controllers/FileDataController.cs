using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace RestApi.Controllers;

[Authorize]
public class FileDataController : Controller
{
    private ConicErpContext DB;
    public IConfiguration _configuration { get; }

    public FileDataController(ConicErpContext dbcontext, IConfiguration configuration)
    {
        DB = dbcontext;
        _configuration = configuration;
    }

    [Authorize]
    [Route("Files/GetProfilePictureByObjId")]
    [HttpGet]
    public IActionResult GetProfilePictureByObjId(string TableName, long ObjId)
    {

        var file = DB.FileData.Where(i => i.TableName == TableName && i.Fktable == ObjId && i.Type == "ProfilePicture").FirstOrDefault();
        if (file is not null)
        {
            string ImageUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/images/{TableName}/{file.Type}/{Path.GetFileName(file.FilePath)}";
            return Ok(ImageUrl);
        }
        //  return Ok(file);

        return Ok(false);
    }
    [Route("Files/SetTypeByObjId")]
    [HttpPost]
    public async Task<IActionResult> SetTypeByObjId(long Id, string type)
    {

        var file = DB.FileData.Where(i => i.Id == Id).SingleOrDefault();

        if (file != null)
        {
            file.Type = type;
            await DB.SaveChangesAsync(new CancellationToken(), User.Identity.Name);
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
            FileUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/images/{TableName}/{x.Type}/{Path.GetFileName(x.FilePath)}",
            x.FilePath,
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
            FileUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/images/{TableName}/{x.Type}/{Path.GetFileName(x.FilePath)}",
            x.FileType
        }).ToList();

        return Ok(files);

    }
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    [Route("Files/Create")]
    [HttpPost]
    public async Task<IActionResult> Create(FileDatum collection)
    {
        if (ModelState.IsValid)
        {

            if (collection.FileType == "image")
            {
                string base64String = Regex.Replace(collection.File, @"^data:image\/[a-zA-Z]+;base64,", string.Empty);
                if (string.IsNullOrWhiteSpace(collection.Type))
                {
                    collection.Type = "Picture";
                }

                collection.File = string.Empty;
                DB.FileData.Add(collection);

                await DB.SaveChangesAsync(new CancellationToken(), User.Identity.Name);
                string path = LoadJpeg(base64String, collection.Type, collection.TableName, collection.Id);
                if (!string.IsNullOrWhiteSpace(path))
                {
                    collection.FilePath = path;
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
    public async Task<IActionResult> FixBase64ToPathWithLoaded()
    {
        try
        {
            using (ConicErpContext _db = new ConicErpContext(_configuration))
            {
                var files = await _db.FileData.Where(x => x.FileType == "image" && x.File != null && x.File != "")
                .Select(x => new
                {
                    x.Id,
                    x.Type,
                    x.FileType,
                    x.TableName,
                    x.Fktable,
                })
                .ToListAsync();
                foreach (var file in files)
                {

                    var fileData = await _db.FileData.SingleOrDefaultAsync(i => i.Id == file.Id);

                    if (string.IsNullOrWhiteSpace(fileData.Type))
                    {
                        fileData.Type = "Picture";
                    }
                    string base64String = Regex.Replace(fileData.File, @"^data:image\/[a-zA-Z]+;base64,", string.Empty);

                    string path = LoadJpeg(base64String, fileData.Type, fileData.TableName, fileData.Id);

                    if (!string.IsNullOrWhiteSpace(path))
                    {
                        fileData.FilePath = path;
                        fileData.File = "";
                        await _db.SaveChangesAsync();
                    }

                }
                return Ok(true);
            }
        }
        catch (Exception ex)
        {
            return Ok(ex.Message);
        }

    }
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    public string LoadJpeg(string base64String, string foldarType, string foldarName, long imageName)
    {
        try
        {
            string productImagePath = Path.GetFullPath(_configuration.GetConnectionString("ProductImagePath") ?? "C:\\ConicImages");
            string SourceFoldar = Path.Combine(productImagePath, foldarName, foldarType);
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
    public Boolean LoadImage(string base64String, long imageName, string foldarName)
    {
        try
        {
            string productImagePath = Path.GetFullPath(_configuration.GetConnectionString("ProductImagePath") ?? "C:\\ConicImages");
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
