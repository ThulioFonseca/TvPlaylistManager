// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

document.addEventListener("DOMContentLoaded", function () {
    let toastElements = document.querySelectorAll(".toast");
    toastElements.forEach(function (toastEl) {
        let toast = new bootstrap.Toast(toastEl, { delay: 5000 });
        toast.show();
    });
});
