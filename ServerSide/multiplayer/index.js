var express = require('express');
var app = express();

//important to initilaize
var server = require('http').createServer(app);
var io = require('socket.io').listen(server);

app.set('port', process.env.PORT || 3000);

var clients = [];
var rooms = ["Lobby"];

server.listen(app.get('port'),function(){
    console.log("------ SERVER IS RUNNING ------");
});


// when the connection is established
io.on("connection", function(socket){

    var currentUser;
    // when the player connected.
    // it will call this callback function directly to recieve other clients rooms
    socket.on("user_connect", function()
    {
        for(var i = 0; i<clients.length; i++)
        {
            socket.emit("user_awake", {room:clients[i].room});

        }
    });
    // when the client send a message with "play" tag to the server
    socket.on("play", function(data){

        // handle the currentUser
        currentUser = {
            name:data.name,
            position:data.position,
            room:data.room
        }

        clients.push(currentUser);

        console.log("a user is connected to room:"+currentUser.room+" name:"+currentUser.name);

        // join the room
        socket.join(currentUser.room);

        //send message back to the client
        socket.emit("play", currentUser);

        // and broadcast the message to the all clients.
        socket.broadcast.to(currentUser.room).emit("user_connected", currentUser);
    });

    // tries to send the informations of clients with the same room as current player.
    socket.on("get_players", function(){
        for(var i = 0; i<clients.length; i++)
        {
            if(clients[i].room == currentUser.room && clients[i].name != currentUser.name)
            {

                socket.emit("user_connected",
                 {
                     name:clients[i].name, 
                     position:clients[i].position,
                     room:clients[i].room
                });
            }
        }
    });

    // when a player tries to move, it sends to a message to the client. 
    socket.on("move", function(data)
    {
        currentUser.position = data.position;
        
        console.log("User "+currentUser.name+" has moved to "+currentUser.position);
        // send back a message to the client 
        // and other clients that connected to the server with the same room with this user.
        socket.emit("move", currentUser);
        socket.broadcast.to(currentUser.room).emit("move", currentUser);

    });

    // a user dissconnect
    socket.on("disconnect", function(){
        if(currentUser !== undefined)
        {
            socket.broadcast.to(currentUser.room).emit("user_disconnected", currentUser);
            for(var i = 0; i < clients.length; i++)
            {
                if(clients[i].name == currentUser.name)
                {
                    console.log("User is disconnected: ",currentUser);
                    clients.splice(i,1);
                }
            }
        }
    });

    /* CHAT  CONTROLLER AREA */
    socket.on("chat", function(data){
        socket.emit("chat", {name:currentUser.name, message:data.message});
        socket.broadcast.to(currentUser.room).emit("chat", {name:currentUser.name, message:data.message});
    });

    

});


