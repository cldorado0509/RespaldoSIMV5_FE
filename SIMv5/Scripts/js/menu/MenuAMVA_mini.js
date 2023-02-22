;
(function($, window, undefined) {

    'use strict';

      var Modernizr = window.Modernizr,
        $body = $('body');

    $.MenuAmvaMini = function(options, element) {
        this.$el = $(element);
        this._init(options);
    };

    
    $.MenuAmvaMini.defaults = {
        animationClasses: {
            classin: 'dlMini-animate-in-1',
            classout: 'dlMini-animate-out-1'
        },
       onLevelClick: function(el, name) {
            return false;
        },
         onLinkClick: function(el, ev) {
            return false;
        }
    };

    $.MenuAmvaMini.prototype = {
        _init: function(options) {

                      this.options = $.extend(true, {}, $.MenuAmvaMini.defaults, options);
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
                      this.animEndEventName = animEndEventNames[Modernizr.prefixed('animation')] + '.MenuAmvaMini';
                      this.transEndEventName = transEndEventNames[Modernizr.prefixed('transition')] + '.MenuAmvaMini',
                              this.supportAnimations = Modernizr.cssanimations,
                this.supportTransitions = Modernizr.csstransitions;

            this._initEvents();

        },
        _config: function() {
            this.open = false;
            this.$trigger = this.$el.children('.dlMini-trigger');
            this.$menu = this.$el.children('ul.dlMini-menu');
            this.$menuitems = this.$menu.find('li:not(.dlMini-back)');
            $(".dlMini-back").remove()
            this.$el.find('ul.dlMini-submenu').prepend('<li class="dlMini-back"><a href="#">Atras</a></li>');
            this.$back = this.$menu.find('li.dlMini-back');
            if (document.body.clientHeight < 300 || document.body.clientWidth < 790 || navigator.platform == 'iPad' || navigator.platform == 'iPhone' || navigator.platform == 'iPod' || navigator.platform == 'Linux armv7l') {
                var self = this;
                $(".dlMini-trigger").css("display", "block")
            }
        },
        _initEvents: function() {

            var self = this;

            this.$trigger.on('click.MenuAmvaMini', function() {

                if (self.open) {
                    self._closeMenu();
                } else {
                    self._openMenu();
                }
                return false;

            });

            this.$menuitems.on('click.MenuAmvaMini', function(event) {

                event.stopPropagation();

                var $item = $(this),
                    $submenu = $item.children('ul.dlMini-submenu');

                if ($submenu.length > 0) {

                    var $flyin = $submenu.clone().css('opacity', 0).insertAfter(self.$menu),
                        onAnimationEndFn = function() {
                            self.$menu.off(self.animEndEventName).removeClass(self.options.animationClasses.classout).addClass('dlMini-subview');
                            $item.addClass('dlMini-subviewopen').parents('.dlMini-subviewopen:first').removeClass('dlMini-subviewopen').addClass('dlMini-subview');
                            $flyin.remove();
                        };

                    setTimeout(function() {
                        $flyin.addClass(self.options.animationClasses.classin);
                        self.$menu.addClass(self.options.animationClasses.classout);
                        if (self.supportAnimations) {
                            self.$menu.on(self.animEndEventName, onAnimationEndFn);
                        } else {
                            onAnimationEndFn.call();
                        }

                        self.options.onLevelClick($item, $item.children('a:first').text());
                    });

                    return false;

                } else {
                    self.options.onLinkClick($item, event);
                }

            });

            this.$back.on('click.MenuAmvaMini', function(event) {

                var $this = $(this),
                    $submenu = $this.parents('ul.dlMini-submenu:first'),
                    $item = $submenu.parent(),

                    $flyin = $submenu.clone().insertAfter(self.$menu);

                var onAnimationEndFn = function() {
                    self.$menu.off(self.animEndEventName).removeClass(self.options.animationClasses.classin);
                    $flyin.remove();
                };

                setTimeout(function() {
                    $flyin.addClass(self.options.animationClasses.classout);
                    self.$menu.addClass(self.options.animationClasses.classin);
                    if (self.supportAnimations) {
                        self.$menu.on(self.animEndEventName, onAnimationEndFn);
                    } else {
                        onAnimationEndFn.call();
                    }

                    $item.removeClass('dlMini-subviewopen');

                    var $subview = $this.parents('.dlMini-subview:first');
                    if ($subview.is('li')) {
                        $subview.addClass('dlMini-subviewopen');
                    }
                    $subview.removeClass('dlMini-subview');
                });

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

            this.$menu.removeClass('dlMini-menuopen');
            this.$menu.addClass('dlMini-menu-toggle');
            this.$trigger.removeClass('dlMini-active');

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
                      $body.off('click').on('click.MenuAmvaMini', function() {
                self._closeMenu();
            });
            this.$menu.addClass('dlMini-menuopen dlMini-menu-toggle').on(this.transEndEventName, function() {
                $(this).removeClass('dlMini-menu-toggle');
            });
            this.$trigger.addClass('dlMini-active');
            this.open = true;
        },
              _resetMenu: function() {
            this.$menu.removeClass('dlMini-subview');
            this.$menuitems.removeClass('dlMini-subview dlMini-subviewopen');
        }
    };

    var logError = function(message) {
        if (window.console) {
            window.console.error(message);
        }
    };

    $.fn.MenuAmvaMini = function(options) {
        if (typeof options === 'string') {
            var args = Array.prototype.slice.call(arguments, 1);
            this.each(function() {
                var instance = $.data(this, 'MenuAmvaMini');
                if (!instance) {
                    logError("cannot call methods on MenuAmvaMini prior to initialization; " +
                        "attempted to call method '" + options + "'");
                    return;
                }
                if (!$.isFunction(instance[options]) || options.charAt(0) === "_") {
                    logError("no such method '" + options + "' for MenuAmvaMini instance");
                    return;
                }
                instance[options].apply(instance, args);
            });
        } else {
            this.each(function() {
                var instance = $.data(this, 'MenuAmvaMini');
                if (instance) {
                    instance._init();
                } else {
                    instance = $.data(this, 'MenuAmvaMini', new $.MenuAmvaMini(options, this));
                }
            });
        }
        return this;
    };

})(jQuery, window);