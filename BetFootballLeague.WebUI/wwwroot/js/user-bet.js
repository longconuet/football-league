var betChart = null;

$(document).ready(function () {
    getUserBetList();

    let chartData = {
        labels: [
            'Team 1',
            'Team 2',
            'Not bet'
        ],
        datasets: [{
            data: [4, 1, 3],
            backgroundColor: [
                'rgb(255, 99, 132)',
                'rgb(54, 162, 235)',
                'rgb(255, 205, 86)'
            ],
            hoverOffset: 4
        }]
    };

    var options = {
        plugins: {
            tooltip: {
                callbacks: {
                    label: function (context) {
                        let data = context.dataset.data;
                        const sum = data.reduce((partialSum, a) => partialSum + a, 0);
                        if (sum == 0) {
                            return '0%';
                        }

                        const userNum = context.formattedValue;
                        let percentage = (userNum * 100 / sum).toFixed(2) + "%";
                        return `${userNum} members - ${percentage}`;
                    }
                }
            }
        }
    };


    betChart = new Chart($('#chart-statistic'), {
        type: 'pie',
        data: chartData,
        options: options
    });
});

var token = $('input[name="__RequestVerificationToken"]').val();

function getUserBetList() {
    let matchStatus = $('#match-status-filter').val();

    $.ajax({
        type: "GET",
        url: '/UserBet/GetUserBetListAjax',
        contentType: 'application/json',
        headers: {
            'RequestVerificationToken': token
        },
        data: {
            Status: matchStatus ? parseInt(matchStatus) : null
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

                    // header
                    divHtml += `<div class="col-12 mb-3">`;
                    divHtml += `<div class="row">`;
                    divHtml += `<div class="col-4">`;
                    divHtml += `<span class="badge bg-light text-dark me-2 float-start">${item.round.name}</span>`;
                    divHtml += `<span class="me-2 float-start">Match ${item.indexOrder}</span>`;
                    divHtml += `</div>`;
                    divHtml += `<div class="col-4"><h6>${item.dateTime}</h6></div>`;
                    divHtml += `<div class="col-4"><span class="float-end">` + getMatchStatusLabel(item.betStatus) + `</span></div>`;
                    divHtml += `</div>`;
                    divHtml += `</div>`;                    

                    // team 1
                    divHtml += `<div class="col-12">`;
                    divHtml += `<div class="row">`;
                    divHtml += `<div class="col-6 d-flex justify-content-end align-items-center">`;
                    if (item.team1) {
                        let teamUserBetClass = '';
                        let teamUserBetBadge = '';
                        let teamUserBetBadgeBg = '';
                        if (item.userBet && item.userBet.betTeamId && item.userBet.betTeamId == item.team1Id) {
                            if (item.userBet.isWin != null) {
                                if (item.userBet.isWin == true) {
                                    teamUserBetClass = 'user-win-bet';
                                    teamUserBetBadge = 'You Bet Win';
                                    teamUserBetBadgeBg = 'bg-success';
                                }
                                else {
                                    teamUserBetClass = 'user-lose-bet';
                                    teamUserBetBadge = 'You Bet Lose';
                                    teamUserBetBadgeBg = 'bg-danger';
                                }
                            }
                            else {
                                teamUserBetClass = 'user-team-bet';
                                teamUserBetBadge = 'You Bet';
                                teamUserBetBadgeBg = 'bg-info';
                            }
                        }

                        divHtml += `<div class="team-badge position-relative ${teamUserBetClass}">`;

                        if (item.winBetTeamId) {
                            if (item.winBetTeamId == item.team1Id) {
                                divHtml += `<span class="badge rounded-pill bg-success ms-2">Win Bet</span>`;
                            }
                            else {
                                divHtml += `<span class="badge rounded-pill bg-danger ms-2">Lose Bet</span>`;
                            }
                        }
                        if (item.upperDoorTeamId && item.upperDoorTeamId == item.team1Id) {
                            divHtml += `<span class="text-success ms-2"><i class="bi bi-caret-up-square-fill"></i> ${item.odds}</span>`;
                        }
                        else if (item.upperDoorTeamId && item.upperDoorTeamId != item.team1Id) {
                            divHtml += `<span class="text-danger ms-2"><i class="bi bi-caret-down-square-fill"></i> ${item.odds}</span>`;
                        }
                        divHtml += `<span class="team-name ms-4">${item.team1.name}</span>`;
                        divHtml += `<span class="ms-2"><img src="${item.team1.image}" style="max-width: 80px; max-height: 80px;" alter="img" title="${item.team1.name}"></span>`;
                        divHtml += item.team1Score != null ? `<span class="score-item">${item.team1Score}</span>` : `<span class="no-score-item">?</span>`;

                        if (teamUserBetClass != '') {
                            divHtml += `<div class="position-absolute top-0 start-0 translate-middle"><span class="badge rounded-pill ${teamUserBetBadgeBg} me-2">${teamUserBetBadge}</span></div>`;
                        }

                        divHtml += `</div>`;
                    }
                    else {
                        divHtml += `<span class="no-score-item">?</span>`;
                    }
                    
                    divHtml += `</div>`;

                    // team 2
                    divHtml += `<div class="col-6 d-flex justify-content-start align-items-center">`;
                    if (item.team2) {
                        let teamUserBetClass = '';
                        let teamUserBetBadge = '';
                        let teamUserBetBadgeBg = '';
                        if (item.userBet && item.userBet.betTeamId && item.userBet.betTeamId == item.team2Id) {
                            if (item.userBet.isWin != null) {
                                if (item.userBet.isWin == true) {
                                    teamUserBetClass = 'user-win-bet';
                                    teamUserBetBadge = 'You Bet Win';
                                    teamUserBetBadgeBg = 'bg-success';
                                }
                                else {
                                    teamUserBetClass = 'user-lose-bet';
                                    teamUserBetBadge = 'You Bet Lose';
                                    teamUserBetBadgeBg = 'bg-danger';
                                }
                            }
                            else {
                                teamUserBetClass = 'user-team-bet';
                                teamUserBetBadge = 'You Bet';
                                teamUserBetBadgeBg = 'bg-info';
                            }
                        }

                        divHtml += `<div class="team-badge position-relative ${teamUserBetClass}">`;

                        divHtml += item.team2Score != null ? `<span class="score-item">${item.team2Score}</span>` : `<span class="no-score-item">?</span>`;
                        divHtml += `<span class="me-2"><img src="${item.team2.image}" style="max-width: 80px; max-height: 80px;" alter="img" title="${item.team2.name}"></span>`;
                        divHtml += `<span class="team-name me-4">${item.team2.name}</span>`;
                        if (item.upperDoorTeamId && item.upperDoorTeamId == item.team2Id) {
                            divHtml += `<span class="text-success me-2"><i class="bi bi-caret-up-square-fill"></i> ${item.odds}</span>`;
                        }
                        else if (item.upperDoorTeamId && item.upperDoorTeamId != item.team2Id) {
                            divHtml += `<span class="text-danger me-2"><i class="bi bi-caret-down-square-fill"></i> ${item.odds}</span>`;
                        }
                        if (item.winBetTeamId) {
                            if (item.winBetTeamId == item.team2Id) {
                                divHtml += `<span class="badge rounded-pill bg-success me-2">Win Bet</span>`;
                            }
                            else {
                                divHtml += `<span class="badge rounded-pill bg-danger me-2">Lose Bet</span>`;
                            }
                        }

                        if (teamUserBetClass != '') {
                            divHtml += `<div class="position-absolute top-0 start-100 translate-middle"><span class="badge rounded-pill ${teamUserBetBadgeBg} me-2">${teamUserBetBadge}</span></div>`;
                        }

                        divHtml += `</div>`;
                    }
                    else {
                        divHtml += `<span class="no-score-item">?</span>`;
                    }
                    divHtml += `</div>`;
                    divHtml += `</div>`;
                    divHtml += `</div>`;

                    // User bet status
                    if (item.userBet) {
                        if (item.betStatus == 2 && item.userBet.isWin) {
                            divHtml += `<div class="text-success mt-3"><strong>YOU WIN <i class="bi bi-emoji-smile-fill"></i></strong></div>`;
                        }
                        else if (item.betStatus == 2 && !item.userBet.isWin) {
                            divHtml += `<div class="text-danger mt-3"><strong>YOU LOSE <i class="bi bi-emoji-frown-fill"></i></strong></div>`;
                        }
                    }                    

                    // footer button                    
                    divHtml += `<div class="col-12 mt-3">`;
                    divHtml += `<div class="row">`;
                    divHtml += `<div class="col-6 d-flex justify-content-start">`;
                    if (item.userBet) {
                        divHtml += `<span class="text-success mt-3">You have bet</span>`;
                    }
                    else {
                        divHtml += `<span class="text-danger mt-3">You have not bet</span>`;
                    }
                    divHtml += `</div>`;
                    divHtml += `<div class="col-6 d-flex justify-content-end">`;
                    if (item.betStatus == 1) { // opening
                        divHtml += `<button type="button" class="btn btn-success ms-2" onclick="showBetModal('${item.id}')">Bet</button>`;
                    }
                    if (item.betStatus == 2 && item.userBet) { // ended
                        divHtml += `<button type="button" class="btn btn-info ms-2" onclick="showBetDetailModal('${item.userBet.id}')">View Bet Detail</button>`;
                    }
                    if (item.betStatus != 0) {
                        divHtml += `<button type="button" class="btn btn-primary ms-2" onclick="showBetStatisticModal('${item.id}')">View Statistic</button>`;
                    }
                    divHtml += `</div>`;
                    divHtml += `</div>`;
                    divHtml += `</div>`;
                    
                    divHtml += `</div>`;
                });
            }
            else {
                divHtml = '<div class="text-center match-area">No match</div>';
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
    clearBetTeamLabel();

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
                    $('#bet-text').html(`<p class="text-info">You bet for <strong>${matchInfo.team1.name}</strong></p>`);
                }
                else {
                    $('#bet-team1').prop('checked', false);
                    $('#bet-team2').prop('checked', true);
                    $('#bet-text').html(`<p class="text-info">You bet for <strong>${matchInfo.team2.name}</strong></p>`);
                }
                $('#bet-text').append(`<i class="text-sm">Created at: ${matchInfo.userBet.createdAtStr}</i>`)
                if (matchInfo.userBet.updatedAtStr) {
                    $('#bet-text').append(`<br/><i class="text-sm">Last updated at: ${matchInfo.userBet.updatedAtStr}</i>`)
                }

                setBetTeamLabel();
            }
            else {
                $('#bet-text').html(`<p class="text-danger">You have not bet!</p>`);
            }            

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

