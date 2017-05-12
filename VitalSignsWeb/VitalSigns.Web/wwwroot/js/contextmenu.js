$(document).ready(function () {


    if ($("#test").addEventListener) {
        $("#test").addEventListener('contextmenu', function (e) {
            alert("You've tried to open context menu"); //here you draw your own menu
            e.preventDefault();
        }, false);
    } else {

        //document.getElementById("test").attachEvent('oncontextmenu', function() {
        //$(".test").bind('contextmenu', function() {
        $('body').on('contextmenu', 'div.aServer', function (t) {


            //alert("contextmenu"+event);
            //alert(t.currentTarget.id);
            document.getElementById("rmenu").style.top = (mouseY(event) - 40) + 'px';
            document.getElementById("rmenu").style.left = (mouseX(event) - 40) + 'px';
            document.getElementById("rmenu").className = "showcustom";
            document.getElementById("rmenu").name = t.currentTarget.id;

            window.event.returnValue = false;


        });
    }

});

// this is from another SO post...  
$(document).bind("click", function (event) {
    var elem = document.getElementById("rmenu");
    if (elem != null){
        elem.className = "hide";
    }
});



function mouseX(evt) {
    if (evt.pageX) {
        return evt.pageX;
    } else if (evt.clientX) {
        return evt.clientX + (document.documentElement.scrollLeft ?
            document.documentElement.scrollLeft :
            document.body.scrollLeft);
    } else {
        return null;
    }
    //return evt.clientX;
}

function mouseY(evt) {
    if (evt.pageY) {
        return evt.pageY;
    } else if (evt.clientY) {
        return evt.clientY + (document.documentElement.scrollTop ?
        document.documentElement.scrollTop :
        document.body.scrollTop);
    } else {
        return null;
    }
    //return evt.clientY;
}