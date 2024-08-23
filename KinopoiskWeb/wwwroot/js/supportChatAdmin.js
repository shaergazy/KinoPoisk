$(document).ready(function () {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/supportChatHub")
        .build();

    let selectedUser = null;

    connection.on("ReceiveMessage", function (user, message) {
        if (selectedUser === user) {
            const msg = $("<div></div>").text(user + ": " + message);
            $("#chatMessages").append(msg);
            $("#chatMessages").scrollTop($("#chatMessages")[0].scrollHeight);
        } else {
            // Обновить список пользователей, если это новое сообщение от пользователя
            addToUserList(user);
        }
    });

    connection.start().catch(function (err) {
        console.error(err.toString());
    });

    function addToUserList(user) {
        if ($("#userList").find("li[data-user='" + user + "']").length === 0) {
            const userItem = $("<li data-user='" + user + "'></li>").text(user);
            userItem.click(function () {
                selectedUser = $(this).data("user");
                $("#chatUserName").text(selectedUser);
                loadChatHistory(selectedUser); // Здесь можно загрузить историю чата
            });
            $("#userList").append(userItem);
        }
    }

    $("#sendButton").click(function (event) {
        const message = $("#messageInput").val();
        if (message.trim() === "" || !selectedUser) return;

        connection.invoke("SendMessageToUser", selectedUser, message).catch(function (err) {
            console.error(err.toString());
        });
        $("#messageInput").val("");
        event.preventDefault();
    });
});
