var crypto = require('crypto')

module.exports.encryptToMD5 = function encryptToMD5(str)
{
    return crypto.createHash('md5').update(str).digest("hex");
}

