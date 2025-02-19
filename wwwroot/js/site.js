// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

    document.addEventListener('DOMContentLoaded', function () {
    const tooltipTriggerList = [].slice.call(document.querySelectorAll('[title], [data-bs-title]'));
    tooltipTriggerList.forEach(function (tooltipTriggerEl) {
        new bootstrap.Tooltip(tooltipTriggerEl);
    });
  });

    $(document).ready(function () {
        $("#buscarUsuario").on("input", function () {
            var nombre = $(this).val();

            if (nombre.length > 2) {
                $.get("/Usuario/Buscar", { nombre: nombre }, function (data) {
                    $("#listaUsuarios").html(data);
                });
            } else {
                $("#listaUsuarios").empty();
            }
        });
});


const toastTrigger = document.getElementById('liveToastBtn')
const toastLiveExample = document.getElementById('liveToast')

if (toastTrigger) {
    const toastBootstrap = bootstrap.Toast.getOrCreateInstance(toastLiveExample)
    toastTrigger.addEventListener('click', () => {
        toastBootstrap.show()
    })
}

document.getElementById("togglePassword").addEventListener("click", function () {
    var passwordInput = document.getElementById("passwordInput");
    var eyeIcon = document.getElementById("eyeIcon");

    if (passwordInput.type === "password") {
        passwordInput.type = "text";
        eyeIcon.classList.remove("bi-eye");
        eyeIcon.classList.add("bi-eye-slash");
    } else {
        passwordInput.type = "password";
        eyeIcon.classList.remove("bi-eye-slash");
        eyeIcon.classList.add("bi-eye");
    }
});