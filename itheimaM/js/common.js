/**
 * ITCAST WEB
 * Created by zhousg on 2016/4/29.
 */
/*定义一个全局变量*/
window.itcast = {};/*存储  我们将要封装的  事件方法*/
/*设置 属性 定义一个  transitionEnd 一个事件绑定方法*/
itcast.transitionEnd  = function(dom,callback){
    /*过渡结束事件的绑定*/
    /*
     * 1.谁需要绑定  transitionEnd
     * 2.需要处理的函数
     * */
    if(dom && typeof dom === 'object'){
        dom.addEventListener('webkitTransitionEnd',function(){
            /* if(callback){
             callback();
             }*/
            callback && callback();
        });
        dom.addEventListener('transitionEnd',function(){
            callback && callback();
        });
    }

};
/*封装tap事件*/
itcast.tap = function(dom,callback){
    /*
    *tap:
     1.响应速度要比click快
     2.不能滑动过
    * */
    if(dom && typeof dom === 'object'){
        /*记录是否滑动过*/
        var isMove = false;
        var startTime = 0;
        dom.addEventListener('touchstart',function(e){
            startTime = Date.now();  /*new Date().getTime();获取时间戳*/
        });
        /*触摸滑动事件*/
        dom.addEventListener('touchmove',function(e){
            isMove = true;
        });
        /*触摸结束事件*/
        dom.addEventListener('touchend',function(e){
            /*
             *tap:
             1.响应速度要比click快  click 300ms 响应时间  150ms
             2.不能滑动过
             * */
            if((Date.now()-startTime) < 150 && !isMove){
                /*在end事件的时候才执行click需要执行的操作  提高的响应*/
                /*onclick = function(e){}*/
                callback && callback(e);
            }

            /*重置*/
            isMove = false;
            startTime = 0;
        })

    }
}