$(document).ready(function () {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/supportChatHub")
        .build();

    let selectedUser = null;

    // Обработка получения сообщений
    connection.on("ReceiveMessage", function (user, message) {
        console.log('Received message from', user, ':', message);
        addMessageToChat(user, message);

        if (selectedUser !== user) {
            addToUserList(user);
        }
    });

    connection.start().then(function () {
        console.log('Connected to SignalR hub');
    }).catch(function (err) {
        console.error('Connection error:', err.toString());
    });

    function addToUserList(user) {
        if ($("#userList").find("li[data-user='" + user + "']").length === 0) {
            const userItem = $("<li data-user='" + user + "'></li>").text(user);
            userItem.click(function () {
                selectedUser = $(this).data("user");
                $("#chatUserName").text(selectedUser);
                $("#chatMessages").empty(); // Очистить сообщения при смене пользователя
            });
            $("#userList").append(userItem);
        }
    }

    $("#sendButton").click(function (event) {
        const message = $("#messageInput").val();
        if (message.trim() === "" || !selectedUser) {
            console.warn('No message or no selected user.');
            return;
        }

        connection.invoke("SendMessageToUser", selectedUser, message).then(function () {
            console.log('Message sent to', selectedUser);
        }).catch(function (err) {
            console.error('Failed to send message:', err.toString());
        });

        addMessageToChat("Admin", message);
        $("#messageInput").val(""); // Очистить поле ввода
        event.preventDefault();
    });

    function addMessageToChat(user, message) {
        const msg = $("<div></div>").text(user + ": " + message);
        $("#chatMessages").append(msg);
        $("#chatMessages").scrollTop($("#chatMessages")[0].scrollHeight); // Автопрокрутка до последнего сообщения
    }
});
