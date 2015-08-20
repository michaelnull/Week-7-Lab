(function () {
    var app = angular.module('picks', []);

    app.controller('DisplayController', ['$http', function ($http) {

        this.page = 1;
        var display = this;

        this.goto = function (whither) {
            this.page = whither;
            $http.get('/picks/displaypicks/' + this.page).success(function (data) {
                display.pictures = data;
            });
        };

        this.goto(this.page);

        this.add = function (pick) {
            display.works = " create function called successfully";
              
            $http.post('/picks/create', { File: pick.file, Description: pick.description, Url: pick.url })
                 .then(function (response) {
                     console.log(response.data);
                     console.log("HEELO THIS WORKED!");
                     $('#createModal').modal('hide');
                     display.pictures.unshift(response.data);

                 });
            
        };
    }]);
    app.controller('CreateController' , ['$http' , function($http){
    }]);
})();