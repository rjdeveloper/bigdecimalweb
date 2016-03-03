'use strict';
wizzApp.controller('SettingsCtrl', function (dbService, $scope, $modal, $http, ngTableParams, $locale, $timeout) {
    reset();
    resetMsg();
    getAdminSettings();
    function reset() {
        $scope.Settings = {
            perHourFees: null,
            commission: null,
            perStudentCharge: null
        };
    }


    function resetMsg() {
        $scope.error = null;
        $scope.success = null;
    }

    function validation() {
        if ($scope.Settings.perHourFees == null || $scope.Settings.perHourFees == '') {
            $scope.error = "Please enter fees per hour";
            return false;
        }
        else if ($scope.Settings.commission == null || $scope.Settings.commission == '') {
            $scope.error = "Please enter commission";
            return false;
        }
        else if ($scope.Settings.perStudentCharge == null || $scope.Settings.perStudentCharge == '') {
            $scope.error = "Please enter charges per student";
            return false;
        }
        else if ($scope.Settings.perStudentCharge * $scope.Settings.commission * $scope.Settings.commission == 0) {
            $scope.error = "No amount can have zero or null values";
            return false;
        }
        else {
            return true;
        }
    }

    function getAdminSettings() {
        $http.post(siteUrl + '/api/AdminApi/GetAdminSettings', 'data').success(function (response) {
            LoaderStart();
           
            if (response != null) {
                $scope.Settings.perHourFees = response[0].perHourFees;
                $scope.Settings.commission = response[0].commission;
                $scope.Settings.perStudentCharge = response[0].perStudentCharge;
            }
            LoaderStop();
        })
    }
    $scope.submitted = false;
    $scope.saveSettings = function () {
        $scope.submitted = true;
        if (validation()) {
            $scope.submitted = false;
            resetMsg();
            LoaderStart();
            $http.post(siteUrl + '/api/AdminApi/SaveAdminSettings', $scope.Settings).success(function (response) {
            
                if (response.Success) {
                    $scope.success = "Settings Saved Successfully";
                    LoaderStop();
                    $timeout(function () {
                        $scope.success = null;
                    }, 3000);

                }
                else {

                    $scope.error = "Please check values or try again!!!";
                    LoaderStop();
                    $timeout(function () {
                        $scope.error = null;
                    }, 3000);
                }
            })
        }
    }



});