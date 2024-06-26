$(document).ready(function () {
    getTeamList();
    getGroupList();
    hideLoading();
});

var token = $('input[name="__RequestVerificationToken"]').val();

function getGroupList() {
    $.ajax({
        type: "GET",
        url: '/Team/GetGroupListAjax',
        contentType: 'application/json',
        headers: {
            'RequestVerificationToken': token, // Thêm token vào header
        },
        success: function (response) {
            if (response.status == 0) {
                toastr.error(response.message, 'Error');
                return;
            }

            let selectGroupHtml = '';
            let groupData = response.data;
            if (groupData.length > 0) {
                $.each(groupData, function (index, group) {
                    selectGroupHtml += '<option value="' + group.id + '">' + group.name + '</option>';
                });
                $('#group-id').html(selectGroupHtml);
                $('#group-id-edit').html(selectGroupHtml);
            }            
        },
        error: function (error) {
            toastr.error('Error', error)
        }
    });
}

function getTeamList() {
    $.ajax({
        type: "GET",
        url: '/Team/GetTeamListAjax',
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
            let teamData = response.data;
            if (teamData.length > 0) {
                $.each(teamData, function (index, team) {
                    tableHtml += '<tr>';
                    tableHtml += `<td class="text-center"><img src="${team.image}" style="max-width: 100px; max-height: 100px;" alter="img" title="img"></td>`;
                    tableHtml += '<td>' + team.name + '</td>';
                    tableHtml += '<td>' + team.group?.name + '</td>';

                    let editBtnHtml = `<button type="button" class="btn btn-primary m-1" onClick="showEditModal('${team.id}')"><i class="bi bi-pencil-fill"></i> Edit</button>`;
                    let deleteBtnHtml = `<button type="button" class="btn btn-danger" onClick="showDeleteModal('${team.id}')"><i class="bi bi-trash-fill"></i> Delete</button>`;

                    tableHtml += '<td>' + editBtnHtml + deleteBtnHtml + '</td>';
                    tableHtml += '</tr>';
                });
            }
            else {
                tableHtml = '<tr><td colspan="4" class="text-center">No data</td></tr>';
            }

            $('#team-table-data').html(tableHtml);
        },
        error: function (error) {
            toastr.error('Error', error)
        }
    });
}

function showCreateModal() {
    $('#create-team-modal').modal('show');
}

function submitCreateTeam() {
    var image = $('#image')[0].files[0];
    if (!image) {
        toastr.error("Please choose the image", "Error");
        return;
    }

    var formData = new FormData();
    formData.append('Name', $('#name').val());
    formData.append('Image', image);
    formData.append('GroupId', $('#group-id').val());

    $.ajax({
        url: '/Team/CreateTeamAjax',
        data: formData,
        type: "POST",
        processData: false,
        contentType: false,
        headers: {
            'RequestVerificationToken': token
        },
        success: function (response) {
            if (response.status == 0) {
                toastr.error(response.message, 'Error');
                return;
            }

            toastr.success(response.message, 'Success');
            getTeamList();
            $('#create-team-modal').modal('hide');
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
        url: '/Team/GetTeamInfoAjax/' + id,
        contentType: 'application/json',
        headers: {
            'RequestVerificationToken': token
        },
        success: function (response) {
            if (response.status == 0) {
                toastr.error(response.message, 'Error');
                return;
            }

            let teamInfo = response.data;
            $('#update-team-id').val(id);
            $('#selected-image-edit').attr('src', teamInfo.image);
            $('#selected-image-edit').show();
            $('#name-edit').val(teamInfo.name);
            $('#group-id-edit').val(teamInfo.groupId);
            $('#edit-team-modal').modal('show');
        },
        error: function (error) {
            $('#update-team-id').val('');
            toastr.error('Error', error)
        }
    });
}

function submitUpdateTeam() {
    var image = $('#image-edit')[0].files[0];
    if (!image && $('#selected-image-edit').attr('src') == '') {
        toastr.error("Please choose the image", "Error");
        return;
    }

    var formData = new FormData();
    formData.append('Id', $('#update-team-id').val());
    formData.append('Name', $('#name-edit').val());
    formData.append('Image', image);
    formData.append('GroupId', $('#group-id-edit').val());

    $.ajax({
        url: '/Team/UpdateTeamAjax',
        data: formData,
        type: "POST",
        processData: false,
        contentType: false,
        headers: {
            'RequestVerificationToken': token
        },
        success: function (response) {
            if (response.status == 0) {
                toastr.error(response.message, 'Error');
                return;
            }

            toastr.success(response.message, 'Success');
            getTeamList();
            $('#edit-team-modal').modal('hide');
        },
        error: function (error) {
            toastr.error('Error', error)
        }
    });
}

function showDeleteModal(id) {
    $('#delete-team-id').val(id);
    $('#delete-team-modal').modal('show');
}

function submitDeleteTeam() {
    $.ajax({
        url: '/Team/DeleteTeamAjax/' + $('#delete-team-id').val(),
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
            getTeamList();
            $('#delete-team-modal').modal('hide');
        },
        error: function (error) {
            toastr.error('Error', error)
        }
    });
}

function clearCreateModalInput() {
    $('#name').val('');
}

function showSelectedImage(input, mode = '') {
    var file = input.files[0];
    if (file) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#selected-image' + mode).attr('src', e.target.result);
            $('#selected-image' + mode).show();
        }

        reader.readAsDataURL(file);
    }
}