module.exports.sendStateNotification = function sendStateNotification(content)
{
    'use strict';
    var snackbarContainer = document.querySelector('#toastHandler');
    var data = { message: content.toString() };
    snackbarContainer.MaterialSnackbar.showSnackbar(data);
}