var goodToContinue = false;
var currentStage = 1;
var preview = document.getElementById("preview-display");
var dimensionBox = document.getElementById("dimensions");
var monsterBox = document.getElementById("monsters");
var lootBox = document.getElementById("loot");

var tempOk = document.getElementById("temp-ok");
var finalOk = document.getElementById("final-ok");
var backButton = document.getElementById("back-button");
var createTitle = document.getElementById("create-title");

function Next() {
    switch (currentStage) {
        case 1:
            dimensionBox.style.display = "none";
            monsterBox.style.display = "block";
            createTitle.innerHTML = "Dungeon Monsters"
            backButton.style.visibility = "visible";
            preview.innerHTML = "";
            break;
        case 2:
            monsterBox.style.display = "none";
            lootBox.style.display = "block";
            createTitle.innerHTML = "Dungeon Loot"
            tempOk.style.display = "none";
            finalOk.style.display = "block";
            preview.innerHTML = "";
            break;
    }
    currentStage++;
}
function Back() {
    switch (currentStage) {
        case 2:
            GenerateDungeon();
            monsterBox.style.display = "none";
            dimensionBox.style.display = "block";
            createTitle.innerHTML = "Dungeon Dimensions"
            backButton.style.visibility = "hidden";
            break;
        case 3:
            GenetateMonsters();
            lootBox.style.display = "none";
            monsterBox.style.display = "block";
            createTitle.innerHTML = "Dungeon Monsters"
            finalOk.style.display = "none";
            tempOk.style.display = "block";
            break;
    }
    currentStage--;
}
function Generate() {
    if (CheckValid()) {
        switch (currentStage) {
            case 1:
                GenerateDungeon(); break;
            case 2:
                GenetateMonsters(); break;
            case 3:
                GenerateLoot(); break;
        }
    }
}
function GenerateDungeon() {
    preview.innerHTML = "<canvas id='dungeon-canvas' width='500' height='500'></canvas>";
    var canvas = document.getElementById("dungeon-canvas");
    var ctx = canvas.getContext('2d');
    var blocksize;
    var map = [
        [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
        [0, 0, 0, 2, 2, 2, 0, 0, 0, 0, 0, 0, 5, 5, 5, 0, 0, 0, 0, 0],
        [1, 1, 1, 2, 2, 2, 0, 4, 4, 4, 0, 0, 5, 5, 5, 0, 0, 0, 0, 0],
        [0, 0, 0, 2, 2, 2, 0, 4, 4, 4, 1, 1, 5, 5, 5, 0, 0, 0, 0, 0],
        [0, 0, 0, 0, 1, 0, 0, 4, 4, 4, 0, 0, 5, 5, 5, 0, 0, 0, 0, 0],
        [0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 5, 5, 5, 0, 0, 0, 0, 0],
        [0, 0, 3, 3, 3, 3, 3, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0],
        [0, 0, 3, 3, 3, 3, 3, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0],
        [0, 0, 3, 3, 3, 3, 3, 1, 1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0],
        [0, 0, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0],
        [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6, 6, 6, 6, 0, 0, 0, 0],
        [0, 0, 0, 8, 8, 8, 0, 0, 0, 0, 6, 6, 6, 6, 6, 6, 0, 0, 0, 0],
        [0, 0, 0, 8, 8, 8, 1, 1, 0, 0, 6, 6, 6, 6, 6, 6, 0, 0, 0, 0],
        [0, 0, 0, 8, 8, 8, 0, 1, 0, 0, 6, 6, 6, 6, 6, 6, 0, 0, 0, 0],
        [0, 0, 0, 8, 8, 8, 0, 1, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0],
        [0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 1, 0, 9, 9, 9, 9, 9, 0],
        [0, 0, 0, 0, 7, 7, 7, 0, 0, 1, 0, 0, 1, 0, 9, 9, 9, 9, 9, 0],
        [0, 0, 1, 1, 7, 7, 7, 1, 1, 1, 1, 1, 1, 1, 9, 9, 9, 9, 9, 0],
        [0, 0, 1, 0, 7, 7, 7, 0, 0, 0, 0, 0, 0, 0, 9, 9, 9, 9, 9, 0],
        [0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 9, 9, 9, 9, 9, 0]
    ];
    function draw() {
        blocksize = canvas.clientHeight / map.length;
        for (var i = 0; i < map.length; i++) {
            for (var j = 0; j < map.length; j++) {
                if (map[i][j] == 0) {
                    drawRect(j * blocksize, i * blocksize, 0, 0, 0);
                } else if (map[i][j] == 1) {
                    drawRect(j * blocksize, i * blocksize, 255, 255, 255);
                } else {
                    drawRect(j * blocksize, i * blocksize, 55, 255, 55);
                }
            }
        }
    }
    function drawRect(x, y, r, g, b) {
        ctx.fillStyle = "rgb(" + r + "," + g + "," + b + ")";
        ctx.fillRect(x, y, blocksize, blocksize);
    }
    draw();
}
function GenerateDungeon2() {
    console.log("hello");
    document.getElementById("dung-canvas").innerHTML = "<canvas id='dungeon-canvas' width='500' height='500'></canvas>";
    var canvas = document.getElementById("dungeon-canvas");
    var ctx = canvas.getContext('2d');
    var blocksize;
    var map = [
        [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
        [0, 0, 0, 2, 2, 2, 0, 0, 0, 0, 0, 0, 5, 5, 5, 0, 0, 0, 0, 0],
        [1, 1, 1, 2, 2, 2, 0, 4, 4, 4, 0, 0, 5, 5, 5, 0, 0, 0, 0, 0],
        [0, 0, 0, 2, 2, 2, 0, 4, 4, 4, 1, 1, 5, 5, 5, 0, 0, 0, 0, 0],
        [0, 0, 0, 0, 1, 0, 0, 4, 4, 4, 0, 0, 5, 5, 5, 0, 0, 0, 0, 0],
        [0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 5, 5, 5, 0, 0, 0, 0, 0],
        [0, 0, 3, 3, 3, 3, 3, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0],
        [0, 0, 3, 3, 3, 3, 3, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0],
        [0, 0, 3, 3, 3, 3, 3, 1, 1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0],
        [0, 0, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0],
        [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6, 6, 6, 6, 0, 0, 0, 0],
        [0, 0, 0, 8, 8, 8, 0, 0, 0, 0, 6, 6, 6, 6, 6, 6, 0, 0, 0, 0],
        [0, 0, 0, 8, 8, 8, 1, 1, 0, 0, 6, 6, 6, 6, 6, 6, 0, 0, 0, 0],
        [0, 0, 0, 8, 8, 8, 0, 1, 0, 0, 6, 6, 6, 6, 6, 6, 0, 0, 0, 0],
        [0, 0, 0, 8, 8, 8, 0, 1, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0],
        [0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 1, 0, 9, 9, 9, 9, 9, 0],
        [0, 0, 0, 0, 7, 7, 7, 0, 0, 1, 0, 0, 1, 0, 9, 9, 9, 9, 9, 0],
        [0, 0, 1, 1, 7, 7, 7, 1, 1, 1, 1, 1, 1, 1, 9, 9, 9, 9, 9, 0],
        [0, 0, 1, 0, 7, 7, 7, 0, 0, 0, 0, 0, 0, 0, 9, 9, 9, 9, 9, 0],
        [0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 9, 9, 9, 9, 9, 0]
    ];
    function draw() {
        blocksize = canvas.clientHeight / map.length;
        for (var i = 0; i < map.length; i++) {
            for (var j = 0; j < map.length; j++) {
                if (map[i][j] == 0) {
                    drawRect(j * blocksize, i * blocksize, 0, 0, 0);
                } else if (map[i][j] == 1) {
                    drawRect(j * blocksize, i * blocksize, 255, 255, 255);
                } else {
                    drawRect(j * blocksize, i * blocksize, 55, 255, 55);
                }
            }
        }
    }
    function drawRect(x, y, r, g, b) {
        ctx.fillStyle = "rgb(" + r + "," + g + "," + b + ")";
        ctx.fillRect(x, y, blocksize, blocksize);
    }
    draw();
}
function GenetateMonsters() {
    preview.innerHTML = "Monsters! :D";
}
function GenerateLoot() {
    preview.innerHTML = "Loot! :D";
}
function CheckValid() {
    //TODO
    return true;
}