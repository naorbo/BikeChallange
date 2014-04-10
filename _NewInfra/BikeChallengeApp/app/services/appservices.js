app.factory('authFactory', function ($http, $q) {

    var service = {};

    service.loginToken = {};

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
                loggedIn = true; 
                deferred.resolve(response);
            })
            .error(function (response) {
                deferred.reject(response);
            });

        return deferred.promise;
    };

    service.logout = function () {
        var deferred = $q.defer();

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