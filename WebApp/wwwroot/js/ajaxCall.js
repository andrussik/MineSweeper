function saveGame() {
    const data = {};
    data.BoardName = document.getElementById("saveGameName").value;
    data.JsonString = JSON.stringify(GameBoard);
    
    $.ajax({
        type: "POST",
        url: "?handler=Save",
        data: JSON.stringify(data),
        contentType: "application/json",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        success: function(response) {
            alert("Saved game " + data.BoardName);
            window.location = "Game?handler=Game&id=" + response;
        },
        error: function (req, status, error) {
            alert(error)
        }
    });
}

function deleteGame() {
    const id = $("select[name = 'boardId']").val();

    $.ajax({
        type: "POST",
        url: "?handler=Delete",
        data: JSON.stringify(id),
        contentType: "application/json",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        success: function(response) {
            alert("Deleted game " + response);
            document.getElementsByName(id)[0].remove();
        },
        error: function (req, status, error) {
            alert(error)
        }
    });
}