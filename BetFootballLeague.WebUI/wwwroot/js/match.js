$(document).ready(function () {
    getMatchList();
});

var token = $('input[name="__RequestVerificationToken"]').val();

function getMatchList() {
    $.ajax({
        type: "GET",
        url: '/Match/GetMatchListAjax',
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
            let matchData = response.data;
            if (matchData.length > 0) {
                $.each(matchData, function (index, match) {
                    tableHtml += '<tr>';
                    tableHtml += '<td>' + match.name + '</td>';

                    tableHtml += '<td>';
                    if (match.teams.length > 0) {
                        tableHtml += '<div class="row">';
                        $.each(match.teams, function (index, team) {
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

                    let editBtnHtml = '<button type="button" class="btn btn-primary m-1" onClick="showEditModal(\'' + match.id + '\')"><i class="bi bi-pencil-fill"></i> Edit</button>';
                    let deleteBtnHtml = '<button type="button" class="btn btn-danger" onClick="showDeleteModal(\'' + match.id + '\')"><i class="bi bi-trash-fill"></i> Delete</button>';

                    tableHtml += '<td>' + editBtnHtml + deleteBtnHtml + '</td>';
                    tableHtml += '</tr>';
                });
            }
            else {
                tableHtml = '<tr><td colspan="3" class="text-center">No data</td></tr>';
            }

            $('#match-table-data').html(tableHtml);
        },
        error: function (error) {
            toastr.error('Error', error)
        }
    });
}

function showCreateModal() {
    $('#create-match-modal').modal('show');
}

function submitCreateMatch() {
    var data = {
        Name: $('#name').val(),
    };

    $.ajax({
        url: '/Match/CreateMatchAjax',
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
            getMatchList();
            $('#create-match-modal').modal('hide');
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
        url: '/Match/GetMatchInfoAjax/' + id,
        contentType: 'application/json',
        headers: {
            'RequestVerificationToken': token
        },
        success: function (response) {
            if (response.status == 0) {
                toastr.error(response.message, 'Error');
                return;
            }

            let matchInfo = response.data;
            $('#update-match-id').val(id);
            $('#name-edit').val(matchInfo.name);
            $('#edit-match-modal').modal('show');
        },
        error: function (error) {
            $('#update-match-id').val('');
            toastr.error('Error', error)
        }
    });
}

function submitUpdateMatch() {
    var data = {
        Id: $('#update-match-id').val(),
        Name: $('#name-edit').val()
    };

    $.ajax({
        url: '/Match/UpdateMatchAjax',
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
            getMatchList();
            $('#edit-match-modal').modal('hide');
        },
        error: function (error) {
            toastr.error('Error', error)
        }
    });
}

function showDeleteModal(id) {
    $('#delete-match-id').val(id);
    $('#delete-match-modal').modal('show');
}

function submitDeleteMatch() {
    $.ajax({
        url: '/Match/DeleteMatchAjax/' + $('#delete-match-id').val(),
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
            getMatchList();
            $('#delete-match-modal').modal('hide');
        },
        error: function (error) {
            toastr.error('Error', error)
        }
    });
}

function clearCreateModalInput() {
    $('#name').val('');
}