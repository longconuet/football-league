$(document).ready(function () {
    getMatchList();

    initDateTimePicker();

    setTeamDataForSelect2();
    setRoundDataForSelect();
});

function initDateTimePicker(mode = '') {
    $('#date' + mode).datepicker({
        uiLibrary: 'bootstrap5',
        format: 'dd/mm/yyyy'
    });

    $('#time' + mode).timepicker({
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
            toastr.error(error, 'Error')
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
            toastr.error(error, 'Error')
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

                    // team 1
                    tableHtml += '<td>';
                    if (match.team1) {
                        tableHtml += '<div>';
                        tableHtml += `<span><img src="${match.team1.image}" style="max-width: 50px; max-height: 50px;" alter="img" title="${match.team1.name}"></span>`;
                        tableHtml += `<span class="m-2">${match.team1.name}</span>`;
                        if (match.upperDoorTeamId && match.upperDoorTeamId == match.team1Id) {
                            tableHtml += `<span class="text-success"><i class="bi bi-caret-up-square-fill"></i> ${match.odds}</span>`;
                        }
                        else if (match.upperDoorTeamId && match.upperDoorTeamId != match.team1Id) {
                            tableHtml += `<span class="text-danger"><i class="bi bi-caret-down-square-fill"></i> ${match.odds}</span>`;
                        }
                        if (match.team1Score != null) {
                            tableHtml += `<span class="badge bg-success mt-2 me-2"><h3>${match.team1Score}</h3></span>`;
                        }
                        tableHtml += '</div>';

                        if (match.winBetTeamId && match.winBetTeamId == match.team1Id) {
                            tableHtml += `<span class="badge rounded-pill bg-success mt-2 me-2">Win Bet</span>`;
                        }
                    }
                    else {
                        tableHtml += '<div class="text-center">?</div>';
                    }
                    tableHtml += '</td>';

                    // team 2
                    tableHtml += '<td>';
                    if (match.team2) {
                        tableHtml += '<div>';
                        tableHtml += `<span><img src="${match.team2.image}" style="max-width: 50px; max-height: 50px;" alter="img" title="${match.team2.name}"></span>`;
                        tableHtml += `<span class="m-2">${match.team2.name}</span>`;
                        if (match.upperDoorTeamId && match.upperDoorTeamId == match.team2Id) {
                            tableHtml += `<span class="text-success"><i class="bi bi-caret-up-square-fill"></i> ${match.odds}</span>`;
                        }
                        else if (match.upperDoorTeamId && match.upperDoorTeamId != match.team2Id) {
                            tableHtml += `<span class="text-danger"><i class="bi bi-caret-down-square-fill"></i> ${match.odds}</span>`;
                        }
                        if (match.team2Score != null) {
                            tableHtml += `<span class="badge bg-success mt-2 me-2"><h3>${match.team2Score}</h3></span>`;
                        }
                        tableHtml += '</div>';

                        if (match.winBetTeamId && match.winBetTeamId == match.team2Id) {
                            tableHtml += `<span class="badge rounded-pill bg-success mt-2 me-2">Win Bet</span>`;
                        }
                    }
                    else {
                        tableHtml += '<div class="text-center">?</div>';
                    }
                    tableHtml += '</td>';
                    tableHtml += '<td>' + getStatusLabel(match.betStatus) + '</td>';

                    // score
                    tableHtml += '<td class="text-center">';
                    if (match.team1Score != null && match.team2Score != null) {
                        tableHtml += `<span class="m-2">${match.team1Score} - ${match.team2Score}</span>`;
                    }
                    tableHtml += '</td>';

                    // odds button
                    let setOddsBtnHtml = '';
                    if (match.team1 && match.team2 && match.betStatus == 0) {
                        setOddsBtnHtml = `<button type="button" class="btn btn-info m-1" onClick="showSetOddsModal('${match.id}')" title="Set odds"><i class="bi bi-arrow-bar-up"></i></button>`;
                    }

                    // score button
                    let updateScoreBtnHtml = '';
                    if (match.betStatus == 1) {
                        updateScoreBtnHtml = `<button type="button" class="btn btn-success m-1" onClick="showUpdateScoreModal('${match.id}')" title="Update score"><i class="bi bi-caret-right-square"></i></button>`;
                    }

                    let updateStatusBtnHtml = `<button type="button" class="btn btn-primary m-1" onClick="showUpdateStatusModal('${match.id}')" title="Update status"><i class="bi bi-arrow-left-right"></i></button>`;
                    let editBtnHtml = `<button type="button" class="btn btn-primary m-1" onClick="showEditModal('${match.id}')" title="Edit"><i class="bi bi-pencil-fill"></i></button>`;
                    let deleteBtnHtml = `<button type="button" class="btn btn-danger" onClick="showDeleteModal('${match.id}')" title="Delete"><i class="bi bi-trash-fill"></i></button>`;

                    tableHtml += '<td>' + setOddsBtnHtml + updateScoreBtnHtml + updateStatusBtnHtml + editBtnHtml + deleteBtnHtml + '</td>';
                    tableHtml += '</tr>';
                });
            }
            else {
                tableHtml = '<tr><td colspan="8" class="text-center">No data</td></tr>';
            }

            $('#match-table-data').html(tableHtml);
        },
        error: function (error) {
            toastr.error(error, 'Error')
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
    $('#index-order').val($('#match-table-data tr').length + 1);
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
            toastr.error(error, 'Error')
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

            initDateTimePicker('-edit');

            let matchInfo = response.data;
            $('#update-match-id').val(id);
            $('#index-order-edit').val(matchInfo.indexOrder);
            $('#date-edit').val(matchInfo.date);
            $('#time-edit').val(matchInfo.time);
            $('#round-edit').val(matchInfo.roundId);

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
            toastr.error(error, 'Error')
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
            toastr.error(error, 'Error')
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
            toastr.error(error, 'Error')
        }
    });
}

