function popUp(header, container, type){
    const check = document.querySelector(".info");
    if(check){
      check.remove();
    }
    const popUp = document.createElement("div");
    popUp.classList.add("info");
    const content = document.createElement("p");
    content.textContent = "" + header;
    popUp.appendChild(content);
    container.appendChild(popUp);
    popUp.classList.add("popIn", type);
    popUp.addEventListener("animationend", (e)=>{
      popUp.classList.remove("popIn");
      setTimeout((e)=>{
          popUp.classList.add("popOut");
      }, 3000)
      popUp.addEventListener("animationend", (e)=>{
          popUp.classList.remove("popOut");
          popUp.remove();
      })
    })
  }

  export {popUp};