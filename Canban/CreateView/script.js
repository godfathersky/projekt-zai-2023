

if (localStorage.getItem("JWTToken")) {
    const JWT = localStorage.getItem("JWTToken");
    const decodedJWT = decodeJWT(JWT);
    const header = document.querySelector("h1");
    const page = document.querySelector(".wrapper");
    var name = decodedJWT["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"];
    header.innerHTML = "Witaj, jesteś zalogowany jako " + "<span class='username'>" + name + "<span>";
    const createView = document.querySelector(".createView");
    var data = {
        user: "",
        nazwaPrzestrzenRobocza: "",
        dataUtworzeniaPrzestrzenRobocza: ""
    }
    data.user = name;
    createView.addEventListener("click", (e) => {
        createWizard(page, 0, "Nazwij swoją pierwszą przestrzeń roboczą", data);
    })
}

function createWizard(page, currentStep, header, data = null) {
    const allSteps = 2;
    const wizardWrapper = document.createElement("div");
    wizardWrapper.classList.add("container");
    const wizard = document.createElement("div");
    wizard.classList.add("wizard");
    const stepContainer = document.createElement("div");
    stepContainer.classList.add("stepsWrapper");
    let steps = [];
    for(let i=0; i<allSteps; i++){
        var step = document.createElement("span");
        steps.push(step);
        step.classList.add("currentStep");
        stepContainer.appendChild(step);
    }
    steps[0].classList.add("active");
    const head = document.createElement("div");
    head.classList.add("header");
    const headContent = document.createElement("p");
    headContent.textContent = header;
    const input = document.createElement("input");
    input.classList.add("spaceName");
    input.type = "text";
    const buttonWrapper = document.createElement("div");
    buttonWrapper.classList.add("buttonWrapper");
    const button = document.createElement("button");
    const closeButton = document.createElement("input");
    closeButton.type = "image";
    closeButton.src = "/media/reject (2).png"
    closeButton.classList.add("exit");
    button.textContent = "Stwórz przestrzeń"
    button.classList.add("submit");
    wizard.appendChild(stepContainer);
    head.appendChild(headContent);
    buttonWrapper.appendChild(button);
    wizard.appendChild(head);
    wizard.appendChild(input);
    wizard.appendChild(buttonWrapper);
    wizard.appendChild(closeButton);
    wizardWrapper.appendChild(wizard);
    page.appendChild(wizardWrapper);
    closeButton.addEventListener("click", (e)=>{
        e.target.closest(".container").remove();
    })

    button.addEventListener("click", (e)=>{
        switch(currentStep){
            case 0:{
                if(input.value.trim() != ""){
                    data.nazwaPrzestrzenRobocza = input.value.trim();
                    const dateTime = new Date();
                    data.dataUtworzeniaPrzestrzenRobocza = dateTime.toISOString().slice(0,10);
                    const xhr = new XMLHttpRequest();
                    xhr.onreadystatechange = ()=>{
                        if(xhr.readyState === 4){
                            if(xhr.status=== 200){
                                e.target.closest(".container").remove();
                                this.createWizard(page, 1, "Nazwij pierwszą tablicę", data);
                                const steps = document.querySelectorAll(".currentStep");
                                steps[1].classList.add("active");
                            }
                        }
                    }
                    xhr.open("POST", "https://localhost:7221/api/PrzestrzenieRoboczes");
                    xhr.setRequestHeader("Accept", "application/json");
                    xhr.setRequestHeader("Content-Type", "application/json");
                    xhr.send(JSON.stringify(data));
                }
                break;
            }
            case 1:{
                if(input.value.trim() != ""){
                    let board = {
                        user: "",
                        nazwaPrzestrzen: "",
                        nazwaTablicaZadan: "",
                        dataUtworzeniaTablicaZadan: ""
                    }
                    const dateTime = new Date();
                    board.dataUtworzeniaTablicaZadan = dateTime.toISOString().slice(0,10);
                    board.user = name;
                    board.nazwaPrzestrzen = data.nazwaPrzestrzenRobocza;
                    board.nazwaTablicaZadan = input.value.trim();
                    const xhr = new XMLHttpRequest();
                    xhr.onreadystatechange = ()=>{
                        if(xhr.readyState === 4){
                            if(xhr.status=== 200){
                               e.target.closest(".container").remove();
                               location.href = "/MainPage/index.html";
                            }
                        }
                    }
                    xhr.open("POST", "https://localhost:7221/api/PrzestrzenieRoboczes/board");
                    xhr.setRequestHeader("Accept", "application/json");
                    xhr.setRequestHeader("Content-Type", "application/json");
                    xhr.send(JSON.stringify(board));
                }
                break;
            }
        }
    })

}

function decodeJWT(jwt) {
    var base64Url = jwt.split('.')[1];
    var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    var jsonPayload = decodeURIComponent(window.atob(base64).split('').map(function (c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));

    return JSON.parse(jsonPayload);
}