function showSetOddsModal(id) {
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
            $('#set-odds-match-id').val(id);
            $('#odds').val(matchInfo.odds);
            $('#upper-door-team1').val(matchInfo.team1Id);
            $('#upper-door-team2').val(matchInfo.team2Id);
            $('#odds-team1-label').html(`<img src="${matchInfo.team1.image}" style="max-width: 50px; max-height: 50px;" alter="img" title="img"><span>${matchInfo.team1.name}</span>`);
            $('#odds-team2-label').html(`<img src="${matchInfo.team2.image}" style="max-width: 50px; max-height: 50px;" alter="img" title="img"><span>${matchInfo.team2.name}</span>`);

            if (matchInfo.upperDoorTeamId) {
                if (matchInfo.upperDoorTeamId == matchInfo.team1Id) {
                    $('#upper-door-team1').prop('checked', true);
                    $('#upper-door-team2').prop('checked', false);
                }
                else { 
                    $('#upper-door-team1').prop('checked', false);
                    $('#upper-door-team2').prop('checked', true);
                }
            }
            else {
                $('#upper-door-team1').prop('checked', false);
                $('#upper-door-team2').prop('checked', false);
            }
            setOddsTeamLabel();

            $('#set-odds-match-modal').modal('show');
        },
        error: function (error) {
            $('#set-odds-match-id').val('');
            toastr.error(error, 'Error')
        }
    });
}

$('#set-odds-match-modal input[name="upperDoorTeam"]').on('click', function () {
    setOddsTeamLabel();
});

function setOddsTeamLabel() {
    $('#set-odds-match-modal input[name="upperDoorTeam"]').each(function () {
        if ($(this).prop('checked') == false) {
            $(this).parent().removeClass('checked-odds-team');
            $(this).parent().addClass('unchecked-odds-team');
        }
        else {
            $(this).parent().removeClass('unchecked-odds-team');
            $(this).parent().addClass('checked-odds-team');
        }
    });
}

