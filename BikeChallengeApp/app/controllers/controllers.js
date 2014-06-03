/// <reference path="../Scripts/angular.js" />

// ####################################################################################################################################################### // 
// #########################################                workAreaController               ################################################################ // 
// ####################################################################################################################################################### // 



app.controller('workAreaController', function ($rootScope, $scope, dataFactory, AUTH_EVENTS) {
    
   // api/Ranking?grpname=secondgroup&orgname=ebay&gender=&order=Points 

     //$scope.getRanks = function () {
     //    dataFactory.getValues('Ranking', true, goupName +"&orgname="+ orgName + "&gender=&orderPoints")
     //         .success(function (values) {
     //             $scope.orgs = angular.fromJson(values);
     //             console.log($scope.orgs);
                 
               
     //         })
     //         .error(function (error) {
     //             $scope.status = 'Unable to load Orgs data: ' + error.message;
     //         });

    // Update User's Info - DO NOT DELETE // 


    $scope.editorEnabled = false;
    $scope.toggleEditor = function () {
        $scope.editorEnabled = !$scope.editorEnabled;
    }
    $scope.updatedRecords = {
        UserEmail: "",
        UserAddress: "",
        RiderCity: "",
        BirthDate: "",
        UserPhone: "",
    }


    $scope.personalInfoHolder = {
        UserEmail: "A@b.com",
        UserAddress: "test adrs",
        RiderCity: "test city",
        BirthDate: "2014-05-10",
        UserPhone: "098878899",
    }
        
    $scope.save = function () {
        var userDetails = {
            RiderEmail:"",
            RiderFname:"",
            RiderLname:"",
            Gender  :"",
            RiderAddress:"",
            City:"",
            RiderPhone:"",
            BicycleType:"",
            ImagePath:"",
            BirthDate: "",     
            UserName:"",
            Captain:"",
            Organization:"",
            Group: "",

    }
        dataFactory.updateValues('Rider', userDetails, false)
                     .success(function (values) {
                         if (angular.fromJson(values) == "Error")
                         { alert(" בדוק את הפרטים שהזנת ונסה בשנית!"); }
                         else
                         {
                             dataFactory.getValues('Rider', true, "username=" + $scope.currentUser)
                                .success(function (values) {
                                    $scope.personalInfoHolder = values[0];
                                    $rootScope.userPersonalInfo = values[0];
                                    console.log("Fetch user info for " + $scope.currentUser);
                                    console.log($scope.personalInfoHolder);
                                }
                                .error(function () {alert('error')}));
                             //$rootScope.$broadcast(AUTH_EVENTS.registrationSuccess);
                         }

                     })
                     .error(function (error) {
                         alert("העדכון נכשל");
                     });

    }
  
    // PUT api/Rider?username=[UserName you want to update]
    //{"RiderEmail":"Rider Email", "RiderFname":"Updated val" , "RiderLname":"Updated val", "Gender": "M/F", "RiderAddress":"Updated val" ,  "City":"Updated val", "RiderPhone":"Updated val",  "BicycleType": "Updated val" , "ImagePath":"Updated val" , "BirthDate":"Updated val", "UserName":"username of the updated rider", "Captain":1, "Organization":"Updated val", "Group":"Updated val"}

});





// ####################################################################################################################################################### // 
// #########################################                bikeChallengeController               ################################################################ // 
// ####################################################################################################################################################### // 



