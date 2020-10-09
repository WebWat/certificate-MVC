"use strict";
function start(a, b) {
    document.location.href = location.origin + "/Public/Details/" + b + "/" + a;
}

window.onload = () => {
    document.body.classList.add("loaded_hiding");
    window.scrollTo(0, +sessionStorage.getItem("scroll"));
}

window.addEventListener("scroll", () => {
    sessionStorage.setItem("scroll", pageYOffset);
});
