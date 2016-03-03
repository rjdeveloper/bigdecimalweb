wizzApp.controller('dashbolardCtrl', function ($scope, $http, $modal, $filter, ngTableParams, $q) {
    LoaderStop();
    // getPiechartData();
    //$scope.pielabels = ["Number Of Vendors", "Number Of Customers"];
    $scope.piedata = [];
    $scope.labels = ["January", "February", "March", "April", "May", "June", "July"];
    $scope.series = ['Series A', 'Series B'];
    $scope.data = [
      [65, 59, 80, 81, 56, 55, 40],
      [28, 48, 40, 19, 86, 27, 90]
    ];
    //bar chart
    $scope.barColorData = ['#65CFF6', '#65CFF6', '#65CFF6'];
    $scope.barlabels = ['2006', '2007', '2008'];
    $scope.barseries = ['Bar series A'];

    $scope.bardata = [[65, 59, 80]];
    $scope.pielabels = ["Download Sales", "In-Store Sales", "Mail-Order Sales"];
    $scope.piedata = [300, 500, 100];
    $scope.realTimeEffect = function (idx, d) {
        $scope.piedata[idx] = d;

    }
    $scope.realTimeEffectBar = function (idx, d) {

        $scope.bardata[idx] = d;
    }
    $scope.onClick = function (points, evt) {
        console.log(points, evt);
    };


});