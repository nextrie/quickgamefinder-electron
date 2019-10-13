//SCRIPT POUR LE CHANGEMENT D'AFFICHAGE DYNAMIQUE DE LA PAGE
var currentPage = "login";
$("#bodyContent").load("./html/index/" + currentPage + ".html");
$("#" + currentPage).attr("href", "./html/index/" + currentPage + ".css");

function changeCurrentPage(path, nextPage, pageTitle) 
{
    $("#bodyContent").fadeOut(200, () => {
        $("#bodyContent").empty();
        document.title = "QuickGameFinder - " + pageTitle;
        $("#bodyContent").load("./html/" + path + "/" + nextPage + ".html");
        $("#" + currentPage).attr("href", "./html/" + path + "/" + nextPage + ".css");
        $("#" + currentPage).attr("id" , nextPage);
        currentPage = nextPage;
    });
    $("#bodyContent").fadeIn();
}

function changeCurrentMainPage(path, nextPage, pageTitle) 
{
    $("#bodyContent").fadeOut(200, () => {
        $("#bodyContent").empty();

        $("#" + currentPage).attr("href", "");
        $("#" + currentPage).attr("id" , nextPage);

        document.title = "QuickGameFinder - " + pageTitle;

        $("#bodyContent").load("./html/" + path + "/" + nextPage + ".html");
        $("#maincss").attr("href", "./html/" + path + "/" + nextPage + ".css");

        currentPage = nextPage;
    });
    $("#bodyContent").fadeIn();
}

module.exports.changeCurrentPage = changeCurrentPage;
module.exports.changeCurrentMainPage = changeCurrentMainPage;
