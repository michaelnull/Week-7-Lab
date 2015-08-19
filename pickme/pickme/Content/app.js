(function () {
    var app = angular.module('picks', []);

    app.controller('DisplayController', ['$http', function ($http) {
        var display = this;
        display.pictures = [];
        display.row1 = [];
        $http.get('/picks/displaypicks/1').success(function (data) {
            display.pictures = data;
            display.row1 = [data[0], data[1], data[2], data[3], data[4], data[5]];
            display.row2 = [data[6], data[7], data[8], data[9], data[10], data[11]];
        });

        this.goto = function (whither) {
            this.page = whither;
            $http.get('/picks/displaypicks/' + this.page).success(function (data) {
                display.row1 = [data[0], data[1], data[2], data[3], data[4], data[5]];
                display.row2 = [data[6], data[7], data[8], data[9], data[10], data[11]];
            });
        };

        this.add = function (pick) {
            display.works = " create function called successfully";
           

            $http.post('/picks/create', { File: pick.file, Description: pick.description, Url: pick.url }).
                then({
                    $http:get('/picks/displaypicks/' + this.page)
                        .success(function () {
                            this.goto(1);
                    })
                });
        };
    }]);
    app.controller('CreateController' , ['$http' , function($http){
    }]);
})();