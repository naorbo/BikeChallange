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
            controller: 'registerController'
        }).
        when('/about', {
            templateUrl: 'app/partials/about.html',
            controller: 'aboutController'
        }).
        otherwise({
            redirectTo: '/about'
        });
  }]);









