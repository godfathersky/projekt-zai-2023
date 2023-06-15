import {popUp} from "/popUp.js";

const register = document.querySelector(".buttonWrapper").querySelector("button");
const form = document.querySelector("form");
const wrapper = document.querySelector(".wrapper");
const home = document.querySelector(".home");
const date = document.querySelector("#birthDate");

date.addEventListener("blur", function(){
    const enteredDate = new Date(this.value);
    const minDate = new Date(this.min);

    if(enteredDate < minDate){
        popUp("Niepoprawna data urodzenia", wrapper, "error");
        this.value = "1900-01-01";
    }
})

home.addEventListener("click", ()=>{
    location.href="/StartPage/index.html";
})

let user = {
    login: "",
    password: "",
    email: "",
    forname: "",
    birthDate: ""
}

register.addEventListener("click", (e)=>{
    e.preventDefault();
    const inputs = document.querySelectorAll("input");
    let repeatPassword = "";
    inputs.forEach(input=>{
        if(input.name == "login"){
            user.login = input.value.trim();
        }
        else if(input.name == "password"){
            user.password = input.value.trim();
        }
        else if(input.name == "forname"){
            user.forname = input.value.trim();
        }
        else if(input.name == "mail"){
            user.email = input.value.trim();
        }
        else if(input.name == "birthDate"){
            user.birthDate = input.value;
        }
        else if(input.name == "repeatPassword"){
            repeatPassword = input.value;
        }
    })

    let xhr = new XMLHttpRequest();
    if(user.password === repeatPassword){
        xhr.onreadystatechange = function(){
            if (xhr.readyState === 4){ 
                if(xhr.status === 200){ 
                    inputs.forEach(element=>{
                        element.value = "";
                    })
                    popUp("Konto utworzono pomyślnie", wrapper, "ok");
                }
                else{
                    var response = JSON.parse(xhr.response);
                    popUp(response, wrapper, "error");
                }
            }
        } 
        xhr.open("POST", "https://localhost:7221/api/Auth/register");
        xhr.setRequestHeader("Accept", "application/json");
        xhr.setRequestHeader("Content-Type", "application/json");
        xhr.send(JSON.stringify(user));
    }
    else{
        popUp("Hasła nie zgadzają się", wrapper, "error");
        const pass = document.querySelector("#password");
        const rep = document.querySelector("#repeatPassword");
        pass.classList.add("wrong");
        rep.classList.add("wrong");
        setTimeout(function() {
            pass.value = "";
            rep.value = "";
            pass.classList.remove("wrong");
            rep.classList.remove("wrong");
        }, 3000);
    } 
})
