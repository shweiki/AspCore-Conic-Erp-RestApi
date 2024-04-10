using Application.Common.Helpers;
using Application.Common.Interfaces;
using Application.Features.SystemConfiguration.Queries.GetSystemConfiguration;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Text.RegularExpressions;

namespace RestApi.Controllers;

[Authorize]
public class FileDataController : Controller
{
    private readonly IApplicationDbContext DB;
    public IConfiguration _configuration { get; }
    private readonly ISender _mediator;

    public FileDataController(IApplicationDbContext dbcontext, IConfiguration configuration, ISender mediator)
    {
        DB = dbcontext;
        _configuration = configuration;
        _mediator = mediator;
    }

    [Authorize]
    [Route("Files/GetProfilePictureByObjId")]
    [HttpGet]
    public IActionResult GetProfilePictureByObjId(string TableName, long ObjId)
    {
        try
        {
            var file = DB.FileData.Where(i => i.TableName == TableName && i.Fktable == ObjId && i.Type == "ProfilePicture").OrderByDescending(o => o.Id).FirstOrDefault();
            if (file is not null)
            {
                string ImageUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/images/{TableName}/{file.Type}/{Path.GetFileName(file.FilePath)}";
                return Ok(ImageUrl);
            }
            //  return Ok(file);
            return Ok(false);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Route("Files/SetTypeByObjId")]
    [HttpPost]
    public async Task<IActionResult> SetTypeByObjId(long Id, string type)
    {
        try
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
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Route("Files/GetFileByObjId")]
    [HttpGet]
    public IActionResult GetFileByObjId(string TableName, long ObjId)
    {
        try
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
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    [Route("Files/GetFilesByObjId")]
    [HttpGet]
    public IActionResult GetFilesByObjId(string TableName, long ObjId)
    {
        try
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
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }

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
                var systemConfiguration = await _mediator.Send(new GetSystemConfigurationQuery());

                await DB.SaveChangesAsync(new CancellationToken(), User.Identity.Name);
                string path = ImageHelper.LoadJpeg(base64String, systemConfiguration.DefaultFilesPath, collection.Type, collection.TableName, collection.Id);
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
}
