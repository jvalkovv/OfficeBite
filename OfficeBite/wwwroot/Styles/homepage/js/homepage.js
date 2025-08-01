(function () {

    const select = (el, all = false) => {
        el = el.trim()
        if (all) {
            return [...document.querySelectorAll(el)]
        } else {
            return document.querySelector(el)
        }
    }


    window.addEventListener('load', () => {
        AOS.init({
            duration: 2500,
            easing: 'ease-in-out',
            once: true,
            mirror: false
        })
    });

    let preloader = select('#preloader');
    if (preloader) {
        window.addEventListener('load', () => {
            preloader.remove()
        });
    }
})()