
'use strict';
//var siteUrl = '';
/* Services */


// Demonstrate how to register services
// In this case it is a simple value service.
var sercieApp = angular.module('myApp.services', []);
sercieApp.factory("dbService", function ($http, $modal, $filter, $locale) {

    return {
        //getData: function (method, onSuccess, onError) {
        // //   LoaderStart(); //Loader Stop on your success method
        //    $http.get(method).success(onSuccess).error(onError);
        //},
       
        addnums: function (addNumsd) {
           return addNumsd + 2;
         
        },
        minusnums: function (addNumsd) {
            return addNumsd - 2; 
        },
        getSwitch: function (id, ON) {
            if (ON == 2) {
                obj.type == 1
                var obj = {
                    pkId: id,
                    type: 2

                }
            } else {
                var obj = {
                    pkId: id,
                    type: 0

                }
            }
           
          
            return obj;

        },
        set: function(key, value) {
            $window.localStorage[key] = value;
        },
        get: function(key, defaultValue) {
            return $window.localStorage[key] || defaultValue;
        },
        setObject: function(key, value) {
            $window.localStorage[key] = JSON.stringify(value);
        },
        getObject: function(key) {
            return JSON.parse($window.localStorage[key] || '{}');
        },
        setDate:function(dateId,min,max){
            //......Date Picker for offerFromDate........
            $('#offerFromDate').datepicker({
                controlType: 'select',
                timeFormat: 'hh:mm tt',
                changeYear: true,
                yearRange: '-90:+0',
                onSelect: function (selected) {
                    var from = new Date($(this).datepicker('getDate'));
                    $("#offerToDate").datepicker("option", "minDate", selected)
                    $('#offerToDate').val("");
       
                    $scope.OffersModel.offerFromDate = selected;
                }
            });

            //......Date Picker for offerToDate........
            $('#offerToDate').datepicker({
                controlType: 'select',
                timeFormat: 'hh:mm tt',
                changeYear: true,
                yearRange: '-90:+0',
                onSelect: function (selected) {
                    var to = new Date($(this).datepicker('getDate'));
         
                    $scope.OffersModel.offerToDate = selected;
                }
            });

            return date;
        }
    }
});
