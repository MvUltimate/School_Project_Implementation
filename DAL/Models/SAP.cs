using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    [PrimaryKey(nameof(UUID))]
    public class SAP
    {
        
        public Guid UUID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }

        public int? ClassId { get; set; } //Nullable si admin

        public Class Class { get; set; } = null!;

        public int DepartementId { get; set; }
        public Departement Departement { get; set; } = null!;
     
       
    }
}
