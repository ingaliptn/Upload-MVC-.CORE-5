using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication3.Models
{
    public class Asset
    {
        [Key]
        [Column("Id")]
        public Guid Id { get; set; }

        [Column("FileName")]
        [MaxLength(255)]
        public string FileName { get; set; }

        [Column("OriginalFileName")]
        [MaxLength(255)]
        public string OriginalFileName { get; set; }

        [Column("MIMEType")]
        [MaxLength(64)]
        public string MIMEType { get; set; }

        [Column("EXT")]
        [MaxLength(64)]
        public string FileExtention { get; set; }
    }
}