(function () {
    var app = angular.module('picks', []);
    app.controller('DisplayController',['$http', function ($http) {
        var display = this;
        display.pictures = [];
        var page = 1
       $http.get('/picks/displaypicks/{{page}}').success(function (data) {
           display.pictures = data;
       });

    }]);
   
})();