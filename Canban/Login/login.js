import { popUp } from "/popUp.js";
const login = document.querySelector(".buttonWrapper").querySelector("button");
const wrapper = document.querySelector(".wrapper");
const home = document.querySelector(".home");

home.addEventListener("click", ()=>{
    location.href = "/StartPage/index.html";
})

login.addEventListener("click", (e) => {
    e.preventDefault();
    const data = {
        login: "",
        password: ""
    }

    const inputs = document.querySelectorAll("input");

    inputs.forEach(input => {
        if (input.name == "login") {
            data.login = input.value.trim();
            input.value = "";
        }
        else if (input.name = "password") {
            data.password = input.value.trim();
            input.value = "";
        }
    })

    let xhr = new XMLHttpRequest();
    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4) {
            if (xhr.status === 200) {
                localStorage.setItem("JWTToken", xhr.response);
                const decodedToken = decodeJWT(xhr.response);
                const user = decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"];
                const xhrSpaces = new XMLHttpRequest();
                xhrSpaces.onreadystatechange = () =>{
                    if(xhrSpaces.readyState === 4){
                        if(xhrSpaces.status === 200){
                            if(xhrSpaces.response >= 1){
                                location.href = "/MainPage/index.html";
                            }
                            else{
                                location.href = "/CreateView/index.html";
                            }
                        }
                    }
                }
                xhrSpaces.open("GET", "https://localhost:7221/api/PrzestrzenieRoboczes/workSpaces/" + user)
                xhrSpaces.send();
                //window.location.href = '/CreateView/index.html';
            }
            else {
                var response = JSON.parse(xhr.response);
                popUp(response, wrapper, "error");
            }
        }
    }
    xhr.open("POST", "https://localhost:7221/api/Auth/login");
    xhr.setRequestHeader("Accept", "application/json");
    xhr.setRequestHeader("Content-Type", "application/json");
    xhr.send(JSON.stringify(data));
})

function decodeJWT(jwt) {
    var base64Url = jwt.split('.')[1];
    var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    var jsonPayload = decodeURIComponent(window.atob(base64).split('').map(function (c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));

    return JSON.parse(jsonPayload);
}