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



app.controller('signUpController', function ($rootScope, $scope, $http, $timeout, $upload, dataFactory, authFactory, AUTH_EVENTS) {

    //SignUp function - register the user with the ASP.NET EF
    $scope.signUp = function () {
        console.log("Trying to EF");
        authFactory.register($scope.userName, $scope.password, $scope.regDetails.confirmPassword.$viewValue).then(function () {
            console.log("Signup successfull (EF), BCing success");
            $rootScope.$broadcast(AUTH_EVENTS.registrationSuccessEF);
            $scope.loadCities();
        }, function () {
            $rootScope.$broadcast(AUTH_EVENTS.registrationFailed);
            console.log("Registration Failure ");
        } 
        )};

    
    //userRegistration - register the new user with BC DB 
    $scope.userRegistration = function (userName) {
        //authFactory.registerDetails($scope.personalDetails).then(function () {
        //})
        console.log("this is the user"+$scope.regDetails.userName.$viewValue);
        // Parse all info and adjust to server vars
        var userDetails = {};
        userDetails.RiderEmail = $scope.$$childHead.personalDetails.email;
        userDetails.RiderFname = $scope.$$childHead.personalDetails.firstName;
        userDetails.RiderLname = $scope.$$childHead.personalDetails.lastName;
        userDetails.Gender = $scope.$$childHead.personalDetails.gender;
        userDetails.RiderAddress = $scope.$$childHead.personalDetails.address;
        userDetails.City = $scope.$$childHead.personalDetails.city.CityName;
        userDetails.RiderPhone = $scope.$$childHead.personalDetails.phone;
        userDetails.BicycleType = $scope.$$childHead.personalDetails.bikeType;
        userDetails.ImagePath = $scope.$$childHead.personalDetails.imagePath;
        userDetails.BirthDate = $scope.$$childHead.personalDetails.bDay;
        userDetails.UserName = $scope.regDetails.userName.$viewValue;
        userDetails.Captain = "0"; 
        // Should be added - default KM per Day
            // Captain Flag 
            if ($scope.newOrgFlag || $scope.newTeamFlag) { userDetails.Captain = "1";}

            userDetails.Organization = $scope.$$childHead.personalDetails.org;
            userDetails.Group = $scope.$$childHead.personalDetails.team.GroupName;

        userDetails = angular.toJson(userDetails, true);
        
        // Post to Server
        
        dataFactory.postValues(userDetails,'Rider')
                 .success(function (values) {
                     alert("ההרשמה הסתיימה בהצלחה!");
                     $rootScope.$broadcast(AUTH_EVENTS.registrationSuccess);
                     
                 })
                 .error(function (error) {
                     alert("ההרשמה נכשלה!");
                 });
        }



        
    

    ///////Organization Handling 

    
    //Save & Close - Modal Window - Passes selected Org from modal windows to attribute 
    $scope.myOrg = function (chosenOrg) {
        console.log("org is " + chosenOrg);
        $scope.$$childHead.personalDetails.org = chosenOrg;
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
        $scope.$$childHead.personalDetails.org = null;
        $scope.$$childHead.personalDetails.team = null;
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
                 $scope.$$childHead.personalDetails.org = newOrgObj.OrganizationName;
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
        console.log("This is the Org PAssed" + $scope.$$childHead.personalDetails.org)
        newTeamObj.OrganizationsName = $scope.$$childHead.personalDetails.org;
        dataFactory.postValues(newTeamObj, 'Group')
             .success(function (response) {
                 console.log(response);
                 newTeamObj.OrganizationsName = $scope.$$childHead.personalDetails.org;
                 $scope.newTeamFlag = true;
                 alert("  הקבוצה  " + newTeamObj.GroupName + "  נוצרה בהצלחה ! ");
                 // Closing Modal window 
                 $scope.$$childHead.personalDetails.team = newTeamObj.GroupName;
                 $('#myNewTeamModal').modal('hide');
                 $scope.getTeamByOrg(newTeamObj.OrganizationsName);
             })
             .error(function (error) {
                 $scope.status = 'Unable to create a new team: ' + error.message;
                 alert("שגיאה ביצירת קבוצה חדשה");
             });
    }

    // Upload image handling 


    $scope.upload = [];
    $scope.fileUploadObj = { testString1: "Test string 1", testString2: "Test string 2" };

    $scope.onFileSelect = function ($files) {
        //$files: an array of files selected, each file has name, size, and type.
        for (var i = 0; i < $files.length; i++) {
            var $file = $files[i];
            (function (index) {
                $scope.upload[index] = $upload.upload({
                    url: "/api/UserImage?UserName=" + $scope.userName, // webapi url
                    method: "POST",
                    data: { fileUploadObj: $scope.fileUploadObj },
                    file: $file
                }).progress(function (evt) {
                    // get upload percentage
                    console.log('percent: ' + parseInt(100.0 * evt.loaded / evt.total));
                }).success(function (data, status, headers, config) {
                    // file is uploaded successfully
                    console.log(data);
                    $scope.$$childHead.personalDetails.imagePath = data.returnData;
                }).error(function (data, status, headers, config) {
                    // file failed to upload
                    console.log(data);
                });
            })(i);
        }
    }

    $scope.abortUpload = function (index) {
        $scope.upload[index].abort();
    }



    //Get City list

   


    $scope.loadCities = function () {
        console.log('trying to get teams');
        dataFactory.getValues('Cities', false, 0)
            .success(function (values) {
                $scope.citiesHolder = angular.fromJson(values);
            })
    }

    


    
    



   
    // Event Handlers

    $scope.$on('reg-success-ef', function () {
        $scope.showDetails = true;
        // Add here disable main form of registration 

    })

    $scope.$on('reg-success', function () {
        //login and redirection to userpage after successfull registration
        authFactory.login($scope.regDetails.userName.$viewValue, $scope.regDetails.password.$viewValue).then(function () {
            $rootScope.$broadcast(AUTH_EVENTS.loginSuccess);
            console.log("Event BC - user logged ");
        }, function () {
            $rootScope.$broadcast(AUTH_EVENTS.loginFailed);
            console.log("Event BC - bad login ");
            $scope.failFlag = true;
        })
    })


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




app.controller('myTeamController', function ($rootScope, $scope,dataFactory, authFactory, AUTH_EVENTS) {

    console.log("Inside myTeam View");

    //Fetch team data 
    $scope.teamData = function () {
        // get user team info
    }


});

