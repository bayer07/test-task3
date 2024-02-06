using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using TestTask3.Models;
using Dapper;
using System.IO;
using AutoMapper;
using TestTask3.Common.Entities;
using TestTask3.Common.Enums;

namespace TestTask3.Services
{
    public class FileService
    {
        public FileService(IDbConnection dbConnection, IMapper mapper)
        {
            _dbConnection = dbConnection;
            _mapper = mapper;
        }

        private readonly IDbConnection _dbConnection;
        private readonly IMapper _mapper;

        public void Add(IFormFile file)
        {
            byte[] fileBytes;
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                fileBytes = ms.ToArray();
            }

            _dbConnection.Execute("INSERT INTO Files ([content], file_name, date_add, state) VALUES(@content, @fileName, @dateAdd, @state); ",
                new { content = fileBytes, fileName = file.FileName, dateAdd = DateTime.UtcNow, state = FileState.Added });
        }

        public List<FileUploadModel> GetAll()
        {
            var files = _dbConnection.Query<FileUploadEntity>("SELECT id, file_name, date_add, state FROM Files").AsList();
            return _mapper.Map<List<FileUploadEntity>, List<FileUploadModel>>(files);
        }
    }
}
