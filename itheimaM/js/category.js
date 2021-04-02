/**
 * ITCAST WEB
 * Created by zhousg on 2016/5/3.
 */
window.onload = function(){
    leftSwipe();
    itcast.iScroll({
        swipeDom:document.querySelector('.hm_category_right'),
        swipeType:'y',
        swipeDistance:100
    });
};
/*左侧的滑动效果*/
function leftSwipe(){
    /*
    * 1.滑动  touch
    * 2.在一定的区间范围内  滑动  通过控制  滑动定位的区间的实现
    * 3.在一定的区间内 做定位     定位区间
    * 4.点击  滑动到顶部  改变当前的样式   当滑动到底部的时候不需要做定位  tap
    * */

    /*获取dom元素*/
    /*父盒子*/
    var parentBox = document.querySelector('.hm_category_left');
    /*子盒子*/
    var childBox = parentBox.querySelector('ul');

    var parentHeight = parentBox.offsetHeight;
    var childHeight = childBox.offsetHeight;

    /*定位区间*/
    var maxPosition = 0;//最大的定位区间
    var minPosition = parentHeight-childHeight;//最小的定位区间

    /*缓冲的距离*/
    var distance = 150;

    /*滑动区间*/
    var maxSwipe = maxPosition + 150; // 最大滑动区间
    var minSwipe = minPosition - 150; // 最小滑动区间


    /*添加过渡*/
    var addTransition = function () {
        childBox.style.webkitTransition = "all .2s";/*兼容*/
        childBox.style.transition = "all .2s";
    };
    /*删除过渡*/
    var removeTransition = function () {
        childBox.style.webkitTransition = "none";/*兼容*/
        childBox.style.transition = "none";
    };
    /*改变位子*/
    var setTranslateY = function(translateY){
        childBox.style.webkitTransform = "translateY("+translateY+"px)";
        childBox.style.transform = "translateY("+translateY+"px)";
    };

    /*1.滑动  touch*/
    /*参数*/
    var startY = 0;
    var moveY = 0;
    var distanceY = 0;
/*
    var isMove = false;
*/

    /*记录当前定位*/
    var currY = 0;

    childBox.addEventListener('touchstart',function(e){
        startY = e.touches[0].clientY;
    });
    childBox.addEventListener('touchmove',function(e){
        moveY = e.touches[0].clientY;
        distanceY = moveY - startY;

        /*2.在一定的区间范围内  滑动  通过控制  滑动定位的区间的实现*/
        /*我们将要去做定位的位子 要在  滑动区间范围内*/
        if((currY + distanceY) < maxSwipe && (currY + distanceY) > minSwipe){
            /*删除过渡*/
            removeTransition();
            /*做定位*/
            setTranslateY(currY + distanceY);
        }

    });
    /*避免模拟器上的bug问题   事件冒泡机制*/
    window.addEventListener('touchend',function(e){
        /*3.在一定的区间内 做定位     定位区间*/
        /*将要定位的位子 大于  最大定位的时候*/
        if((currY + distanceY) > maxPosition){
            currY = maxPosition;
            /*加过渡*/
            addTransition();
            /*设置位子*/
            setTranslateY(currY);
        }
        /*将要定位的位子 小于  最小定位的时候*/
        else  if ((currY + distanceY) < minPosition){
            currY = minPosition;
            /*加过渡*/
            addTransition();
            /*设置位子*/
            setTranslateY(currY);
        }
        /*正常*/
        else {
            /*设置当前的定位*/
            currY = currY + distanceY;
        }

        /*重置参数*/
        startY = 0;
        moveY = 0;
        distanceY = 0;
    });

    /*4.点击  滑动到顶部  改变当前的样式   当滑动到底部的时候不需要做定位  tap*/
    var lis = childBox.querySelectorAll('li');
    itcast.tap(childBox,function(e){
        /*清除所有里的当前样式*/
        for(var i =0;i<lis.length;i++){
            lis[i].className = " ";
            lis[i].index = i;
        }
        var li = e.target.parentNode;/*当前点击的li*//*触发源*/
        li.className = 'now';
        /*需要知道当前你需要去定位的位子  计算出来*/
        console.log(li.index);

        var translateY = -li.index * 50;/*向上滑动*/

        /*当超过了最小定位区间  不能定位*/
        /*满足定位*/
        if(translateY > minPosition){
            currY = translateY;
            /*加过渡*/
            addTransition();
            /*去做定位*/
            setTranslateY(currY);
        }else{
            currY = minPosition;
            /*加过渡*/
            addTransition();
            /*去做定位*/
            setTranslateY(currY);
        }

    });

}