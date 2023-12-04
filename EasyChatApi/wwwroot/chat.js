let userName = '';
let messages = [];
const connection = new signalR.HubConnectionBuilder().withUrl("https://localhost:7057/ChatHub").build();

document.addEventListener("DOMContentLoaded", function () {


    connection.on("ReceiveMessage", function (user, message) {
        addMessage(user, message);
    });

    connection.start().then(function () {
        console.log("Connected to the hub");
    }).catch(function (err) {
        return console.error(err.toString());
    });

    connection.onclose(async () => {

        console.error("Disconnected from the hub");
        connection.start().catch(function (err) { return console.error(err.toString()) });
        
    });

});

function connectToChat() {
    userName = document.getElementById('username').value;
    if (userName) {
        document.getElementById('usernameSection').style.display = 'none';
        document.getElementById('chatSection').style.display = 'block';
        document.getElementById('usernameDisplay').prepend(userName);
        loadMessages();
    }
}

function sendMessage() {
    const messageToSend = document.getElementById("messageToSend").value;
    if (messageToSend && connection.state === signalR.HubConnectionState.Connected) {
        connection.invoke("SendMessage", userName, messageToSend).catch(function (err) {
            return console.error(err.toString());
        });
        console.log("Message sent");
        document.getElementById("messageToSend").value = '';
    } else {
        console.error("Cannot send message. The connection is not open.");
    }
}


function addMessage(user, message) {
    const messageElement = document.createElement('div');
    const userElement = document.createElement('strong');
    if (user === "System") {
        messageElement.classList.add('text-info'); 
    }
    userElement.textContent = user;

    let messageText = document.createTextNode(`: ${message}`);


    messageElement.appendChild(userElement);
    messageElement.appendChild(messageText);

    document.getElementById('messages').appendChild(messageElement);
}

function loadMessages() {
    addMessage('System', 'Chat Loading...');
    fetch('https://localhost:7057/Messages')
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            data.forEach(message => {
                addMessage(message.user, message.content);
            });
            if (data.length === 0) {
                addMessage('System', 'No messages yet 😶');
            }
            else {
                addMessage('System', "☝ What you've missed while you were gone! 🎉🎉");
            }
        })
        .catch(error => {
            console.error('There has been a problem during the fetch operation:', error);
        });
}
