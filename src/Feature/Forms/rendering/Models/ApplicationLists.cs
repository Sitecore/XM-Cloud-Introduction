using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvp.Feature.Forms.Models
{
    public class ApplicationLists
    {
        public IEnumerable<Country> Country { get; set; }
        public IEnumerable<EmploymentStatus> EmploymentStatus { get; set; }
        public IEnumerable<MvpCategory> MvpCategory { get; set; }
    }
}
