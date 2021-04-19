"use strict";
function start(a, b, c) {
    sessionStorage.setItem("isReturn", true);
    document.location.href = location.origin + "/Public/Details/" + b + "/" + a + "?page=" + c;
}

window.onload = () => {
    document.body.classList.add("loaded_hiding");
    if (sessionStorage.getItem("isReturn") === "true") {
        window.scrollTo(0, +sessionStorage.getItem("scroll"));
    }
    sessionStorage.setItem("isReturn", false);
}

window.addEventListener("scroll", () => {
    sessionStorage.setItem("scroll", pageYOffset);
});

let form = document.getElementsByName('search');
document.addEventListener('keypress', (e) => {
    if (e.code === 13)
        form.submit();
})
