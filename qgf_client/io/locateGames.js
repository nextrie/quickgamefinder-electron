var fs = require('fs');

function locateGames(detectionDivID)
{
        fs.readdir('./imgs/examplegamelogo', (err, directory) => {
            if(err) console.log(err);
            for(let file of directory)
                $("#" + detectionDivID).append("<img id='detectionItem' src='./imgs/examplegamelogo/" + file + "' alt=''>");
        });
    return ["arma3", "csgo"];
}