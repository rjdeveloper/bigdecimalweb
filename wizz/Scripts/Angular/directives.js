'use strict';

/* Directives */


var app = angular.module('myApp.directives', []);
var skpatterns = false;
var emailpattern = /^([a-z0-9_\.-]+)@([\da-z\.-]+)\.([a-z\.]{2,6})$/;
var alphapattern = /^([a-zA-Z0-9 _-]+)$/;
var onlynumbers = /^\d*(?:\.\d{1,2})?$/;
var strongpass = /^[a-z0-9_-]{6,12}$/;
var strongpromo = /^[a-z0-9_-]{4,16}$/;
var urlpattern = /^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$/;
var pass = '';
// var patternApp = angular.module("skPatterns", ['']);

app.directive("mywidget", function () {
    return {
        restrict: "E",
        replace: true,
        template: "<p>Hello World</p>"
    };
});
app.directive("taalert", function () {
    return {
        restrict: "AE",
        replace: true,
        template: '<div class="col-md-12" style="text" ng-click="success=null;error=null" ng-show="error || success">' +
            '<span ng-show="error" class="alert alert-danger errorClass" ng-click="error=null" >{{error}} </span>' +
            '<span ng-show="success" class="alert alert-success errorClass" ng-click="success=null" >{{success}}</span>' +
            '</div>'
    };
});
app.directive("nodata", function () {
    return {
        restrict: "AE",
        replace: true,
        template: '<div ng-show="tableParams.data.length==0" style="overflow:auto;padding-top:20px"></div>'
    };
});


//app.directive("tasuccess", function () {
//    return {
//        restrict: "AE",
//        replace: true,
//        template: "<span ng-click='success=null' class='alert alert-success' ng-show='success'>{{success}} X</span>"
//    };
//});
//<span class='alert alert-success' ng-show='success'>{{success}}</span>
//<span class='alert alert-danger' ng-show='success'>{{success}}</span> <span ng-show='success|| error' ng-click='success=null;error=null'>>X</span>
app.directive("datepickerFrom", function () {
    return {
        restrict: "A",
        require: "ngModel",
        link: function (scope, elem, attrs, ngModelCtrl) {
            var updateModel = function (dateText) {
                scope.$apply(function () {
                    ngModelCtrl.$setViewValue(dateText);
                });
            };
            var options = {
                dateFormat: "dd/mm/yy",
                onSelect: function (dateText) {
                    updateModel(dateText);
                }
            };
            elem.datepicker(options);
        }
    }
});
app.directive("datepickerTo", function () {
    return {
        restrict: "A",
        require: "ngModel",
        link: function (scope, elem, attrs, ngModelCtrl) {
            var updateModel = function (dateText) {
                scope.$apply(function () {
                    ngModelCtrl.$setViewValue(dateText);
                });
            };
            var options = {
                dateFormat: "dd/mm/yy",
                onSelect: function (dateText) {
                    updateModel(dateText);
                }
            };
            elem.datepicker(options);
        }
    }
});
app.directive('ngEnter', function () {
    return function (scope, element, attrs) {
        element.bind("keydown keypress", function (event) {
            if (event.which === 13) {
                scope.$apply(function () {
                    scope.$eval(attrs.ngEnter);
                });
                event.preventDefault();
            }
        });
    };
});
app.directive('skemail', function () {
    return {
        replace: false,
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, elem, attr) {
            scope.$watch(attr.ngModel, function (skvalue) {
                if (skvalue != null || skvalue === undefined) {
                    skvalue = skvalue.toLowerCase();
                }

                if (emailpattern.test(skvalue)) {

                    scope.skemail = true;
                } else {
                    scope.skemail = false;


                }
            });
        }
    }
})
app.directive('skalpha', function () {
    return {
        replace: false,
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, elem, attr) {
            scope.$watch(attr.ngModel, function (skvalue) {

                if (alphapattern.test(skvalue) && skvalue != null) {
                    scope.skalpha = true;
                } else {
                    scope.skalpha = false;
                }
            });
        },

    }
})
app.directive('skstrong', function () {
    return {
        replace: false,
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, elem, attr, modelCtrl) {
            scope.$watch(attr.ngModel, function (skvalue) {
                if (skvalue == null || skvalue === 'undefined' || skvalue == "") {
                    scope.skstrong = false;

                } else if (strongpass.test(skvalue)) {
                    pass = skvalue;
                    if (skvalue.length > 12) {
                        skvalue = skvalue.substring(0, skvalue.length - 1);
                        modelCtrl.$setViewValue(skvalue);
                        modelCtrl.$render();
                    }
                    scope.skstrong = true;

                } else {
                    if (skvalue.length > 12) {
                        skvalue = skvalue.substring(0, skvalue.length - 1);
                        modelCtrl.$setViewValue(skvalue);
                        modelCtrl.$render();
                    }
                    scope.skstrong = false;
                    elem.removeClass('alert-success');
                    elem.addClass('alert-danger');
                }
            });
        },

    }
})
app.directive('skconfirm', function () {
    return {
        replace: false,
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, elem, attr, modelCtrl) {
            scope.$watch(attr.ngModel, function (skvalue) {
                if (skvalue == null || skvalue === 'undefined' || skvalue == "") {
                    scope.skconfirm = false;

                } else if (pass == skvalue) {
                    pass = skvalue;
                    if (skvalue.length > 12) {
                        skvalue = skvalue.substring(0, skvalue.length - 1);
                        modelCtrl.$setViewValue(skvalue);
                        modelCtrl.$render();
                    }
                    scope.skconfirm = true;

                } else {
                    if (skvalue.length > 12) {
                        skvalue = skvalue.substring(0, skvalue.length - 1);
                        modelCtrl.$setViewValue(skvalue);
                        modelCtrl.$render();
                    }
                    scope.skconfirm = false;

                }
            });
        },

    }
})
app.directive('sknumber', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attrs, modelCtrl) {
            modelCtrl.$parsers.push(function (skvalue) {
                if (skvalue == undefined) return ''
                var rjvalue = skvalue.replace(/[^0-9]/g, '');
                if (rjvalue != skvalue) {
                    modelCtrl.$setViewValue(rjvalue);
                    modelCtrl.$render();
                }
                return rjvalue;
            });
        }
    };
});//rishabh jain


