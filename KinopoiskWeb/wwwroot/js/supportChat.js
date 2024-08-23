$(document).ready(function () {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/supportChatHub")
        .build();

    connection.on("ReceiveMessage", function (user, message) {
        const msg = $("<div></div>").text(user + ": " + message);
        $("#chatMessages").append(msg);
        $("#chatMessages").scrollTop($("#chatMessages")[0].scrollHeight);
    });

    connection.start().catch(function (err) {
        console.error(err.toString());
    });

    $("#sendButton").click(function (event) {
        const message = $("#messageInput").val();
        if (message.trim() === "") return;

        // Regular user sends message to admin
        connection.invoke("SendMessageToAdmin", message).catch(function (err) {
            console.error(err.toString());
        });
        $("#messageInput").val("");
        event.preventDefault();
    });

    $("#closeChat").click(function () {
        $("#chatWindow").hide();
    });
});
