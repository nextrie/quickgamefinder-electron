using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WOA_DEVServer.Network.Users;

namespace WOA_DEVServer.Network.Rooms
{
    public class Room
    {
        public  string title;
        public  string desc;
        public List<User> users;
        public  string author;
        public  int current;
        public  int max;
        public  int RoomID;
        public Room(string roomtitle, string roomdesc, List<User> l, string roomadmin, int currentplayer, int maxplayer, int roomid)
        {
            title = roomtitle;
            desc = roomdesc;
            users = l;
            author = roomadmin;
            current = currentplayer;
            max = maxplayer;
            RoomID = roomid;
        }
        public string GetUsers()
        {
            string toreturn = "";
            foreach(User u in users)
            {
                toreturn = toreturn + u.username + "/" + u.rank + ";";
            }
            return toreturn;
        }
    }
}
