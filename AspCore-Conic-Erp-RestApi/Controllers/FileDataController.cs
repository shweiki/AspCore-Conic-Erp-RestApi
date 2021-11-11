using System;
using System.Collections.Generic;
using System.Data;
using Entities;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspCore_Conic_Erp_RestApi.Controllers
{

    public class FileDataController : Controller
    {
        private ConicErpContext DB;
        public FileDataController(ConicErpContext dbcontext)
        {
            DB = dbcontext;
        }

        [AllowAnonymous]
        [Route("Files/Upload")]
        [HttpPost]
        public IActionResult Upload(string filex)
        {

            return Ok(true);
        }

        [Authorize]
        [Route("Files/GetProfilePictureByObjId")]
        [HttpGet]
        public IActionResult GetProfilePictureByObjId(string TableName, long ObjId)
        {

            var file = DB.FileData.Where(i => i.TableName == TableName && i.Fktable == ObjId && i.Type == "ProfilePicture").Select(x => new {
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
        public IActionResult SetTypeByObjId(long Id ,string type)
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
        public IActionResult GetFileByObjId(string TableName , long ObjId )
        {

            var file = DB.FileData.Where(i => i.TableName == TableName && i.Fktable == ObjId).Select(x => new {
                x.Id,
                x.Type,
                x.File,
                x.FileType
            }).ToList().LastOrDefault();
    
          if(file != null)
            return Ok(file);

            return Ok(false);
        }
        [Route("Files/GetFilesByObjId")]
        [HttpGet]
        public IActionResult GetFilesByObjId(string TableName, long ObjId)
        {

            var file = DB.FileData.Where(i => i.TableName == TableName && i.Fktable == ObjId).Select(x => new {
                x.Id,
                x.Type,
                x.File,
                x.FileType
            }).ToList();

            if (file != null)
                return Ok(file);

            return Ok(false);
        }
        [Route("Files/Create")]
        [HttpPost]
        public IActionResult Create(FileDatum filex)
        {
            if (ModelState.IsValid)
            {

                if (LoadImage(filex.File, filex.Fktable, filex.TableName) && filex.FileType == "image")
                {
                    filex.File = "data:image/jpeg;base64," + filex.File;
                    DB.FileData.Add(filex);

                    DB.SaveChanges();
                    return Ok(true);
                }
                else
                    return Ok(false);
            }

            return Ok(false);
        }

        ///  LoadImage
        public Boolean LoadImage(String Base64String, long ImageNameById , string where)
        {

            string paths = Path.Combine("Images/" + where);
            string SourceFoldar = Path.Combine(paths);
            string SourcePath = Path.Combine(SourceFoldar, Path.GetFileName("" + ImageNameById + ".jpeg"));
            if (!Directory.Exists(SourceFoldar))
            {
                Directory.CreateDirectory(SourceFoldar);
            }
            if (System.IO.File.Exists(SourcePath))
            {
                // If file  found it delete it and  save it     

                System.IO.File.Delete(SourcePath);
            }
            var image = new Bitmap(LoadImageFromBase64String(Base64String));
            image.Save(SourcePath, ImageFormat.Bmp);
            return (true);
        }

        public Image LoadImageFromBase64String(String Base64String)
        {
            //data:image/gif;base64,
            //this image is a single pixel (black)
            byte[] bytes = Convert.FromBase64String(Base64String);

            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
            }

            return image;
        }

    }
}
