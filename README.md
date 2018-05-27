# Network Test (for a 2D Game)

Simple example to understand networking code in Unity.

Problem is that if you update your SyncVar outside a command it will only update for the host-player, not the clients.

The blue box shows the direction the player is facing (think of it as blue eyes). Note that the player slides a bit, the player controller is very simple but for this example we concentrate on the network communication.

When the player presses a key the command "CmdSetDirection" will get executed on the server. In this [Command]-code we set the direction variable. Becaue it is marked with [SyncVar] it triggers the clients to update it and call the OnDirectionChange hook. And only at that point we calculate the new player position on the screen.
