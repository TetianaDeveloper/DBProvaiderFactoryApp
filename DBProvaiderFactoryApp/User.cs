using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBProvaiderFactoryApp
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        /*private int id;
        private string login;
        private string password;
        public int Id 
        { 
            get { return id; }
            set { id = value; } 
        }
        public string Login
        {
            get { return login; }
            set { login = value; }
        }
        public string Password 
        {
            get { return password; }
            set { password = value; }
        }
        public User(int id, string login, string password)
        {
            this.id = id;
            this.login = login;
            this.password = password;
        }*/
    }
}
