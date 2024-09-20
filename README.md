# Billing Calculator

## Description
This project is a simple implementation of a solution to a sample exercise. 

### Exercise Description
You have a set of projects, and you need to calculate a reimbursement amount for the set. Each project has a start date 
and an end date. The first day of a project and the last day of a project are always "travel" days. Days in the middle 
of a project are "full" days. There are also two types of cities a project can be in, high cost cities and low cost 
cities.

#### Domain Rules
 - First day and last day of a project, or sequence of projects, is a travel day.
 - Any day in the middle of a project, or sequence of projects, is considered a full day.
 - If there is a gap between projects, then the days on either side of that gap are travel days.
 - If two projects push up against each other, or overlap, then those days are full days as well.
 - Any given day is only ever counted once, even if two projects are on the same day.
 - A travel day is reimbursed at a rate of 45 dollars per day in a low cost city.
 - A travel day is reimbursed at a rate of 55 dollars per day in a high cost city.
 - A full day is reimbursed at a rate of 75 dollars per day in a low cost city.
 - A full day is reimbursed at a rate of 85 dollars per day in a high cost city.

## Getting Started
This getting started guide is tailored for MacOS users.
1. Install dotnet via: `brew install dotnet@8` -> this installs the dotnet 8.0 SDK.
2. Run the app by executing the following command from the root directory of the project:
   ```shell
   dotnet run --project ./Billing.Console/Billing.Console.csproj
   ```
3. If you run into issues with any missing dependencies, you may need to download them (although `dotnet run` should
   handle this for you). Restore the dependencies by running the following command:
   ```shell
   dotnet restore
   ```


## Running the Tests
From the root directory of the project, run the following command:
```shell
dotnet test
```


## Other Considerations
I found logical omissions in the requirements:
1. The requirements do not specify how to handle the case where a travel day is shared between a high and low cost city;
   this creates ambiguity in the requirements, as the cost of the travel day cannot be determined because there is no
   rule for a prevailing rate. To cover this case, I have added a configuration option, `OverlapConfiguration`, which
   allows the developer to specify one of these four mechanisms to handle the case:
    - `Take First Day`: Take the billable day of the project that starts first.
    - `Take Last Day`: Take the billable day of the project starts last.
    - `Take Greater Amount`: Take the billable day of the project with the greater amount.
    - `Take Lesser Amount`: Take the billable day of the project with the lesser amount .
2. Since a date can only be billed once, there is another issue when a travel day is shared between two projects. There
   is no clear rule for which project should be billed for the travel day.  This impacts the total project cost, where
   costs _may_ be understated.