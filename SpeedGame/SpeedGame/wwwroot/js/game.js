"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/gameHub").build();
var Selected;
var HandJS;
var OpponentHandCountJS;
var PlayerDrawStackJS;
var OpponentDrawStackCountJS;
var PlayStack1JS;
var PlayStack2JS;
var ExStack1JS;
var ExStack2JS;
var PlayStack1JSTop;
var PlayStack2JSTop;

//Disable the send button until connection is established.
document.getElementById("playButton").disabled = true;

connection.on("UpdateGame", function (playerStack, oppStackCount, playerDrawStack, oppDrawStackCt, playStack1, playStack2, exStack1, exStack2, playStack1Top, playStack2Top) {
    HandJS = JSON.parse(playerStack);
    OpponentHandCountJS = oppStackCount;
    PlayerDrawStackJS = JSON.parse(playerDrawStack);
    OpponentDrawStackCountJS = oppDrawStackCt;
    PlayStack1JS = JSON.parse(playStack1);
    PlayStack2JS = JSON.parse(playStack2);
    ExStack1JS = JSON.parse(exStack1);
    ExStack2JS = JSON.parse(exStack2);
    PlayStack1JSTop = JSON.parse(playStack1Top);
    PlayStack2JSTop = JSON.parse(playStack2Top);

    document.getElementById("playerStackCt").innerHTML = "Your Stack: " + PlayerDrawStackJS.length + " Cards";
    document.getElementById("opponentStackCt").innerHTML = "Opponent Stack: " + OpponentDrawStackCountJS + " Cards";

    document.getElementById("playStack1").src = PlayStack1JSTop.associatedImg;
    document.getElementById("playStack2").src = PlayStack2JSTop.associatedImg;

    document.getElementById("playerCard1").src = HandJS[0].associatedImg;
    document.getElementById("playerCard2").src = HandJS[1].associatedImg;
    document.getElementById("playerCard3").src = HandJS[2].associatedImg;
    document.getElementById("playerCard4").src = HandJS[3].associatedImg;
    document.getElementById("playerCard5").src = HandJS[4].associatedImg;
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
    Selected = 0;
    document.getElementById("playerCard1").border = 1;
    document.getElementById("playerCard2").border = 0;
    document.getElementById("playerCard3").border = 0;
    document.getElementById("playerCard4").border = 0;
    document.getElementById("playerCard5").border = 0;
})

document.getElementById("playerCard2").addEventListener("click", function (event) {
    Selected = 1;
    document.getElementById("playerCard1").border = 0;
    document.getElementById("playerCard2").border = 1;
    document.getElementById("playerCard3").border = 0;
    document.getElementById("playerCard4").border = 0;
    document.getElementById("playerCard5").border = 0;
})
document.getElementById("playerCard3").addEventListener("click", function (event) {
    Selected = 2;
    document.getElementById("playerCard1").border = 0;
    document.getElementById("playerCard2").border = 0;
    document.getElementById("playerCard3").border = 1;
    document.getElementById("playerCard4").border = 0;
    document.getElementById("playerCard5").border = 0;
})
document.getElementById("playerCard4").addEventListener("click", function (event) {
    Selected = 3;
    document.getElementById("playerCard1").border = 0;
    document.getElementById("playerCard2").border = 0;
    document.getElementById("playerCard3").border = 0;
    document.getElementById("playerCard4").border = 1;
    document.getElementById("playerCard5").border = 0;

})
document.getElementById("playerCard5").addEventListener("click", function (event) {
    Selected = 4;
    document.getElementById("playerCard1").border = 0;
    document.getElementById("playerCard2").border = 0;
    document.getElementById("playerCard3").border = 0;
    document.getElementById("playerCard4").border = 0;
    document.getElementById("playerCard5").border = 1;
})
document.getElementById("playStack1").addEventListener("click", function (event) {
    connection.invoke("compareCard", JSON.stringify(PlayStack1JS), JSON.stringify(HandJS[Selected]), JSON.stringify(HandJS), OpponentHandCountJS, JSON.stringify(PlayerDrawStackJS), OpponentDrawStackCountJS, JSON.stringify(PlayStack1JSTop), JSON.stringify(PlayStack2JS), JSON.stringify(ExStack1JS), JSON.stringify(ExStack2JS)).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});
document.getElementById("playStack2").addEventListener("click", function (event) {
    connection.invoke("compareCard", JSON.stringify(PlayStack2JS), JSON.stringify(HandJS[Selected]), JSON.stringify(HandJS), OpponentHandCountJS, JSON.stringify(PlayerDrawStackJS), OpponentDrawStackCountJS, JSON.stringify(PlayStack1JS), JSON.stringify(PlayStack2JS), JSON.stringify(ExStack1JS), JSON.stringify(ExStack2JS)).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});