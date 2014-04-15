// Authentication services (signin, signout, signup, logout) 
// ********************************************************* //
app.factory('authFactory', function ($rootScope, $http, $q, session, AUTH_EVENTS, httpBuffer) {

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


/**
    * $http interceptor.
    * On 401 response (without 'ignoreAuthModule' option) stores the request
    * and broadcasts 'event:angular-auth-loginRequired'.
    */


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
        console.log("session destroyed" + session.id)
       // this.userRole = null;
    };
    return this;
});



// Http Buffer Used Intranally by the 401 interceptor 

app.factory('httpBuffer', ['$injector', function ($injector) {
    /** Holds all the requests, so they can be re-requested in future. */
    var buffer = [];

    /** Service initialized later because of circular dependency problem. */
    var $http;

    function retryHttpRequest(config, deferred) {
        function successCallback(response) {
            deferred.resolve(response);
        }
        function errorCallback(response) {
            deferred.reject(response);
        }
        $http = $http || $injector.get('$http');
        $http(config).then(successCallback, errorCallback);
    }

    return {
        /**
         * Appends HTTP request configuration object with deferred response attached to buffer.
         */
        append: function (config, deferred) {
            buffer.push({
                config: config,
                deferred: deferred
                
            });
        },

        /**
         * Abandon or reject (if reason provided) all the buffered requests.
         */
        rejectAll: function (reason) {
            if (reason) {
                for (var i = 0; i < buffer.length; ++i) {
                    buffer[i].deferred.reject(reason);
                }
            }
            buffer = [];
        },

        /**
         * Retries all the buffered requests clears the buffer.
         */
        retryAll: function (updater) {
            for (var i = 0; i < buffer.length; ++i) {
                retryHttpRequest(updater(buffer[i].config), buffer[i].deferred);
            }
            buffer = [];
        }
    };
}]);



