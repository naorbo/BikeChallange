/// <reference path="../Scripts/angular.js" />

//Users Management filter 
//city, org, gender, group, bike

app.filter('cityFilter', [function () {
    return function (users, city) {
        var filteredUsers = [];
        if (city != undefined) {
            angular.forEach(users, function (user) {
                if (user.RiderCity == city)
                    filteredUsers.push(user);
            })
            return filteredUsers;
        }
        else
            return users;
    }
}])

app.filter('genderFilter', [function () {
    return function (users, gender) {
        var filteredUsers = [];
        if (gender != undefined) {
            if (gender == 'זכר' ? gender = 'M' : gender = 'F');
            angular.forEach(users, function (user) {
                if (user.Gender == gender)
                    filteredUsers.push(user);
            })
            return filteredUsers;
        }
        else
            return users;
    }
}])

app.filter('orgFilter', [function () {
    return function (users, org) {
        var filteredUsers = [];
        if (org != undefined) {
            angular.forEach(users, function (user) {
                if (user.OrganizationDes == org)
                    filteredUsers.push(user);
            })
            return filteredUsers;
        }
        else
            return users;
    }
}])


app.filter('groupFilter', [function () {
    return function (users, group) {
        var filteredUsers = [];
        if (group != undefined) {
            angular.forEach(users, function (user) {
                if (user.GroupDes == group.GroupDes)
                    filteredUsers.push(user);
            })
            return filteredUsers;
        }
        else
            return users;
    }
}])








