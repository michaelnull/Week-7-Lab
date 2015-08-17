(function () {
    var app = angular.module('picks', []);

    app.controller('DisplayController', ['$http', function ($http) {
        var display = this;
        display.pictures = [];

        $http.get('/picks/displaypicks/1').success(function (data) {
            display.pictures = data;
        });
        this.goto = function (whither) {
            this.page = whither;
            $http.get('/picks/displaypicks/' + this.page).success(function (data) {
                display.pictures = data;
            });
        };
    }]);
    app.controller('CreateController', ['$http',function ( $http) {

        this.works = "awaiting data";

        $scope.add = function (pick) {
            console.log(pick);

            $http.post('/picks/create', { File: pick.file, Description: pick.description, Url: pick.url }).
      then(function (response) {
          // this callback will be called asynchronously
          // when the response is available
          display.pictures.unshift(response);
          display.goto(display.page);
          this.works = response + "success";
      },
      function (response) {
          this.works = response + "failure";
      }
      );
        };


    }]);

})();