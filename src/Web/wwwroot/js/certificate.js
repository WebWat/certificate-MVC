﻿"use strict";
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

function start(a) {
    sessionStorage.setItem("current_page", "1");
    document.location.href = location.origin + "/Certificate/Details/" + a;
}

window.onload = () => {
    document.body.classList.add("loaded_hiding");
    window.scrollTo(0, +sessionStorage.getItem("scroll"));
}

window.addEventListener("scroll", () => {
    sessionStorage.setItem("scroll", pageYOffset);
});

let form = document.getElementsByName('search');
document.addEventListener('keypress', (e) => {
    if (e.code === 13)
        form.submit();
})