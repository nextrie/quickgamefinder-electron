using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WOA_DEVServer.Core;
using WOA_DEVServer.Network.Groups;
using WOA_DEVServer.Network.Stats;
using WOA_DEVServer.Network.Users;
using System.Net.Mail;
using WOA_DEVServer.Network.Rooms;

namespace WOA_DEVServer
{
    public class Program
    {
        static readonly object _lock = new object();
        static readonly Dictionary<int, TcpClient> list_clients = new Dictionary<int, TcpClient>();
        public enum Lt
        {
            OUVERTURE,
            CONNEXION,
            INSCRIPTION,
            BAN,
            DECONNEXION,
            ERREUR,
            SERVEUR

        }
        public static void Log(string text, Lt l)
        {
            switch (l)
            {
                case Lt.CONNEXION:
                    Console.WriteLine("[" + DateTime.Now.ToString() + "] - " + text + " viens de se connecter", Console.ForegroundColor = ConsoleColor.Green);
                    break;
                case Lt.BAN:
                    Console.WriteLine("[" + DateTime.Now.ToString() + "] - " + "Un joueur banni (" + text + ") essaye de se connecter", Console.ForegroundColor = ConsoleColor.DarkYellow);
                    break;
                case Lt.DECONNEXION:
                    Console.WriteLine("[" + DateTime.Now.ToString() + "] - " + text + " viens de se déconnecter", Console.ForegroundColor = ConsoleColor.Magenta);
                    break;
                case Lt.ERREUR:
                    Console.WriteLine("----------------------------------", Console.ForegroundColor = ConsoleColor.Red);
                    Console.WriteLine("[" + DateTime.Now.ToString() + "] - " + text, Console.ForegroundColor = ConsoleColor.Red);
                    Console.WriteLine("----------------------------------", Console.ForegroundColor = ConsoleColor.Red);
                    break;
                case Lt.INSCRIPTION:
                    Console.WriteLine("[" + DateTime.Now.ToString() + "] - " + "Le gamer " + text + " viens de s'inscrire", Console.ForegroundColor = ConsoleColor.Yellow);
                    break;
                case Lt.OUVERTURE:
                    Console.WriteLine("[" + DateTime.Now.ToString() + "] - " + text, Console.ForegroundColor = ConsoleColor.Cyan);
                    break;
                case Lt.SERVEUR:
                    Console.WriteLine("[" + DateTime.Now.ToString() + "] - " + text, Console.ForegroundColor = ConsoleColor.Gray);
                    break;
                    
            }
            
        }
        public static void Main(string[] args)
        {
            
            
            Console.Title = "QuickGameFinder - Server";
            int count = 1;

            TcpListener ServerSocket = new TcpListener(IPAddress.Any, 5000);
            
            Log("Ouverture des connexions...", Lt.SERVEUR);
            try
            {
                ServerSocket.Start();
                Log("Succès", Lt.SERVEUR);
            }
            catch
            {
                Log("Failed", Lt.ERREUR);
            }
            while (true)
            {
                TcpClient client = ServerSocket.AcceptTcpClient();
                lock (_lock) list_clients.Add(count, client);
                

                Thread t = new Thread(handle_clients);
                t.Start(count);
                count++;
            }
        }
        public static bool dbonline = false;
        public static int onlineplayers = 0;
        public static void handle_clients(object o)
        {
            MySqlConnection connection = new MySqlConnection("datasource=mysql-quickgamefinder.alwaysdata.net; port=3306;Initial Catalog='quickgamefinder_dev';username=189919;password=Yujilaosyalere94");
            
            
            try
            {
                

                if (dbonline == false)
                {
                    Log("Connexion a la DB...", Lt.SERVEUR);
                    connection.Open();
                    Log("Connecté !", Lt.SERVEUR);
                    //OnlineStats.GetStats(connection);
                    dbonline = true;
                }
            }
            catch(Exception e)
            {
                Log(e.ToString(), Lt.ERREUR);
            }
            int id = (int)o;
            TcpClient client;

            lock (_lock) client = list_clients[id];
            Log("Nouvelle ouverture du logiciel", Lt.OUVERTURE);
            string zpassword = "9856943630rpz";
            int groupID = 0;
            while (true)
            {
             Console.Title = "QuickGameFinder - Server [" + OnlineStats._onlineplayers.ToString() + " Online players]";
              try {
                    #region sys
                    
                    NetworkStream stream = client.GetStream();
                    byte[] buffer = new byte[1024];
                
                    int byte_count = stream.Read(buffer, 0, buffer.Length);

                    if (byte_count == 0)
                    {
                        break;
                    }

                    string data = Encoding.ASCII.GetString(buffer, 0, byte_count);
                    Log(data, Lt.SERVEUR);
                    #endregion

                    if (data.Contains("sendglobal")&& data.Contains(zpassword))
                    {
                        string[] splitter = data.Split('|');
                        string msg = splitter[1];
                        broadcast("notif|" + msg);
                    }
                    if(data.Contains("sendnotif") && data.Contains(zpassword))
                    {
                        string[] splitter = data.Split('|');
                        string username = splitter[1];
                        string msg = splitter[2];
                        TcpClient clients = new TcpClient();
                        byte[] msgs = Encoding.ASCII.GetBytes("notifsuccess");
                        stream.Write(msgs, 0, msgs.Length);

                        foreach (User u in ServerLists.users)
                        {
                            if(u.username == username)
                            {
                                clients = u.tcp_id;
                                Console.WriteLine("Envoie de la notification a " + username + "(" + u.tcp_id.ToString() + ")", Lt.SERVEUR);
                            }
                            
                        }
                 NetworkStream sstream = clients.GetStream();
                 byte[] imsg = Encoding.ASCII.GetBytes("notif|" + msg);
                 stream.Write(imsg, 0, imsg.Length);

                    }
                    if(data.Contains("kick") && data.Contains(zpassword))
                    {
                        string[] splitter = data.Split('|');
                        string user = splitter[1];
                        
                        TcpClient clients = new TcpClient();
                        foreach (User u in ServerLists.users)
                        {
                            if (u.username == user)
                            {
                                clients = u.tcp_id;
                            }

                        }
                        
                                    NetworkStream sstream = clients.GetStream();
                                    byte[] imsg = Encoding.ASCII.GetBytes("quit");
                                    stream.Write(imsg, 0, imsg.Length);
                                
                            
                        

                    }
                    if (data.Contains("kickall") && data.Contains(zpassword))
                    {
                        string[] splitter = data.Split('|');

                        broadcast("quitall");
                    }
                    if (data.Contains("AuthRequest"))
                    {
                        
                        string[] splitter = data.Split('|');
                        string username = splitter[1];
                        string password = splitter[2];
                        TcpClient clients = new TcpClient();
                        clients = client;
                        byte[] msg = Auth(connection, username, password, clients);
                       
                        stream.Write(msg, 0, msg.Length);
                       
                    }
                    if (data.Contains("RegisterRequest"))
                    {
                        TcpClient clients = new TcpClient();
                        clients = client;
                        string[] splitter = data.Split('|');
                        string username = splitter[1];
                        string email = splitter[2];
                        string password = splitter[3];
                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }
                        MySqlDataAdapter checkids = new MySqlDataAdapter("SELECT * FROM users WHERE username = '" + username + "'", connection);
                        DataTable table = new DataTable();
                        checkids.Fill(table);
                        byte[] toreturn = null;
                        string toconvert = "";
                        //Log("Tentative d'inscription...");
                        if (table.Rows.Count == 0)
                        {
                            MySqlDataAdapter checkidss = new MySqlDataAdapter("SELECT * FROM users WHERE email = '" + email + "'", connection);
                            DataTable tables = new DataTable();
                            checkidss.Fill(tables);
                            if (tables.Rows.Count == 0)
                            {
                                MySqlCommand c = new MySqlCommand("INSERT INTO `users` (`username`, `email`, `password`, `rank`, `ban`) VALUES ('" + username + "', '" + email + "', '" + password + "', 'free', '0')", connection);
                                c.ExecuteNonQuery();
                                toconvert = "regsuccess";
                                Log(username, Lt.INSCRIPTION);
                            
                            }
                            else
                            {
                                toconvert = "regfailed";
                                //Log("Impossible de procéder a l'inscription");
                            }
                        }
                        else
                        {
                            toconvert = "regfailed";
                            //Log("Impossible de procéder a l'inscription");
                        }

                        toreturn = Encoding.ASCII.GetBytes(toconvert + "|" + username);
                        // con.Close();
                        Log(Encoding.ASCII.GetString(toreturn), Lt.SERVEUR);
                        NetworkStream nssss = clients.GetStream();
                        nssss.Write(toreturn, 0, toreturn.Length);
                        connection.Close();
                    }
                    if (data.Contains("DisconnectRequest"))
                    {

                        string[] splitter = data.Split('|');
                        string username = splitter[1];
                        byte[] yoloh = Encoding.ASCII.GetBytes("disconnectsuccess");
                        Log(username, Lt.DECONNEXION);
                        try
                        {
                            stream.Write(yoloh, 0, yoloh.Length);
                        }
                        catch (Exception e)
                        {
                            Log(e.ToString(), Lt.ERREUR);
                        }

                        User u = new User();
                        foreach (User user in ServerLists.users)
                        {
                            if (user.username == username)
                            {
                                u = user;
                                ServerLists.users.Remove(user);
                            }
                        }

                        foreach (Group g in ServerLists.groups)
                        {
                            if (g.users.Contains(u))
                            {
                                g.users.Remove(u);
                                foreach (User gu in g.users)
                                {
                                    NetworkStream givedc = gu.tcp_id.GetStream();
                                    byte[] b = Encoding.ASCII.GetBytes("PlayerDisconnect|" + username);
                                    givedc.Write(b, 0, b.Length);
                                }
                            }
                            if (g.author == username)
                            {
                                ServerLists.groups.Remove(g);
                            }
                        }
                        if (username != "")
                        {
                            OnlineStats._onlineplayers--;
                            broadcast("OP|" + OnlineStats._onlineplayers.ToString() + "|" + OnlineStats._onlinegroups.ToString());
                            broadcast(Encoding.ASCII.GetString(ServerLists.GetGroups()));
                        }
                       



                    }
                    
                    if (data.Contains("CreateGroupRequest"))//CreateGroupRequest|author|actualplayers|desiredplayers|public/private|gameIDs|groupname|groupdesc
                    {
                        
                        string[] splitter = data.Split('|');
                        string author = splitter[1];
                        int actualplayers = int.Parse(splitter[2]);
                        int desiredplayers = int.Parse(splitter[3]);
                        string _public = splitter[4];
                        string gameID = splitter[5];
                        string roomname = splitter[6];
                        string roomdesc = splitter[7];
                     
                        string rank = "";
                        Log("Recherche du rank de " + author + " (" + ServerLists.users.Count.ToString() + ")", Lt.SERVEUR);
                        foreach (User u in ServerLists.users)
                        {
                            if(u.username == author)
                            {
                                rank = u.rank;
                            }
                        }
                        Random rnd = new Random();
                        groupID = rnd.Next(1, 99999999);
                        foreach(Group g in ServerLists.groups)
                        {
                            if(g.author == author)
                            {
                                ServerLists.groups.Remove(g);
                            }
                        }
                        Group grp = new Group(roomname, roomdesc, author, actualplayers, desiredplayers, gameID, _public, groupID, rank);
                        ServerLists.groups.Add(grp);
                       
                        Log("Groupe " + gameID + " créé par " + author + "(" + rank +")", Lt.SERVEUR);
                        broadcast(Encoding.ASCII.GetString(ServerLists.GetGroups()));
                        OnlineStats._onlinegroups++;
                        broadcast("OP|" + OnlineStats._onlineplayers.ToString() + "|" + OnlineStats._onlinegroups.ToString());

                    }
                    if (data.Contains("MissingGame"))
                    {
                        string[] splitter = data.Split('|');
                        string game = splitter[1];
                        string sender = splitter[2];
                        Log("Jeu manquant : " + game + " , rapport envoyé par " + sender, Lt.SERVEUR);
                        string content = "Le joueur " + sender + " a reporté le jeu '" + game + "' comme manquant.";
                        SendMail.Mail(content, SendMail.MailType.MISSINGGAME);
                    }
                    if (data.Contains("GetGroups"))
                    {
                        byte[] msg = ServerLists.GetGroups();
                        stream.Write(msg, 0, msg.Length);
                        Log("Groupes envoyés (" + ServerLists.groups.Count.ToString() + ")", Lt.SERVEUR);
                        broadcast("OP|" + OnlineStats._onlineplayers.ToString() +"|" + OnlineStats._onlinegroups.ToString());
                    }
                    if (data.Contains("Error"))
                    {
                        string[] splitter = data.Split('|');

                        Log(splitter[1], Lt.ERREUR);
                    }
                    if (data.Contains("RemoveRoom"))
                    {
                        string[] splitter = data.Split('|');
                        int room = int.Parse(splitter[1]);
                        
                        foreach (Group g in ServerLists.groups)
                        {
                            if(g.roomiD == room)
                            {
                                string author = g.author;
                                foreach (User u in g.users)
                                {
                                    if(u.username == author)
                                    {
                                        NetworkStream remove = u.tcp_id.GetStream();
                                        byte[] message = Encoding.ASCII.GetBytes("RoomDisbanded");
                                        remove.Write(message, 0, message.Length);
                                        g.users.Remove(u);
                                       
                                    }

                                }
                                foreach (User u in g.users)
                                {
                                    NetworkStream remove = u.tcp_id.GetStream();
                                    byte[] message = Encoding.ASCII.GetBytes("RoomKicked|nextriesh");
                                    remove.Write(message, 0, message.Length);
                                }
                                Group.DisbandGroup(room);
                                broadcast(Encoding.ASCII.GetString(ServerLists.GetGroups()));
                            }
                        }
                    }
                    if (data.Contains("SendMessageRequest"))
                    {
                        string[] splitter = data.Split('|');
                        string content = splitter[1];
                        string auteur = splitter[2];
                        int room = int.Parse(splitter[3]);
                     
                        foreach (Group r in ServerLists.groups)
                        {
                            if(r.roomiD == room)
                            {
                                foreach(User u in r.users)
                                {
                                    
                                        NetworkStream s = u.tcp_id.GetStream();
                                        byte[] buffers = Encoding.ASCII.GetBytes("NewMessage|" + content + "|" + auteur);
                                        s.Write(buffers, 0, buffers.Length);


                                    
                                }
                            }
                        }
                    }
                    if (data.Contains("JoinCreatedGroup"))
                    {
                        string[] splitter = data.Split('|');
                        string user = splitter[1];
                        User u = new User();
                        foreach(User usesr in ServerLists.users)
                        {
                            if(usesr.username== user)
                            {
                                u = usesr;
                            }
                        }
                        int done = 0;
                      
                        foreach(Group r in ServerLists.groups)
                        {

                            if(r.author == user)
                            {
                                r.users.Add(u);
                                byte[] boffer = Encoding.ASCII.GetBytes("JoinSuccess|" + user + "/free;" +"|" + r.roomname + "|" + r.roomdescription + "|" + r.author + "|" + user + "|" + r.roomiD.ToString());
                                Log("JoinSuccess|" + user + "/free;" + "|" + r.roomname + "|" + r.roomdescription + "|" + r.author + "|" + user, Lt.SERVEUR);
                                NetworkStream sss = u.tcp_id.GetStream();
                                sss.Write(boffer, 0, boffer.Length);
                                done = 1;
                            }
                            if(done == 1)
                            {
                                break;
                            }
                        }
                    }
                    if (data.Contains("RoomKick"))
                    {
                        string[] splitter = data.Split('|');
                        string tokick = splitter[1];
                        string admin = splitter[2];
                        foreach(User u in ServerLists.users)
                        {
                            if(u.username == tokick)
                            {
                                
                                NetworkStream sss = u.tcp_id.GetStream();
                                byte[] tobuf = Encoding.ASCII.GetBytes("RoomKicked|" + admin);
                                sss.Write(tobuf, 0, tobuf.Length);
                                Log(admin + " a kick " + tokick, Lt.SERVEUR);
                                foreach(Group r in ServerLists.groups)
                                {
                                    if(r.author == admin)
                                    {
                                        try
                                        {
                                            r.playercounter--;
                                            r.users.Remove(u);
                                            foreach(User use in r.users)
                                            {
                                                NetworkStream ss = use.tcp_id.GetStream();
                                                byte[] tobufs = Encoding.ASCII.GetBytes("PlayerKicked|" + tokick);
                                                ss.Write(tobufs, 0, tobufs.Length);
                                            }
                                        }
                                        catch
                                        {

                                        }
                                    }
                                }
                            }

                        }
                    }
                    if (data.Contains("JoinRoomRequest"))
                    {
                        string[] splitter = data.Split('|');
                        int roomid = int.Parse(splitter[1]);
                        string username = splitter[2];
                        User user = new User();
                        #region finduser
                        foreach(User u in ServerLists.users)
                        {
                            if(u.username == username)
                            {
                                user.username = u.username;
                                user.tcp_id = u.tcp_id;
                                user.rank = u.rank;
                            }
                        }
                        Log(user.username, Lt.SERVEUR);
                        #endregion
                        #region findroom
                   
                        foreach(Group r in ServerLists.groups)
                        {
                            if(r.roomiD == roomid)
                            {
 
                                if (r.playercounter != r.desiredplayers)
                                {
                                    r.playercounter++;


                                    r.users.Add(user);
                                    string users = r.GetUsers();
                                    byte[] boffer = Encoding.ASCII.GetBytes("JoinSuccess|" + users + "|" + r.roomname + "|" + r.roomdescription + "|" + r.author + "|" + username + "|" + r.roomiD.ToString());
                                    NetworkStream sss = user.tcp_id.GetStream();
                                    sss.Write(boffer, 0, boffer.Length) ;
                                    Log("JoinSuccess|" + users + "|" + r.roomname + "|" + r.roomdescription + "|" + r.author + "|" + username + "|" + r.roomiD.ToString(), Lt.SERVEUR);
                                    Log("Taille salon: (" + r.playercounter.ToString() + "/" + r.desiredplayers.ToString() + ")", Lt.SERVEUR);
                                    
                                    foreach (User u in r.users)
                                    {

                                        if (r.users.Count > 0)
                                        {
                                            if(u.username != username)
                                            {
                                               
                                               broadcast("PlayerJoined|" + username + "|" + user.rank + "|" + r.roomiD.ToString());
                                               
                                                Log("PlayerJoined|" + username + "|" + user.rank + "|" + r.roomiD.ToString(), Lt.SERVEUR);
                                            }
                                          
                                        }
                                        
                                        
                                    }
                                  

                                }
                                else
                                {
                                    Log("Salon plein ("+r.playercounter.ToString() + "/" + r.desiredplayers.ToString() + ")" , Lt.ERREUR);

                                        //JoinSuccess|user1/premium;user2/free;user3/free;|roomtitle|roomdesc|author
                                        NetworkStream s = user.tcp_id.GetStream();

                                        string users = r.GetUsers();
                                        byte[] buffers = Encoding.ASCII.GetBytes("RoomFull");
                                        s.Write(buffers, 0, buffers.Length);
                                    
                                }
                            }
                        }
                        #endregion
                    }
                    #region endsys
                }

                catch
                {
                    break;
                }
            }

