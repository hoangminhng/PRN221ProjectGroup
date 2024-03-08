const connection = new signalR.HubConnectionBuilder().withUrl("/onlineHub").build();

const onlineUsers = JSON.parse(localStorage.getItem("onlineUsers")) || {};

function updateStatusElement(userId) {
    const element = document.getElementById("user_" + userId);
    if (element) {
        if (onlineUsers[userId]) {
            element.innerHTML = "<span class='fas fa-circle chat-online'></span> Online";
        } else {
            element.innerHTML = "<span class='fas fa-circle chat-offline'></span> Offline";
        }
    }
}

connection.on("UserConnected", function (userId) {
    console.log("UserConnected:", userId);
    onlineUsers[userId] = true;
    updateStatusElement(userId);
     Update localStorage
    localStorage.setItem("onlineUsers", JSON.stringify(onlineUsers));
    console.log("Online Users from localStorage:", onlineUsers);
});

connection.on("UserDisconnected", function (userId) {
    console.log("UserDisconnected:", userId);
    delete onlineUsers[userId];
    updateStatusElement(userId);
    localStorage.setItem("onlineUsers", JSON.stringify(onlineUsers));
    console.log("Online Users from localStorage:", onlineUsers);
});

connection.start().then(function () {
    console.log("Connection established");
}).catch(function (err) {
    console.error("Connection error:", err);
});

window.addEventListener("beforeunload", function (event) {
    localStorage.setItem("onlineUsers", JSON.stringify(onlineUsers));
});

for (const userId in onlineUsers) {
    updateStatusElement(userId);
}