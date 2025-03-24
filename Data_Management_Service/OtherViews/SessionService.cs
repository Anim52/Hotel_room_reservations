using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Management_Service.OtherViews
{
    public static class SessionService
    {
        public static string CurrentUserLogin { get; set; }
        public static Guid CurrentUserId { get; set; }
    }
}
