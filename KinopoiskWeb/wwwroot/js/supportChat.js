$(document).ready(function () {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/supportChatHub")
        .withAutomaticReconnect()
        .configureLogging(signalR.LogLevel.Information)
        .build();

    let lastMessage = null;

    connection.on("ReceiveMessage", function (user, message) {
        const newMessage = `${user}: ${message}`;

        if (newMessage !== lastMessage) {
            const msg = $("<div></div>").text(newMessage);
            $("#chatMessages").append(msg);
            $("#chatMessages").scrollTop($("#chatMessages")[0].scrollHeight);

            lastMessage = newMessage;
        }
    });

    connection.onreconnecting(function (error) {
        console.log(`Attempting to reconnect: ${error}`);
        const msg = $("<div></div>").text(getTranslation('chat.reconnecting'));
        $("#chatMessages").append(msg);
    });

    connection.onreconnected(function (connectionId) {
        console.log(`Reconnected with connectionId: ${connectionId}`);
        const msg = $("<div></div>").text(getTranslation('chat.reconnected'));
        $("#chatMessages").append(msg);
    });

    connection.onclose(function (error) {
        console.error(`Connection closed: ${error}`);
        const msg = $("<div></div>").text(getTranslation('chat.connection_closed'));
        $("#chatMessages").append(msg);
    });

    connection.start()
        .then(function () {
            console.log("Connected to SupportChatHub");
            const msg = $("<div></div>").text(getTranslation('chat.connected'));
            $("#chatMessages").append(msg);
        })
        .catch(function (err) {
            console.error(`Connection failed: ${err}`);
            const msg = $("<div></div>").text(getTranslation('chat.connection_failed'));
            $("#chatMessages").append(msg);
        });

    connection.on("UpdateUserList", function (users) {
        console.log(getTranslation('chat.update_user_list') + ':', users);
    });

    $("#sendButton").click(function (event) {
        const message = $("#messageInput").val();
        if (message.trim() === "") return;

        const msg = $("<div></div>").text(getTranslation('chat.send') + message);
        $("#chatMessages").append(msg);
        $("#chatMessages").scrollTop($("#chatMessages")[0].scrollHeight);

        connection.invoke("SendMessageToAdmin", message)
            .then(function () {
                $("#messageInput").val("");
            })
            .catch(function (err) {
                console.error(`Failed to send message: ${err}`);
                const errorMsg = $("<div></div>").text(getTranslation('chat.failed_to_send_message'));
                $("#chatMessages").append(errorMsg);
            });

        event.preventDefault();
    });

    $("#closeChat").click(function () {
        $("#chatWindow").hide();
        connection.stop()
            .then(function () {
                console.log(getTranslation('chat.close_chat'));
            })
            .catch(function (err) {
                console.error(`Failed to stop connection: ${err}`);
            });
    });

    $("#chatIcon").click(function () {
        $("#chatIcon").hide();
        $("#chatWindow").show();
    });

    $("#closeChat").click(function () {
        $("#chatWindow").hide();
        $("#chatIcon").show();
    });
});