app.controller('bikeChallengeController', function ($scope) {
    $scope.oneAtATime = true;

   
    $scope.addItem = function () {
        var newItemNo = $scope.items.length + 1;
        $scope.items.push('Item ' + newItemNo);
    };

    $scope.status = {
        isFirstOpen: true,
        isFirstDisabled: false
    };
});



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
        authFactory.register($scope.regDetails.userName.$viewValue, $scope.regDetails.password.$viewValue, $scope.regDetails.confirmPassword.$viewValue).then(function () {
            console.log("Signup successfull (EF), BCing success");
            $rootScope.$broadcast(AUTH_EVENTS.registrationSuccessEF);
            $scope.loadCities();
        }, function () {
            $rootScope.$broadcast(AUTH_EVENTS.registrationFailed);
            console.log("Registration Failure ");
        } 
        )};

    
    //userRegistration - register the new user with BC DB 

    $scope.userRegistration = function (personalDetails) {
        
        if (personalDetails.org == undefined || personalDetails.org == null) { alert("לא נבחרה קבוצה, בחר קבוצה ונסה שנית")}
        else {

            var userDetails = {};

            userDetails.RiderEmail = personalDetails.email.$viewValue;
            userDetails.RiderFname = personalDetails.firstName.$viewValue;
            userDetails.RiderLname = personalDetails.lastName.$viewValue;
            userDetails.Gender = personalDetails.gender.$viewValue;
            userDetails.RiderAddress = personalDetails.address.$viewValue;
            userDetails.City = personalDetails.city.$viewValue.CityName;
            userDetails.RiderPhone = personalDetails.phone.$viewValue;
            userDetails.BicycleType = personalDetails.bikeType.$viewValue;
            if (personalDetails.imagePath == undefined)
            { userDetails.ImagePath = "\\ProfileImages\\Users\\defaultUser\\defaultUserImage.jpg" }
            else
            { userDetails.ImagePath = personalDetails.imagePath; }
            userDetails.BirthDate = personalDetails.bDay.$viewValue;
            userDetails.UserName = $scope.regDetails.userName.$viewValue;

            // Captain Flag 
            if ($scope.newOrgFlag || $scope.newTeamFlag) { userDetails.Captain = "1"; }
            else { userDetails.Captain = "0"; };

            userDetails.Organization = personalDetails.org;
            userDetails.Group = personalDetails.team.$viewValue.GroupName;

            userDetails = angular.toJson(userDetails, true);



            // Post to Server

            dataFactory.postValues('Rider', userDetails, false)
                     .success(function (values) {
                         if (angular.fromJson(values) == "Error")
                         { alert(" בדוק את הפרטים שהזנת ונסה בשנית ,ההרשמה נכשלה!"); }
                         else
                         {
                             alert("ההרשמה הסתיימה בהצלחה!");
                             $rootScope.$broadcast(AUTH_EVENTS.registrationSuccess);
                         }

                     })
                     .error(function (error) {
                         alert("ההרשמה נכשלה!");
                     });

        }
        }

    // Organization Handling 

    // Save & Close - Modal Window - Passes selected Org from modal windows to attribute 
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
        $scope.$$childHead.personalDetails.org = null;
        $scope.$$childHead.personalDetails.team = null;
        return;
    };

    // New Organization Creator 

    $scope.regNewOrg = function (newOrgObj) {
        
        if (newOrgObj.imagePath == undefined) {
            newOrgObj.imagePath = "\ProfileImages\Users\defaultUser\defaultUserImage.jpg";
        }
        
        var newOrg = {
            OrganizationName: newOrgObj.OrganizationName,
            OrganizationCity: newOrgObj.OrganizationCity.CityName,
            //OrganizationDes: newOrgObj.OrganizationDes,
            OrganizationType: newOrgObj.OrganizationType,
            OrganizationImage: newOrgObj.imagePath
            
        };
   
        dataFactory.postValues('Organization',newOrg,false)
             .success(function (response) {
                 console.log(response);
                 $scope.newOrgFlag = true;
                 alert("  הארגון  " + newOrg.OrganizationName + "  נוצר בהצלחה ! ");
                 // Closing Modal window 
                 $scope.$$childHead.personalDetails.org = newOrg.OrganizationName;
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
        newTeamObj.OrganizationName = $scope.$$childHead.personalDetails.org;
        dataFactory.postValues('Group',newTeamObj, false)
             .success(function (response) {
                 console.log(response);
                 newTeamObj.OrganizationName = $scope.$$childHead.personalDetails.org;
                 $scope.newTeamFlag = true;
                 alert("  הקבוצה  " + newTeamObj.GroupName + "  נוצרה בהצלחה ! ");
                 // Closing Modal window 
                 //$scope.$$childHead.personalDetails.team = newTeamObj;
                 //$scope.$$childHead.personalDetails.team = newTeamObj.GroupName;
                 $('#myNewTeamModal').modal('hide');
                 $scope.getTeamByOrg(newTeamObj.OrganizationName);
             })
             .error(function (error) {
                 $scope.status = 'Unable to create a new team: ' + error.message;
                 alert("שגיאה ביצירת קבוצה חדשה");
             });
    }

    // Upload image handling for user profile 


    $scope.upload = [];
    $scope.fileUploadObj = { testString1: "Test string 1", testString2: "Test string 2" };

    $scope.onFileSelect = function ($files) {
        //$files: an array of files selected, each file has name, size, and type.
        for (var i = 0; i < $files.length; i++) {
            var $file = $files[i];
            (function (index) {
                $scope.upload[index] = $upload.upload({
                    url: "/api/UserImage?UserName=" + $scope.regDetails.userName.$viewValue, // webapi url
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


    // Upload image handling for Org profile 

    // api/OrganizationImage?OrgName=[orgname]
    $scope.upload = [];
    $scope.fileUploadObj = { testString1: "Test string 1", testString2: "Test string 2" };

    $scope.onFileSelectOrg = function ($files) {
        //$files: an array of files selected, each file has name, size, and type.
        for (var i = 0; i < $files.length; i++) {
            var $file = $files[i];
            (function (index) {
                $scope.upload[index] = $upload.upload({
                    url: "/api/OrganizationImage?OrgName=" + $scope.newOrg.OrganizationName, // webapi url
                    method: "POST",
                    data: { fileUploadObj: $scope.fileUploadObj },
                    file: $file
                }).progress(function (evt) {
                    // get upload percentage
                    console.log('percent: ' + parseInt(100.0 * evt.loaded / evt.total));
                }).success(function (data, status, headers, config) {
                    // file is uploaded successfully
                    console.log(data);
                    $scope.newOrg.imagePath = data.returnData;
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

    $scope.$on('update-org'), function () {
        $scope.personalDetails.org = $scope.orgSelection;
        console.log($scope.personalDetails.org);
    }
    
});

// ####################################################################################################################################################### // 
// #########################################                mainController               ################################################################ // 
// ####################################################################################################################################################### // 



app.controller('mainController', function ($rootScope, $location, $scope ,authFactory,dataFactory, session, AUTH_EVENTS) {
    

   

    //User login handling (display user info and redirection)

        $scope.currentUser = null;
        $scope.$on('auth-login-success', function () {
            $scope.currentUser = session.userId;
            dataFactory.getValues('Rider', true, "username=" + $scope.currentUser)
                    .success(function (values) {
                        $scope.personalInfoHolder = values[0];
                        $rootScope.userPersonalInfo = values[0];
                        console.log("Fetch user info for " + $scope.currentUser);
                        console.log($scope.personalInfoHolder);

                        dataFactory.getValues('Group', true, "grpname=" + $rootScope.userPersonalInfo.GroupName + "&orgname=" + $rootScope.userPersonalInfo.OrganizationName)
                            .success(function (values) {
                                $scope.userGroup = values[0];
                                
                                dataFactory.getValues('Organization', true, "orgname=" + $rootScope.userPersonalInfo.OrganizationName)
                                    .success(function (values) {
                                        $scope.userOrg = values[0];
                                        $location.url("/userProfile");
                                    })
                                    .error(function (value) {
                                        console.log("error");

                                        $location.url("/userProfile");
                                    })
                            .error(function (value) {
                                console.log("error");
                            });
                            })
                    .error(function (value) {
                        console.log("error");
                    });       
                    } )});

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

app.controller('userProfileController', function ($rootScope, $location, $scope, $timeout, $http, dataFactory) {
    
    console.log($scope.currentUser);
    $scope.userGroupInfo = $rootScope.userGroup;
    
    $scope.userPersonalInfo = $rootScope.userPersonalInfo;
    $rootScope.userStats = [
        {
            personal: {},
            group: {},
            organization: {}
        }
    ];

    $rootScope.ranks = [
        {
            group: {},
            organization: {}
        }
    ];
    var getGroup = function () {
        dataFactory.getValues('Group', true, "grpname=" + $rootScope.userPersonalInfo.GroupName + "&orgname=" + $rootScope.userPersonalInfo.OrganizationName)
                .success(function (values) {
                    return  values;
                })
                .error(function (value) {
                    console.log("error");
                });
    };

    dataFactory.getValues('Rider', true, "username=" + $scope.currentUser)
                .success(function (values) {
                    $scope.personalInfoHolder = values[0];
                    $rootScope.userPersonalInfo = values[0];
                    $scope.myGroup = getGroup();
                    console.log("Fetch user info for " + $scope.currentUser);
                    console.log($scope.personalInfoHolder);
                })
                .error(function (value) {
                    console.log("error");
                });
    
    dataFactory.getValues('Rides', true, "username=" + $scope.currentUser)
                        .success(function (values) {
                            $rootScope.userHistory = values;
                            console.log($rootScope.userHistory);
                        })
                        .error(function (value) {
                            console.log("error");
                        });

    dataFactory.getValues('Stat', true, "grpname=" + $rootScope.userPersonalInfo.GroupName + "&orgname=" + $rootScope.userPersonalInfo.OrganizationName)
                        .success(function (values) {
                            $rootScope.userStats.group = values;
                            
                        })
                        .error(function (value) {
                            console.log("error");
                        });

    dataFactory.getValues('Stat', true, "username=" + $scope.currentUser)
                        .success(function (values) {
                            $rootScope.userStats.personal = values;

                        })
                        .error(function (value) {
                            console.log("error");
                        });
    dataFactory.getValues('Stat', true, "orgname=" + $rootScope.userPersonalInfo.OrganizationName)
                        .success(function (values) {
                            $rootScope.userStats.organization = values;
                        })
                        .error(function (value) {
                            console.log("error");
                        });
    
    dataFactory.getValues('Ranking', true, "grpname=" + $scope.userPersonalInfo.GroupDes + "&orgname=" + $scope.userPersonalInfo.OrganizationDes + "&gender=&order=Points")
                 .success(function (values) {
                     $rootScope.ranks.group = angular.fromJson(values);

                 })
                 .error(function (error) {
                     console.log("error");
                 });
    


    


    $scope.$on('event:auth-loginRequired', function () {
        alert("You are not Authorized to view this page ... Please sign in");
        $location.url("/login")
    });

    
  
});


// ####################################################################################################################################################### // 
// #########################################               myTeamController               ########################################################### // 
// ####################################################################################################################################################### // 

app.controller('myTeamController', function ($rootScope, $scope,dataFactory, authFactory, AUTH_EVENTS) {

    console.log("Inside myTeam View");
    //Fetch team data 
    $scope.userDetails = $rootScope.userPersonalInfo;
    $scope.myGroupName = $scope.userDetails.GroupName;
    dataFactory.getValues('Rider', true, "grpname=" + $scope.userDetails.GroupName + "&orgname=" + $scope.userDetails.OrganizationName)
                .success(function (values) {
                    $scope.teamData = values;
                })
                .error(function (value) {
                    console.log("error");
                });
});

// ####################################################################################################################################################### // 
// #########################################               dashboardController               ########################################################### // 
// ####################################################################################################################################################### // 


app.controller('dashboardController', function ($rootScope, $scope, dataFactory, AUTH_EVENTS) {
    console.log("Inside dashboard View");
      
    
    $scope.refreshCal = false;

    todayVar = new Date(); // Handle active month - initial cal 
    $scope.calMonth = todayVar.getMonth(); // Handle active month - initial cal 
    $scope.calYear = todayVar.getFullYear(); // Handle active month - initial cal 

    $scope.calDates = []; // Used for refreshing the cal after changes 
    
    
    $scope.addRideFlag = false; 
    $scope.routeFlag = false;
   
    $scope.flipRideFlag = function () {
        $scope.addRideFlag = !($scope.addRideFlag);
    };

    // User Stats Holder
    $scope.userStats = [
        {
            personal: {},
            group: {},
            organization: {}
        }
    ];

    // Group / Organization ranking holder 
    $scope.ranks = [
        {
            group: {},
            organization: {}
        }
    ];

    // CAL Navigation Buttons 

    $scope.calNabButtons = function (arg) {
        if (arg == -1) {
            $scope.calMonth = $scope.calMonth - 1;
            $scope.refreshCal = !$scope.refreshCal;
        }
        else if (arg == 1) {
            $scope.calMonth = $scope.calMonth + 1;
            $scope.refreshCal = !$scope.refreshCal;
        }

        else {

            var tmp = new Date();
            $scope.calMonth = tmp.getMonth();
            $scope.refreshCal = !$scope.refreshCal;

        }

    }
    


    // Gets & Stores user History and Routes / Stats / Ranking 
    $scope.userStats.personal = $rootScope.userStats.personal;
    $scope.userStats.group = $rootScope.userStats.group;
    $scope.userStats.organization = $rootScope.userStats.organization;
    $scope.ranks.group = $rootScope.ranks.group;
    $scope.ranks.organization = $rootScope.ranks.organization;
    $scope.userHistory = $rootScope.userHistory;
    $scope.getHistory = function () { 
        dataFactory.getValues('Rides', true, "username=" + $scope.userPersonalInfo.UserName)
                        .success(function (values) {
                            $scope.userHistory = values;
                            console.log($scope.userHistory); 
                        })
                        .error(function (value) {
                            console.log("error");
                        });
        dataFactory.getValues('Routes', true, "username=" + $scope.userPersonalInfo.UserName)
                        .success(function (values) {
                            $scope.userRoutes = values;
                            console.log($scope.userRoutes);
                        })
                        .error(function (value) {
                            console.log("error");
                        });
        dataFactory.getValues('Stat', true, "username=" + $scope.userPersonalInfo.UserName)
                        .success(function (values) {
                            $scope.userStats.personal = values;
                            
                        })
                        .error(function (value) {
                            console.log("error");
                        });

        dataFactory.getValues('Stat', true, "grpname=" + $scope.userPersonalInfo.GroupName + "&orgname=" + $scope.userPersonalInfo.OrganizationName)
                        .success(function (values) {
                            $scope.userStats.group = values;

                        })
                        .error(function (value) {
                            console.log("error");
                        });
        dataFactory.getValues('Stat', true, "orgname=" + $scope.userPersonalInfo.OrganizationName)
                        .success(function (values) {
                            $scope.userStats.organization = values;
                        })
                        .error(function (value) {
                            console.log("error");
                        });

        
        dataFactory.getValues('Ranking', true, "grpname=" + $scope.userPersonalInfo.GroupDes + "&orgname=" + $scope.userPersonalInfo.OrganizationDes + "&gender=&order=Points")
                 .success(function (values) {
                     $scope.ranks.group = angular.fromJson(values);
                     
                 })
                 .error(function (error) {
                     console.log("error");
                 });
    }
    
    
    // Get user History per month
    $scope.getRidesPerMonth = function () {
        $scope.getHistory();
        var tempDate = new Date($scope.calYear, $scope.calMonth, 1);

        var month = tempDate.getMonth() +1 ;
        var year = tempDate.getFullYear();

        var monthlyRidesArr = [];

        var yearParsed = new String;
        var monthParsed = new String;
        var dayParsed = new String;

        angular.forEach($scope.userHistory, function (ride) {
            yearParsed = ride.RideDate.substr(0, 4);
            monthParsed = ride.RideDate.substr(5, 2);
            dayParsed = ride.RideDate.substr(8, 2);

            if (yearParsed == year && monthParsed == month) {
                day2push = parseInt(dayParsed);
                if ($.inArray(day2push,monthlyRidesArr) == -1)
                    monthlyRidesArr.push(day2push);
            }

        
               
        });
        
        return monthlyRidesArr;
        
    };


    
    // Trigger from popover - open rides for rider
    $scope.alarmFromPop = function ($event) {
        console.log($event.target.id);
        if ($scope.routeFlag == true) { $scope.routeFlag = false }
        else { $scope.routeFlag = true; };
        $scope.popDate = $event.target.parentElement.parentElement.parentElement.attributes.name.value;
        $rootScope.activeDay = $scope.popDate; // Holds Active day @ root scope var
    };

    // Submit a ride from Existing Route


    $scope.submitRide = function (selectedRoute,$event) {
        var x = new String;
        var newRide = {
            username: $rootScope.userPersonalInfo.UserName,
            routename: selectedRoute.routeName.RouteName,
            ridedate: $event.target.parentElement.parentElement.parentElement.getAttribute('name'),
            roundtrip: selectedRoute.roundTrip
        }
        
        var dataString = "username=" + newRide.username + '&routename=' + newRide.routename + "&ridedate=" + newRide.ridedate + "&roundtrip=" + newRide.roundtrip;
       
        // api/Rides?username=tester1&routename=[Existing Route Name]&ridedate=01-01-1985&roundtrip=True/False
       
        dataFactory.postValues('Rides', newRide, true, dataString)
                        .success(function (response) {
                            x = x.concat("#").concat(newRide.ridedate);
                            $(x).popover("hide");
                            $(x).addClass("cal-highlight");
                            $scope.flipRideFlag();
                            $scope.calDates = $scope.getRidesPerMonth()
                            
                        })
                          
                        .error(function (response) {
                            console.log("error");
                        });


    }

    

    $scope.inverseFlag = function () { $scope.routeFlag = false; }


    // Right Hand Dashboard

    $scope.rightDash = {};
    $scope.rightDash.switch = 1;

    // Flags 
    $scope.showMyRoutesFlag = false;
    $scope.flipShowMyRoutesFlag = function () { $scope.showMyRoutesFlag = !$scope.showMyRoutesFlag };
    $scope.addNewRouteFlag = false;
    $scope.flipAddNewRouteFlag = function () { $scope.addNewRouteFlag = !$scope.addNewRouteFlag };
    $scope.addNewSRideFlag = false;
    $scope.flipAddNewSRideFlag = function () { $scope.addNewSRideFlag = !$scope.addNewSRideFlag };
    $scope.userStatsFlag = false;
    $scope.flipUserStatsFlag = function () {
        $scope.userStatsFlag = !$scope.userStatsFlag;
        if ($scope.groupStatsFlag || $scope.organizationStatsFlag) {
            $scope.groupStatsFlag = $scope.organizationStatsFlag = false;
        }
    };
    $scope.groupStatsFlag = false;
    $scope.flipGroupStatsFlag = function () {
        $scope.groupStatsFlag = !$scope.groupStatsFlag;
        if ($scope.userStatsFlag || $scope.organizationStatsFlag) {
            $scope.userStatsFlag = $scope.organizationStatsFlag = false;
        }
    };
    $scope.organizationStatsFlag = false;
    $scope.flipOrganizationStatsFlag = function () {
        $scope.organizationStatsFlag = !$scope.organizationStatsFlag;
        if ($scope.userStatsFlag || $scope.groupStatsFlag) {
            $scope.userStatsFlag = $scope.groupStatsFlag = false;
        }
    };

    // Add a new Route 
    $scope.addNewRoute = function (newRoute) {
        var route = {
            UserName: $rootScope.userPersonalInfo.UserName,
            RouteType: newRoute.type,
            RouteLength: newRoute.lenght,
            Comments: newRoute.comments,
            RouteSource: newRoute.source,
            RouteDestination: newRoute.destination
        }

        dataFactory.postValues('Routes', route, false)
                       .success(function (response) {
                           $scope.getHistory();
                           $scope.newRoute = null;
                           $scope.flipAddNewRouteFlag();
                           console.log(response);
                       })
                       .error(function (response) {
                           console.log("error");
                       });
        
    }

    // Delete a route 

    $scope.deleteRoute = function (route) {

        var routeDelete = {
            userName: $rootScope.userPersonalInfo.UserName,
            routename: route.RouteName
        }

        var par = "username=" + routeDelete.userName + "&routename=" + routeDelete.routename;
        dataFactory.deleteValues('Routes', par)
                       .success(function (response) {
                           $scope.getHistory();
                           $scope.flipShowMyRouteFlag();
                           console.log(response);
                       })
                       .error(function (response) {
                           console.log("error");
                       });

        console.log(route);

    };

    // Stats Handling 
    //  #######################

    // Gauger chart


    // Get user stats - type (calCo2/kmRides ) , period (-1 = since registrating , 0 = specific month) ,  (month, year) 
    $scope.statSelector = -1;
    

    $scope.getStats = function (entity, type, period, month, year){ 

        var rawStats = {};
        if (entity == "personal")
        {  rawStats = $scope.userStats.personal; }
        else if (entity == "group")
        {  rawStats = $scope.userStats.group; }
        else
        {  rawStats = $scope.userStats.organization; }
        
        
        if (period == -1) {
            if (type == "calCo2") {

                var co2Summed = 0;
                var calSummed = 0;

                angular.forEach(rawStats, function (monthStat) {
                    if (entity == "personal")
                    {
                        co2Summed = co2Summed + monthStat.User_CO2_Kilograms_Saved;
                        calSummed = calSummed + monthStat.User_Calories;
                    }
                    else if (entity == "group")
                    {
                        co2Summed = co2Summed + monthStat.Group_CO2_Kilograms_Saved;
                        calSummed = calSummed + monthStat.Group_Calories;
                    }
                    else
                    {
                        co2Summed = co2Summed + monthStat.Organization_CO2_Kilograms_Saved;
                        calSummed = calSummed + monthStat.Organization_Calories;
                    }
                    
                });

                calSummed = Math.round(calSummed * 100) / 100;
                co2Summed = Math.round(co2Summed * 100) / 100;
                var statArray = [
                    ['Label', 'Value'],
                    ['קלוריות', calSummed],
                    ['CO2', co2Summed],
                ];

                return statArray;
            }
            if (type == "kmRides") {

                var kmSummed = 0;
                var ridesSummed = 0;
                var fuelSummed = 0;
                var moneySummed = 0;
                angular.forEach(rawStats, function (monthStat) {
                    if (entity == "personal") {
                        kmSummed = kmSummed + monthStat.User_KM;
                        ridesSummed = ridesSummed + monthStat.Num_of_Rides;
                    }
                    else if (entity == "group") {
                        kmSummed = kmSummed + monthStat.Group_KM;
                        ridesSummed = ridesSummed + monthStat.Num_of_Rides;
                    }
                    else {
                        kmSummed = kmSummed + monthStat.Organization_KM;
                        ridesSummed = ridesSummed + monthStat.Num_of_Rides;
                    }

                   
                });

                fuelSummed = Math.ceil(kmSummed / 11);
                moneySummed = Math.ceil(fuelSummed * 7.5);
                if (entity == "personal") {
                    $scope.userKmRides = {
                        km: kmSummed,
                        rides: ridesSummed,
                        fuel: fuelSummed.toString(),
                        money: moneySummed.toString()
                    };
                }
                else if (entity == "group") {
                    $scope.groupKmRides = {
                        km: kmSummed,
                        rides: ridesSummed,
                        fuel: fuelSummed,
                        money: moneySummed
                    };
                }
                else {
                    $scope.organizationKmRides = {
                        km: kmSummed,
                        rides: ridesSummed,
                        fuel: fuelSummed,
                        money: moneySummed
                    };
                }
                

            }
        }
        else {

            var statDate = new Date(year, month, 1);
            var monthParsed = statDate.getMonth() + 1;
            var yearParsed = statDate.getFullYear();
            if (type == "calCo2") {

                var co2Summed = 0;
                var calSummed = 0;
                

                angular.forEach(rawStats, function (monthStat) {

                    if (entity == "personal") {
                        if (monthParsed == monthStat.Month && yearParsed == monthStat.Year) {
                            co2Summed = co2Summed + monthStat.User_CO2_Kilograms_Saved;
                            calSummed = calSummed + monthStat.User_Calories;
                        }
                    }
                    else if (entity == "group") {
                        if (monthParsed == monthStat.Month && yearParsed == monthStat.Year) {
                            co2Summed = co2Summed + monthStat.Group_CO2_Kilograms_Saved;
                            calSummed = calSummed + monthStat.Group_Calories;
                        }
                    }
                    else {
                        if (monthParsed == monthStat.Month && yearParsed == monthStat.Year) {
                            co2Summed = co2Summed + monthStat.Organization_CO2_Kilograms_Saved;
                            calSummed = calSummed + monthStat.Organization_Calories;
                        }
                    }
               
                });

                calSummed = Math.round(calSummed * 100) / 100;
                co2Summed = Math.round(co2Summed * 100) / 100;

                var statArray = [
                    ['Label', 'Value'],
                    ['קלוריות', calSummed],
                    ['CO2', co2Summed],
                ];

                return statArray;
            }
            if (type == "kmRides") {

                var kmSummed = 0;
                var ridesSummed = 0;
                var fuelSummed = 0;
                var moneySummed = 0;
                angular.forEach(rawStats, function (monthStat) {
                    if (entity == "personal") {
                        if (monthParsed == monthStat.Month && yearParsed == monthStat.Year) {
                            kmSummed = kmSummed + monthStat.User_KM;
                            ridesSummed = ridesSummed + monthStat.Num_of_Rides;
                        }
                    }
                    else if (entity == "group") {
                        if (monthParsed == monthStat.Month && yearParsed == monthStat.Year) {
                            kmSummed = kmSummed + monthStat.Group_KM;
                            ridesSummed = ridesSummed + monthStat._of_Rides;
                        }
                    }
                    else {
                        if (monthParsed == monthStat.Month && yearParsed == monthStat.Year) {
                            kmSummed = kmSummed + monthStat.Organization_KM;
                            ridesSummed = ridesSummed + monthStat.Num_of_Rides;
                        }
                    }


                       
                });
            }

                
        };
        fuelSummed = Math.ceil(kmSummed / 11);
        moneySummed = Math.ceil(fuelSummed * 7.5);
        if (entity == "personal") {
       
            $scope.userKmRides = {
                km: kmSummed,
                rides: ridesSummed,
                fuel: fuelSummed,
                money: moneySummed
            }
        }
        else if (entity == "group") {
            $scope.groupKmRides = {
                km: kmSummed,
                rides: ridesSummed,
                fuel: fuelSummed,
                money: moneySummed
            }
        }
        else {
            $scope.organizationKmRides = {
                km: kmSummed,
                rides: ridesSummed,
                fuel: fuelSummed,
                money: moneySummed
            }
        }
    
        
    };


    $scope.getRanks = function (entity, period, month, year) {

        if (entity == "personal") { }
        else if (entity == "group") {
            var rawRanks = $scope.ranks.group;
            if (period == -1) {
                var summedArrPerUser = [];
                var match = false;
                summedArrPerUser.push(["רוכב", "קמ"]);
                angular.forEach(rawRanks, function (entry) {
                    for (var i = 0; i < summedArrPerUser.length; i++) {
                        if (entry.UserName == summedArrPerUser[i][0]) {
                            match = 1;
                            summedArrPerUser[i][1] = summedArrPerUser[i][1] + entry.User_KM;
                        } 
                    }
                    if (match == 0) { summedArrPerUser.push([entry.UserName, entry.User_KM]); }
                });
                return summedArrPerUser;
            }
            else if (period == 0) {
                var statDate = new Date(year, month, 1);
                //var monthParsed = statDate.getMonth() + 1;
                //var yearParsed = statDate.getFullYear();
                //angular.forEach(rawRanks, function (enytry) {
                //    if (monthParsed == entry.Month && yearParsed == entry.Year) {
                //        kmSummed = kmSummed + entry.Organization_KM;
                //        ridesSummed = ridesSummed + entry.Num_of_Rides;
                //}
                //})

            }}
        else if (entity == "organization") { }
    };


    //$scope.getRanks = function () {
    //    var rawRanks = $scope.ranks.group;
    //    var groupKMdist = [];
    //    groupKMdist.push(["רוכב", "קמ"]);
    //    angular.forEach(rawRanks, function (userRank) {
    //        var tempArr = []
    //        tempArr[0] = userRank.UserName;
    //        tempArr[1] = userRank.User_KM;
    //        groupKMdist.push(tempArr);
    //    });     

    //      //   ['Task', 'Hours per Day'],
    //      //['Work',     11],
    //      //['Eat',      2],
    //      //['Commute',  2],
    //      //['Watch TV', 2],
    //      //['Sleep',    7]
    //    return groupKMdist;
    //}

    //{"UserName":"tester1", "RideType":"" , "RideLength":10, "RideSource":"A" ,"RideDate":"01-01-2014" "RideDestination":"B" }
    $scope.addNewSpontanicRide = function (newRide) {
        
        

        var newSRide = {
            UserName: $rootScope.userPersonalInfo.UserName,
            RideType: newRide.type,
            RideLength: newRide.lenght,
            RideSource: newRide.source,
            RideDate: newRide.date,
            RideDestination: newRide.destination
        }
        dataFactory.postValues('Rides', newSRide, false)
                                .success(function (response) {
                                    $scope.addNewSRideFlag = false;
                                    $scope.getHistory();
                                    console.log(response);
                                    $("#" + newRide.date).addClass("cal-highlight");
                                })
                                .error(function (response) {
                                    console.log("error");
                                });
    }

    //{"UserName":"tester1", "RideType":"" , "RideLength":10, "RideSource":"A" , "RideDestination":"B" }


    // Controller init 
    $scope.init = function () {       
    }

});

