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

            //if (matchInfo.upperDoorTeamId) {
            //    if (matchInfo.upperDoorTeamId == matchInfo.team1Id) {
            //        $('#upper-door-team1').prop('checked', true);
            //        $('#upper-door-team2').prop('checked', false);
            //    }
            //    else {
            //        $('#upper-door-team1').prop('checked', false);
            //        $('#upper-door-team2').prop('checked', true);
            //    }
            //}
            //else {
            //    $('#upper-door-team1').prop('checked', false);
            //    $('#upper-door-team2').prop('checked', false);
            //}
            //setOddsTeamLabel();

            $('#bet-match-modal').modal('show');
        },
        error: function (error) {
            $('#match-id').val('');
            toastr.error(error, 'Error')
        }
    });
}