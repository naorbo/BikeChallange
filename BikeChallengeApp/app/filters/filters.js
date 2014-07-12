/// <reference path="../Scripts/angular.js" />

//Users Management filters
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

app.filter('phonePattrenFilter', [function () {
    return function (phoneNum) {
        var template = new String;
        if (phoneNum.length == 10) {
            template = template.concat('(').concat(phoneNum.slice(0, 3)).concat(') ').concat(phoneNum.slice(3, 10));
            return template;
        }
        else if (phoneNum.length == 9) {
            template = template.concat('(').concat(phoneNum.slice(0, 2)).concat(') ').concat(phoneNum.slice(2, 9));
            return template;
        }
        else
            return phoneNum;
    }
}])

app.filter('datePattrenFilter', [function () {
    return function (date) {
        var template = new String;
        try {
            template = template.concat(date.slice(8, 10)).concat('-').concat(date.slice(5, 7)).concat('-').concat(date.slice(0, 4));
            return template;
        }
        catch (err)
        {
            return date;
        }
        
    }
}])


//Groups Management filters


app.filter('groupsOrgFilter', [function () {
    return function (orgs, orgGrgMgmnt) {
        var filteredGroups = [];
        if (orgGrgMgmnt != undefined) {
            angular.forEach(orgs, function (org) {
                if (org.OrganizationDes == orgGrgMgmnt)
                    filteredGroups.push(org);
            })
            return filteredGroups;
        }
        else
            return orgs;
    }
}])



// Organiztion Management Filters

app.filter('orgCityFilter', [function () {
    return function (organizations, city) {
        var filteredOrganizations = [];
        if (city != undefined) {
            angular.forEach(organizations, function (org) {
                if (org.CityName == city)
                    filteredOrganizations.push(org);
            })
            return filteredOrganizations;
        }
        else
            return organizations;
    }
}])


//Event Date Filter (return only future events)

app.filter('eventsDateFilter', (function ($filter) {
    return function (events) {
        var filteredEvents = [];
        var tempDate = new Date;
        var tempDayStringed = $filter('date')(tempDate, "yyyy-MM-dd");
            angular.forEach(events, function (event) {
                if (event.EventDate > tempDayStringed)
                    filteredEvents.push(event);
            })
            return filteredEvents;
        
    }
}))

// Return the events the user haven't registered to yet
app.filter('eventsRegisteredFilter', [function () {
    return function (events, userEvents) {
        var filteredEvents = [];
        angular.forEach(events, function (sysEvent)
         {
            var noMatch = true;
            angular.forEach(userEvents, function (userEvent) {
                if (userEvent.EventDes == sysEvent.EventDes) {
                    noMatch = false;
                }
            })
            if (noMatch) {
                filteredEvents.push(sysEvent);
            }
        })
        return filteredEvents;
    }
}])

