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
        const msg = $("<div></div>").text("Reconnecting...");
        $("#chatMessages").append(msg);
    });
    
    connection.onreconnected(function (connectionId) {
        console.log(`Reconnected with connectionId: ${connectionId}`);
        const msg = $("<div></div>").text("Reconnected.");
        $("#chatMessages").append(msg);
    });
    
    connection.onclose(function (error) {
        console.error(`Connection closed: ${error}`);
        const msg = $("<div></div>").text("Connection closed. Please refresh the page to reconnect.");
        $("#chatMessages").append(msg);
    });
    
    connection.start()
        .then(function () {
            console.log("Connected to SupportChatHub");
            const msg = $("<div></div>").text("Connected to chat.");
            $("#chatMessages").append(msg);
        })
        .catch(function (err) {
            console.error(`Connection failed: ${err}`);
            const msg = $("<div></div>").text("Failed to connect. Please refresh the page.");
            $("#chatMessages").append(msg);
        });
    connection.on("UpdateUserList", function (users) {
        console.log('Updating user list:', users);
    });
    $("#sendButton").click(function (event) {
        const message = $("#messageInput").val();
        if (message.trim() === "") return;

        const msg = $("<div></div>").text("You: " + message);
        $("#chatMessages").append(msg);
        $("#chatMessages").scrollTop($("#chatMessages")[0].scrollHeight);

        connection.invoke("SendMessageToAdmin", message)
            .then(function () {
                $("#messageInput").val("");
            })
            .catch(function (err) {
                console.error(`Failed to send message: ${err}`);
                const errorMsg = $("<div></div>").text("Failed to send message. Please try again.");
                $("#chatMessages").append(errorMsg);
            });

        event.preventDefault();
    });
    
    $("#closeChat").click(function () {
        $("#chatWindow").hide();
        connection.stop()
            .then(function () {
                console.log("Connection stopped");
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
