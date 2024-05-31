using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Class
    {

        public int ClassId { get; set; }
        public string Name {  get; set; }

        public ICollection<SAP> SAPs { get; set; } = new List<SAP>();
    }
}
