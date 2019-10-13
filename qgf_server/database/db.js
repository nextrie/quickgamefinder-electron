var mysql = require("mysql");

class Database
{
    constructor(host, user, database, password)
    {
        this.host = host;
        this.user = user;
        this.database = database;
        this.password = password;
    }

    connectToDB()
    {
        var con = mysql.createConnection({
            host: this.host,
            user: this.user,
            database: this.database,
            password: this.password
        });
        
        return con;
        //  con.connect(function (err) {
        //      if (err) throw err;
        //      console.log("Connected to DB");
        //  });
    }

    queryDB(query, callback) {
        var con = this.connectToDB();
        var queryresult = con.query(query, function (err, result) {
            if (err) callback(err, null);
            else callback(null, result);
        });
        con.end();
    }
}

module.exports.Database = Database;