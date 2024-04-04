using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasusZuydFitV0._1
{
    public class Eventorganisor : User
    {
        public List<Event> Events { get; set; }

        public Eventorganisor(int userId, string userName, string userEmail, string userPassword, List<Event> events) : base(userId, userName, userEmail, userPassword)
        {
            Events = events;
        }
    }
}