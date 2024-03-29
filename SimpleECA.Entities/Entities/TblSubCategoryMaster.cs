﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SimpleECA.Entities
{
    [Table("TblSubCategoryMaster")]
    public class TblSubCategoryMaster
    {
        [Key]
        public int subcatid { get; set; }
        public int categoryid { get; set; }
        public string subcategoryname { get; set; }
        public DateTime createdat { get; set; }
        public DateTime updatedat { get; set; }
        public bool isactive { get; set; }
    }
}
