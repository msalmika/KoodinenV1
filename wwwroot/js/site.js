// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// OPPITUNTILAYOUTIA VARTEN
$(document).ready(function () {
	$('.contenedor-menu li:has(ul)').click(function (e) {
		e.preventDefault();

		if ($(this).hasClass('activado')) {
			$(this).removeClass('activado');
			$(this).children('ul').slideUp();
		} else {
			$('.contenedor-menu li ul').slideUp();
			$('.contenedor-menu li').removeClass('activado');
			$(this).addClass('activado');
			$(this).children('ul').slideDown();
		}

		$('.contenedor-menu li ul li a').click(function () {
			window.location.href = $(this).attr('href');
		})
	});
});