using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvp.Feature.Forms.Models
{
    public class ApplicationLists
    {
        public IEnumerable<ApplicationListData> Country { get; set; }
        public IEnumerable<ApplicationListData> EmploymentStatus { get; set; }
        public IEnumerable<ApplicationListData> MvpCategory { get; set; }
    }
}
