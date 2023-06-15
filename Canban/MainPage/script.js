import {popUp} from '/popUp.js'
import { Task } from './test.js';

if(localStorage.getItem("JWTToken")){
    const sidePanelArrow = document.querySelector(".sidePanelArrow");
    const listPanel = document.querySelector(".listPanel");
    let container = document.querySelector(".wrapper");
    const addList = document.querySelector(".addList");
    const logout = document.querySelector(".logout");
    let clicked = false;

    sidePanelArrow.addEventListener("click", function(e){
        if(!clicked){
            clicked = true;
            const sidePanel = e.target.closest(".sidePanel");
            e.target.classList.toggle("sidePanelArrowReverse");
            e.target.classList.toggle("sidePanelArrowTransform");
            if(sidePanel.classList.contains("hidden")){
                listPanel.classList.add("moved");
                sidePanel.classList.toggle("slideIn");
                listPanel.classList.toggle("moveRight");
            }
            else{
                listPanel.classList.add("moveToLeft");
                sidePanel.classList.toggle("slideOut");
                listPanel.classList.toggle("moveLeft");
            }
            sidePanel.classList.toggle("hidden");
            listPanel.classList.toggle("toRight");
            sidePanel.addEventListener("animationend", e=>{
                if(sidePanel.classList.contains("hidden")){
                    sidePanel.classList.remove("slideOut");
                    listPanel.classList.remove("moveLeft");
                    listPanel.classList.remove("moveToLeft");
                    listPanel.classList.remove("moved");
                    clicked = false;
                }
                else{
                    listPanel.classList.remove("moved");
                    sidePanel.classList.remove("slideIn");
                    listPanel.classList.remove("moveRight");
                    clicked = false
                }
            })
        }
    })

    addList.addEventListener("click", ()=>{
        const list = new Task();
        list.createPopUp(container);
    })

    logout.addEventListener("click", (e)=>{
        localStorage.removeItem("JWTToken");
        location.href = "/Login/login.html";
    })
}
else{
    location.href = "/Login/login.html";
}
