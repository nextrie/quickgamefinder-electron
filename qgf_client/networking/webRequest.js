const axios = require('axios')

async function sendGetRequest(destination)
{
    var tmp = await axios.get(destination)
        .then(function (response) {
            return response;
        })
        .catch(function (error) {
            return "Request Error";
        });
    return tmp;
}

module.exports.sendGetRequest = sendGetRequest;
