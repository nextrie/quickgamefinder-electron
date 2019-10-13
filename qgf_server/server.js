var net = require("net");
const port = 5000;
const server = new net.Server();
const Database = require("./database/db").Database;
const User = require("./IO/user").User;
const isDevBuild = true;
const Room = require("./client/room/room").Room;

var clients = [];
var rooms = [];

var db = null;

if(isDevBuild)
{
    server.listen(5000, "0.0.0.0", () => {
        console.log("Server ready");
        db = new Database("mysql-quickgamefinder.alwaysdata.net", "189919", "quickgamefinder_dev", "Yujilaosyalere94");
    });    
}
else
{
    server.listen(10671, "0.0.0.0", () => {
        console.log("Server ready");
        db = new Database("mysql-quickgamefinder.alwaysdata.net", "189919", "quickgamefinder_dev", "Yujilaosyalere94");
    });
}

//#region gestion serveur / packets
server.on('connection', function (socket) {
    console.log('A new connection has been established.');
    socket.setEncoding('utf8')
    //socket.write('Hello, client.');
    // The server can also receive data from the client by reading from its socket.
    socket.on('data', function (chunk) {
        console.log(chunk);
        handleData(chunk, socket);
    });

    socket.on('end', function() {
        console.log('Closing connection with the client');
    });

    socket.on('error', function(err) {
        console.log("Error: " + err.toString());
    });
});

function handleData(data, socket)
{
    var tab = JSON.parse(data);
    if(tab.length >= 1)
    {
        var requestID = tab[0];
        switch(requestID)
        {
            case "AuthenticationRequest"://username md5password
                if(tab.length == 3)
                    authenticateUser(tab[1], tab[2], socket); 
                break;
            case "RegisterRequest": //email username pass
                if(tab.length == 4)    
                    registerUser(tab[1], tab[2], tab[3], socket);
                break;
                
            default:
                sendWrongQuery(socket);
                break;
        }
    }
}

function sendWrongQuery(socket)
{
    writeSocket(["PacketInterpretationFail","Erreur lors de l'interpretation de la requete"], socket);
}

function writeSocket(data, socket)
{
    try {
        socket.write(JSON.stringify(data));
    } catch (error) {
        console.log(error);
    }  
}
//#endregion

//#region connexion_register

function findUser(username)
{
    var toreturn = null;
    clients.forEach((element) => {
        if (element.data["username"] == username) {
            toreturn = element;
        }
    });
    return toreturn;
}

function removeUser(username)
{
    for (var i = 0; i < clients.length; i++) { 
        if(clients[i].data["username"] == username)
        {
            clients.splice(i, 1);
            break;
        }
    } 
}

function authenticateUser(username, password, socket)
{
    db.queryDB("SELECT * FROM users WHERE username='" + username + "' AND password='" + password + "'", function(err, data)
    {
        if(err)
        {
            console.log(err);
            sendWrongQuery(socket);
        }
        else
        {
            if(data.length == 1)
            {
                data = data[0]; // RowDataPacket content
                if(data["ban"] > 0)
                    writeSocket(["LoginFailed", "Vous avez été banni"], socket);
                else
                {
                    var user = new User(socket, data, "connected");
                    var userfound = findUser(data["username"]);
                    if(userfound != null)
                    {
                        userfound.socket.write(JSON.stringify(["DoubleConnectRequest", "Quelqu'un s'est connecté a votre compte."])); // Déconnecte l'utilisateur si double connexion a son compte;
                        removeUser(data["username"]);
                        clients.push(user);
                    }
                    clients.push(user);
                    console.log("Connexion de " + data["username"]);
                    writeSocket(["LoginSuccess", data["username"], data["rank"]], socket);
                }
            }
            else
                writeSocket(["LoginFailed", "Nom de compte ou mot de passe incorrect"], socket);
        }
    });
}

function registerUser(email, username, password, socket)
{
    db.queryDB("SELECT * FROM users WHERE email='"+ email +"'", function(err, data)
    {
        if(data.length == 0)
        {
            db.queryDB("SELECT * FROM users WHERE username='" + username + "'", function(err, data){
                if(data.length == 0)
                {
                    db.queryDB("INSERT INTO users(username, email, password, rank, ban) VALUES('" + username + "', '" + email +"', '" + password + "', 'free', 0)", function(err, data){
                        if(err) console.log(err);
                    });
                    console.log("Utilisateur " + username + " inscrit !");
                    writeSocket(["RegistrationSuccess", "Inscription réussie, vous pouvez maintenant vous connecter."], socket);
                }
                else
                    writeSocket(["RegistrationUsernameTaken", "Ce nom d'utilisateur existe déjà"], socket);
            });
        }
        else
            writeSocket(["RegistrationEmailTaken", "Cet email existe déjà"], socket);
    });
}

//#endregion

//#region rooms

function findRoom(username)
{
    var toreturn = null;
    rooms.forEach((room) => {
        if(room.author.data["username"] == username)
            toreturn = room;
    });
    return toreturn;
}

function findRoomByID(roomid)
{
    var toreturn = null;
    rooms.forEach((room) => {
        if(room.roomid == roomid)
            toreturn = room;
    });
    return toreturn;
}

function removeRoom(username)
{
    for (var i = 0; i < rooms.length; i++) { 
        if(rooms[i].author.data["username"] == username)
        {
            clients.splice(i, 1);
            break;
        }
    } 
}

function destroyRoom(host) 
{
    var room = findRoom(host.data["username"]);
    if(host.room != null)
    {
        console.log("Suppression du salon '" + host.room.name + "' (" + host.data["username"] + ")");
        room.members.forEach((element) => {
            if(element.data["username"] != room.author.data["username"])
                writeSocket(["RoomClosing", "L'hôte du salon a fermé le serveur."], element.socket);
            else
                writeSocket(["RoomLeft", "Vous avez quitté le salon '" + room.name + "'"], element.socket);
        });
        removeRoom(host.data["username"]);
        host.room = null;
    }
}

function createRoom(user, parameters)
{
    if(user.room == null)
    {
        var room = new Room
        (
            user, 
            parameters["room_name"], 
            parameters["room_desc"],
            parameters["room_max_players"],
            parameters["room_game_id"]
        );
        room.initRoom();
        rooms.push(room);
        writeSocket(["RoomCreateSuccess", room, "Salon créé !"], user.socket);
    }
    else
        writeSocket(["AlreadyInRoom", "Vous ne pouvez pas rejoindre 2 salons en même temps !"], user.socket);
}

function writeToRoom(room, message)
{
    room.members.forEach(user => {
        writeSocket(message, user.socket);
    });
}

function joinRoom(user, roomid)
{
    if(user.room == null)
    {
        var room = findRoomByID(roomid);
        if(room != null)
        {
            if(room.members.length < room.max_players)
            {
                //peut rejoindre le salon
                writeToRoom(room, ["UserJoin", user, "L'utilisateur " + user.data["username"] + " a rejoint le salon"]);
                writeSocket(["RoomJoinSuccess", room, "Salon rejoint"], user.socket);
            }
        }
        else
            writeSocket(["RoomDoesNotExit", "Impossible de rejoindre ce salon"]);
    }  
    else
        writeSocket(["AlreadyInRoom", "Vous ne pouvez pas rejoindre 2 salons en même temps !"], user.socket);
}

//#endregion