(function () {
    var app = angular.module('picks', []);

    app.controller('DisplayController',['$http', function ($http) {
        var display = this;
        display.pictures = [];
        
       $http.get('/picks/displaypicks/1').success(function (data) {
           display.pictures = data;
       });
       this.goto = function (whither) {
           this.page = whither;
           $http.get('/picks/displaypicks/'+ this.page).success(function (data) {
               display.pictures = data;
           });
       };
    }]);
    app.controller('CreateController', ['$http', function ($http) {

    }]);
   
})();