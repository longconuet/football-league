$(document).ready(function () {
    getGroupList();
});

var token = $('input[name="__RequestVerificationToken"]').val();

function getGroupList() {
    $.ajax({
        type: "GET",
        url: '/Group/GetGroupListAjax',
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
            let groupData = response.data;
            if (groupData.length > 0) {
                $.each(groupData, function (index, group) {
                    tableHtml += '<tr>';
                    tableHtml += '<td>' + group.name + '</td>';

                    tableHtml += '<td>';
                    if (group.teams.length > 0) {
                        tableHtml += '<div class="row">';
                        $.each(group.teams, function (index, team) {
                            tableHtml += '<div class="col-md-3">';
                            tableHtml += `<span><img src="${team.image}" style="max-width: 50px; max-height: 50px;" alter="img" title="img"></span>`;
                            tableHtml += `<span class="m-2">${team.name}</span>`;
                            tableHtml += '</div>';
                        });
                        tableHtml += '</div>';
                    }
                    else {
                        tableHtml += 'No teams';
                    }
                    tableHtml += '</td>';

                    let editBtnHtml = '<button type="button" class="btn btn-primary m-1" onClick="showEditModal(\'' + group.id + '\')"><i class="bi bi-pencil-fill"></i> Edit</button>';
                    let deleteBtnHtml = '<button type="button" class="btn btn-danger" onClick="showDeleteModal(\'' + group.id + '\')"><i class="bi bi-trash-fill"></i> Delete</button>';

                    tableHtml += '<td>' + editBtnHtml + deleteBtnHtml + '</td>';
                    tableHtml += '</tr>';
                });
            }
            else {
                tableHtml = '<tr><td colspan="3" class="text-center">No data</td></tr>';
            }

            $('#group-table-data').html(tableHtml);
        },
        error: function (error) {
            toastr.error('Error', error)
        }
    });
}

function showCreateModal() {
    $('#create-group-modal').modal('show');
}

function submitCreateGroup() {
    var data = {
        Name: $('#name').val(),
    };

    $.ajax({
        url: '/Group/CreateGroupAjax',
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
            getGroupList();
            $('#create-group-modal').modal('hide');
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
        url: '/Group/GetGroupInfoAjax/' + id,
        contentType: 'application/json',
        headers: {
            'RequestVerificationToken': token
        },
        success: function (response) {
            if (response.status == 0) {
                toastr.error(response.message, 'Error');
                return;
            }

            let groupInfo = response.data;
            $('#update-group-id').val(id);
            $('#name-edit').val(groupInfo.name);
            $('#edit-group-modal').modal('show');
        },
        error: function (error) {
            $('#update-group-id').val('');
            toastr.error('Error', error)
        }
    });
}

function submitUpdateGroup() {
    var data = {
        Id: $('#update-group-id').val(),
        Name: $('#name-edit').val()
    };

    $.ajax({
        url: '/Group/UpdateGroupAjax',
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
            getGroupList();
            $('#edit-group-modal').modal('hide');
        },
        error: function (error) {
            toastr.error('Error', error)
        }
    });
}

function showDeleteModal(id) {
    $('#delete-group-id').val(id);
    $('#delete-group-modal').modal('show');
}

function submitDeleteGroup() {
    $.ajax({
        url: '/Group/DeleteGroupAjax/' + $('#delete-group-id').val(),
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
            getGroupList();
            $('#delete-group-modal').modal('hide');
        },
        error: function (error) {
            toastr.error('Error', error)
        }
    });
}

function clearCreateModalInput() {
    $('#name').val('');
}