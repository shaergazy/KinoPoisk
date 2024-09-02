$(document).ready(function () {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/supportChatHub")
        .withAutomaticReconnect()
        .configureLogging(signalR.LogLevel.Information)
        .build();

    let selectedUser = null;
    let messageHistory = {};

    connection.on("ReceiveMessage", function (user, message) {
        console.log(getTranslation('notification.received_message_from'), user, ':', message);

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
        console.log(getTranslation('notification.updating_user_list'), users);
        $("#adminUserList").empty();
        users.forEach(function (user) {
            addToUserList(user);
        });
    });

    connection.start().then(function () {
        console.log(getTranslation('notification.connected_to_signalr_hub'));
    }).catch(function (err) {
        console.error(getTranslation('error.connection_error'), err.toString());
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
            console.warn(getTranslation('warning.no_message_or_no_selected_user'));
            return;
        }

        connection.invoke("SendMessageToUser", selectedUser, message).then(function () {
            console.log(getTranslation('notification.message_sent_to'), selectedUser);
        }).catch(function (err) {
            console.error(getTranslation('error.failed_to_send_message'), err.toString());
        });

        if (!messageHistory[selectedUser]) {
            messageHistory[selectedUser] = [];
        }
        messageHistory[selectedUser].push({ user: getTranslation('admin'), message });

        addMessageToChat(getTranslation('admin'), message);

        $("#adminMessageInput").val("");
        event.preventDefault();
    });

    function addMessageToChat(user, message) {
        const msg = $("<div></div>").text(user + ": " + message);
        $("#adminChatMessages").append(msg);

        const chatMessages = $("#adminChatMessages")[0];
        if (chatMessages) {
            $("#adminChatMessages").scrollTop(chatMessages.scrollHeight);
        }
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
