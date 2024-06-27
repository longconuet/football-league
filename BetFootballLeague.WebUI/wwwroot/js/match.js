$(document).ready(function () {
    getMatchList();

    initDateTimePicker();

    setTeamDataForSelect2();
    setRoundDataForSelect();
});

function initDateTimePicker() {
    $('.datepicker').datepicker({
        uiLibrary: 'bootstrap5',
        format: 'dd/mm/yyyy'
    });

    $('.timepicker').timepicker({
        uiLibrary: 'bootstrap5'
    });
}

function formatState(state) {
    var $state = $(
        '<span><img src="' + state.image + '" class="img-flag" width="50px" /> ' + state.text + '</span>'
    );
    return $state;
};

var token = $('input[name="__RequestVerificationToken"]').val();

function setTeamDataForSelect2() {
    var teamData = [];
    $.ajax({
        type: "GET",
        url: '/Team/GetSimpleTeamListAjax',
        contentType: 'application/json',
        headers: {
            'RequestVerificationToken': token, // Thêm token vào header
        },
        success: function (response) {
            if (response.status == 0) {
                toastr.error(response.message, 'Error');
                return;
            }

            let data = response.data;
            if (data.length > 0) {
                $.each(data, function (index, team) {
                    teamData.push({
                        id: team.id,
                        text: team.name,
                        image: team.image
                    });
                });
            }


            $("#team1, #team2").select2({
                dropdownParent: $("#create-match-modal"),
                data: teamData,
                templateResult: formatState,
                allowClear: true,
                placeholder: 'Select a team...'
            });

            $("#team1-edit, #team2-edit").select2({
                dropdownParent: $("#edit-match-modal"),
                data: teamData,
                templateResult: formatState,
                allowClear: true,
                placeholder: 'Select a team...'
            });
        },
        error: function (error) {
            toastr.error('Error', error)
        }
    });

    return teamData;
}

function setRoundDataForSelect() {
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

            let selectHtmml = '<option value="">Select Round...</option>';
            let data = response.data;
            if (data.length > 0) {
                $.each(data, function (index, round) {
                    selectHtmml += `<option value="${round.id}">${round.name}</option>`
                });
            }

            $('#round').html(selectHtmml);
            $('#round-edit').html(selectHtmml);
        },
        error: function (error) {
            toastr.error('Error', error)
        }
    });
}

function getMatchList() {
    $.ajax({
        type: "GET",
        url: '/Match/GetMatchListAjax',
        contentType: 'application/json',
        headers: {
            'RequestVerificationToken': token,
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
                    tableHtml += '<td>' + match.indexOrder + '</td>';
                    tableHtml += '<td>' + match.date + '</td>';
                    tableHtml += '<td>' + match.time + '</td>';
                    tableHtml += '<td>' + match.round.name + '</td>';

                    tableHtml += '<td>';
                    if (match.team1) {
                        tableHtml += '<div>';
                        tableHtml += `<span><img src="${match.team1.image}" style="max-width: 50px; max-height: 50px;" alter="img" title="img"></span>`;
                        tableHtml += `<span class="m-2">${match.team1.name}</span>`;
                        tableHtml += '</div>';
                    }
                    tableHtml += '</td>';

                    tableHtml += '<td>';
                    if (match.team2) {
                        tableHtml += '<div>';
                        tableHtml += `<span><img src="${match.team2.image}" style="max-width: 50px; max-height: 50px;" alter="img" title="img"></span>`;
                        tableHtml += `<span class="m-2">${match.team2.name}</span>`;
                        tableHtml += '</div>';
                    }
                    else {
                        tableHtml += '<div class="text-center>?</div>';
                    }
                    tableHtml += '</td>';
                    tableHtml += '<td>' + getStatusLabel(match.betStatus) + '</td>';

                    let editBtnHtml = `<button type="button" class="btn btn-primary m-1" onClick="showEditModal('${match.id}')"><i class="bi bi-pencil-fill"></i> Edit</button>`;
                    let deleteBtnHtml = `<button type="button" class="btn btn-danger" onClick="showDeleteModal('${match.id}')"><i class="bi bi-trash-fill"></i> Delete</button>`;

                    tableHtml += '<td>' + editBtnHtml + deleteBtnHtml + '</td>';
                    tableHtml += '</tr>';
                });
            }
            else {
                tableHtml = '<tr><td colspan="8" class="text-center">No data</td></tr>';
            }

            $('#match-table-data').html(tableHtml);
        },
        error: function (error) {
            toastr.error('Error', error)
        }
    });
}

function getStatusLabel(status) {
    let label;
    switch (status) {
        case 0:
            label = '<span class="badge bg-danger">Not allowed to bet</span>';
            break;
        case 1:
            label = '<span class="badge bg-success">Opening</span>';
            break;
        case 2:
            label = '<span class="badge bg-secondary">Closed</span>';
            break;
    }

    return label;
}

function showCreateModal() {
    $('#create-match-modal').modal('show');
}

function submitCreateMatch() {
    var data = {
        IndexOrder: $('#index-order').val(),
        Date: $('#date').val(),
        Time: $('#time').val(),
        RoundId: $('#round').val(),
        Team1Id: $('#team1').val(),
        Team2Id: $('#team2').val(),
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
            $('#index-order-edit').val(matchInfo.indexOrder);
            //$('#date-edit').val(matchInfo.date);
            //$('#time-edit').val(matchInfo.time);
            $('#round-edit').val(matchInfo.roundId);

            //$('.date-edit').datepicker({
            //    uiLibrary: 'bootstrap5',
            //    format: 'dd/mm/yyyy'
            //});
            //initDateTimePicker();
            if (matchInfo.team1Id != null) {
                $('#team1-edit').val(matchInfo.team1Id).trigger('change');
            }
            if (matchInfo.team2Id != null) {
                $('#team2-edit').val(matchInfo.team2Id).trigger('change');
            }

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
        IndexOrder: $('#index-order-edit').val(),
        Date: $('#date-edit').val(),
        Time: $('#time-edit').val(),
        RoundId: $('#round-edit').val(),
        Team1Id: $('#team1-edit').val(),
        Team2Id: $('#team2-edit').val(),
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
    $('#index-order').val('');
    $('#round').val('');
    $('#team1').val(null).trigger('change');
    $('#team2').val(null).trigger('change');
}