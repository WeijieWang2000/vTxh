/**
 * ITCAST WEB
 * Created by zhousg on 2016/5/3.
 */
window.onload = function(){
    /*
    * 1.显示弹出层
    * 2.做动画
    * 3.删除盒子需要做
    * 4.点击取消按钮  关闭  弹出层
    * */


    /*获取弹出层*/
    var hmWin = document.querySelector('.hm_win');
    /*获取框*/
    var hmWinBox = hmWin.querySelector('.hm_win_box');
    /*获取所有的删除按钮*/
    var deleteList = document.querySelectorAll('.deleteBox');

    /*记录当前点击的是按个按钮*/
    var deleteBtn = null;

    for(var i = 0 ; i < deleteList.length ; i ++){
        deleteList[i].onclick = function(){
            /*1.显示弹出层*/
            hmWin.style.display = "block";
            /*2.做动画*/
            hmWinBox.classList.add('bounceInDown');
            /*删除盒子需要做*/
            console.log(this);

            deleteBtn = this;
            var up = deleteBtn.querySelector('.up');
            console.log(up);
            /*加过渡*/
            up.style.webkitTransition = "all 1s";
            up.style.transition = "all 1s";
            /*定义旋转原点*/
            up.style.webkitTransformOrigin = "0 5px";
            up.style.transformOrigin = "0 5px";
            /*加改变*/
            up.style.webkitTransform = "rotate(-30deg) translateY(2px)";
            up.style.transform = "rotate(-30deg) translateY(2px)";
        }
    }

    /*4.点击取消按钮  关闭  弹出层*/
    hmWinBox.querySelector('.cancel').onclick = function(){
        hmWin.style.display = "none";
        hmWinBox.classList.remove('bounceInDown');

        /*当前点击过*/
        if(deleteBtn){
            var up = deleteBtn.querySelector('.up');
            up.style.webkitTransform = "none";
            up.style.transform = "none";
        }
    }

};
