var siteUrl = '';
//svar siteUrl ='/wizz'
var emailpattern = /^([a-z0-9_\.-]+)@([\da-z\.-]+)\.([a-z\.]{2,6})$/;
function LoaderStart()
{
    $('#loader').show();
}

function LoaderStop()
{
    $('#loader').hide();
}