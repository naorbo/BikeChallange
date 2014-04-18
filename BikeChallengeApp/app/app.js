/// <reference path="../Scripts/angular.js" />

/*#######################################################################
  
  /app
      /controllers      
      /directives
      /services
      /partials
      /views

  #######################################################################*/

//This configures the routes and associates each route with a view and a controller

var app = angular.module('bChallenge', ['ngRoute']);

app.config(['$routeProvider',
  function ($routeProvider) {
      $routeProvider.
        when('/userRegistration', {
            templateUrl: 'app/partials/userRegistration.html',
            controller: 'signUpController'
        }).
        when('/about', {
            templateUrl: 'app/partials/about.html',
            controller: 'aboutController'
        }).

        when('/home', {
            templateUrl: 'app/partials/home.html',
            controller: 'homeController'
        }).

        when('/login', {
            templateUrl: 'app/partials/logIn.html',
            controller: 'loginController'
        }).

        when('/userProfile', {
            templateUrl: 'app/partials/userProfile.html',
            controller: 'userProfileController'
        }).

        otherwise({
            redirectTo: '/home'
        });
  }]);


app.constant('AUTH_EVENTS', {
    loginSuccess: 'auth-login-success',
    loginFailed: 'auth-login-failed',
    logoutSuccess: 'auth-logout-success',
    sessionTimeout: 'auth-session-timeout',
    notAuthenticated: 'auth-not-authenticated',
    notAuthorized: 'auth-not-authorized'
});



// Handles 401 Repsonse - Not authorized requests 

app.config(['$httpProvider', function ($httpProvider) {
    $httpProvider.interceptors.push(['$rootScope', '$q', 'httpBuffer', function ($rootScope, $q, httpBuffer) {
        return {

            responseError: function (rejection) {
               
                if (rejection.status === 401 && !rejection.config.ignoreAuthModule) {
                    console.log("401.401");
                    var deferred = $q.defer();
                    httpBuffer.append(rejection.config, deferred);
                    $rootScope.$broadcast('event:auth-loginRequired', rejection);
                    return deferred.promise;
                }
                // otherwise, default behaviour
                return $q.reject(rejection);
            }
        };
    }]);
}]);








