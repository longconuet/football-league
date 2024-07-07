$(document).ready(function () {
    getUserInfoModal();
});

var token = $('input[name="__RequestVerificationToken"]').val();

function getUserInfoModal() {
    $.ajax({
        type: "GET",
        url: '/UserProfile/GetCurrentUserInfoAjax',
        contentType: 'application/json',
        headers: {
            'RequestVerificationToken': token
        },
        success: function (response) {
            if (response.status == 0) {
                toastr.error(response.message, 'Error');
                return;
            }

            let userInfo = response.data;
            $('#name-edit').val(userInfo.fullName);
            $('#username-edit').val(userInfo.username);
            $('#phone-edit').val(userInfo.phone);
            $('#email-edit').val(userInfo.email);
        },
        error: function (error) {
            toastr.error(error, 'Error')
        }
    });
}

function submitUpdateUserProfile() {
    var data = {
        FullName: $('#name-edit').val(),
        Phone: $('#phone-edit').val(),
        Email: $('#email-edit').val()
    };

    $.ajax({
        url: '/UserProfile/UpdateUserAjax',
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

            toastr.success(response.message, 'Success');
            getUserList();
            $('#edit-user-modal').modal('hide');
        },
        error: function (error) {
            toastr.error(error, 'Error')
        }
    });
}

function showChangePasswordModal() {
    $('#new-password').val('');
    $('#change-password-modal').modal('show');
}

function submitChangePassword() {
    var data = {
        Id: $('#update-user-id').val(),
        FullName: $('#name-edit').val(),
        Phone: $('#phone-edit').val(),
        Email: $('#email-edit').val()
    };

    $.ajax({
        url: '/User/UpdateUserAjax',
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

            toastr.success(response.message, 'Success');
            getUserList();
            $('#edit-user-modal').modal('hide');
        },
        error: function (error) {
            toastr.error(error, 'Error')
        }
    });
}