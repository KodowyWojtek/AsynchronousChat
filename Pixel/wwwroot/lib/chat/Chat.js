"use strict"
var connection = new signalR.HubConnectionBuilder().withUrl("/UserChat").build();

$("#btnSend").attr("disabled", true);

connection.on("ReceiveMessage", function (userFrom, message, userTo) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt").replace(/>/g, "&gt;");
    var encodeMsg = msg;
    var list = $("#messageList");
    var regex = new RegExp('(https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|www\.[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9]+\.[^\s]{2,}|www\.[a-zA-Z0-9]+\.[^\s]{2,})');
    if (regex.test(message)) {
        var url = regex.exec(message).shift();
        var text = message.replace(url, '');
        if (!url.startsWith("http://") && !url.startsWith("https://")) {
            url = "http://" + url;
        }
        list.append(text);
        list.append($('<a href="' + url + '">' + url + '</a>'));
        list.append("<br>");
    }
    else {
        list.append(encodeMsg + "<br>");
    }
    //list.append(encodeMsg + "<br>");
    scrollToBottom();
});

connection.start().then(function () {
    $("#btnSend").attr("disabled", false);
    scrollToBottom();
    $('#txtMessage').select();
}).catch(function (err) {
    return alert(err.toString());
});

function scrollToBottom() {
    var div = document.getElementById("messageBox");
    div.scrollTop = div.scrollHeight - div.clientHeight;
}

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
    $('#txtMessage').select();
});

