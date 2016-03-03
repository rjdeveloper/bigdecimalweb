'use strict';
wizzApp.controller('userManagementCtrl',function(dbService,$uibModal, $scope, $modal, $http, ngTableParams, $locale, $filter, $timeout) {
    // Do something with dbService
    reset();
    getStudents();
    function reset() {
        $scope.userModel = {
            pkUserId: null,
            userName: null,
            userEmail: null,
            profilePic: null,
            credits: null,
            isActive: null,
            isDelete: null,
            isTutor: null,
            isVarified: null,
        }
    }
    $scope.error = null;
    $scope.success = null;
    $scope.addPromoCode = function () {

        $('#promoCodeModal').modal('show')
    } 
    function getStudents() {
        LoaderStart();
        $http({
            method: "GET",
            url: siteUrl + '/api/AdminApi/GetUsers'
        }).success(function (data) {
            bindData(data.Result)
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
            url: siteUrl + '/api/AdminApi/GetUsers'
        }).success(function (data) {
            listData = data.Result;
            $scope.tableParams.reload();
            $scope.tableParams.page(1);
            LoaderStop();
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
            }, $scope: { $data: $scope.data }
        });
    }
    function showMsg(data) {
        console.log(data)
        LoaderStop();
        $scope.success = data.Message;
        $timeout(function () {
            $scope.success = null;
        }, 2000);
        if (data.Success)
            reloadTableData();
    }
    $scope.ConfirmBox = function (id, ON) {
        $scope.animationsEnabled = false;
        var obj = {
            pkId: id,
            type: ON
        }
        //var modalInstance = .open({
        //    animation: $scope.animationsEnabled,
        //    templateUrl: 'myModalContent.html',
        //    controller: 'ModalInstanceCtrl',
           
        //    resolve: {
        //        items: function () {
        //            return $scope.items;
        //        }
        //    }


        var modalInstance = $uibModal.open({
            templateUrl: 'confirmBox.html',
            animation:false,
            controller: function ($scope, $modalInstance) {
                if (ON == 2)
                    $scope.sure = 'Are you sure you want to delete this User?';
                else
                    $scope.sure = 'Are you sure you want to change the status of this User?';
                $scope.confirmMe = function (d) {
                    if (d == true) {
                        $modalInstance.close();
                        LoaderStart();
                        $http.post(siteUrl + '/api/AdminApi/PostUserActions', obj).success(function (data) {
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