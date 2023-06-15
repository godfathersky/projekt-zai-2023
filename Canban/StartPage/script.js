const register = document.querySelector(".register");
const login = document.querySelector(".login");

register.addEventListener("click", ()=>{
    location.href = "/Register/register.html";
})

login.addEventListener("click", (e)=>{
    location.href = "/Login/login.html";
})