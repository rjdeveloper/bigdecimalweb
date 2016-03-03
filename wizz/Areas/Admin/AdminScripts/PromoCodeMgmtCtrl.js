'use strict';
wizzApp.controller('promoCodeMgmtCtrl', function (dbService, $scope, $modal, $http, ngTableParams, $locale, $filter, $timeout, $uibModal) {
    // Do something with myService
    var a = dbService.addnums(50);

    function reset() {
        $scope.promoModel = {
            pkPromoId:null,
            promoName: null,
            counts: null,
            discount: null,
            validFrom: null,
            validTo: null,
            description: null,
            isActive:null,
            isDelete:null
        }
        $('#offerToDate').val("");
        $('#offerFromDate').val("");
    }
    reset();
    $scope.error = null;
    $scope.success = null;
    $scope.addPromoCode = function () {
       
        $('#promoCodeModal').modal('show')
    }

    $scope.resetMsg=function()
    {
        $scope.error = null;
        $scope.success = null;
        $scope.submitted = false;
        $scope.resetAll();
        reset();
    }
    $scope.resetAll = function () {
        $('#promoCodeModal').modal('hide')
        reset();
       
    }
    $scope.savePrmoCode = function () {
        $scope.submitted = true;
        if (validPrmoCode()) {
            $scope.submitted = false;
   
            LoaderStart();
            $http.post(siteUrl + '/api/AdminApi/PostPromo', $scope.promoModel).success(function (data) {
                
                if (data.Success) {
                    $scope.success = data.Message;
                    reloadTableData();
                } else {
                    $scope.error = data.Message;
                }
               
                LoaderStop();
                $timeout(function () {
                    $scope.success = null;
                    $scope.error = null;
                }, 3000);
                $scope.resetAll();
            });

        }
      

    }
    $scope.editPromo = function (d) {
        $scope.promoModel ={
            pkPromoId: d.pkPromoId,
            promoName: d.promoName,
            counts: d.counts,
            discount: d.discount,
            validFrom: d.validFrom,
            validTo: d.validTo,
            description: d.description,
            isActive: d.isActive,
            isDelete: d.isDelete
        
        }
        $('#promoCodeModal').modal('show')

    }
    //......Date Picker for offerFromDate........
    $('#offerFromDate').datepicker({
        controlType: 'select',
        timeFormat: 'hh:mm tt',
        changeYear: true,
        yearRange: '-90:+0',
        minDate: new Date(),
        onSelect: function (selected) {
            var from = new Date($(this).datepicker('getDate'));
            $("#offerToDate").datepicker("option", "minDate", selected)
            $('#offerToDate').val("");
            var dt = new Date(selected);
            $scope.promoModel.validFrom = selected;
            dt.setDate(dt.getDate() + 1);
            $("#offerToDate").datepicker("option", "minDate", dt);
        }
    });
    //......Date Picker for offerToDate........
    $('#offerToDate').datepicker({
        controlType: 'select',
        timeFormat: 'hh:mm tt',
        changeYear: true,
        yearRange: '-90:+0',
        minDate: offerDate,
        onSelect: function (selected) {
            var to = new Date($(this).datepicker('getDate'));
            $scope.promoModel.validTo = selected;
        }
    });
    var offerDate = null;
    function validPrmoCode() {
        if ($scope.promoModel.promoName == null || $scope.promoModel.promoName == '' || $scope.promoModel.promoName.length < 4 || $scope.promoModel.promoName == null || $scope.promoModel.validFrom == null || $scope.promoModel.validTo == null || $scope.promoModel.counts == null || $scope.promoModel.discount == null || $scope.promoModel.description == null)
            return false;
        else
            return true
    }
    getPromoCodes();
    function getPromoCodes() {
        LoaderStart();
        $http({
            method: "GET",
            url: siteUrl + '/api/AdminApi/GetPromoCodes'
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
            url: siteUrl + '/api/AdminApi/GetPromoCodes'
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
            page: 1,            // show first page
            count: 10,          // count per page
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
            }//, $scope: { $data: $scope.data }
        });
        LoaderStop();
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
            controller: function ($scope, $modalInstance) {
                if (ON == 2)
                    $scope.sure = 'Are you sure you want to delete this Promo code?';
                else
                    $scope.sure = 'Are you sure you want to change the status of this Promo code?';
                $scope.confirmMe = function (d) {
                    if (d == true) {
                        $modalInstance.close();
                        LoaderStart();
                        $http.post(siteUrl + '/api/AdminApi/PostPromoActions', obj).success(function (data) {
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

