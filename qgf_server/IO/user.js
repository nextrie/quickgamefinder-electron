class User
{
    constructor(socket, data, state)
    {
        this.socket = socket;
        this.data = data;
        this.state = state;
    }

    sendNotification(content, writeSocket)
    {
        writeSocket(["SendNotificationRequest", content]);
    }
}

module.exports.User = User;