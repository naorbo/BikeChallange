/// <reference path="../Scripts/angular.js" />


// ####################################################################################################################################################### // 
// #########################################                aboutController               ################################################################ // 
// ####################################################################################################################################################### // 



app.controller('aboutController', function ($scope) {

});


// ####################################################################################################################################################### // 
// #########################################                homeController               ################################################################ // 
// ####################################################################################################################################################### // 


app.controller('homeController', function ($scope) {

});


// ####################################################################################################################################################### // 
// #########################################                loginController               ################################################################ // 
// ####################################################################################################################################################### // 



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

// ####################################################################################################################################################### // 
// #########################################                signUpController               ############################################################### // 
// ####################################################################################################################################################### // 



app.controller('signUpController', function ($rootScope, $scope, $http, dataFactory, authFactory, AUTH_EVENTS) {

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

    ///////Organization Handling 

    
    //Save & Close - Modal Window - Passes selected Org from modal windows to attribute 
    $scope.myOrg = function (chosenOrg) {
        console.log("org is " + chosenOrg);
        $scope.personalDetails.org = chosenOrg;
        $('#myModal').modal('hide');
        $scope.getTeamByOrg(chosenOrg);
        return;
    }

    
    //Refresh orgs list from DB
    $scope.refreshOrgs = function () {
        console.log("Inside refresher orgs");
        
        $scope.orgHolder = angular.fromJson(dataFactory.getValues('Organization', false, 0));
        console.log("This is the data :" +  $scope.orgHolder);

    }

    $scope.getOrgs = function () {
        dataFactory.getValues('Organization', false, 0)
             .success(function (values) {
                 $scope.orgs = angular.fromJson(values);
                 console.log($scope.orgs);
                 
               
             })
             .error(function (error) {
                 $scope.status = 'Unable to load Orgs data: ' + error.message;
             });
    }

    //Clears org selection
    $scope.clearSelection = function () {
        console.log("Im reseting this one ");
        $scope.orgSelection = null;
        $scope.personalDetails.org = null;
        return;
    };

    // New Organization Creator 

    $scope.regNewOrg = function (newOrgObj) {
        dataFactory.postValues(newOrgObj,'Organization')
             .success(function (response) {
                 console.log(response);
                 $scope.newOrgFlag = true;
                 alert("  הארגון  " + newOrgObj.OrganizationName + "  נוצר בהצלחה ! ");
                 // Closing Modal window 
                 $scope.personalDetails.org = newOrgObj.OrganizationName;
                 $('#myNewOrgModal').modal('hide');
             })
             .error(function (error) {
                 $scope.status = 'Unable to load Orgs data: ' + error.message;
                 alert("שגיאה ביצירת ארגון חדש");
             });
    }

    $scope.newOrgFlag = false;
    $scope.newTeamFlag = false;

    $scope.flagReset = function () {
        if ($scope.newOrgFlag == false) 
            $scope.newOrgFlag = true;
        else
            $scope.newOrgFlag = false;

    }

    // Team Handlers 

    // Fetch team per Org
    $scope.getTeamByOrg = function (orgName) {
        console.log('trying to get teams')
        dataFactory.getValues('Group', true, 'orgname=' + orgName)
            .success(function (values) {
                $scope.teamPerOrg = angular.fromJson(values);
            }) 
    }

    // Register a new team 

    $scope.regNewTeam = function (newTeamObj, org) {
        newTeamObj.OrganizationsName = org;
        dataFactory.postValues(newTeamObj, 'Group')
             .success(function (response) {
                 console.log(response);
                 newTeamObj.OrganizationsName = $scope.personalDetails.org;
                 $scope.newTeamFlag = true;
                 alert("  הקבוצה  " + newTeamObj.GroupName + "  נוצרה בהצלחה ! ");
                 // Closing Modal window 
                 $scope.personalDetails.team = newTeamObj.GroupName;
                 $('#myNewTeamModal').modal('hide');
                 $scope.getTeamByOrg(newTeamObj.OrganizationsName);
             })
             .error(function (error) {
                 $scope.status = 'Unable to create a new team: ' + error.message;
                 alert("שגיאה ביצירת קבוצה חדשה");
             });
    }

    // Upload image handling 

    $scope.files = [];
    //listen for the file selected event
    $scope.$on("fileSelected", function (event, args) {
        $scope.$apply(function () {
            //add the file object to the scope's files collection
            $scope.files.push(args.file);
        });
    });

    //the save method
    $scope.save = function() {
        $http({
            method: 'POST',
            url: "/Api/PostStuff",
            //IMPORTANT!!! You might think this should be set to 'multipart/form-data' 
            // but this is not true because when we are sending up files the request 
            // needs to include a 'boundary' parameter which identifies the boundary 
            // name between parts in this multi-part request and setting the Content-type 
            // manually will not set this boundary parameter. For whatever reason, 
            // setting the Content-type to 'false' will force the request to automatically
            // populate the headers properly including the boundary parameter.
            headers: { 'Content-Type': false },
            //This method will allow us to change how the data is sent up to the server
            // for which we'll need to encapsulate the model data in 'FormData'
            transformRequest: function (data) {
                var formData = new FormData();
                //need to convert our json object to a string version of json otherwise
                // the browser will do a 'toString()' on the object which will result 
                // in the value '[Object object]' on the server.
                formData.append("model", angular.toJson(data.model));
                //now add all of the assigned files
                for (var i = 0; i < data.files; i++) {
                    //add each file to the form data and iteratively name them
                    formData.append("file" + i, data.files[i]);
                }
                return formData;
            },
            //Create an object that contains the model and files which will be transformed
            // in the above transformRequest method
            data: { model: $scope.model, files: $scope.files }
        }).
        success(function (data, status, headers, config) {
            alert("success!");
        }).
        error(function (data, status, headers, config) {
            alert("failed!");
        });

        
    };



    // Post Test 
    $scope.dataObj = {
        RiderEmail: "tester@bc.com",
        RiderFname: "Tester",
        RiderLname: "Testeron",
        Gender: "זכר",
        RiderAddress: "testing adderess",
        City: "חיפה",
        RiderPhone: "0505550000",
        BicycleType: "חשמליות",
        ImagePath: "pic location",
        BirthDate: "01-01-1985",
        UserName: "tester15",
        Captain: 1,
        Organization: "orgname",
        Group: "groupname"
    };

   
    // Event Handlers

    $scope.$on('reg-success-ef', function () {
        $scope.showDetails = true;
        // Add here disable main form of registration 

    });



});

// ####################################################################################################################################################### // 
// #########################################                mainController               ################################################################ // 
// ####################################################################################################################################################### // 



app.controller('mainController', function ($rootScope, $location, $scope ,authFactory, session, AUTH_EVENTS) {
    
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


// ####################################################################################################################################################### // 
// #########################################               userProfileController               ########################################################### // 
// ####################################################################################################################################################### // 


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