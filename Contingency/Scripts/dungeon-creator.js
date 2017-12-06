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

var canvas = document.getElementById("dungeon-canvas");
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

var err = document.getElementById("err");
var monsterdone = false;
var lootdone = false;
var map;
function Next() {
    if (CheckValid()) {
        err.innerHTML = "";
        switch (currentStage) {
            case 1:
                if(!monsterdone)
                document.getElementById('temp-ok').disabled = true;
                dimensionBox.style.display = "none";
                monsterBox.style.display = "block";
                createTitle.innerHTML = "Dungeon Monsters"
                backButton.style.visibility = "visible";
                preview.innerHTML = "";
                break;
            case 2:
                if(!lootdone)
                document.getElementById('final-ok').disabled = true;
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
}
function Back() {
    switch (currentStage) {
        case 2:
            GenerateDungeon();
            monsterBox.style.display = "none";
            dimensionBox.style.display = "block";
            createTitle.innerHTML = "Dungeon Dimensions"
            backButton.style.visibility = "hidden";
            document.getElementById('temp-ok').disabled = false;
            break;
        case 3:
            GenetateMonsters();
            lootBox.style.display = "none";
            monsterBox.style.display = "block";
            createTitle.innerHTML = "Dungeon Monsters"
            finalOk.style.display = "none";
            tempOk.style.display = "block";
            document.getElementById('temp-ok').disabled = false;
            break;
    }
    currentStage--;
}

function GenerateDungeon() {
    preview.innerHTML = "<canvas id='dungeon-canvas' width='500' height='500'></canvas>";
    var canvas = document.getElementById("dungeon-canvas");
    var ctx = canvas.getContext('2d');
    var blocksize;

    map = [
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
    document.getElementById("dung-canvas").innerHTML = "<canvas id='dungeon-canvas' width='500' height='500'></canvas>";
    var canvas = document.getElementById("dungeon-canvas");
    canvas.addEventListener("click", function (event) {
        var mousX = event.pageX;
        var mousY = event.pageY;
        //console.log(mousY);
        if (124 <= mousX && mousX <= 200) {
            if (171 <= mousY && mousY <= 244) {
                //.location.href = "/Dungeon/Room/99999/2";
                console.log("Room 2");
            }
        } if (100 <= mousX && mousX <= 225) {
            if (275 <= mousY && mousY <= 375) {
                console.log("Room 3");
            }
        } if (225 <= mousX && mousX <= 300) {
            if (197 <= mousY && mousY <= 272) {
                console.log("Room 4");
            }
        } if (350 <= mousX && mousX <= 425) {
            if (171 <= mousY && mousY <= 298) {
                console.log("Room 5");
            }
        } if (300 <= mousX && mousX <= 450) {
            if (398 <= mousY && mousY <= 497) {
                console.log("Room 6");
            }
        } if (400 <= mousX && mousX <= 524) {
            if (500 <= mousY && mousY <= 624) {
                console.log("Room 7");
            }
        } if (125 <= mousX && mousX <= 200) {
            if (423 <= mousY && mousY <= 520) {
                console.log("Room 8")
            }
        } if (150 <= mousX && mousX <= 223) {
            if (547 <= mousY && mousY <= 620) {
                console.log("Room 9");
            }
        }
    })
    var ctx = canvas.getContext('2d');
    var blocksize;
    map = [
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
    document.getElementById('temp-ok').disabled = false;
    preview.innerHTML = "Monsters! :D";
    monsterdone = true;
}
function GenerateLoot() {
    document.getElementById('final-ok').disabled = false;
    preview.innerHTML = "Loot! :D";
    lootdone = true;
}

function CheckValid() {
    switch (currentStage) {
        case 1:
            if (document.getElementById("widthFinal").value >= 15 &&
                document.getElementById("widthFinal").value <= 200 &&
                document.getElementById("heightFinal").value >= 15 &&
                document.getElementById("heightFinal").value <= 200) {
                if ((document.getElementById("rm-small").checked) ||
                    (document.getElementById("rm-medium").checked) ||
                    (document.getElementById("rm-large").checked) ||
                    (document.getElementById("rm-massive").checked)) {
                    return true;
                } else {
                    err.innerHTML = "<p>*Must have at least one room group.</p>";
                    return false;
                }
                
            } else {
                err.innerHTML = "<p>*Width and Height must be between 15-200</p>";
                return false;
            }
    }
    return true;
}