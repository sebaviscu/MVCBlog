using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCBlog.Core.Entities
{
    public class ImageGallery : EntityBase
    {

        public ImageGallery()
        {
            ImageList = new List<string>();
        }

        [Key]

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string ImagePath { get; set; }

        [NotMapped]
        public List<string> ImageList { get; set; }

    }
}
