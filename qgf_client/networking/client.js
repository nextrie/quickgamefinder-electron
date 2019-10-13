var net = require("net");
var encryptToMD5 = require("../io/md5").encryptToMD5;
var client = new net.Socket();
client.setEncoding('utf8');
const changeCurrentPage = require("../js_tools/changePage").changeCurrentPage;
const changeCurrentMainPage = require("../js_tools/changePage").changeCurrentMainPage;
const isDevBuild = true;

function sleep(ms){
    return new Promise(resolve=>{
        setTimeout(resolve,ms)
    })
}

class Client
{
    constructor() 
    {
        this.username = undefined;
        this.rank = undefined;
        this.connection = client;
    }

    ConnectToServer(sendNotification)
    {
        if(isDevBuild)
            client.connect({host: "127.0.0.1", port: 5000});
        else
            client.connect({host: "srv06.skoa.li", port: 10671});

        client.on("connect", () => {
            //this.WriteToServer("Connecte !");
            //sendNotification("Connecté au serveur");
        });

        client.on("data", (data) => {
            console.log(JSON.parse(data));
            this.HandleData(JSON.parse(data), sendNotification);
        });

        client.on("end", () => {
            sendNotification("Connexion perdue !");
        });

        client.on("error", (err) => {
            console.log(err);
            sendNotification("Impossible de se connecter au serveur");
            client.end();
            return "connectionFailed";
        });
    }
    
    async HandleData(data, sendNotification)
    {
        var requestID = data[0];
        var length = data.length;
        switch(requestID)
        {
            case "LoginSuccess":
                sendNotification("Connecté ! Bienvenue, " + data[1]);
                this.rank = data[2];
                await sleep(3500);
                changeCurrentMainPage("firstconnexion", "firstconnexion", "Detection");
                break;
            case "LoginFailed":
                sendNotification("Echec de la connexion: " + data[1]);
                break;
            case "DoubleConnectRequest":
                sendNotification("Vous avez été déconnecté: " + data[1]);
                changeCurrentPage("index", "login");
                break;
            case "RegistrationSuccess":
                sendNotification(data[1]);
                changeCurrentPage("index", "login");
                break;
            case "RegistrationUsernameTaken":
                sendNotification("Erreur lors de l'inscription: " + data[1]);
                break;
            case "RegistrationEmailTaken":
                sendNotification("Erreur lors de l'inscription: " + data[1]);
                break;
            case "PacketInterpretationFail": 
                sendNotification("Echec de l'interpretation du packet");
                console.log(data[1]);
                break;
            default:
                sendNotification("Echec de l'interpretation du packet");
                console.log("Echec de l'interpretation du packet : " + data[0]);
                break;
        }
    }

    AuthenticateUser(user)
    {
        var md5Pass = encryptToMD5(user.password);
        var jsonRequest = JSON.stringify(["AuthenticationRequest", user.username, md5Pass]);
        this.WriteToServer(jsonRequest);
    }

    RegisterUser(data)
    {
        var email = data[0];
        var username = data[1];
        var pass = data[2];
        var md5Pass = encryptToMD5(pass);
        var jsonRequest = JSON.stringify(["RegisterRequest", email, username, md5Pass]);
        this.WriteToServer(jsonRequest);
    }

    WriteToServer(message)
    {
        client.write(message);
    }
}

module.exports.client = Client;




// client.on('connect', function () {
//     console.log('Client: connection established with server');

//     console.log('---------client details -----------------');
//     var address = client.address();
//     var port = address.port;
//     var family = address.family;
//     var ipaddr = address.address;
//     console.log('Client is listening at port' + port);
//     console.log('Client ip :' + ipaddr);
//     console.log('Client is IP4/IP6 : ' + family);


//     // writing data to server
//     client.write('hello from client');

// });

// client.setEncoding('utf8');

// client.on('data', function (data) {
//     console.log('Data from server:' + data);
// });

//setTimeout(function () {
//   client.end('Bye bye server');
//}, 5000);

