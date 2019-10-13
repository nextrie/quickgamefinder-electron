class Room
{
    constructor(author, name, description, max_players, gameid)
    {
        this.roomid = null;

        this.author = author;
        this.name = name;
        this.description = description;
        this.gameid = gameid;

        this.max_players = max_players;
        this.min_players = 1;
        
        this.members = [author];
        this.room_rank = author.data["rank"];
    }

    initRoom(rooms)
    {
        this.roomid = Math.random() * (9999999 - 1000) + 1000;
        rooms.forEach(element => {
            if(element.roomid == this.roomid)
                this.roomid = Math.random() * (9999999 - 1000) + 1000;
        });
    }
}

module.exports.Room = Room;