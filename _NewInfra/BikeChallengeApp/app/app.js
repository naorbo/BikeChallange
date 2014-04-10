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

        when('/logIn', {
            templateUrl: 'app/partials/logIn.html',
            controller: 'userAuthController'
        }).

        when('/userProfile', {
            templateUrl: 'app/partials/userProfile.html',
            controller: 'userProfileController'
        }).

        otherwise({
            redirectTo: '/home'
        });
  }]);