//abcAbc ^ Xyz                 var rjvalue = skvalue.replace(/[^0-9]/g, '');
app.directive('skonlyalpha', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attrs, modelCtrl) {
            modelCtrl.$parsers.push(function (skvalue) {
                if (skvalue == undefined) return ''
                var rjvalue = skvalue.replace(/[^a-zA-Z]/g, '');
                if (rjvalue != skvalue) {
                    modelCtrl.$setViewValue(rjvalue);
                    modelCtrl.$render();
                }

                return rjvalue;
            });
        }
    };
});
app.directive('mypromo', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attrs, modelCtrl) {
            modelCtrl.$parsers.push(function (skvalue) {
                if (skvalue === undefined || skvalue == null) {
                    scope.strongpromo = false;

                }
                var rjvalue = skvalue.replace(/[^a-zA-Z0-9]/, '');

                if (strongpromo.test(rjvalue)) {
                    scope.strongpromo = true;
                }
                else {
                    scope.strongpromo = false;
                }
                if (rjvalue != skvalue) {                        
                    modelCtrl.$setViewValue(rjvalue);
                    modelCtrl.$render();                    
                }             
                return rjvalue;
            });
        }
    };
});
app.directive('skurl', function () {
    return {
        replace: false,
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, elem, attr) {
            scope.$watch(attr.ngModel, function (skvalue) {
                if (urlpattern.test(skvalue)) {
                    scope.skurl = true;

                } else {
                    scope.skurl = false;

                }
            });
        },
    }
});

app.directive('typeaheadItem', function () {
    return {
        require: '^typeahead',
        link: function (scope, element, attrs, controller) {

            var item = scope.$eval(attrs.typeaheadItem);

            scope.$watch(function () { return controller.isActive(item); }, function (active) {
                if (active) {
                    element.addClass('active');
                } else {
                    element.removeClass('active');
                }
            });

            element.bind('mouseenter', function (e) {
                scope.$apply(function () { controller.activate(item); });
            });

            element.bind('click', function (e) {
                scope.$apply(function () { controller.select(item); });
            });
        }
    };
});
app.directive('slick', function ($timeout) {
    return function (scope, el, attrs) {
        $timeout((function () {
            el.slick({
                arrows: true,
                dots: true,
                infinite: true,
                autoplay: true,
                autoplaySpeed: 6500,
                speed: 1500,
                slidesToShow: 1,
                slidesToScroll: 1,
                fade: true,
                cssEase: 'linear'
            })
        }), 100)
    }
})
app.directive('ngEnter', function () {
    return function (scope, element, attrs) {
        element.bind("keydown keypress", function (event) {
            if (event.which === 13) {
                scope.$apply(function () {
                    scope.$eval(attrs.ngEnter);
                });

                event.preventDefault();
            }
        });
    };
});

