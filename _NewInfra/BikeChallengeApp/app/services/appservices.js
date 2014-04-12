// Authentication services (signin, signout, signup, logout) 
// ********************************************************* //
app.factory('authFactory', function ($rootScope, $http, $q, session, AUTH_EVENTS) {

    var service = {};
    service.loginToken = {};
    var currentSession = {};

   

    service.getAccessToken = function () {
        return loginToken.access_token;
    };

    service.login = function (userName, password) {
        var deferred = $q.defer();

        var loginData = {
            grant_type: "password",
            username: userName,
            password: password
        };

        $http({
            method: 'POST',
            url: '/Token',
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
            data: $.param(loginData)
        })
            .success(function (response) {
                service.loginToken = response;
                console.log(response)
                session.create(response.access_token, response.userName);
                //service.isAuthenticated();
                //currentSession = session.userId;
                //$scope.testModel = "worksAsWell"
                //$scope.currentUser = response.userName;
                deferred.resolve(response);
            })
            .error(function (response) {
                deferred.reject(response);
            });

        return deferred.promise;
    };

    service.isAuthenticated = function () {
        console.log(!!session.userId)
        return !!session.userId;
    };

  
    service.logout = function () {
        var deferred = $q.defer();
        console.log("out..");
        $http({
            method: 'POST',
            url: '/api/Account/Logout',
            headers: { 'Authorization': 'Bearer ' + this.loginToken.access_token },
        })
            .success(function (response) {
                service.loginToken = [];
                loggedIn = false;
                deferred.resolve(response);
            })
            .error(function (response) {
                deferred.reject(response);
            });

        return deferred.promise;
    };

    service.register = function (userName, password, confirmPassword) {
        var deferred = $q.defer();
        $http({
            method: 'POST',
            url: '/api/Account/Register',
            headers: '{Content-Type: application/json}',
            data: 
                {
                    "UserName": userName,
                    "Password": password,
                    "ConfirmPassword": confirmPassword
                }
                ,
        })
            .success(function (response) {
                service.loginToken = [];
                deferred.resolve(response);
            })
            .error(function (response) {
                deferred.reject(response);
            });

        return deferred.promise;


    }

    return service;
});


// Session handler (userId as username , id as AccessToken) 
// ********************************************************* //


app.service('session', function () {
    this.create = function (sessionId, userId /*, userRole */) {
        this.id = sessionId;
        this.userId = userId;

        console.log("this is your info"+this.id + this.userId)
        // this.userRole = userRole;
    };
    this.destroy = function () {
        this.id = null;
        this.userId = null;
       // this.userRole = null;
    };
    return this;
});


