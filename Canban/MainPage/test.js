import { popUp } from "../popUp.js";

const addList = document.querySelector(".addList");
const sidePanelArrow = document.querySelector(".sidePanelArrow");

document.addEventListener("keypress", openMenu);

function openMenu(e){
    if(e.key == "m"){
        sidePanelArrow.click();
    }
}

export class Task{

    createPopUp(container){
        document.removeEventListener("keypress", openMenu);
        const popUpContainer = document.createElement("div");
        popUpContainer.classList.add("popUpWrapper");
        const popUp = document.createElement("div");
        popUp.classList.add("popUp");
        const listInput = document.createElement("input");
        listInput.type = "text";
        listInput.placeholder = "wpisz nazwę...";
        const buttonWrapper = document.createElement("div");
        buttonWrapper.classList.add("buttonWrapper");
        const createList = document.createElement("button");
        createList.classList.add("create");
        const cancel = document.createElement("button");
        cancel.classList.add("cancel");
        const acceptIcon = document.createElement("i");
        acceptIcon.classList.add("fa-solid","fa-check", "acceptIcon");;
        const cancelIcon = document.createElement("i");
        cancelIcon.classList.add("fa-solid", "fa-xmark", "cancelIcon");
        createList.appendChild(acceptIcon);
        cancel.appendChild(cancelIcon);
        buttonWrapper.appendChild(createList);
        buttonWrapper.appendChild(cancel);
        popUp.appendChild(listInput);
        popUp.appendChild(buttonWrapper);
        popUpContainer.appendChild(popUp);
        container.appendChild(popUpContainer);

        createList.addEventListener("click", (e)=>{
            document.addEventListener("keypress", openMenu);
            const listName = listInput.value.trim();
            if(listName != ""){
                const listWrapper = document.querySelector(".listPanel");
                this.createList(listWrapper, listName);
                popUpContainer.remove();
            }
        });

        cancel.addEventListener("click", ()=>{
            document.addEventListener("keypress", openMenu);
            popUpContainer.remove();
        })
    }

