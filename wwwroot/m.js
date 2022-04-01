var o = "o",
    T = true,
    H = 300000,
    Z = 0,
    g = "GET",
    P = "POST",
    D = window.document,
    a = undefined;

function w(x, u, e) {
    if (e === undefined) {
        x.open(g, u, T);
        x.setRequestHeader("a", a);
        x.timeout = H;
        x.send();
    }
    else {
        x.open(P, u, T);
        x.setRequestHeader("a", a);
        x.timeout = H;
        var f = new FormData();
        f.append("f", e);
        x.send(f);
    }
}

function u() {
    m('/l/', 'a');
}

function s() {
    var x = new XMLHttpRequest();
    t(x, function () {
        z();
    });
    w(x, '/y/' + D.getElementsByClassName('s')[Z].innerHTML
        + '?t=' + encodeURIComponent(D.getElementById('x').value));
}

function t(x, handler) {
    x.addEventListener("loadend", handler);
}

function m(u, p) {
    var x = new XMLHttpRequest();
    t(x, function () {
        D.getElementById(p).innerHTML = x.responseText;
    });
    w(x, u);
}

function r(c, l, p, e, q) {
    var x = new XMLHttpRequest();
    t(x, function () {
        n('/o/');
    });
    x.open(P, "/g/", T);
    x.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    x.timeout = H;
    x.send("c=" + encodeURIComponent(c) + "&l=" + encodeURIComponent(l)
        + "&p=" + encodeURIComponent(p)
        + "&e=" + encodeURIComponent(e) + "&n=" + encodeURIComponent(q));
}
function c() {
    var e = D.getElementById('u').value;
    var a = D.getElementById('z').value;
    var p = D.getElementById('q').value;
    var b = D.getElementById('b').value;
    if (e === a && p === b)
        r(D.getElementById('y').value,
            D.getElementById('l').value, p, e,
            D.getElementById('e').value);
    else
        alert('Не совпадает повторный ввод пароля или секретного слова.');
}

function n(u) {
    var x = new XMLHttpRequest();
    t(x, function () {
        if (x.status == 200 && x.DONE)
            D.getElementById(o).innerHTML = x.responseText;
    });
    w(x, u);
}

function v(c, l, p) {
    var x = new XMLHttpRequest();
    t(x, function () {
        a = x.responseText;
        z();
        if (a.length === 0)
            alert('Не удалось войти.');
    });
    w(x, "/a/?c=" + encodeURIComponent(c)
        + "&l=" + encodeURIComponent(l) + "&p=" + encodeURIComponent(p));
}

function d() {
    v(D.getElementById('y').value,
        D.getElementById('l').value, D.getElementById('q').value);
}

function q() {
    m('/n/', 't');
}

function l() {
    var x = new XMLHttpRequest();
    t(x, function () {
        z();
    });
    w(x, '/h/' + D.getElementsByClassName('s')[Z].innerHTML
        + '?t=' + encodeURIComponent(D.getElementsByTagName('input')[Z].value)
        + '&m=' + encodeURIComponent(D.getElementById('x').value));
}
function b() {
    n('/d/1');
}
function f() {
    m('/m/', 'a');
}
function p(e) {
    if (e === undefined)
        n('/q/');
    else {
        var x = new XMLHttpRequest();
        t(x, function () {
            z();
        });
        var i = D.getElementsByTagName('input');
        var d = "";
        var l = i.length - 2;
        var s = '&b=';

        for (var k = 1; k <= l; k += 2)
            if (i[k].checked ^ i[k + 1].checked)
                d = d + s + i[k].checked;

        w(x, '/k/' + e
            + '?t=' + encodeURIComponent(D.getElementById('x').value)
            + d,
            D.getElementById('f').files[Z]);
    }
}
function e() {
    var x = new XMLHttpRequest();
    t(x, function () {
        z();
    });
    w(x, '/b/' + D.getElementsByClassName('s')[Z].innerHTML
        + '?t=' + encodeURIComponent(D.getElementById('x').value));
}
function k() {
    m('/i/', 'd');
}
function i() {
    var x = new XMLHttpRequest();
    t(x, function () {
        z();
    });
    w(x, '/j/?n=' + encodeURIComponent(D.getElementsByTagName('input')[Z].value)
        + '&m=' + encodeURIComponent(D.getElementById('x').value));
}
function j() {
    n('/o/');
}
function h() {
    n('/r/');
}
function z() {
    n('/c/');
}
