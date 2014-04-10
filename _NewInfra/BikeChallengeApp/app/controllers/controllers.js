/// <reference path="../Scripts/angular.js" />



//app.controller('mainController', function ($scope) {
   
//    $scope.$watch(function () { return $location.path(); }, function (newValue, oldValue) {
//        if ($scope.loggedIn == false && newValue != '/login') {
//            $location.path('/login')}
//        else {
//            $location.path('/userProfile')
//        }
//    });
//});

app.controller('registerController', function ($scope) {
   
});

app.controller('aboutController', function ($scope) {

});



app.controller('homeController', function ($scope) {

});

app.controller('userAuthController', function ($scope, authFactory) {
    $scope.signIn = function () {
        authFactory.login($scope.loginDetails.userName, $scope.loginDetails.password);
        //console.log($scope.loginDetails.userName + " " + $scope.loginDetails.password)
    }
});

app.controller('signUpController', function ($scope, authFactory) {
    $scope.signUp = function () {
        authFactory.register($scope.regDetails.userName, $scope.regDetails.password, $scope.regDetails.confirmPassword);
        //console.log(" " + $scope.regDetails.userName + " " + $scope.regDetails.password + " " + $scope.regDetails.confirmPassword);
    }
});

app.controller('userProfileController', function ($scope, authFactory) {

});
