"use strict";

var events = ['Робофест', 'Шаг в будущее']; //TODO: Add events

function autocomplete(inp, arr) {
    let currentFocus;
    inp.addEventListener('input', function (e) {
        let a, b, val = this.value;
        closeAllLists();
        if (!val) { return false; }
        currentFocus = -1;
        a = document.createElement('DIV');
        a.setAttribute('id', this.id + 'autocomplete-list');
        a.setAttribute('class', 'autocomplete-items');
        this.parentNode.appendChild(a);
        for (let i = 0; i < arr.length; i++) {
            if (arr[i].substr(0, val.length).toUpperCase() == val.toUpperCase()) {
                b = document.createElement('DIV');
                if (i + 30 > arr.length) {
                    b.id = 100;
                }
                b.innerHTML = '<strong>' + arr[i].substr(0, val.length) + '</strong>';
                b.innerHTML += arr[i].substr(val.length);
                b.innerHTML += "<input type='hidden' value='" + arr[i] + "'>";
                b.addEventListener('click', function (e) {
                    inp.value = this.getElementsByTagName('input')[0].value;
                    closeAllLists();
                });
                a.appendChild(b);
            }           
        }
    });

    inp.addEventListener('keydown', function (e) {
        let subElem;
        let elem = document.getElementById(this.id + 'autocomplete-list');
        if (elem) subElem = elem.getElementsByTagName('div');

        if (e.keyCode == 40) { //Down
            currentFocus++;
            addActive(subElem);
        } else if (e.keyCode == 38) { //Up
            currentFocus--;
            addActive(subElem);
        } else if (e.keyCode == 13) { //Enter
            e.preventDefault();
            if (currentFocus > -1) {
                if (subElem) subElem[currentFocus].click();
            }
        }    
    });

    function addActive(x) {
        if (!x) return false;
        removeActive(x);
        if (currentFocus >= x.length) currentFocus = 0;
        if (currentFocus < 0) currentFocus = (x.length - 1);
        x[currentFocus].classList.add('autocomplete-active');       
    }

    function removeActive(x) {
        for (let i = 0; i < x.length; i++) {
            x[i].classList.remove('autocomplete-active');
        }
    }

    function closeAllLists(elmnt) {

        let x = document.getElementsByClassName('autocomplete-items');
        for (let i = 0; i < x.length; i++) {
            if (elmnt != x[i] && elmnt != inp) {
                x[i].parentNode.removeChild(x[i]);
            }
        }
    }

    document.addEventListener('click', function (e) {
        closeAllLists(e.target);     
    });
}

function scrollParentToChild(parent, child) { //TODO: Fix the scrolling of the drop-down list 
    let parentRect = parent.getBoundingClientRect();
    let parentViewableArea = {
        height: parent.clientHeight,
        width: parent.clientWidth
    };
    let childRect = child.getBoundingClientRect();
    let isViewable = (childRect.top >= parentRect.top) && (childRect.top <= parentRect.top + parentViewableArea.height);
    if (!isViewable) {
        parent.scrollTop = (childRect.top + parent.scrollTop) - parentRect.top
    }
}

let elem = document.getElementsByClassName('autocom')[0];

autocomplete(elem, events);