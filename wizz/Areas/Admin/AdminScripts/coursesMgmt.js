'use strict';
wizzApp.controller('courseCtrl', function (dbService, $timeout, $uibModal, $scope, $modal, $http, ngTableParams, $locale, $filter) {
    // Do something with dbService
    reset();
    getCourses();

    function reset() {
        $scope.courseModel = {
            pkCourseId: null,
            courseName: null,
            fkClassId: 0,
            isActive: null,
            isDelete: null
        }
    }
    $scope.clientJson = [{
        id: null,
        name: null
    }]
    $scope.error = null;
    $scope.success = null;
    $scope.addPromoCode = function () {
        $('#promoCodeModal').modal('show')
    }
    $scope.resetAll = function () {
        $('#promoCodeModal').modal('hide')
        reset();
    }
    $scope.saveCourse = function () {
        $scope.submitted = true;
        $http.post(siteUrl + '/api/AdminApi/PostCourse', $scope.courseModel).success(function (data) {
            console.log(data)
            if (data.Success) {
                $scope.success = data.Message;
                $('#promoCodeModal').modal('hide')
                $scope.submitted = false;
                reloadTableData();
            }
        });
    }
    $scope.editPromo = function (d) {

        $scope.courseModel = {
            pkCourseId: d.pkCourseId,
            courseName: d.courseName,
            fkClassId: d.fkClassId,
            isActive: d.isActive,
            isDelete: d.isDelete
        }
        $('#promoCodeModal').modal('show');
    }
    $scope.validPrmoCode = function () {
        if ($scope.courseModel.courseName == null || $scope.courseModel.fkClassId == null)
            return true;
        else
            return false
    }
    $scope.classesList = [];

    function getCourses() {
        LoaderStart();
        $http({
            method: "GET",
            url: siteUrl + '/api/AdminApi/GetCourses'
        }).success(function (data) {
            bindData(data.Result)
            LoaderStop();
        });
        $http({
            method: "GET",
            url: siteUrl + '/api/AdminApi/GetClasses'
        }).success(function (data) {

            $scope.classesList = data.Result;
            LoaderStop();
        });

    }
    //  reloadTableData();
    //----Reload Table Data-----
    function reloadTableData() {
        var obj = {
            searchword: $scope.search
        };
        if (obj.searchword == null || obj.searchword == '') {
            obj.searchword = '';
            //    LoaderStart();
        }
        $http({
            method: "GET",
            url: siteUrl + '/api/AdminApi/GetCourses'
        }).success(function (data) {
            listData = data.Result;
            $scope.tableParams.reload();
            $scope.tableParams.page(1);
            //  LoaderStop();
        });
    }
    //-------ng-Table Code Start here ------------
    var listData = [];

    function bindData(data) {
        listData = data;

        $scope.tableParams = new ngTableParams({
            page: 1, // show first page
            count: 15, // count per page
            sorting: false
        }, {
            total: listData.length, // length of data
            getData: function ($defer, params) {
                // use build-in angular filter
                var filteredData = params.filter() ?
                    $filter('filter')(listData, params.filter()) :
                    listData;
                var orderedData = params.sorting() ?
                    $filter('orderBy')(filteredData, params.orderBy()) :
                    listData;
                params.total(orderedData.length); // set total for recalc pagination
                $defer.resolve(orderedData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
            },
            $scope: {
                $data: $scope.data
            }
        });
    }

    function showMsg(data) {
        LoaderStop();
        $scope.success = data.Message;
        $timeout(function () {
            $scope.success = null;
            $scope.error = null;

        }, 2000);
        if (data.Success)
            reloadTableData();
    }
    $scope.ConfirmBox = function (id, ON) {
        var obj = {
            pkId: id,
            type: ON
        };
        var modalInstance = $uibModal.open({
            templateUrl: 'confirmBox.html',
            animation: false,
            controller: function ($scope, $modalInstance) {
                if (ON == 2)
                    $scope.sure = 'Are you sure you want to delete this Subject?';
                else
                    $scope.sure = 'Are you sure you want to change the status of this Subject?';
                $scope.confirmMe = function (d) {
                    if (d == true) {
                        $modalInstance.close();
                        LoaderStart();
                        $http.post(siteUrl + '/api/AdminApi/PostCourseActions', obj).success(function (data) {
                            showMsg(data)
                        });
                    } else {
                        $modalInstance.dismiss('cancel');
                    }
                }
            },
            size: 'sm'
        })
    }

});