using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class AssetController : Controller
    {
        private ApplicationDbContext _context;

        public AssetController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Add(IFormCollection formCollection)
        {
            if (formCollection.Files.Count > 0)
            {
                /*Parallel.ForEach(formCollection.Files, filee =>
                 {
                     Guid id = Guid.NewGuid();
                     string ext = Path.GetExtension(filee.FileName);
                     Asset asset = new Asset
                     {
                         Id = id,
                         FileName = id.ToString() + ext,
                         OriginalFileName = filee.FileName,
                         MIMEType = filee.ContentType,
                         FileExtention = ext
                     };
                     using (Stream stream = new FileStream("./Assets" + filee.FileName,
                         FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
                     {
                         filee.CopyToAsync(stream);
                     }
                     _context.Set<Asset>().Add(asset);
                     _context.SaveChanges();
                 });*/
                foreach (IFormFile filee in formCollection.Files)
                {
                    Guid id = Guid.NewGuid();
                    string ext = Path.GetExtension(filee.FileName);
                    Asset asset = new Asset
                    {
                        Id = id,
                        FileName = id.ToString() + ext,
                        OriginalFileName = filee.FileName,
                        MIMEType = filee.ContentType,
                        FileExtention = ext
                    };
                    using (Stream stream = new FileStream("./Assets/" + asset.FileName,
                        FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
                    {
                        filee.CopyToAsync(stream);
                    }

                    _context.Set<Asset>().Add(asset);
                }
                _context.SaveChanges();
            }

            return Redirect("/Home/Index");
        }

        public IActionResult Get(Guid? id)
        {
            if (!id.HasValue) return new StatusCodeResult((int)HttpStatusCode.BadRequest);

            Asset asset = _context.Set<Asset>().FirstOrDefault(x => x.Id == id.Value);

            if (asset == null) return new StatusCodeResult((int)HttpStatusCode.BadRequest);

            Stream s = new FileStream("./Assets/" + asset.FileName, FileMode.Open, FileAccess.Read, FileShare.Write);
            return new FileStreamResult(s, asset.MIMEType);
        }

        public IActionResult Delete(Guid? id)
        {
            if (!id.HasValue) return new StatusCodeResult((int)HttpStatusCode.BadRequest);

            Asset asset = _context.Set<Asset>().FirstOrDefault(x => x.Id == id.Value);

            if (asset == null) return new StatusCodeResult((int)HttpStatusCode.BadRequest);

            _context.Assets.Remove(asset);
            System.IO.File.Delete("./Assets/" + asset.FileName);
            _context.SaveChanges();

            return Redirect("/Home/index");
        }

        public IActionResult Update(Guid? id, IFormCollection formCollection)
        {
            if (formCollection.Files.Count > 0)
            {
                foreach (IFormFile filee in formCollection.Files)
                {
                    Asset old = _context.Assets.FirstOrDefault(x => x.Id == id);
                    string ext = Path.GetExtension(filee.FileName);

                    System.IO.File.Delete("./Assets/" + old.Id);

                    old.FileName = old.Id.ToString() + ext;
                    old.FileExtention = ext;
                    old.MIMEType = filee.ContentType;
                    old.OriginalFileName = old.FileName;

                    using (Stream stream = new FileStream("./Assets/" + old.FileName,
                        FileMode.Create, FileAccess.Write, FileShare.Read))
                    {
                        filee.CopyToAsync(stream);
                    }

                    _context.Set<Asset>().Update(old);
                }
                _context.SaveChanges();
            }
            return Redirect("/Home/Index");
        }
    }
}