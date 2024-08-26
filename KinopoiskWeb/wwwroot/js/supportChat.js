$(document).ready(function () {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/supportChatHub")
        .withAutomaticReconnect()
        .configureLogging(signalR.LogLevel.Information)
        .build();

    // Clear any existing handlers to prevent duplication
    connection.off("ReceiveMessage");

    connection.on("ReceiveMessage", function (user, message) {
        const msg = $("<div></div>").text(user + ": " + message);
        $("#chatMessages").append(msg);
        $("#chatMessages").scrollTop($("#chatMessages")[0].scrollHeight);
    });

    connection.onreconnecting(function (error) {
        console.log(`Attempting to reconnect: ${error}`);
    });

    connection.onreconnected(function (connectionId) {
        console.log(`Reconnected: ${connectionId}`);
        connection.invoke("UpdateConnectionId")
            .catch(function (err) {
                console.error(`Failed to update connection: ${err}`);
            });
    });

    connection.onclose(function (error) {
        console.error(`Connection closed: ${error}`);
    });

    connection.start()
        .then(function () {
            console.log("Connected to SupportChatHub");
        })
        .catch(function (err) {
            console.error(`Connection failed: ${err}`);
        });

    $("#sendButton").click(function (event) {
        const message = $("#messageInput").val();
        if (message.trim() === "") return;

        connection.invoke("SendMessageToAdmin", message)
            .then(function () {
                $("#messageInput").val("");
            })
            .catch(function (err) {
                console.error(`Failed to send message: ${err}`);
            });

        event.preventDefault();
    });

    $("#closeChat").click(function () {
        $("#chatWindow").hide();
    });
});
