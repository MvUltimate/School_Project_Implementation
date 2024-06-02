namespace WebApi_SchoolProject.Models
{

    //Combinaison of informations from SAP table and Account table
    public class StudentsInfoM
    {

        public Guid Uuid { get; set; }
        public string UserName {  get; set; }

        public double Amount {  get; set; }
        
        public int NbrPage { get; set; }
        public string Class {  get; set; }

        public int DepartmentId { get; set; }

    }
}
