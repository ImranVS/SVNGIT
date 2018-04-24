function bootstrapNavigator() {

    var menuWidth = 240; // px
    var menuTop = 140; // px
    var animSpeed = 500; // miliseconds
    var menuLevel = ['Home'];
    var menuItems = [];
    var l = $(".items-bounding a");

    function createNextBtn(i) {
        l.eq(i).after("<a class='nextLevel menuClick glyphicon glyphicon-arrow-right'></a>");
    }

    function animateMenu(d) {
        if (d == 'back') var dir = "+=" + menuWidth + "";
        if (d == 'next') var dir = "-=" + menuWidth + "";
        if (d == 'stay') var dir = "-=0";
        $('.items-bounding').animate({ left: dir }, animSpeed).promise().then(function () {
            if (d == 'back') $(".items-bounding .active:last").removeClass('active');
        });
        if (menuLevel.length > 1) {
            if (d == 'back') var dropMenuH = $('ul.active:eq(-2)').height() + menuTop;
            if (d == 'next') var dropMenuH = $('ul.active:last').height() + menuTop;
            if (d == 'stay') var dropMenuH = $('ul.active:last').height() + menuTop;
            $("#zeusContextNavigationText .dropdown-menu .backBtn").css('display', 'block');
            $("#zeusContextNavigationText .dropdown-menu .branchTitle").css('float', 'right');
        } else {
            var dropMenuH = 'auto';
            $("#zeusContextNavigationText .dropdown-menu .backBtn").css('display', 'none');
            $("#zeusContextNavigationText .dropdown-menu .branchTitle").css('float', 'left');
        }
        $('#zeusContextNavigationText .dropdown-menu').animate({
            height: dropMenuH
        }, animSpeed);
    }

    function resetTitle() {
        if (menuLevel.length != 0) $("#zeusContextNavigationText .dropdown-menu .branchTitle").html(menuLevel[menuLevel.length - 1]);
    }

    $('.dropdown').css('display', 'inline-block');

    for (i = 0; i < $(".items-bounding ul").length; i++) {
        $(".items-bounding ul:eq(" + i + ") ul").css('height', '0');
        $(".items-bounding ul:eq(" + i + ")").css('height', $(".items-bounding ul:eq(" + i + ")").outerHeight(true) + 'px');
        $(".items-bounding ul:eq(" + i + ") ul").css('height', 'auto');
    }

    for (i = 0; i < $(".items-bounding a").length; i++) {
        menuItems.push($(".items-bounding a").eq(i).html())
    }

    // SET WIDTH
    $(".items-bounding ul").css('position', 'absolute');
    $(".items-bounding ul").css('width', menuWidth + 'px');
    $(".items-bounding ul li").css('margin-left', menuWidth + 'px');
    $(".items-bounding ul li").css('width', '100%');
    $("#zeusContextNavigationText .dropdown-menu").css('width', menuWidth + 'px');


    // CREATE BUTTONS NEXT
    for (i = 0; i < l.length; i++) {
        if (l.eq(i).parent().find('ul').length) createNextBtn(i);
    }

    // MOVING MENU TO #zeusContextNavigationText .dropdown-menu
    $(".items-bounding").appendTo("#zeusContextNavigationText .dropdown-menu");
    $(".items-bounding ul").css('display', 'none');

    // EVENTS
    $(".nextLevel").click(function () {
        menuLevel.push($(this).prev().text());
        resetTitle();
        $(this).parent().find('ul').eq(0).addClass('active');
        animateMenu('next');
        $(".backBtn").css('display', 'block');

    });

    $(".backBtn").click(function () {
        if (menuLevel.length > 1) {
            menuLevel.pop();
            animateMenu('back');
        }
        resetTitle();
    })

    $("#menuSearch").autocomplete({
        source: menuItems,
        appendTo: "#zeusContextNavigationText .dropdown-menu",
        open: function (event, ui) {
            $('#zeusContextNavigationText .dropdown-menu').css('height', 'auto');
            $(".items-bounding").css('display', 'none');
            $(".controlBar").css('display', 'none');
        },
        close: function () {
            $(".items-bounding").css('display', 'block');
            $(".controlBar").css('display', 'block');
            animateMenu('stay');
        }
    });

    $('html').click(function (e) {
        if ($(e.target).hasClass('menuClick')) $(".dropdown").addClass('stay-open');
        else $(".stay-open").removeClass('stay-open');
    });

}