"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/gameHub").build();

//Disable the send button until connection is established.
document.getElementById("playButton").disabled = true;

connection.on("UpdateGame", function (playerStack, oppStackCount, playerDrawStack, oppDrawStackCt, playStack1Top, playStack2Top, exStack1Flg, exStack2Flg) {
    var playerHand = JSON.parse(playerStack);
    var playStack1 = JSON.parse(playStack1Top);
    var playStack2 = JSON.parse(playStack2Top);
    var playerDS = JSON.parse(playerDrawStack);

    document.getElementById("playerStackCt").innerHTML = "Your Stack: " + playerDS.length + " Cards";
    document.getElementById("opponentStackCt").innerHTML = "Opponent Stack: " + oppDrawStackCt + " Cards";

    document.getElementById("playStack1").src = playStack1.associatedImg;
    document.getElementById("playStack2").src = playStack2.associatedImg;

    document.getElementById("playerCard1").src = playerHand[0].associatedImg;
    document.getElementById("playerCard2").src = playerHand[1].associatedImg;
    document.getElementById("playerCard3").src = playerHand[2].associatedImg;
    document.getElementById("playerCard4").src = playerHand[3].associatedImg;
    document.getElementById("playerCard5").src = playerHand[4].associatedImg;
    document.getElementById("playButton").style.display = "none";
});                                              

connection.start().then(function () {
    document.getElementById("playButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("playButton").addEventListener("click", function (event) {
    connection.invoke("InitiateGame").catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("playerCard1").addEventListener("click", function (event) {
    connection.invoke("selectCard", 0).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
})