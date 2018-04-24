window.zeus = window.zeus || {};
zeus.debug = false;

function injectSVG() {

    // Elements to inject
    var _svgFilesToInject = document.querySelectorAll('img.svgInject');
    SVGInjector(_svgFilesToInject);

}

function bootstrapZeus() {

    zeus.eventManager = new EventManager();

    zeus.interface = function () {

        var _zeusWrapper = document.getElementById('zeusWrapper');
        var _headerHeight = 21;
        var _resolution = {
            current: 'md',
            isMobile: false,
            list: {
                xs: {
                    minWidth: 0
                },
                sm: {
                    minWidth: 768
                },
                md: {
                    minWidth: 992
                },
                lg: {
                    minWidth: 1200
                }
            }
        }

        /**
		 * Bind the Interface Events
		 */
        var _bindEvents = function () {

            $(window).on('resize', function () {
                _setWrapperSize();
                _defineResolution();
            });

        }


        /**
		 * Resize the "zeusWrapper" Div
		 */
        var _setWrapperSize = function () {
            _zeusWrapper.style.height = (window.innerHeight - _headerHeight) + 'px';
        }


        /**
		 * Define the resolution class name in the tag <html>
		 * and store resolution variables in memory
		 */
        var _defineResolution = function () {

            var _viewportWidth = window.innerWidth;
            var _beforeResolutionName = _resolution.current;

            // xs
            if (_viewportWidth >= _resolution.list.xs.minWidth && _viewportWidth < _resolution.list.sm.minWidth) {
                _resolution.current = 'xs';
                _resolution.isMobile = true;
            }
                // sm
            else if (_viewportWidth >= _resolution.list.sm.minWidth && _viewportWidth < _resolution.list.md.minWidth) {
                _resolution.current = 'sm';
                _resolution.isMobile = false;
            }
                // md
            else if (_viewportWidth >= _resolution.list.md.minWidth && _viewportWidth < _resolution.list.lg.minWidth) {
                _resolution.current = 'md';
                _resolution.isMobile = false;
            }
                // lg
            else if (_viewportWidth >= _resolution.list.lg.minWidth) {
                _resolution.current = 'lg';
                _resolution.isMobile = true;
            }
                // others cases
            else {
                _resolution.current = 'md';
                _resolution.isMobile = false;
            }

            // Push resolution class name in html tag
            // If the resolution has changed
            if (_beforeResolutionName != _resolution.current) {
                $('html').removeClass(_beforeResolutionName).addClass(_resolution.current);
            }

            if (zeus.debug) {
                if ($('#resolutionName').length == 0) {
                    $('body').append('<div id="resolutionName"></div>');
                }
                $('#resolutionName').text(_resolution.current);
            }

        }


        /**
		 * Initialize the Zeus Interface
		 */
        var _init = function () {

            // Add the class "zeus" to the <html> tag if not already there
            if (!document.getElementsByTagName('html')[0].classList.contains('zeus')) {
                document.getElementsByTagName('html')[0].classList.add('zeus');
            }

            _bindEvents();
            _setWrapperSize();
            _defineResolution();

        }();


        return {

            resolution: function () {
                return _resolution;
            }

        };


    }();





    zeus.search = function () {

        /*https://www.google.fr/search?q=tet*/

        /**
		 * Bind the Search Events
		 */
        var _bindEvents = function () {

            // Expand search input on mobile view
            $('#zeusMain').on('click', '#launchSearchBt', function () {

                var _searchInput = $('#zeusMain #searchInput');

                if (zeus.interface.resolution().isMobile) {
                    _searchInput.focus();
                }

            });

        }

        var _init = function () {

            _bindEvents();

        }();

    }();





    zeus.menu = function () {

        var _menuIsExpanded = false;

        var _bindEvents = function () {

            // Expand / Collapse menu
            $('#zeusMenu .toggleMenuBt, #zeusHeader .toggleMenuBt').on('click', function () {

                _menuIsExpanded = !_menuIsExpanded;
                $('#zeusMenu').toggleClass('menuExpanded');

                zeus.eventManager.dispatchEvent('toggleMenu');

            });


            // Click on an menu of First Level
            $('#zeusMenu .menuIcon:not(:first)').on('click', function () {

                $('#zeusMenu .selected').removeClass('selected');
                $(this).closest('li').addClass('selected');

            });


            // Open submenu on rollover when menu is NOT expanded
            $('#zeusMenu ul > li').on('mouseenter', function () {
                if (!_menuIsExpanded) {

                    var subTitle = $(this).find('.menuIcon .zeusMenuTitle').text();

                    var subMenu = $(this).children('ul');

                    if (subMenu.find('.zeusMenuTitle').length == 0) {
                        subMenu.prepend('<li class="zeusMenuTitle">' + subTitle + '</li>');
                    }

                    subMenu.addClass('visible');

                }
            });

            // Close submenu on rollover when menu is NOT expanded
            $('#zeusMenu ul > li').on('mouseleave', function () {
                if (!_menuIsExpanded) {

                    var subMenu = $(this).children('ul');
                    subMenu.children('li.zeusMenuTitle').remove();

                    subMenu.removeClass('visible');
                }
            });


        };

        _bindEvents();

        return {}

    }();

}
