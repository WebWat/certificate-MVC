"use strict";
async function copy() {
    const response = await fetch("/Certificate/Share", {
        method: "GET",
        headers: { "Accept": "application/json" }
    });
    if (response.ok) {
        const text = await response.json();
        window.prompt("URL: ", text);
    }
}

function start(a, b) {
    sessionStorage.setItem("isReturn", true);
    document.location.href = location.origin + "/Certificate/Details/" + a + "?page=" + b;
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