function submitSetOddsMatch() {
    var checkedTeamId = $('#set-odds-match-modal input[name="upperDoorTeam"]:checked').val();
    if (!checkedTeamId) {
        toastr.error('Please select a team', 'Error');
        return;
    }

    var data = {
        Id: $('#set-odds-match-id').val(),
        Odds: $('#odds').val(),
        UpperDoorTeamId: checkedTeamId
    };

    $.ajax({
        url: '/Match/SetOddsMatchAjax',
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
            $('#set-odds-match-modal').modal('hide');
        },
        error: function (error) {
            toastr.error(error, 'Error')
        }
    });
}

function showUpdateStatusModal(id) {
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
            let currentStatus = matchInfo.betStatus;
            $('#update-status-match-id').val(id);

            $('#update-status-match-modal input[name="statusUpdate"]').prop('disabled', false).prop('checked', false);
            $('#status-update-' + currentStatus).prop('disabled', true).prop('checked', true);

            if (currentStatus == 0 && (matchInfo.team1Id == null || matchInfo.team2Id == null || matchInfo.odds == null)) {
                $('#status-update-1').prop('disabled', true);
                $('#status-update-2').prop('disabled', true);
            }

            if (currentStatus == 1 && matchInfo.team1Score == null) {
                $('#status-update-0').prop('disabled', true);
                $('#status-update-2').prop('disabled', true);
            }

            if (currentStatus == 2) {
                $('#status-update-0').prop('disabled', true);
            }

            $('#update-status-match-modal').modal('show');
        },
        error: function (error) {
            $('#update-status-match-id').val('');
            toastr.error(error, 'Error')
        }
    });
}

function submitUpdateStatusMatch() {
    var checkedStatusItem = $('#update-status-match-modal input[name="statusUpdate"]:checked');
    if (!checkedStatusItem.val() || checkedStatusItem.prop('disabled') == true) {
        toastr.error('Please select a status', 'Error');
        return;
    }

    var data = {
        Id: $('#update-status-match-id').val(),
        BetStatus: parseInt(checkedStatusItem.val())
    };

    $.ajax({
        url: '/Match/UpdateMatchStatusAjax',
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
            $('#update-status-match-modal').modal('hide');
        },
        error: function (error) {
            toastr.error(error, 'Error')
        }
    });
}

function showUpdateScoreModal(id) {
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
            $('#update-score-match-id').val(id);

            $('#score-team1-name').html(matchInfo.team1.name);
            $('#score-team1-img').attr('src', matchInfo.team1.image);
            $('#score-team2-name').html(matchInfo.team2.name);
            $('#score-team2-img').attr('src', matchInfo.team2.image);

            if (matchInfo.team1Score != null) {
                $('#score-team1').val(matchInfo.team1Score);
            }
            if (matchInfo.team2Score != null) {
                $('#score-team2').val(matchInfo.team2Score);
            }

            $('#update-score-match-modal').modal('show');
        },
        error: function (error) {
            $('#update-score-match-id').val('');
            toastr.error(error, 'Error')
        }
    });
}

function submitUpdateScoreMatch() {
    var data = {
        Id: $('#update-score-match-id').val(),
        Team1Score: parseInt($('#score-team1').val()),
        Team2Score: parseInt($('#score-team2').val())
    };

    $.ajax({
        url: '/Match/UpdateMatchScoreAjax',
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
            $('#update-score-match-modal').modal('hide');
        },
        error: function (error) {
            toastr.error(error, 'Error')
        }
    });
}

function clearCreateModalInput() {
    $('#index-order').val('');
    $('#round').val('');
    $('#date').val('');
    $('#time').val('');
    $('#team1').val(null).trigger('change');
    $('#team2').val(null).trigger('change');
}