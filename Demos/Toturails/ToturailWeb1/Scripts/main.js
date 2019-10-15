function login(n) {
    var t = location.href, i = t.indexOf("#");
    return n && i > 0 && (t = t.substr(0, i)), t = t + "#" + n, location.href = "https://passport.cnblogs" + getHostPostfix() + "/login.aspx?ReturnUrl=" + encodeURIComponent(t), !1
}