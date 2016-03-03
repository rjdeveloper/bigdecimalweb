'use strict';
wizzApp.controller('userCtrl', function (dbService, $scope, $modal, $http, ngTableParams, $locale, $timeout) {
    reset();
    resetMsg();
    LoaderStop();
    
    function reset() {
        $scope.user = {
            oldPassword: null,
            newPassword: null,
            confirmPassword: null
        };
    }


    function resetMsg() {
        $scope.error = null;
        $scope.success = null;
    }

    function validation() {
        if ($scope.user.oldPassword == null || $scope.user.oldPassword == '') {
            $scope.error = "Please enter old password";
            return false;
        }
        else if ($scope.user.newPassword == null || $scope.user.newPassword == '') {
            $scope.error = "Please enter new password";
            return false;
        }
        else if ($scope.user.confirmPassword == null || $scope.user.confirmPassword == '') {
            $scope.error = "Please enter confirm password";
            return false;
        }
        else {
            return true;
        }
    }
    $scope.submitted = false;
    $scope.saveuser = function () {
        $scope.submitted = true;
        if (validation()) {
            $scope.submitted = false;
            resetMsg();
            LoaderStart();
            $http.post(siteUrl + '/api/AdminApi/ChangePassword', $scope.user).success(function (response) {

                if (response.Success) {
                    $scope.success = "Password Changed Successfully !";
                    LoaderStop();
                    $timeout(function () {
                        $scope.success = null;
                    }, 3000);

                }
                else {

                    $scope.error = "Old password is not valid !";
                    LoaderStop();
                    $timeout(function () {
                        $scope.error = null;
                    }, 3000);
                }
            })
        }
    }



});