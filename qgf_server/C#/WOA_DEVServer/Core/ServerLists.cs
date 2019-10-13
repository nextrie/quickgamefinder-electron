using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WOA_DEVServer.Network.Groups;
using WOA_DEVServer.Network.Rooms;
using WOA_DEVServer.Network.Users;

namespace WOA_DEVServer.Core
{
    public class ServerLists
    {

        public static List<User> users = new List<User>();
   
        public static List<Group> groups = new List<Group>();
        public static List<Room> rooms = new List<Room>();
        public static byte[] GetGroups()
        {
            string msg = "Groups|";
            foreach(Group g in groups)
            {
                msg = msg + g.author + "|" + g.playercounter.ToString() + "|" + g.desiredplayers.ToString() + "|" + g._public.ToString() + "|" + g.roomname + "|" + g.roomdescription + "|" + g.roomiD.ToString() + "|" +g.gameID + "|"+ g.rank + ";"; 
            }
            byte[] r = Encoding.ASCII.GetBytes(msg);
            return r;
        }
    }
}
