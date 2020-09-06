"use strict";
let inputFile = document.getElementById('File');
let textLabel = document.getElementById('text');
inputFile.addEventListener('change', function (e) {
    let uploadedFileName = e.target.files[0].name;
    textLabel.innerHTML = uploadedFileName;
})