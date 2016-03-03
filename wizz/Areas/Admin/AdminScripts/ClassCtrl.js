'use strict';
wizzApp.controller('ClassCtrl', function (dbService, $scope, $uibModal, $http, ngTableParams, $locale, $filter, $timeout) {
    resetModel();
    resetMsg();
    getClassesData();
    $scope.ClassList = [];
    $scope.msg = null;
    //To reset Classmodel
    function resetModel() {
        $scope.classModel = {
            pkClassId: null,
            className: null,
            isActive: null,
            isDelete: null,
            createdDate: null,
            updatedDate: null
        };
    }

    // To reset all variables
    $scope.reset = function () {
        resetModel();
        resetMsg();
        $('#classModal').modal('hide')
    }

    //To reset error n success messages
    function resetMsg() {
        $scope.error = null;
        $scope.success = null;
    }

    //To show modal for saving classes
    $scope.addClass = function () {
        $('#classModal').modal('show')
    }

    function getClassesData() {
        LoaderStart();
        $http.get(siteUrl + '/api/AdminApi/GetClassesData').success(function (response) {
            if (response != null) {
                listData = response;
                bindData(response);
                $scope.ClassList = response;
            }
            LoaderStop();
        })
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

    //----Reload Table Data-----
    function reloadTableData() {
        $http({
            method: "GET",
            url: siteUrl + '/api/AdminApi/GetClassesData'
        }).success(function (data) {
            listData = data;
            $scope.tableParams.reload();
            $scope.tableParams.page(1);
            LoaderStop();
        });
    }

    $scope.editClass = function (d) {
        $scope.classModel = {
            pkClassId: d.pkClassId,
            className: d.className,
            isActive: d.isActive,
            isDelete: d.isDelete,
            createdDate: d.createdDate,
            updatedDate: d.updatedDate
        };
        $('#classModal').modal('show');
    }

    //To save classes
    $scope.saveClass = function () {
        LoaderStart();
        resetMsg();
        $http.post(siteUrl + '/api/AdminApi/SaveClass', $scope.classModel).success(function (response) {
            if (response == 1) {
                $scope.reset();
                $scope.success = "Class is saved successfully";
                reloadTableData();
            } else if (response == 2) {
                $scope.reset();
                $scope.success = "Class is updated successfully";
                reloadTableData();
            } else if (response == 3) {
                $scope.reset();
                $scope.error = "Class name already exists";
                LoaderStop();
            } else {
                $scope.reset();
                $scope.error = "Error occured";
                LoaderStop();
            }
            $timeout(function () {
                $scope.success = null;
                $scope.error = null;
            }, 3000);
        })
    }

    function showMsg(data) {
        $scope.success = data.Message;
        $timeout(function () {
            $scope.success = null;
        }, 2000);
        if (data.Success)
            reloadTableData();
    }
    $scope.ConfirmBox = function (id, ON) {
        var obj = {
            pkId: id,
            type: ON
        }
        var modalInstance = $uibModal.open({
            templateUrl: 'confirmBox.html',
            animation: false,
            controller: function ($scope, $uibModalInstance) {
                if (ON == 2)
                    $scope.sure = 'Are you sure you want to delete this?';
                else
                $scope.sure = 'Are you sure you want to change the status?';
                $scope.confirmMe = function (d) {
                    if (d == true) {
                        $uibModalInstance.close();
                        LoaderStart();
                        $http.post(siteUrl + '/api/AdminApi/ClassActions', obj).success(function (data) {
                            showMsg(data)
                        });
                    } else {
                        $uibModalInstance.dismiss('cancel');
                    }
                }
            }
        })
    }
});