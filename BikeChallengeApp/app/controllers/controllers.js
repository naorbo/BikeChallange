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

    //SignUp function - register the user with the ASP.NET EF
    $scope.signUp = function () {
        authFactory.register($scope.userName, $scope.password, $scope.regDetails.confirmPassword.$viewValue).then(function () {
            console.log("Signup successfull (EF), BCing success");
            $rootScope.$broadcast(AUTH_EVENTS.registrationSuccessEF);
        }, function () {
            $rootScope.$broadcast(AUTH_EVENTS.registrationFailed);
            console.log("Registration Failure ");
        } 
        )};


    //userRegistration - register the new user with BC DB 
    $scope.userRegistration = function () {
        //authFactory.registerDetails($scope.personalDetails).then(function () {
        //})

        //login and redirection to userpage after successfull registration
        authFactory.login($scope.regDetails.userName, $scope.regDetails.password).then(function () {
            $rootScope.$broadcast(AUTH_EVENTS.loginSuccess);
            console.log("Event BC - user logged ");
        }, function () {
            $rootScope.$broadcast(AUTH_EVENTS.loginFailed);
            console.log("Event BC - bad login ");
            $scope.failFlag = true;
        });

    }

    //Organization Picker ... Just a temp 

    $scope.myOrg = function () {
        console.log($scope.value);
        $('#myModal').modal('hide');
        return;
    }


    $scope.orgs = [
                    { Organizationname: "CEVA", OrganizationCity: 0, OrganizationDes: "john q", OrganizationEmail: "qwerty", OrganizationAddress: "12345qwerty", OrganizationPhone: "john q", OrganizationType: "Group1" },
                    { Organizationname: "Elbit", OrganizationCity: 0, OrganizationDes: "john q", OrganizationEmail: "qwerty", OrganizationAddress: "12345qwerty", OrganizationPhone: "john q", OrganizationType: "Group2" },
                    { Organizationname: "Quartus", OrganizationCity: 0, OrganizationDes: "john q", OrganizationEmail: "qwerty", OrganizationAddress: "12345qwerty", OrganizationPhone: "john q", OrganizationType: "Group3" },
                    { Organizationname: "Verint", OrganizationCity: 0, OrganizationDes: "john q", OrganizationEmail: "qwerty", OrganizationAddress: "12345qwerty", OrganizationPhone: "john q", OrganizationType: "Group4" },
                    { Organizationname: "Microsoft", OrganizationCity: 0, OrganizationDes: "john q", OrganizationEmail: "qwerty", OrganizationAddress: "12345qwerty", OrganizationPhone: "john q", OrganizationType: "Group5" },
                    { Organizationname: "Google", OrganizationCity: 0, OrganizationDes: "john q", OrganizationEmail: "qwerty", OrganizationAddress: "12345qwerty", OrganizationPhone: "john q", OrganizationType: "Group6" },
                    { Organizationname: "Ruppin", OrganizationCity: 0, OrganizationDes: "john q", OrganizationEmail: "qwerty", OrganizationAddress: "12345qwerty", OrganizationPhone: "john q", OrganizationType: "Group7" },
                    { Organizationname: "RedHat", OrganizationCity: 0, OrganizationDes: "john q", OrganizationEmail: "qwerty", OrganizationAddress: "12345qwerty", OrganizationPhone: "john q", OrganizationType: "Group8" }

    ];


    $scope.clearSelection = function () {
        console.log("Im reseting this one - " + $scope.value);
        $scope.personalDetails.team = null;
        return;
    };

    // Event Handlers

    $scope.$on('reg-success-ef', function () {
        $scope.showDetails = true;
        // Add here disable main form of registration 

    });



});




app.controller('mainController', function ($rootScope, $location, $scope,  authFactory, session, AUTH_EVENTS) {
    
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
        session.userId = null;
        session.id = null;
        $location.url("/home");
    });


});




//User Profile - Get Data 

app.controller('userProfileController', function ($rootScope, $location, $scope, valuesFactory) {

    console.log("inside upc");
    $scope.getMyData = function () {
        api = "Values";
        console.log("trying to fetch....");
        myData = valuesFactory.getValues(api);
        $scope.uiData = myData;
   
    };

    $scope.$on('event:auth-loginRequired', function () {
        alert("You are not Authorized to view this page ... Please sign in");
        $location.url("/login")
    });
   
});



//app.controller('signUpController', function ($rootScope, $scope, authFactory, AUTH_EVENTS) {

//    //SignUp function - register the user with the ASP.NET EF
//    $scope.signUp = function () {
//        authFactory.register($scope.regDetails.userName, $scope.regDetails.password, $scope.regDetails.confirmPassword).then(function () {
//            console.log("Signup successfull, redirecting to login");
//            authFactory.login($scope.regDetails.userName, $scope.regDetails.password).then(function () {
//                $rootScope.$broadcast(AUTH_EVENTS.loginSuccess);
//                console.log("Event BC - user logged ");
//            }, function () {
//                $rootScope.$broadcast(AUTH_EVENTS.loginFailed);
//                console.log("Event BC - bad login ");
//                $scope.failFlag = true;
//            });

//        }, function () {
//            alert("Registration Failed");
//        });
//        //console.log(" " + $scope.regDetails.userName + " " + $scope.regDetails.password + " " + $scope.regDetails.confirmPassword);
//    }