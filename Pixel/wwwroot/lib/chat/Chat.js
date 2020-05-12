"use strict"
var connection = new signalR.HubConnectionBuilder().withUrl("/UserChat").build();

$("#btnSend").attr("disabled", true);

connection.on("ReceiveMessage", function (user, message, userTo) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt").replace(/>/g, "&gt;");
    var encodeMsg = user + ": " + msg;
    $("#messageList").append(encodeMsg +"<br>");
});

connection.start().then(function () {
    $("#btnSend").attr("disabled", false);
}).catch(function (err) {
    return alert(err.toString());
});

$("#btnSend").on("click", function () {
    var userTo = $("#txtUserTo").val();
    var user = $("#txtUserName").val();
    var message = $("#txtMessage").val();

    if (user != "" && message != "" && userTo != "") {
        connection.invoke("SendMessage", user, message, userTo).catch(function (err) {
            return alert(err.toString())
        });
        event.preventDefault();
    }
    $('#txtMessage').val('');
});

