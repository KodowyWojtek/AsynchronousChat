"use strict"
var connection = new signalR.HubConnectionBuilder().withUrl("/UserChat").build();

$("#btnSend").attr("disabled", true);

connection.on("ReceiveMessage", function (userFrom, message, userTo) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt").replace(/>/g, "&gt;");
    var encodeMsg = userFrom + ": " + msg;
    $("#messageList").append(encodeMsg +"<br>");
});

connection.start().then(function () {
    $("#btnSend").attr("disabled", false);
}).catch(function (err) {
    return alert(err.toString());
});

$("#btnSend").on("click", function () {
    var userTo = $("#txtUserTo").val();
    var userFrom = $("#txtUserName").val();
    var message = $("#txtMessage").val();
    if (message == "") {
        message = " ";
    }

    if (userFrom != "" && message != "" && userTo != "") {
        connection.invoke("SendMessage", userFrom, message, userTo).catch(function (err) {
            return alert(err.toString())
        });
        event.preventDefault();
    }
    $('#txtMessage').val('');
});

