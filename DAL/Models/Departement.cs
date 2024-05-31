using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Departement
    {


        public int DepartementId { get; set; }
        public string DepartementName { get; set; }

        public ICollection<SAP> SAPs { get; set; } = new List<SAP>();
    }
}
