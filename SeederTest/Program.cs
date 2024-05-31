using DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace SeederTest
{
    internal class Program
    {
        static void Main(string[] args)
        {

            using var context = new SchoolContext();
            
            var created = context.Database.EnsureCreated();
            
      
                seed();
                //context.DropDatabase();
                Console.WriteLine("Hello");
            
            


        }

        private static  void seed()
        {
            using var context = new SchoolContext();

            var guid1 = new Guid();
            var guid2 = new Guid();
            var guid3 = new Guid();
            var guid4 = new Guid();

            var class1 = new Class() { Name = "601-PT" };
            var class2 = new Class() { Name = "602-F" };
            var class3 = new Class() { Name = "602-PT" };
            context.Classes.AddRange(class1,class2,class3);


            var departement1 = new Departement() { DepartementName = "Students" };
            var departement2 = new Departement() { DepartementName = "IT" };
            var departement3 = new Departement() { DepartementName = "Admin" };
            context.Departements.AddRange(departement1,departement2,departement3);

            
            var user1 = new SAP() { UUID = guid1, UserName = "frank.miller", FirstName = "frank", LastName = "miller", Departement = departement1, Class = class1 };
            var user2 = new SAP() { UUID = guid2, UserName = "alice.smith", FirstName = "alice", LastName = "smith", Departement = departement1, Class = class1 };
            var user3 = new SAP() { UUID = guid3, UserName = "charlie.brown", FirstName = "charlie", LastName = "brown", Departement = departement1, Class = class2 };
            var user4 = new SAP() { UUID = guid4, UserName = "emma.davis", FirstName = "emma", LastName = "davis", Departement = departement3};

            context.SAPs.AddRange(user1,user2,user3,user4);

            context.SaveChanges();
        }

    }
}
