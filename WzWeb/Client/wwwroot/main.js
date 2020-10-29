window.getBrowserConfig = function () {
    var cfg = {
        innerWidth: window.innerWidth,
        innerHeight: window.innerHeight,
        outerHeight: window.outerHeight,
        outerWidth: window.outerWidth
    }
    return JSON.stringify(cfg);
}