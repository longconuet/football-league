$(document).ready(function () {
    getUserList();
});

var token = $('input[name="__RequestVerificationToken"]').val();

function getUserList() {
    $.ajax({
        type: "GET",
        url: '/User/GetUserListAjax',
        contentType: 'application/json',
        headers: {
            'RequestVerificationToken': token, // Thêm token vào header
        },
        success: function (response) {
            if (response.status == 0) {
                toastr.error(response.message, 'Error');
                return;
            }

            let tableHtml = '';
            let userData = response.data;
            if (userData.length > 0) {
                $.each(userData, function (index, user) {
                    tableHtml += '<tr>';
                    tableHtml += '<td>' + user.fullName + '</td>';
                    tableHtml += '<td>' + user.phone + '</td>';
                    tableHtml += '<td>' + user.email + '</td>';
                    tableHtml += '<td>' + getRoleLabel(user.role) + '</td>';
                    tableHtml += '<td>' + getStatusLabel(user.status) + '</td>';

                    let editBtnHtml = '<button type="button" class="btn btn-primary m-1" onClick="showEditModal(\'' + user.id + '\')"><i class="bi bi-pencil-fill"></i> Edit</button>';
                    let deleteBtnHtml = '<button type="button" class="btn btn-danger" onClick="showDeleteModal(\'' + user.id + '\')"><i class="bi bi-trash-fill"></i> Delete</button>';
                    let updateStatusBtnHtml = '';
                    if (user.status == 0) {
                        updateStatusBtnHtml = '<button type="button" class="btn btn-success m-1" onClick="showUpdateStatusModal(\'' + user.id + '\', ' + user.status + ')"><i class="bi bi-check-circle"></i> Active</button>';
                    }
                    else {
                        updateStatusBtnHtml = '<button type="button" class="btn btn-warning m-1" onClick="showUpdateStatusModal(\'' + user.id + '\', ' + user.status + ')"><i class="bi bi-dash-circle"></i> De-Active</button>';
                    }

                    tableHtml += '<td>' + updateStatusBtnHtml + editBtnHtml + deleteBtnHtml + '</td>';
                    tableHtml += '</tr>';
                });
            }
            else {
                tableHtml = '<tr><td colspan="6" class="text-center">No data</td></tr>';
            }

            $('#user-table-data').html(tableHtml);
        },
        error: function (error) {
            toastr.error('Error', error)
        }
    });
}

function getStatusLabel(status) {
    if (status == 1) {
        return '<span class="badge bg-success">Active</span>';
    }
    return '<span class="badge bg-danger">Inactive</span>';
}

function getRoleLabel(role) {
    if (role == 1) {
        return '<span class="badge rounded-pill bg-success">Normal user</span>';
    }
    return '<span class="badge rounded-pill bg-primary">Admin</span>';
}

function showCreateModal() {
    $('#create-user-modal').modal('show');
}

function submitCreateUser() {
    var data = {
        FullName: $('#name').val(),
        Phone: $('#phone').val(),
        Email: $('#email').val(),
        Role: $('#role').val(),
        Status: $('#status').is(':checked') ? 1 : 0
    };

    $.ajax({
        url: '/User/CreateUserAjax',
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
            $('#create-user-modal').modal('hide');
            clearCreateModalInput();
        },
        error: function (error) {
            toastr.error('Error', error)
        }
    });
}

function showEditModal(id) {
    $.ajax({
        type: "GET",
        url: '/User/GetUserInfoAjax/' + id,
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
            $('#update-user-id').val(id);
            $('#name-edit').val(userInfo.fullName);
            $('#phone-edit').val(userInfo.phone);
            $('#email-edit').val(userInfo.email);
            $('#edit-user-modal').modal('show');
        },
        error: function (error) {
            $('#update-user-id').val('');
            toastr.error('Error', error)
        }
    });
}

function submitUpdateUser() {
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
            toastr.error('Error', error)
        }
    });
}

function showDeleteModal(id) {
    $('#delete-user-id').val(id);
    $('#delete-user-modal').modal('show');
}

function submitDeleteUser() {
    $.ajax({
        url: '/User/DeleteUserAjax/' + $('#delete-user-id').val(),
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
            $('#delete-user-modal').modal('hide');
        },
        error: function (error) {
            toastr.error('Error', error)
        }
    });
}

function showUpdateStatusModal(id, currentStatus) {
    $('#update-status-user-id').val(id);
    $('#new-user-status').val(currentStatus == 0 ? 1 : 0);
    if (currentStatus == 0) {
        $('#status-update-span').html('<span class="text-success">ACTIVE</span>');
    }
    else {
        $('#status-update-span').html('<span class="text-danger">DE-ACTIVE</span>');
    }
    $('#update-status-user-modal').modal('show');
}

function submitUpdateUserStatus() {
    var data = {
        Id: $('#update-status-user-id').val(),
        Status: $('#new-user-status').val()
    };

    $.ajax({
        url: '/User/UpdateUserStatusAjax',
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
            $('#update-status-user-modal').modal('hide');
        },
        error: function (error) {
            toastr.error('Error', error)
        }
    });
}

function clearCreateModalInput() {
    $('#name').val('');
    $('#phone').val('');
    $('#email').val('');
    $('#name').val('');
}