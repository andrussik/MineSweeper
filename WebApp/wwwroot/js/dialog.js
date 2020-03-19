// Get the optionsDiv
const optionsDiv = document.getElementById("optionsDiv");

// Get the dialog
let dialog = document.getElementById("settingsDiv");

// Get the <span> element that closes the modal
let span = dialog.getElementsByClassName("close")[0];

// Set dialog and span according to selection, then open dialog
optionsDiv.onclick = (e ) => {
    switch (e.target) {
        case document.getElementById("settings"):
            dialog = document.getElementById("settingsDiv");
            span = dialog.getElementsByClassName("close")[0];
            dialog.style.display = "table";
            break;
        case document.getElementById("saveGame"):
            dialog = document.getElementById("saveDiv");
            span = dialog.getElementsByClassName("close")[0];
            dialog.style.display = "table";
            break;
        case document.getElementById("loadGame"):
            dialog = document.getElementById("loadDiv");
            span = dialog.getElementsByClassName("close")[0];
            dialog.style.display = "table";
            break;
        default:
            e.preventDefault();
        }
};

// When the user clicks anywhere outside of the modal, close it
// When the user clicks submit, then submit selections
// When the user clicks x, close it
window.onclick = (e) => {
    if (e.target === dialog) {
        dialog.style.display = "none";
    } else if (e.target === span) {
        dialog.style.display = "none";
    } else if (e.target === document.getElementById("submitSettings")) {
        if (!document.getElementById("custom").checked) {
            const inputs = document.getElementsByClassName("dialogTextInput");
            for (let i = 0; i < inputs.length; i++) {
                inputs[i].setAttribute("disabled", "disabled");
            }
        } 
        // validation
        else {
            if (document.getElementById("customHeight").value < 1
                || document.getElementById("customHeight").value > 40) {
                document.getElementById("heightError").removeAttribute("hidden");
                e.preventDefault();
            } else {
                document.getElementById("heightError").setAttribute("hidden", "hidden");
            }
            
            if (document.getElementById("customWidth").value < 1
                || document.getElementById("customWidth").value > 40) {
                document.getElementById("widthError").removeAttribute("hidden");
                e.preventDefault()
            } else {
                document.getElementById("widthError").setAttribute("hidden", "hidden");
            }
            
            if (0 > document.getElementById("customMines").value) {
                document.getElementById("minesError").removeAttribute("hidden");
                e.preventDefault()
            } else {
                document.getElementById("minesError").setAttribute("hidden", "hidden");
            }
        }
    }
};
