using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace RedisAPI.Models
{
    public class User
    {
        public string Name { get; set; }
        public string Date { get; set; }
        public int Age { get; set; }
        public string Profession { get; set; }

        public User()
        {
            this.Name = "ABC";
            this.Date = DateTime.Now.ToString("dd/MM/yyyy");
            this.Age = 35;
            this.Profession = "officer";
        }

        public User(string name)
        {
            this.Name = name;
            this.Date = DateTime.Now.ToString("dd/MM/yyyy");
            this.Age = 48;
            this.Profession = "clerk";
        }

        public User(string name, string date, int age, string profession)
        {
            this.Name = name;
            this.Date = date;
            this.Age = age;
            this.Profession = profession;
        }
    }
    
}