    recreateList(container, name, id, addList){
        const listWrapper = document.createElement("div");
        listWrapper.classList.add("listWrapper");
        const listHeader = document.createElement("div");
        listHeader.classList.add("listHeader");
        const listName = document.createElement("p");
        listName.textContent = name;
        const deleteButton = document.createElement("button");
        deleteButton.classList.add("delete");
        const icon = document.createElement("i");
        icon.classList.add("fa-solid", "fa-trash-can");
        const addTaskWrapper = document.createElement("div");
        addTaskWrapper.classList.add("addTaskWrapper");
        const addTask = document.createElement("button");
        addTask.classList.add("addTask");
        addTask.textContent = "Dodaj kartę";
        const addIcon = document.createElement("i");
        addIcon.classList.add("fa-solid", "fa-plus", "plus");
        addTask.appendChild(addIcon);
        listHeader.appendChild(listName);
        deleteButton.appendChild(icon);
        listHeader.appendChild(deleteButton);
        listWrapper.appendChild(listHeader);
        addTaskWrapper.appendChild(addTask);
        listWrapper.appendChild(addTaskWrapper);
        listWrapper.setAttribute("listId", id);
        container.insertBefore(listWrapper, addList);

        deleteButton.addEventListener("click", (e)=>{
            const tasks = e.target.closest(".listWrapper").querySelectorAll(".task");
            if(tasks.length == 0){
                const xhr = new XMLHttpRequest();
                xhr.onreadystatechange = ()=>{
                    if(xhr.readyState === 4){
                        if(xhr.status === 200){
                            popUp(xhr.response, container, "ok");
                            listWrapper.remove();
                        }
                    }
                }
                const list = e.target.closest(".listWrapper");
                const id = list.getAttribute("listId");
                const encodedID = encodeURIComponent(id);
                const url = "https://localhost:7221/api/PrzestrzenieRoboczes/deleteList?id=" + encodedID;
                xhr.open("DELETE", url);
                xhr.send();
            }
            else{
                popUp("Nie można usunąć listy z zadaniami", container, "error");
            }
        })

        addTask.addEventListener("click", (event)=>{
            document.removeEventListener("keypress", openMenu);
            const popUpContainer = document.createElement("div");
            popUpContainer.classList.add("popUpWrapper");
            let popUps = document.createElement("div");
            popUps.classList.add("popUp");
            const taskInput = document.createElement("input");
            taskInput.type = "text";
            taskInput.placeholder = "wpisz nazwę...";
            const buttonWrapper = document.createElement("div");
            buttonWrapper.classList.add("buttonWrapper");
            const createTask = document.createElement("button");
            createTask.classList.add("create");
            const cancel = document.createElement("button");
            cancel.classList.add("cancel");
            const acceptIcon = document.createElement("i");
            acceptIcon.classList.add("fa-solid","fa-check", "acceptIcon");;
            const cancelIcon = document.createElement("i");
            cancelIcon.classList.add("fa-solid", "fa-xmark", "cancelIcon");
            createTask.appendChild(acceptIcon);
            cancel.appendChild(cancelIcon);
            buttonWrapper.appendChild(createTask);
            buttonWrapper.appendChild(cancel);
            popUps.appendChild(taskInput);
            popUps.appendChild(buttonWrapper);
            popUpContainer.appendChild(popUps);
            container.appendChild(popUpContainer);
            
            cancel.addEventListener("click", ()=>{
                document.addEventListener("keypress", openMenu);
                popUpContainer.remove();
            })

            createTask.addEventListener("click", (e)=>{
                document.addEventListener("keypress", openMenu);
                if(taskInput.value.trim() != ""){
                    const task = document.createElement("div");
                    task.classList.add("task");
                    const taskContent = document.createElement("p");
                    taskContent.textContent = taskInput.value.trim();
                    const remove = document.createElement("button");
                    remove.classList.add("removeTask");
                    const removeIcon = document.createElement("i");
                    removeIcon.classList.add("fa-solid", "fa-xmark");
                    const taskPlace = event.target.closest(".listWrapper");
                    remove.appendChild(removeIcon);
                    task.appendChild(taskContent);
                    task.appendChild(remove);
                    popUpContainer.remove();

                    let data = {
                        user: "",
                        idLista: "",
                        nazwaZadanie: "",
                        dataUtworzenia: ""
                    }

                    const token = localStorage.getItem("JWTToken");
                    const decodedToken = decodeJWT(token);
                    const userName = decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"];
                    const list = event.target.closest(".listWrapper");
                    const listId = list.getAttribute("listId");

                    const dateTime = new Date();

                    data.user = userName;
                    data.idLista = listId;
                    data.nazwaZadanie = taskInput.value.trim();
                    data.dataUtworzenia = dateTime.toISOString().slice(0,10);

                    var container = document.querySelector(".wrapper");

                    const xhr = new XMLHttpRequest();
                    xhr.onreadystatechange = ()=>{
                         if(xhr.readyState === 4){
                            if(xhr.status === 200){
                                taskPlace.insertBefore(task, addTaskWrapper);
                                const getId = new XMLHttpRequest();
                                getId.onreadystatechange = ()=>{
                                    if(getId.readyState === 4){
                                        if(getId.status === 200){
                                            const taskId = getId.response;
                                            task.setAttribute("taskId", taskId);
                                            popUp("Zadanie zostało dodane", container, "ok");
                                        }
                                    }
                                }
                                const urlLastId = "https://localhost:7221/api/PrzestrzenieRoboczes/getLastId";
                                getId.open("GET", urlLastId);
                                getId.send();
                            }
                            else{
                                popUp("Nie udało się dodać zadania", container, "error");
                            }
                         }
                    }
                    const url = "https://localhost:7221/api/PrzestrzenieRoboczes/listTask";
                    xhr.open("POST", url);
                    xhr.setRequestHeader("Accept", "application/json");
                    xhr.setRequestHeader("Content-Type", "application/json");
                    xhr.send(JSON.stringify(data));
                }
            })

        })

    }

    CreateTask(taskName, taskPlace, beforeItem, taskId){
        const task = document.createElement("div");
        task.classList.add("task");
        task.setAttribute("taskId", taskId);
        const taskContent = document.createElement("p");
        taskContent.textContent = taskName;
        const remove = document.createElement("button");
        remove.classList.add("removeTask");
        const removeIcon = document.createElement("i");
        removeIcon.classList.add("fa-solid", "fa-xmark");
        remove.appendChild(removeIcon);
        task.appendChild(taskContent);
        task.appendChild(remove);
        taskPlace.insertBefore(task, beforeItem);

        remove.addEventListener("click", (e)=>{
            let item = {
                username: "",
                taskId: 0
            }
            const token = localStorage.getItem("JWTToken");
            const decodedToken = decodeJWT(token);
            const userName = decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"];
            item.username = userName;
            item.taskId = e.target.closest(".task").getAttribute("taskId");
            const container = document.querySelector(".wrapper");
            const xhr = new XMLHttpRequest();
            xhr.onreadystatechange = ()=>{
                if(xhr.readyState === 4){
                    if(xhr.status === 200){
                        e.target.closest(".task").remove();
                        popUp("Zadanie pomyślnie usunięte", container, "ok");
                    }
                    else{
                        popUp("Nie udało się usunąć zadania", container, "error");
                    }
                }
            }
            const url = "https://localhost:7221/api/PrzestrzenieRoboczes/DeleteTask";
            xhr.open("DELETE", url);
            xhr.setRequestHeader("Content-Type", "application/json");
            xhr.send(JSON.stringify(item));
        })
    }

