var networkStreamExists = false;
let client;
const NetworkClient = require("./client.js").client;

class User
{
    constructor(username, password)
    {
        this.username = username;
        this.password = password;
    }

    login(sendNotification, notifContent)
    {
        if (this.username.length < 3 || this.password.length < 3)
        {
            if (sendNotification != null) {
                sendNotification("Nom de compte ou mot de passe incorrect");
            }
        }
        else
        {

            //login phase
            if (sendNotification != null)
            {
                sendNotification(notifContent);
            }
            initLogin(this, sendNotification);
        }
    }

    register(sendNotification, data) //mail, username, pass
    {
        initRegister(sendNotification, data);
    }
}

function initRegister(sendNotification, data)
{
    if(data.length == 3)
    {
        if (networkStreamExists == false) 
        {
            client = new NetworkClient();
            client.ConnectToServer(sendNotification);
            client.RegisterUser(data);
            networkStreamExists = true;
        }
        else
            client.RegisterUser(data);
        module.exports.client = client;
    }
}

function initLogin(user, sendNotification)
{
    if(networkStreamExists == false)
    {
        client = new NetworkClient();
        client.ConnectToServer(sendNotification);
        client.AuthenticateUser(user);
    }
    else
        client.AuthenticateUser(user);
    networkStreamExists = true;
    module.exports.client = client;
}

module.exports.user = User;
