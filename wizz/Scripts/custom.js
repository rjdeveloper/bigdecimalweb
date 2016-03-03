
// loading function
jQuery(window).load(function () {
	jQuery(".loadingBg").delay(1000).fadeOut("slow");
});	
	
$(document).ready(function() {
	$('.ForgotBtn').click( function(){
		$('.ForgotPasswordSec').slideToggle();
		$('.LoginSec').slideToggle();
	});
	
	$('.CancelBtn').click( function(){
		$('.ForgotPasswordSec').slideToggle();
		$('.LoginSec').slideToggle();
	});	
	
});

$(function(){
    $('.RightContent').css({'min-height':(($(window).height())-70)+'px'});

    $(window).resize(function(){
    $('.RightContent').css({'min-height':(($(window).height())-70)+'px'});
    });
});