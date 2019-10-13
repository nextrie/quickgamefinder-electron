using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WOA_DEVServer.Core;
using WOA_DEVServer.Network.Users;

namespace WOA_DEVServer.Network.Groups
{
    public class Group
    {
        public List<User> users = new List<User>();
        public string roomname;
        public string roomdescription;
        public string author;
        public int playercounter;
        public int desiredplayers;
        public string gameID;
        public string _public;
        public int roomiD;
        public string rank;
        public Group(string nameroom, string descroom, string roomauthor, int players, int maxplayers, string game, string isPublic, int roomID ,string grank){
            roomdescription = descroom;
            roomname = nameroom;
           author = roomauthor;
            playercounter = players;
            desiredplayers = maxplayers;
            _public = isPublic;
            gameID = game;
            roomiD = roomID;
            rank = grank;
        }
        public string GetUsers()
        {
            string toreturn = "";
            foreach (User u in users)
            {
                toreturn = toreturn + u.username + "/" + u.rank + ";";
            }
            return toreturn;
        }
        public static void DisbandGroup(int roomid)
        {
            foreach(Group g in ServerLists.groups)
            {
                if(g.roomiD == roomid)
                {
                    ServerLists.groups.Remove(g);
                }
            }
        }
        // public string description;
    }
}
