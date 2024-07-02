// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ajaxStart(function () {
    $('#loadingGif, #overlay').show();
}).ajaxStop(function () {
    $('#loadingGif, #overlay').hide();
});

function showLoading() {
    $('#loadingGif, #overlay').show();
}

function hideLoading() {
    $('#loadingGif, #overlay').hide();
}

function logout() {
    localStorage.removeItem('jwtToken');
    window.location.href = '/Auth/Logout';
}