function clearBetTeamLabel() {
    $('.bet-team').removeClass('checked-bet-team');
    $('.bet-team').addClass('unchecked-bet-team');
    $('.bet-team input').prop('checked', false);
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

function showBetDetailModal(userBetId) {
    $.ajax({
        type: "GET",
        url: '/UserBet/GetUserBetInfoAjax/' + userBetId,
        contentType: 'application/json',
        headers: {
            'RequestVerificationToken': token
        },
        success: function (response) {
            if (response.status == 0) {
                toastr.error(response.message, 'Error');
                return;
            }

            let userBet = response.data;
            let matchInfo = response.data.match;
            $('#bet-team1-detail-label').html(`<img src="${matchInfo.team1.image}" style="max-width: 50px; max-height: 50px;" alter="img" title="img"><span>${matchInfo.team1.name}</span>`);
            $('#bet-team2-detail-label').html(`<img src="${matchInfo.team2.image}" style="max-width: 50px; max-height: 50px;" alter="img" title="img"><span>${matchInfo.team2.name}</span>`);

            $('#match-datetime-detail').html(matchInfo.dateTime);
            $('#match-status-detail').html(getMatchStatusLabel(matchInfo.betStatus));

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
            $('#team1-odds-detail').html(team1OddsHtml);
            $('#team2-odds-detail').html(team2OddsHtml);

            if (userBet.betTeamId == matchInfo.team1Id) {
                $('#bet-team1-detail').prop('checked', true);
                $('#bet-team2-detail').prop('checked', false);
                $('#bet-text-detail').html(`<p class="text-succes">You bet for <strong>${matchInfo.team1.name}</strong></p>`);
            }
            else {
                $('#bet-team1-detail').prop('checked', false);
                $('#bet-team2-detail').prop('checked', true);
                $('#bet-text-detail').html(`<p class="text-info">You bet for <strong>${matchInfo.team2.name}</strong></p>`);
            }
            $('#bet-text-detail').append(`<i class="text-sm">Created at: ${userBet.createdAtStr}</i>`)
            if (userBet.updatedAtStr) {
                $('#bet-text-detail').append(`<br/><i class="text-sm">Last updated at: ${userBet.updatedAtStr}</i>`)
            }
            setBetTeamLabelForDetail(userBet.isWin);

            $('#team1-score-detail').html(`${matchInfo.team1Score}`);
            $('#team2-score-detail').html(`${matchInfo.team2Score}`);

            if (matchInfo.winBetTeamId) {
                if (matchInfo.winBetTeamId == matchInfo.team1Id) {
                    $('#team1-bet-status-detail').html(`<span class="badge rounded-pill bg-success mt-2 me-2">Win Bet</span>`);
                    $('#team2-bet-status-detail').html(`<span class="badge rounded-pill bg-danger mt-2 me-2">Lose Bet</span>`);
                }
                else {
                    $('#team2-bet-status-detail').html(`<span class="badge rounded-pill bg-success mt-2 me-2">Win Bet</span>`);
                    $('#team1-bet-status-detail').html(`<span class="badge rounded-pill bg-danger mt-2 me-2">Lose Bet</span>`);
                }
            }

            if (userBet.isWin) {
                $('#bet-result-detail').html(`<strong class="text-success">YOU WIN <i class="bi bi-emoji-smile-fill"></i></strong>`);
            }
            else {
                $('#bet-result-detail').html(`<strong class="text-danger">YOU LOSE <i class="bi bi-emoji-frown-fill"></i></strong>`);
            }

            $('#bet-match-detail-modal').modal('show');
        },
        error: function (error) {
            toastr.error(error, 'Error')
        }
    });
}

function setBetTeamLabelForDetail(isWin) {
    $('#bet-match-detail-modal input[name="betTeamDetail"]').each(function () {
        if ($(this).prop('checked') == false) {
            $(this).parent().removeClass('win-bet-team');
            $(this).parent().removeClass('lose-bet-team');
            $(this).parent().addClass('unchecked-bet-team');
        }
        else {
            $(this).parent().removeClass('unchecked-bet-team');
            if (isWin) {
                $(this).parent().addClass('win-bet-team');
                $(this).parent().removeClass('lose-bet-team');
            }
            else {
                $(this).parent().addClass('lose-bet-team');
                $(this).parent().removeClass('win-bet-team');
            }            
        }
    });
}

function showBetStatisticModal(matchId) {
    $.ajax({
        type: "GET",
        url: '/UserBet/GetBetStatisticAjax/' + matchId,
        contentType: 'application/json',
        headers: {
            'RequestVerificationToken': token
        },
        success: function (response) {
            if (response.status == 0) {
                toastr.error(response.message, 'Error');
                return;
            }

            let matchInfo = response.data.match;
            let team1 = response.data.team1;
            let team2 = response.data.team2;
            let notBetUsers = response.data.notBetUsers;
            let chartInfo = response.data.chartInfo;

            // match
            $('#match-round-statistic').html(`<span class="badge bg-light text-dark mt-2 me-2">${matchInfo.roundName}</span>`);
            $('#match-datetime-statistic').html(`${matchInfo.dateTime}`);
            $('#match-status-statistic').html(`<span class="text-right">` + getMatchStatusLabel(matchInfo.betStatus) + `</span>`);

            // team 1
            $('#bet-team1-img-statistic').attr('src', team1.image);
            $('#bet-team1-name-statistic').html(team1.name);
            if (team1.isUpperDoor) {
                $('#bet-team1-odds-statistic').html(`<span class="text-success"><i class="bi bi-caret-up-square-fill"></i> ${matchInfo.odds}</span>`);
            }
            else {
                $('#bet-team1-odds-statistic').html(`<span class="text-danger"><i class="bi bi-caret-down-square-fill"></i> ${matchInfo.odds}</span>`);
            }
            if (team1.score != null) {
                $('#bet-team1-score-statistic').html(team1.score);
            }

            let team1UsersHtml = `<span><i class="bi bi-person-fill"></i> ${team1.betUsers.length}</span>`;
            if (team1.betUsers.length > 0) {                
                $.each(team1.betUsers, function (index, item) {
                    team1UsersHtml += `<div class="my-2">`
                    team1UsersHtml += `<img src="images/avatars/euro-logo.png" class="avatar" alter="avatar">`
                    team1UsersHtml += `<span class="ms-2">${item.fullName}</span>`
                    team1UsersHtml += `<span class="sm-text float-end mt-3"><i class="bi bi-clock"></i> ${item.betTime}</span>`
                    team1UsersHtml += `</div>`
                });
            }
            else {
                team1UsersHtml += `<p>No users bet</p>`;
            }
            $('#bet-team1-users-statistic').html(team1UsersHtml);

            // team 2
            $('#bet-team2-img-statistic').attr('src', team2.image);
            $('#bet-team2-name-statistic').html(team2.name);
            if (team2.isUpperDoor) {
                $('#bet-team2-odds-statistic').html(`<span class="text-success"><i class="bi bi-caret-up-square-fill"></i> ${matchInfo.odds}</span>`);
            }
            else {
                $('#bet-team2-odds-statistic').html(`<span class="text-danger"><i class="bi bi-caret-down-square-fill"></i> ${matchInfo.odds}</span>`);
            }
            if (team2.score != null) {
                $('#bet-team2-score-statistic').html(team2.score);
            }

            let team2UsersHtml = `<span><i class="bi bi-person-fill"></i> ${team2.betUsers.length}</span>`;
            if (team2.betUsers.length > 0) {
                $.each(team2.betUsers, function (index, item) {
                    team2UsersHtml += `<div class="my-2">`
                    team2UsersHtml += `<img src="images/avatars/euro-logo.png" class="avatar" alter="avatar">`
                    team2UsersHtml += `<span class="ms-2">${item.fullName}</span>`
                    team2UsersHtml += `<span class="sm-text float-end mt-3"><i class="bi bi-clock"></i> ${item.betTime}</span>`
                    team2UsersHtml += `</div>`
                });
            }
            else {
                team2UsersHtml += `<p>No users bet</p>`;
            }
            $('#bet-team2-users-statistic').html(team2UsersHtml);

            // not bet users
            let notBetUsersHtml = `<div class="mb-3"><strong>Not bet user <i class="bi bi-person-fill ms-3"></i> ${notBetUsers.length}</strong></div>`;
            if (notBetUsers.length > 0) {
                $.each(notBetUsers, function (index, item) {
                    notBetUsersHtml += `<div class="my-2"><img src="images/avatars/euro-logo.png" class="avatar" alter="avatar"><span class="ms-2">${item.fullName}</span></div>`
                });
            }
            $('#not-bet-users-statistic').html(notBetUsersHtml);

            // update chart data
            let newChartLabel = [
                chartInfo.team1Name,
                chartInfo.team2Name,
                'Not bet'
            ]
            let newChartData = [
                chartInfo.betTeam1Number,
                chartInfo.betTeam2Number,
                chartInfo.notBetNumber
            ]
            updateDataChart(betChart, newChartLabel, newChartData);

            $('#bet-match-statistic-modal').modal('show');
        },
        error: function (error) {
            toastr.error(error, 'Error')
        }
    });
}

function updateDataChart(chart, newLabels, newData) {
    chart.data.labels = newLabels;
    chart.data.datasets[0].data = newData;
    chart.update();
}