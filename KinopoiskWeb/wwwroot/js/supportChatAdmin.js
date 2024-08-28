$(document).ready(function () {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/supportChatHub")
        .withAutomaticReconnect()
        .configureLogging(signalR.LogLevel.Information)
        .build();

    connection.serverTimeoutInMilliseconds = 1000 * 60 * 10;

    let selectedUser = null;
    let messageHistory = {};

    connection.on("ReceiveMessage", function (user, message) {
        console.log('Received message from', user, ':', message);

        if (!messageHistory[user]) {
            messageHistory[user] = [];
        }
        messageHistory[user].push({ user, message });

        if (selectedUser === user) {
            addMessageToChat(user, message);
        } else {
            addToUserList(user);
        }
    });

    connection.on("UpdateUserList", function (users) {
        console.log('Updating user list:', users);
        $("#adminUserList").empty();
        users.forEach(function (user) {
            addToUserList(user);
        });
    });

    connection.start().then(function () {
        console.log('Connected to SignalR hub');
    }).catch(function (err) {
        console.error('Connection error:', err.toString());
    });

    function addToUserList(user) {
        if ($("#adminUserList").find("li[data-user='" + user + "']").length === 0) {
            const userItem = $("<li data-user='" + user + "'></li>").text(user);
            userItem.click(function () {
                selectedUser = $(this).data("user");
                $("#adminChatUserName").text(selectedUser);

                loadChatHistory(selectedUser);
            });
            $("#adminUserList").append(userItem);
        }
    }

    $("#adminSendButton").click(function (event) {
        const message = $("#adminMessageInput").val();
        if (message.trim() === "" || !selectedUser) {
            console.warn('No message or no selected user.');
            return;
        }

        connection.invoke("SendMessageToUser", selectedUser, message).then(function () {
            console.log('Message sent to', selectedUser);
        }).catch(function (err) {
            console.error('Failed to send message:', err.toString());
        });

        if (!messageHistory[selectedUser]) {
            messageHistory[selectedUser] = [];
        }
        messageHistory[selectedUser].push({ user: "Admin", message });

        addMessageToChat("Admin", message);

        $("#adminMessageInput").val("");
        event.preventDefault();
    });

    function addMessageToChat(user, message) {
        const msg = $("<div></div>").text(user + ": " + message);
        $("#adminChatMessages").append(msg);
        $("#adminChatMessages").scrollTop($("#adminChatMessages")[0].scrollHeight);
    }

    function loadChatHistory(user) {
        $("#adminChatMessages").empty();

        if (messageHistory[user]) {
            messageHistory[user].forEach(function (msg) {
                addMessageToChat(msg.user, msg.message);
            });
        }
    }
});
