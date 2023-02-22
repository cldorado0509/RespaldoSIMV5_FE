var contActivos = 0;;
(function($, window, undefined) {

    'use strict';

    
    var Modernizr = window.Modernizr,
        $body = $('body');

    $.MenuAmva = function(options, element) {
        this.$el = $(element);
        this._init(options);
    };

    
    $.MenuAmva.defaults = {
        
        animationClasses: {
            classin: 'dl-animate-in-1',
            classout: 'dl-animate-out-1'
        },
        
        
        onLevelClick: function(el, name) {
            return false;
        },
        
        
        onLinkClick: function(el, ev) {
            return false;
        }
    };

    $.MenuAmva.prototype = {
        _init: function(options) {

            
            this.options = $.extend(true, {}, $.MenuAmva.defaults, options);
            
            this._config();

            var animEndEventNames = {
                    'WebkitAnimation': 'webkitAnimationEnd',
                    'OAnimation': 'oAnimationEnd',
                    'msAnimation': 'MSAnimationEnd',
                    'animation': 'animationend'
                },
                transEndEventNames = {
                    'WebkitTransition': 'webkitTransitionEnd',
                    'MozTransition': 'transitionend',
                    'OTransition': 'oTransitionEnd',
                    'msTransition': 'MSTransitionEnd',
                    'transition': 'transitionend'
                };
            
            this.animEndEventName = animEndEventNames[Modernizr.prefixed('animation')] + '.MenuAmva';
            
            this.transEndEventName = transEndEventNames[Modernizr.prefixed('transition')] + '.MenuAmva',
                
                this.supportAnimations = Modernizr.cssanimations,
                this.supportTransitions = Modernizr.csstransitions;

            this._initEvents();

        },
        _config: function() {
            this.open = false;
            this.$trigger = this.$el.children('.dl-trigger');
            this.$Todos = this.$el.find('.dl-menu ul');
            this.$menu = this.$el.children('ul.dl-menu');
            this.$menuitems = this.$menu.find('li:not(.dl-back)');
            $(".dl-back").remove()
            this.$el.find('ul.dl-submenu').prepend('<li class="dl-back"><a href="#"></a></li>');
            this.$back = this.$menu.find('li.dl-back');

            var self = this;
            self._openMenu();
            $(".dl-trigger").css("display", "none")
            $(".dl-menu li").hover(function(event) {
                var $dato = event;
                var padre = $dato.currentTarget.parentNode;
                if (padre.id == "menuAMVA") {
                    $dato.currentTarget.click()
                    contActivos = 0;
                    if ($dato.currentTarget.children.length == 1) {
                        $("#menuAMVA").css("height", "50px")
                    } else {
                        $("#menuAMVA").css("height", "100px")
                    }

                }
                $("#menuAMVA").hover(function() {}, function() {
                    try {
                        if (contActivos <= 0) {
                            $("#menuAMVA").css("height", "50px")
                        }
                    } catch (e) {

                    }
                });
                return false;

            }, function(event) {
                var $dato = event;
                var padre = $dato.currentTarget.parentNode;
                if (padre.id == "menuAMVA") {

                }
            })


        },
        _initEvents: function() {

            var self = this;

            this.$trigger.on('click.MenuAmva', function() {

                if (self.open) {
                    self._closeMenu();
                } else {
                    self._openMenu();
                }
                return false;

            });

            this.$menuitems.on('click.MenuAmva', function(event) {

                event.stopPropagation();

                var $item = $(this),
                    $submenu = $item.children('ul.dl-submenu');
                var $padre = $submenu.parent()
                $padre = $padre.parent();

                for (var h = 0; h < self.$Todos.length; h++) {
                    var $itemul = self.$Todos[h]
                    try {
                        if ($padre[0].className == "dl-submenu dl_opend") {
                            $("#menuAMVA").css("height", "100px !important")
                        } else {
                            $itemul.className = "dl-submenu";
                            $itemul.style.opacity = "";
                        }
                    } catch (e) {
                        $itemul.className = "dl-submenu";
                        $itemul.style.opacity = "";
                        $("#menuAMVA").css("height", "50px")

                    }


                }
                if ($submenu.length > 0) {
                    var $pa = $submenu.parent()
                    if ($padre[0].className == "dl-submenu dl_opend") {
                        contActivos++;
                        $submenu[0].style.zIndex = "120";

                        $submenu[0].style.top = "0px";
                        $submenu[0].style.left = "-" + $pa[0].offsetLeft + "px";
                    } else {
                        $submenu[0].style.left = "-" + $pa[0].offsetLeft + "px";
                    }
                    $submenu[0].className = "dl-submenu dl_opend";
 $("#menuAMVA").css("height", "100px !important")

                    return false;

                } else {
                    self.options.onLinkClick($item, event);
                }

            });

            this.$back.on('click.MenuAmva', function(event) {

                var $this = $(this);
                var $submen = $this.parent()
                $submen.removeClass('dl_opend');
                contActivos--;
                if (contActivos < 0) {
                    $("#menuAMVA").css("height", "50px")
                }



                return false;

            });

        },
        closeMenu: function() {
            if (this.open) {
                this._closeMenu();
            }
        },
        _closeMenu: function() {
            var self = this,
                onTransitionEndFn = function() {
                    self.$menu.off(self.transEndEventName);
                    self._resetMenu();
                };

            this.$menu.removeClass('dl-menuopen');
            this.$menu.addClass('dl-menu-toggle');
            this.$trigger.removeClass('dl-active');

            if (this.supportTransitions) {
                this.$menu.on(this.transEndEventName, onTransitionEndFn);
            } else {
                onTransitionEndFn.call();
            }

            this.open = false;
        },
        openMenu: function() {
            if (!this.open) {
                this._openMenu();
            }
        },
        _openMenu: function() {
            var self = this;
            
            $body.off('click').on('click.MenuAmva', function() {
                if (contActivos <= 0) {
                    $("#menuAMVA").css("height", "50px")
                }
            });
            this.$menu.addClass('dl-menuopen dl-menu-toggle').on(this.transEndEventName, function() {
                $(this).removeClass('dl-menu-toggle');
            });
            this.$trigger.addClass('dl-active');
            this.open = true;
        },
        
        _resetMenu: function() {
            this.$menu.removeClass('dl-subview');
            this.$menuitems.removeClass('dl-subview dl-subviewopen');
        }
    };

    var logError = function(message) {
        if (window.console) {
            window.console.error(message);
        }
    };

    $.fn.MenuAmva = function(options) {
        if (typeof options === 'string') {
            var args = Array.prototype.slice.call(arguments, 1);
            this.each(function() {
                var instance = $.data(this, 'MenuAmva');
                if (!instance) {
                    logError("cannot call methods on MenuAmva prior to initialization; " +
                        "attempted to call method '" + options + "'");
                    return;
                }
                if (!$.isFunction(instance[options]) || options.charAt(0) === "_") {
                    logError("no such method '" + options + "' for MenuAmva instance");
                    return;
                }
                instance[options].apply(instance, args);
            });
        } else {
            this.each(function() {
                var instance = $.data(this, 'MenuAmva');
                if (instance) {
                    instance._init();
                } else {
                    instance = $.data(this, 'MenuAmva', new $.MenuAmva(options, this));
                }
            });
        }
        return this;
    };

})(jQuery, window);