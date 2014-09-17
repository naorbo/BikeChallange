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

var app = angular.module('bChallenge', ['ngRoute', 'angularFileUpload', /*, 'google-maps'*/  'googlechart', 'ui.bootstrap', 'angucomplete']);

//app.run(function (editableOptions) {
//    editableOptions.theme = 'default';
//});

app.config(['$routeProvider',
  function ($routeProvider) {
      $routeProvider.
        when('/userRegistration', {
            templateUrl: 'app/partials/userRegistration.html',
            controller: 'signUpController'
        }).
        
        when('/work', {
            templateUrl: 'app/partials/work.html',
            controller: 'workController'
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

        when('/myTeam', {
            templateUrl: 'app/partials/myTeam.html',
            controller: 'myTeamController'
        }).
        
        when('/dashboard', {
            templateUrl: 'app/partials/dashboard.html',
            controller: 'dashboardController'
        }).
        

        when('/bikeChallenge', {
            templateUrl: 'app/partials/bikeChallenge.html',
            controller: 'bikeChallengeController'
        }).

        when('/sponsers', {
            templateUrl: 'app/partials/sponsers.html',
            controller: 'homeController'
        }).

        when('/contactUs', {
            templateUrl: 'app/partials/contactUs.html',
            controller: 'contactUsController'
        }).

         
        when('/forgotPassword', {
            templateUrl: 'app/partials/forgotPassword.html',
            controller: 'forgotPasswordController'
        }).

        
        when('/updateProfile', { 
            templateUrl: 'app/partials/updateProfile.html',
            controller: 'updateProfileController'
        }).

        
        when('/adminConsole', {
            templateUrl: 'app/partials/adminConsole.html',
            controller: 'adminConsoleController'
        }).

        when('/pastChallenges', {
            templateUrl: 'app/partials/pastChallenges.html',
            controller: 'pastChallengesController'
        }).

        when('/changePassword', {
            templateUrl: 'app/partials/changePassword.html',
            controller: 'changePasswordController'
        }).

        otherwise({
            redirectTo: '/home'
        });
  }]);


app.constant('AUTH_EVENTS', {
    registrationFailed: 'reg-failed',
    registrationSuccess: 'reg-success',
    registrationFailedEF: 'reg-failed-ef',
    registrationSuccessEF: 'reg-success-ef',
    loginSuccess: 'auth-login-success',
    loginFailed: 'auth-login-failed',
    logoutSuccess: 'auth-logout-success',
    sessionTimeout: 'auth-session-timeout',
    notAuthenticated: 'auth-not-authenticated',
    notAuthorized: 'auth-not-authorized'
});

//app.value('serverBaseUrl', 'http://proj.ruppin.ac.il/igroup1/prod/BikeChallenge');
app.value('serverBaseUrl', 'http://localhost:56634');



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








