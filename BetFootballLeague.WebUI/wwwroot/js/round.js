$(document).ready(function () {
    getRoundList();
});

var token = $('input[name="__RequestVerificationToken"]').val();

function getRoundList() {
    $.ajax({
        type: "GET",
        url: '/Round/GetRoundListAjax',
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
            let roundData = response.data;
            if (roundData.length > 0) {
                $.each(roundData, function (index, round) {
                    tableHtml += '<tr>';
                    tableHtml += '<td>' + round.index + '</td>';
                    tableHtml += '<td>' + round.name + '</td>';
                    tableHtml += '<td>' + round.code + '</td>';
                    tableHtml += '<td>' + round.betPoint + '</td>';

                    let editBtnHtml = '<button type="button" class="btn btn-primary m-1" onClick="showEditModal(\'' + round.id + '\')"><i class="bi bi-pencil-fill"></i> Edit</button>';
                    let deleteBtnHtml = '<button type="button" class="btn btn-danger" onClick="showDeleteModal(\'' + round.id + '\')"><i class="bi bi-trash-fill"></i> Delete</button>';

                    tableHtml += '<td>' + editBtnHtml + deleteBtnHtml + '</td>';
                    tableHtml += '</tr>';
                });
            }
            else {
                tableHtml = '<tr><td colspan="5" class="text-center">No data</td></tr>';
            }

            $('#round-table-data').html(tableHtml);
        },
        error: function (error) {
            toastr.error('Error', error)
        }
    });
}

function showCreateModal() {
    $('#create-round-modal').modal('show');
}

function submitCreateRound() {
    var data = {
        Name: $('#name').val(),
        Code: $('#code').val(),
        Index: $('#index').val(),
        BetPoint: $('#bet-point').val(),
    };

    $.ajax({
        url: '/Round/CreateRoundAjax',
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
            getRoundList();
            $('#create-round-modal').modal('hide');
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
        url: '/Round/GetRoundInfoAjax/' + id,
        contentType: 'application/json',
        headers: {
            'RequestVerificationToken': token
        },
        success: function (response) {
            if (response.status == 0) {
                toastr.error(response.message, 'Error');
                return;
            }

            let roundInfo = response.data;
            $('#update-round-id').val(id);
            $('#name-edit').val(roundInfo.name);
            $('#index-edit').val(roundInfo.index);
            $('#bet-point-edit').val(roundInfo.betPoint);
            $('#edit-round-modal').modal('show');
        },
        error: function (error) {
            $('#update-round-id').val('');
            toastr.error('Error', error)
        }
    });
}

function submitUpdateRound() {
    var data = {
        Id: $('#update-round-id').val(),
        Name: $('#name-edit').val(),
        Index: $('#index-edit').val(),
        BetPoint: $('#bet-point-edit').val(),
    };

    $.ajax({
        url: '/Round/UpdateRoundAjax',
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
            getRoundList();
            $('#edit-round-modal').modal('hide');
        },
        error: function (error) {
            toastr.error('Error', error)
        }
    });
}

function showDeleteModal(id) {
    $('#delete-round-id').val(id);
    $('#delete-round-modal').modal('show');
}

function submitDeleteRound() {
    $.ajax({
        url: '/Round/DeleteRoundAjax/' + $('#delete-round-id').val(),
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
            getRoundList();
            $('#delete-round-modal').modal('hide');
        },
        error: function (error) {
            toastr.error('Error', error)
        }
    });
}

function clearCreateModalInput() {
    $('#name').val('');
    $('#code').val('');
    $('#index').val('');
    $('#bet-point').val('');
}