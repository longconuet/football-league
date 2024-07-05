$(document).ready(function () {
    getUserBetList();
});

var token = $('input[name="__RequestVerificationToken"]').val();

function getUserBetList() {
    $.ajax({
        type: "GET",
        url: '/UserBet/GetUserBetListAjax',
        contentType: 'application/json',
        headers: {
            'RequestVerificationToken': token
        },
        success: function (response) {
            if (response.status == 0) {
                toastr.error(response.message, 'Error');
                return;
            }

            let divHtml = '';
            let data = response.data;
            if (data.length > 0) {
                $.each(data, function (index, item) {
                    divHtml += `<div class="match-area">`;

                    divHtml += `<div><span>${item.dateTime}</span><span class="text-right">` + getMatchStatusLabel(item.betStatus) + `</span></div>`;
                    if (item.userBet) {
                        divHtml += `<div class="text-success">You have bet</div>`;
                    }
                    else {
                        divHtml += `<div class="text-danger">You have not bet</div>`;
                    }

                    if (item.team1) {
                        divHtml += `<div><span>${item.team1.name}</span>`;
                        divHtml += `<span><img src="${item.team1.image}" style="max-width: 50px; max-height: 50px;" alter="img" title="${item.team1.name}"></span>`;
                        divHtml += `<span>${item.team1Score ?? '?'}</span>`;
                    }
                    else {
                        divHtml += `<div><span>?</span>`;
                    }

                    divHtml += `<span>-</span>`;

                    if (item.team2) {
                        divHtml += `<span>${item.team2Score ?? '?'}</span>`;
                        divHtml += `<span><img src="${item.team2.image}" style="max-width: 50px; max-height: 50px;" alter="img" title="${item.team2.name}"></span>`;
                        divHtml += `<span>${item.team2.name}</span></div>`;
                    }
                    else {
                        divHtml += `<span>?</span></div>`;
                    }

                    // User bet status
                    if (item.userBet) {
                        if (item.betStatus == 2 && item.userBet.isWin) {
                            divHtml += `<div class="text-success"><strong>YOU WIN</strong></div>`;
                        }
                        else if (item.betStatus == 2 && !item.userBet.isWin) {
                            divHtml += `<div class="text-danger"><strong>YOU LOSE</strong></div>`;
                        }
                    }                    

                    if (item.betStatus == 1) {
                        divHtml += `<div class="text-right"><button type="button" class="btn btn-success" onclick="showBetModal('${item.id}')">Bet</div>`;
                    }
                    
                    divHtml += `</div>`;
                });
            }
            else {
                divHtml = '<div class="text-center">No data</div>';
            }

            $('#user-bet-data').html(divHtml);
        },
        error: function (error) {
            toastr.error(error, 'Error')
        }
    });
}

function getMatchStatusLabel(status) {
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

function showBetModal(matchId) {
    $.ajax({
        type: "GET",
        url: '/UserBet/GetMatchInfoForUserBetAjax/' + matchId,
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
            $('#match-id').val(matchId);
            $('#bet-team1').val(matchInfo.team1Id);
            $('#bet-team2').val(matchInfo.team2Id);
            $('#bet-team1-label').html(`<img src="${matchInfo.team1.image}" style="max-width: 50px; max-height: 50px;" alter="img" title="img"><span>${matchInfo.team1.name}</span>`);
            $('#bet-team2-label').html(`<img src="${matchInfo.team2.image}" style="max-width: 50px; max-height: 50px;" alter="img" title="img"><span>${matchInfo.team2.name}</span>`);

            $('#match-datetime').html(matchInfo.dateTime);
            $('#match-status').html(getMatchStatusLabel(matchInfo.betStatus));

            let team1OddsHtml = '';
            let team2OddsHtml = '';
            if (matchInfo.upperDoorTeamId == matchInfo.team1Id) {
                team1OddsHtml = `<span class="text-success"><i class="bi bi-caret-up-square-fill"></i> ${matchInfo.odds}</span>`;
                team2OddsHtml = `<span class="text-danger"><i class="bi bi-caret-down-square-fill"></i> ${matchInfo.odds}</span>`;
            }
            else {
                team1OddsHtml = `<span class="text-danger"><i class="bi bi-caret-down-square-fill"></i> ${matchInfo.odds}</span>`;
                team2OddsHtml = `<span class="text-success"><i class="bi bi-caret-up-square-fill"></i> ${matchInfo.odds}</span>`;
            }
            $('#team1-odds').html(team1OddsHtml);
            $('#team2-odds').html(team2OddsHtml);

            if (matchInfo.userBet) {
                if (matchInfo.userBet.betTeamId == matchInfo.team1Id) {
                    $('#bet-team1').prop('checked', true);
                    $('#bet-team2').prop('checked', false);
                    $('#bet-text').html(`<p class="text-succes">You bet for <strong>${matchInfo.team1.name}</strong></p>`);
                }
                else {
                    $('#bet-team1').prop('checked', false);
                    $('#bet-team2').prop('checked', true);
                    $('#bet-text').html(`<p class="text-success">You bet for <strong>${matchInfo.team2.name}</strong></p>`);
                }
                $('#bet-text').append(`<i class="text-sm">Created at: ${matchInfo.userBet.createdAtStr}</i>`)
                if (matchInfo.userBet.updatedAtStr) {
                    $('#bet-text').append(`<br/><i class="text-sm">Last updated at: ${matchInfo.userBet.updatedAtStr}</i>`)
                }                
            }
            else {
                $('#bet-text').html(`<p class="text-danger">You have not bet!</p>`);
            }
            setBetTeamLabel();

            $('#bet-match-modal').modal('show');
        },
        error: function (error) {
            $('#match-id').val('');
            toastr.error(error, 'Error')
        }
    });
}

$('#bet-match-modal input[name="betTeam"]').on('click', function () {
    setBetTeamLabel();
});

function setBetTeamLabel() {
    $('#bet-match-modal input[name="betTeam"]').each(function () {
        if ($(this).prop('checked') == false) {
            $(this).parent().removeClass('checked-bet-team');
            $(this).parent().addClass('unchecked-bet-team');
        }
        else {
            $(this).parent().removeClass('unchecked-bet-team');
            $(this).parent().addClass('checked-bet-team');
        }
    });
}

function submitBetMatch() {
    var checkedTeamId = $('#bet-match-modal input[name="betTeam"]:checked').val();
    if (!checkedTeamId) {
        toastr.error('Please select a team', 'Error');
        return;
    }

    var data = {
        MatchId: $('#match-id').val(),
        BetTeamId: checkedTeamId
    };

    $.ajax({
        url: '/UserBet/BetMatchAjax',
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
            getUserBetList();
            $('#bet-match-modal').modal('hide');
        },
        error: function (error) {
            toastr.error(error, 'Error')
        }
    });
}