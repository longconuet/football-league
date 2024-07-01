var token = $('input[name="__RequestVerificationToken"]').val();

function login() {
    var data = {
        Username: $('#username').val(),
        Password: $('#password').val()
    };

    $.ajax({
        url: '/Auth/Login',
        data: JSON.stringify(data),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        headers: {
            'RequestVerificationToken': token
        },
        success: function (response) {
            if (response.status == 0) {
                toastr.error(response.message, 'Error');
                return;
            }

            localStorage.setItem('token', response.data);
            window.location.href = '/Home/Index';
        },
        error: function (error) {
            toastr.error(error, 'Error')
        }
    });
}