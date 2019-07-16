using DocumentStorage.Models.Repository;
using DocumentStorage.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using DocumentStorage.Models;
using DocumentStorage.Models.Models;
using DocumentStorage.Utils;
using DocumentStorage.Models.Filters;
using static DocumentStorage.Models.FilterViewModel;
using NHibernate;
using NHibernate.Transform;
using System.IO;
using System.Diagnostics;

namespace DocumentStorage.Controllers
{
    public class FileController : Controller
    {
        private FileRepository repository;
        private UserRepository userRepository;
        protected ISession session;
        public FileController(FileRepository repository, UserRepository userRepository, ISession session)
        {
            this.repository = repository;
            this.userRepository = userRepository;
            this.session = session;
        }
        public ActionResult ShowFileList(FileFilter filter, FetchOptions options)
        {
            //var model = new FileListViewModel
            //{
            var File = repository.GetUsersFile(User.Identity.GetUserId<long>(), filter, options);
           // };
            return View(File);
        }
        // GET: File
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(FileCreationViewModel model)
        {
            try
            {//var CreateProcedure = @"use DocStorage go CREATE PROCEDURE [dbo].[sp_InsertFile] @Name nvarchar(100), @Type nvarchar(50), @CreationDate DateTime2, @Author_id bigint, @Path nvarchar(255) AS INSERT INTO [File] (Name, Type, CreationDate, Author_id, Path ) VALUES ( @Name, @Type, @CreationDate, @Author_id, @Path) SELECT SCOPE_IDENTITY() GO";
                var result = session.GetNamedQuery("CreateProcedure");
                var a = result;
                result.ExecuteUpdate();

            }
            catch (Exception ex)
            { }
            var path = AppDomain.CurrentDomain.BaseDirectory;//@"C:\Users\User\source\repos\DocumentStorage\DocumentStorage\Content\Files";
            //var userRepository = new UserRepository();
            var file = new Models.Models.File
            {
                Author = userRepository.GetCurrentUser(),
                CreationDate = DateTime.Now,
                // Content = model.File.InputStream.ToByteArray(),
                Name = model.File.FileName,
                Type = model.File.ContentType,
                Path = Path.Combine(path, @"Content\Files", model.File.FileName)// String.Format(@"{0}\{1}", path, model.File.FileName)
            };
           // model.File.InputStream
           using(var fileStream = System.IO.File.Create(file.Path))
            {
                model.File.InputStream.Seek(0, System.IO.SeekOrigin.Begin);
                model.File.InputStream.CopyTo(fileStream);
            }
            try
            {
                var result = session.CreateSQLQuery("exec sp_InsertFile :Name, :Type, :CreationDate, :Author_id, :Path")
                   // .AddEntity(typeof(StoredFile))
                   // .SetResultTransformer(Transformers.)
                    .SetParameter("Name", file.Name)
                    .SetParameter("Type", file.Type)
                    .SetParameter("CreationDate", file.CreationDate)
                    .SetParameter("Author_id", file.Author.Id)
                    .SetParameter("Path", file.Path)
                    ;
                result.ExecuteUpdate();
               // session.Save(result);
               var res = result;
                    //.List<MyDomainObject>();
            }
            catch
            {
                repository.Save(file);
            }
            

            return RedirectToAction("ShowFileList", "File");
        }
        public FileStreamResult GetFile(long id)
        {
            var f = repository.Load(id);
            if (f==null)
            {
                
                return null;
            }
            byte[] fileContents = System.IO.File.ReadAllBytes( f.Path);
            string contentType = f.Type;
            FileStreamResult result = new FileStreamResult(System.IO.File.Open(f.Path, System.IO.FileMode.Open), contentType);

            result.FileDownloadName = f.Name;
            return result;
        }
        public ActionResult OpenFile (long id)
        {
            return View();
        }
    }
}