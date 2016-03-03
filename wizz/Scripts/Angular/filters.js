'use strict';

/* Filters */

angular.module('myApp.filters', []).filter('ListFilter', function () {

    // In the return function, we must pass in a single parameter which will be the data we will work on.
    // We have the ability to support multiple other parameters that can be passed into the filter optionally
    return function (input, List) {

        var obj = '';
        angular.forEach(List, function (language) {

            if (language.pkClassId == input) {
      
                obj = language.className;
            }

        })
        return obj;
        // Do filter work here



    }

}).filter('imageFilter', function () {
    return function (input) {
        var obj = '';
        var path = 'http://staging10.techaheadcorp.com/wizz' + '/tutorDocs/'
        if (input == null || input == '')
            return obj;      
        var value = input.substring(0, 4);
        if (value == "http")
            obj = input;
         else
            obj = path.concat(input);
        return obj;
    }
}).filter('majorFilter', function () {
    return function (input, List) {
        var obj = '';
        angular.forEach(List, function (language) {
            if (parseInt(language.majorId) == parseInt(input)) {
                obj = language.majorName;
            }
        })
        return obj;
    }
}).filter('collegeFilter', function () {
    return function (input, List) {
 
        var obj = '';
        angular.forEach(List, function (language) {
            if (parseInt(language.collegeId) == parseInt(input)) {
                obj = language.collegeName;
            }
        })
        return obj;
    }
}).filter('mycollegesfilter', function () {
    return function (input, List) {
    
        var obj = '';
        angular.forEach(List, function (language) {
            if (language.pkCollegeId == input) {
                obj = language.collegeName;
            }
        })
        return obj;
    }
}).filter('classFilter', function () {
    return function (input, List) {
        var obj = '';
        angular.forEach(List, function (language) {
            if (input == language.subjectId) {

                if (input== '13')
                console.log(language)
                obj = language.subjectName;
            }
        })
        return obj;
    }
})




