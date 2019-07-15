﻿using DocumentStorage.Models.Repository;
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

namespace DocumentStorage.Controllers
{
    public class FileController : Controller
    {
        private FileRepository repository;
        private UserRepository userRepository;
        public FileController(FileRepository repository, UserRepository userRepository)
        {
            this.repository = repository;
            this.userRepository = userRepository;
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
            var path = @"C:\Users\User\source\repos\DocumentStorage\DocumentStorage\Content\Files";
            //var userRepository = new UserRepository();
            var file = new File
            {
                Author = userRepository.GetCurrentUser(),
                CreationDate = DateTime.Now,
                // Content = model.File.InputStream.ToByteArray(),
                Name = model.File.FileName,
                Type = model.File.ContentType,
                Path = String.Format(@"{0}\{1}", path, model.File.FileName)
            };
           // model.File.InputStream
           using(var fileStream = System.IO.File.Create(file.Path))
            {
                model.File.InputStream.Seek(0, System.IO.SeekOrigin.Begin);
                model.File.InputStream.CopyTo(fileStream);
            }
            
            repository.Save(file);

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