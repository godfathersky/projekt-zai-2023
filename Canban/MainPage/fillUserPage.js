import { popUp } from '../popUp.js';

import { Task } from './test.js';

const token = localStorage.getItem("JWTToken");

const decodedToken = decodeJWT(token);
const name = decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"];

const xhr = new XMLHttpRequest()
xhr.onreadystatechange = ()=>{
    if(xhr.readyState === 4){
        if(xhr.status === 200){
            var response = JSON.parse(xhr.response);
            const boardName = document.querySelector(".boardName").querySelector("p");
            boardName.textContent = response.board;
            const nameOfSpace = document.querySelector(".nameOfSpace").querySelector("p");
            nameOfSpace.textContent = response.space;
            const workSpaces = document.querySelector(".singleWorkSpace");
            const option = document.createElement("li");
            option.textContent = response.space;
            workSpaces.appendChild(option);
            const userBoards = document.querySelector(".userBoards");
            const itemList = document.createElement("li");
            itemList.textContent = response.board;
            userBoards.appendChild(itemList);
            const container = document.querySelector(".listPanel");
            const addList = document.querySelector(".addList");
            response.listy.forEach(element => {
                const list = new Task();
                list.recreateList(container, element.nazwaLista, element.listaId, addList);
                const taskPlace = document.querySelector(`[listId="${element.listaId}"]`);
                const beforeItem = taskPlace.querySelector(".addTaskWrapper");
                if(element.zadaniaLista != null){
                    element.zadaniaLista.forEach(task=>{
                        list.CreateTask(task.taskName, taskPlace, beforeItem, task.taskId);
                    });
                }
            });
        }
        else{
            var response = JSON.parse(xhr.response);
            const container = document.querySelector(".wrapper");
            popUp(response, container, "error");
        }
    }
}
xhr.open("GET", "https://localhost:7221/api/PrzestrzenieRoboczes/userItems/" + name);
xhr.send();

function decodeJWT(jwt) {
    var base64Url = jwt.split('.')[1];
    var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    var jsonPayload = decodeURIComponent(window.atob(base64).split('').map(function (c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));

    return JSON.parse(jsonPayload);
}