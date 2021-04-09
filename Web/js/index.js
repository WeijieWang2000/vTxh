window.onload = function () {
    search();
    banner();
};

function search() {
    var searchBox = document.querySelector('.hm_header_box');
    var bannerBox = document.querySelector('.hm_banner');
    var h = bannerBox.offserHeight;
    window.onscroll = function () {
        var top = document.body.scrollTop;
        var opacity = 0;
        if (top < h) {
            opacity = top / h * 0.85
        } else {
            opacity = 0.85
        }
        searchBox.style.background = "rgba(201,21,35," + opacity + ")";
    }
}

function banner() {
    var banner = document.querySelector('.hm_banner');
    var w = banner.offsetWidth;
    var imageBox = banner.querySelector('ul:first-child');
    var pointBox = banner.querySelector('ul:last-child');
    var points = pointBox.querySelectorAll('li');
    var addTransition = function () {
        imageBox.style.webkitTransition = "all .2s";
        imageBox.style.transition = "all .2s";
    };
    var removeTransition = function () {
        imageBox.style.webkitTransition = "none";
        imageBox.style.transition = "none";
    };
    var setTranslateX = function (translateX) {
        imageBox.style.webkitTransform = "translateX(" + translateX + "px)";
        imageBox.style.transform = "translateX(" + translateX + "px)";
    };
    var index = 1;
    var timer = setInterval(function () {
        index++;
        addTransition();
        setTranslateX(-index * w);
    }, 4000);
    itcast.transitionEnd(imageBox, function () {
        console.log('transitionEnd');
        if (index >= 9) {
            index = 1;
            removeTransition();
            setTranslateX(-index * w);
        } else if (index <= 0) {
            index = 8;
            removeTransition();
            setTranslateX(-index * w);
        }
        setPoint();
    });
    var setPoint = function () {
        for (var i = 0; i < points.length; i++) {
            points[i].className = " ";
        }
        points[index - 1].className = "now";
    }
    var startX = 0;
    var moveX = 0;
    var distanceX = 0;
    var isMove = false;
    imageBox.addEventListener('touchstart', function (e) {
        clearInterval(timer);
        startX = e.touches[0].clientX;
    });
    imageBox.addEventListener('touchmove', function (e) {
        isMove = true;
        moveX = e.touches[0].clientX;
        distanceX = moveX - startX;
        console.log(distanceX);
        var currX = -index * w + distanceX;
        removeTransition();
        setTranslateX(currX);
    });
    imageBox.addEventListener('touchend', function (e) {

        if (isMove && (Math.abs(distanceX) > w / 3)) {
            if (distanceX > 0) {
                index--;
            } else {
                index++;
            }
            addTransition();
            setTranslateX(-index * w);
        } else {

            addTransition();
            setTranslateX(-index * w);
        }
        startX = 0;
        moveX = 0;
        distanceX = 0;
        isMove = false;
        clearInterval(timer);
        timer = setInterval(function () {
            index++;
            addTransition();
            setTranslateX(-index * w);
        }, 4000);
    });
}
