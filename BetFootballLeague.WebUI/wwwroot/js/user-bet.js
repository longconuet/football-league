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

                    divHtml += `<div class="text-right"><button type="button" class="btn btn-success">Bet</div>`;
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