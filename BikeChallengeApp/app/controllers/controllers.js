/// <reference path="../Scripts/angular.js" />


app.controller('aboutController', function ($scope) {

});



app.controller('homeController', function ($scope) {

});

app.controller('loginController', function ($rootScope, $scope, authFactory, AUTH_EVENTS) {
    
    $scope.failMSG = "שם משתמש או סיסמא שגויה, נסה שנית"
    $scope.failFlag = false;
    $scope.signIn = function () {
        authFactory.login($scope.loginDetails.userName, $scope.loginDetails.password).then(function () {
            $rootScope.$broadcast(AUTH_EVENTS.loginSuccess);
            console.log("Event BC - user logged ");
        }, function () {
            $rootScope.$broadcast(AUTH_EVENTS.loginFailed);
            console.log("Event BC - bad login ");
            $scope.failFlag = true;
        });
    }
});


app.controller('signUpController', function ($rootScope, $scope, authFactory, AUTH_EVENTS) {
    $scope.signUp = function () {
        authFactory.register($scope.regDetails.userName, $scope.regDetails.password, $scope.regDetails.confirmPassword).then(function () {
            console.log("Signup successfull, redirecting to login");
            authFactory.login($scope.regDetails.userName, $scope.regDetails.password).then(function () {
                $rootScope.$broadcast(AUTH_EVENTS.loginSuccess);
                console.log("Event BC - user logged ");
            }, function () {
                $rootScope.$broadcast(AUTH_EVENTS.loginFailed);
                console.log("Event BC - bad login ");
                $scope.failFlag = true;
            });

        }, function () {
            alert("Registration Failed");
        });
        //console.log(" " + $scope.regDetails.userName + " " + $scope.regDetails.password + " " + $scope.regDetails.confirmPassword);
    }
});

app.controller('userProfileController', function ($scope, authFactory) {

});


app.controller('mainController', function ($rootScope, $location, $scope, authFactory, session, AUTH_EVENTS) {
    
    //User login handling (display user info and redirection)

    $scope.currentUser = null;
    //$scope.currentUser = "MOFO";
    //currentUser.name = null;
    //$scope.userRoles = USER_ROLES;
    
    $scope.$on('auth-login-success', function () {
        $scope.currentUser = session.userId;
        console.log("logged in as : " + $scope.currentUser);
        $location.url("/userProfile");
    });

    $scope.$on('auth-login-failed', function () { console.log("Login Failed") });





    // User Logout handling 
    $scope.signOut = function () {

        
        authFactory.logout().then(function () {
            $rootScope.$broadcast(AUTH_EVENTS.logoutSuccess);
            console.log("Event BC - user logged out ");
        }, function () {
            
            console.log("Event BC - failed to log out ");
            
        });
    }

    $scope.$on('auth-logout-success', function () {
        $scope.currentUser = null;
        $location.url("/home");
    });


});




//User Profile - Get Data 

app.controller('userProfileController', function ($rootScope, $location, $scope, valuesFactory) {

    console.log("inside upc");
    $scope.getMyData = function () {
        console.log("trying to fetch....");
        myData = valuesFactory.getValues();

    };



});
