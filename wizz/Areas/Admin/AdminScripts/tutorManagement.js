'use strict';
wizzApp.controller('tutorManagementCtrl',function(dbService, $scope, $modal, $http, ngTableParams, $locale, $filter, $timeout) {
    // Do something with dbService
    reset();
    getTutors();

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
    $scope.tutorSubjectList = [];

    $scope.payTutor = function (d) {
        $scope.docUrl = d.docUrl;
        $scope.tutorSubjectList = [];
        $scope.userObj = {
            userId: d.pkUserId
        }
        $http({
            method: "POST",
            url: siteUrl + '/api/AdminApi/GetTutorSubjects',
            data: $scope.userObj
        }).success(function (data) {

            $scope.tutorSubjectList = data.Result;

            $('#payModal').modal('show')

        });
    }
    $scope.showTutorDetails = function (d) {
        $scope.docUrl = d.docUrl;
        $scope.tutorSubjectList = [];
        $scope.userObj = {
            userId: d.pkUserId
        }
        $http({
            method: "POST",
            url: siteUrl + '/api/AdminApi/GetTutorSubjects',
            data: $scope.userObj
        }).success(function (data) {

            $scope.tutorSubjectList = data.Result;
          
            $('#tutorModal').modal('show')

        });

    }
    $scope.resetMsg = function () {
        $('#tutorModal').modal('hide')

    }
    $scope.approveSubjects = function () {
        LoaderStart();
        $('#tutorModal').modal('hide')
        $scope.subjectTutor = {
            userId: $scope.userObj.userId,
            tutorSubjectList: $scope.tutorSubjectList
        }
        $http({
            method: "POST",
            url: siteUrl + '/api/AdminApi/PostTutorSubjects',
            data: $scope.subjectTutor
        }).success(function (data) {
            LoaderStop();
            $scope.tutorSubjectList = [];
            $scope.success = "Tutor subjects approved successfully";
            $timeout(function () {
                $scope.success = null;
                $scope.error = null;
            }, 3000);


        });

    }
    $scope.docUrl = null;
    $scope.majorsList = [];
    $scope.classList = [];
    $scope.collegeList = [];

    function getTutors() {
        LoaderStart();

        $http({
            method: "GET",
            url: siteUrl + '/api/AdminApi/GetSettingsForAdmin'
        }).success(function (data) {
        

            $scope.majorsList = data.Result[0].listMajor;
            $scope.classList = data.Result[0].listClass;
            $scope.collegeList = data.Result[0].listColleges;
        });

        $http({
            method: "GET",
            url: siteUrl + '/api/AdminApi/GetTutors'
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
            url: siteUrl + '/api/AdminApi/GetTutors'
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
            },
            $scope: {
                $data: $scope.data
            }
        });
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
        var modalInstance = $modal.open({
            templateUrl: 'confirmBox.html',
            controller: function ($scope, $modalInstance) {
                $scope.sure = 'Are you sure you want to change this?';
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
            }
        })
    }
});