            lock (_lock) list_clients.Remove(id);
            client.Client.Shutdown(SocketShutdown.Both);
            client.Close();
#endregion
        }

        public static void broadcast(string data)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(data + Environment.NewLine);

            lock (_lock)
            {
                foreach (TcpClient c in list_clients.Values)
                {
                    NetworkStream stream = c.GetStream();
                    stream.Write(buffer, 0, buffer.Length);
                  //  Console.WriteLine("Message envoyé a un utilisateur", Lt.SERVEUR);
                }
            }
        }
        
        public static byte[] Auth(MySqlConnection con, string username, string password, TcpClient id)
        {
            User user = new User();
            user.tcp_id = id;
            user.username = username;
            //Log("Check des Auth pour " + username)
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            MySqlDataAdapter checkids = new MySqlDataAdapter("SELECT * FROM `users` WHERE `username` LIKE '" + username + "' AND `password` LIKE '" + password +"'", con);
            DataTable table = new DataTable();
            checkids.Fill(table);
            byte[] toreturn = null;
            string toconvert = "";
           
            if (table.Rows.Count > 0)
            {
           
                MySqlCommand verifban = new MySqlCommand("SELECT ban FROM users WHERE username = '" + username + "'", con);

                try
                    {
                        using (MySqlDataReader reader = verifban.ExecuteReader())
                    {
                     
                        if (reader.HasRows)
                        {
                          
                            if (reader.Read())
                            {
                                string ban = reader.GetValue(0).ToString();
                                if (ban == "0")
                                {
                                   
                                    toconvert = "authsuccess";

                                    //Memory.users.Add(user);
                                    OnlineStats._onlineplayers = OnlineStats._onlineplayers + 1;
                                    Console.Title = "QuickGameFinder - Server [" + OnlineStats._onlineplayers.ToString() + " Online players]";
                                    Log(username, Lt.CONNEXION);
                                    broadcast("OP|" + OnlineStats._onlineplayers.ToString() + "|" + OnlineStats._onlinegroups.ToString());
                                }
                                else
                                {
                                    Log(username, Lt.BAN);
                                    toconvert = "authbanned";
                                }
                                
                                }
                            
                        }
                        }
                        }

                        catch
                        {
                            Log("Impossible de terminer la commande", Lt.SERVEUR);
                    
                        }
                   
               
                }
            else
            {
                //Log("Aucun compte pour " + username);
                toconvert = "authfailed";

            }
            if(toconvert == "authfailed" || toconvert == "authbanned")
            {

            }
            else
            {
                MySqlCommand verifrank = new MySqlCommand("SELECT rank FROM users WHERE username = '" + username + "'", con);
                using (MySqlDataReader reader = verifrank.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            string rank = (string)reader.GetValue(0);
                            toconvert = toconvert + "|" + rank;
                            user.rank = rank;
                        }
                    }
                }
                        }
            toconvert.Trim();
            toreturn = Encoding.ASCII.GetBytes(toconvert);
            //Log(toconvert);
            con.Close();
            ServerLists.users.Add(user);
            return toreturn;
        }


    }
}

//byte[] b = Encoding.ASCII.GetBytes("alreadyconnected");
//stream.Write(b, 0, b.Length);
//broadcast(data); <- global message