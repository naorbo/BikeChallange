/// <reference path="../Scripts/angular.js" />

app.controller('registerController', function ($scope) {
     
    
});

app.controller('aboutController', function ($scope) {


 
});



app.controller('homeController', function ($scope) {



});

app.controller('userAuthController', function ($scope, authFactory) {

    $scope.imHere = function () {
        alert("Im IN");
    }

    $scope.signIn = function () {
        authFactory.login($scope.userName, $scope.password);      
    }

    
    $scope.loginData 

});

app.controller('logInController', function ($scope, $http, $window) {
    $scope.user = { username: 'john.doe', password: 'foobar' };
    $scope.message = '';
    $scope.submit = function () {
        $http
          .post('/authenticate', $scope.user)
          .success(function (data, status, headers, config) {
              $window.sessionStorage.token = data.token;
              $scope.message = 'Welcome';
          })
          .error(function (data, status, headers, config) {
              // Erase the token if the user fails to log in
              delete $window.sessionStorage.token;

              // Handle login errors here
              $scope.message = 'Error: Invalid user or password';
          });
    };
});