using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvp.Feature.Forms.Models
{
    public class MvpCategory
    {
        public string Name { get; set; }
        public bool Active { get; set; }
        public Guid ID { get; set; }
    }
}