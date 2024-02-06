using System;
using TestTask3.Common.Enums;

namespace TestTask3.Common.Entities
{
    public class FileUploadEntity
    {
        public int Id { get; set; }

        public byte[] Content { get; set; }

        public byte[] PdfContent { get; set; }

        public FileState State { get; set; }

        public string FileName { get; set; }

        public DateTime DateAdd { get; set; }
    }
}