    createList(container, name){
        const listWrapper = document.createElement("div");
        listWrapper.classList.add("listWrapper");
        const listHeader = document.createElement("div");
        listHeader.classList.add("listHeader");
        const listName = document.createElement("p");
        listName.textContent = name;
        const deleteButton = document.createElement("button");
        deleteButton.classList.add("delete");
        const icon = document.createElement("i");
        icon.classList.add("fa-solid", "fa-trash-can");
        const addTaskWrapper = document.createElement("div");
        addTaskWrapper.classList.add("addTaskWrapper");
        const addTask = document.createElement("button");
        addTask.classList.add("addTask");
        addTask.textContent = "Dodaj kartę";
        const addIcon = document.createElement("i");
        addIcon.classList.add("fa-solid", "fa-plus", "plus");
        addTask.appendChild(addIcon);
        listHeader.appendChild(listName);
        deleteButton.appendChild(icon);
        listHeader.appendChild(deleteButton);
        listWrapper.appendChild(listHeader);
        addTaskWrapper.appendChild(addTask);
        listWrapper.appendChild(addTaskWrapper);

        const boardName = document.querySelector(".boardName").querySelector("p").textContent;

        let task = {
            nazwaTablicaZadan: "",
            nazwaListaZadan: "",
            dataUtworzeniaListaZadan: ""
        }

        task.nazwaTablicaZadan = boardName;
        task.nazwaListaZadan = name;

        const dateTime = new Date();
        task.dataUtworzeniaListaZadan = dateTime.toISOString().slice(0,10);

        const xhr = new XMLHttpRequest();
        xhr.onreadystatechange = ()=>{
            if(xhr.readyState === 4){
                if(xhr.status === 200){
                    var response = JSON.parse(xhr.response);
                    popUp(response.response, container, "ok");
                    listWrapper.setAttribute("listId", response.id);
                    container.insertBefore(listWrapper, addList);
                }
                else{

                }
            }
        }
        const url = "https://localhost:7221/api/PrzestrzenieRoboczes/list";
        xhr.open("POST", url);
        xhr.setRequestHeader("Accept", "application/json");
        xhr.setRequestHeader("Content-Type", "application/json");
        xhr.send(JSON.stringify(task));


        deleteButton.addEventListener("click", (e)=>{
            const tasks = e.target.closest(".listWrapper").querySelectorAll(".task");
            if(tasks.length == 0){
                const xhr = new XMLHttpRequest();
                xhr.onreadystatechange = ()=>{
                    if(xhr.readyState === 4){
                        if(xhr.status === 200){
                            popUp(xhr.response, container, "ok");
                            listWrapper.remove();
                        }
                    }
                }
                const list = e.target.closest(".listWrapper");
                const id = list.getAttribute("listId");
                const encodedID = encodeURIComponent(id);
                const url = "https://localhost:7221/api/PrzestrzenieRoboczes/deleteList?id=" + encodedID;
                xhr.open("DELETE", url);
                xhr.send();
            }
            else{
                popUp("Nie można usunąć listy z zadaniami", container, "error");
            }
        })

        addTask.addEventListener("click", (event)=>{
            document.removeEventListener("keypress", openMenu);
            const popUpContainer = document.createElement("div");
            popUpContainer.classList.add("popUpWrapper");
            let popUps = document.createElement("div");
            popUps.classList.add("popUp");
            const taskInput = document.createElement("input");
            taskInput.type = "text";
            taskInput.placeholder = "wpisz nazwę...";
            const buttonWrapper = document.createElement("div");
            buttonWrapper.classList.add("buttonWrapper");
            const createTask = document.createElement("button");
            createTask.classList.add("create");
            const cancel = document.createElement("button");
            cancel.classList.add("cancel");
            const acceptIcon = document.createElement("i");
            acceptIcon.classList.add("fa-solid","fa-check", "acceptIcon");;
            const cancelIcon = document.createElement("i");
            cancelIcon.classList.add("fa-solid", "fa-xmark", "cancelIcon");
            createTask.appendChild(acceptIcon);
            cancel.appendChild(cancelIcon);
            buttonWrapper.appendChild(createTask);
            buttonWrapper.appendChild(cancel);
            popUps.appendChild(taskInput);
            popUps.appendChild(buttonWrapper);
            popUpContainer.appendChild(popUps);
            container.appendChild(popUpContainer);
            
            cancel.addEventListener("click", ()=>{
                document.addEventListener("keypress", openMenu);
                popUpContainer.remove();
            })

            createTask.addEventListener("click", (e)=>{
                document.addEventListener("keypress", openMenu);
                if(taskInput.value.trim() != ""){
                    const task = document.createElement("div");
                    task.classList.add("task");
                    const taskContent = document.createElement("p");
                    taskContent.textContent = taskInput.value.trim();
                    const remove = document.createElement("button");
                    remove.classList.add("removeTask");
                    const removeIcon = document.createElement("i");
                    removeIcon.classList.add("fa-solid", "fa-xmark");
                    const taskPlace = event.target.closest(".listWrapper");
                    remove.appendChild(removeIcon);
                    task.appendChild(taskContent);
                    task.appendChild(remove);
                    popUpContainer.remove();

                    let data = {
                        user: "",
                        idLista: "",
                        nazwaZadanie: "",
                        dataUtworzenia: ""
                    }

                    const token = localStorage.getItem("JWTToken");
                    const decodedToken = decodeJWT(token);
                    const userName = decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"];
                    const list = event.target.closest(".listWrapper");
                    const listId = list.getAttribute("listId");

                    const dateTime = new Date();

                    data.user = userName;
                    data.idLista = listId;
                    data.nazwaZadanie = taskInput.value.trim();
                    data.dataUtworzenia = dateTime.toISOString().slice(0,10);

                    var container = document.querySelector(".wrapper");

                    const xhr = new XMLHttpRequest();
                    xhr.onreadystatechange = ()=>{
                         if(xhr.readyState === 4){
                            if(xhr.status === 200){
                                taskPlace.insertBefore(task, addTaskWrapper);
                                remove.addEventListener("click", (e)=>{
                                    let item = {
                                        username: "",
                                        taskId: 0
                                    }
                                    const token = localStorage.getItem("JWTToken");
                                    const decodedToken = decodeJWT(token);
                                    const userName = decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"];
                                    item.username = userName;
                                    item.taskId = e.target.closest(".task").getAttribute("taskId");
                                    const xhr = new XMLHttpRequest();
                                    xhr.onreadystatechange = ()=>{
                                        if(xhr.readyState === 4){
                                            if(xhr.status === 200){
                                                const container = document.querySelector(".wrapper");
                                                e.target.closest(".task").remove();
                                                popUp("Zadanie pomyślnie usunięte", container, "ok");
                                            }
                                        }
                                        else{
                                            popUp("Nie udało się usunąć zadania", container, "error");
                                        }
                                    }
                                    const url = "https://localhost:7221/api/PrzestrzenieRoboczes/DeleteTask";
                                    xhr.open("DELETE", url);
                                    xhr.setRequestHeader("Content-Type", "application/json");
                                    xhr.send(JSON.stringify(item));
                                })

                                const getId = new XMLHttpRequest();
                                getId.onreadystatechange = ()=>{
                                    if(getId.readyState === 4){
                                        if(getId.status === 200){
                                            const taskId = getId.response;
                                            task.setAttribute("taskId", taskId);
                                            popUp("Zadanie zostało dodane", container, "ok");
                                        }
                                    }
                                }
                                const urlLastId = "https://localhost:7221/api/PrzestrzenieRoboczes/getLastId";
                                getId.open("GET", urlLastId);
                                getId.send();
                            }
                            else{
                                popUp("Nie udało się dodać zadania", container, "error");
                            }
                         }
                    }
                    const url = "https://localhost:7221/api/PrzestrzenieRoboczes/listTask";
                    xhr.open("POST", url);
                    xhr.setRequestHeader("Accept", "application/json");
                    xhr.setRequestHeader("Content-Type", "application/json");
                    xhr.send(JSON.stringify(data));
                }
            })

        })

    }
}

function decodeJWT(jwt) {
    var base64Url = jwt.split('.')[1];
    var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    var jsonPayload = decodeURIComponent(window.atob(base64).split('').map(function (c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));

    return JSON.parse(jsonPayload);
}
