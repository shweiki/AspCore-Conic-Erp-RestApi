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
    [Authorize]

    public class FileDataController : Controller
    {
        private ConicErpContext DB = new ConicErpContext();
        [Route("Files/GetFileByObjID")]
        [HttpGet]
        public IActionResult GetFileByObjID(string TableName , long ObjID )
        {

            var file = DB.FileData.Where(i => i.TableName == TableName && i.Fktable == ObjID).Select(x => new {
                x.Id,
                x.File,
                x.FileType

            }).ToList().LastOrDefault();
    
          if(file != null)
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
