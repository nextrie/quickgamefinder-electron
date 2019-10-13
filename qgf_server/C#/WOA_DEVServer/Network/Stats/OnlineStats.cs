using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WOA_DEVServer.Network.Stats
{
    public class OnlineStats
    {
        public static int _groupscreated;
        public static int _registeredplayers;
        public static int _onlineplayers;
        public static int _onlinegroups;
        public static void GetStats(MySqlConnection con)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM Stats WHERE 'id' = 0", con);
            try
            {
                using (MySqlDataReader reader = cmd.ExecuteReader()) // created groups | registered players
                {
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            string g = reader.GetValue(0).ToString();
                            string r = reader.GetValue(1).ToString();
                            _groupscreated = int.Parse(g);
                            _registeredplayers = int.Parse(r);
                        }
                    }
                }
                Console.WriteLine("Stats obtenues :");
                Console.WriteLine("Groupes créés : " + _groupscreated.ToString());
                Console.WriteLine("Joueurs inscrits : " + _registeredplayers.ToString());
            }
            catch(Exception e)
            {
                Console.WriteLine("Impossible de récupérer les stats : " + e.ToString());
            }
        }
        public static void NewGroup(MySqlConnection con)
        {
            MySqlCommand cmd = new MySqlCommand("UPDATE 'Stats' SET 'createdgroups' = createdgroups + 1");
            cmd.ExecuteNonQuery();
            _groupscreated++;
        }
        public static void NewPlayer(MySqlConnection con)
        {
            MySqlCommand cmd = new MySqlCommand("UPDATE 'Stats' SET 'registeredplayers' = '" + _registeredplayers +"'");
            cmd.ExecuteNonQuery();
            _registeredplayers++;
        }
    }
}
